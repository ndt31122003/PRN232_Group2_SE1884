using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.ImportInventoryRestockExcel;

public sealed record ImportInventoryRestockExcelCommand(IFormFile File) : ICommand<ImportInventoryRestockExcelResult>;

public sealed record InventoryRestockImportFailure(int RowNumber, Guid? ListingId, string? Sku, string Message);

public sealed record ImportInventoryRestockExcelResult(int UpdatedCount, IReadOnlyList<InventoryRestockImportFailure> Failures);

internal sealed record InventoryRestockImportRow(int RowNumber, Guid? ListingId, string? Sku, int Quantity, string? Reason);

public sealed class ImportInventoryRestockExcelCommandValidator : AbstractValidator<ImportInventoryRestockExcelCommand>
{
    private static readonly string[] AllowedExtensions = [".xlsx", ".xlsm"];

    public ImportInventoryRestockExcelCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("Excel file is required.")
            .Must(file => file is not null && file.Length > 0).WithMessage("Excel file must not be empty.")
            .Must(file => file is not null && AllowedExtensions.Contains(Path.GetExtension(file.FileName), StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only .xlsx or .xlsm files are supported.");
    }
}

public sealed class ImportInventoryRestockExcelCommandHandler : ICommandHandler<ImportInventoryRestockExcelCommand, ImportInventoryRestockExcelResult>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ImportInventoryRestockExcelCommandHandler(
        IInventoryRepository inventoryRepository,
        IListingRepository listingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ImportInventoryRestockExcelResult>> Handle(ImportInventoryRestockExcelCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        using var stream = request.File.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.FirstOrDefault();
        if (worksheet is null)
        {
            return Error.Failure("Inventory.Import.EmptyWorkbook", "The uploaded workbook does not contain any worksheets.");
        }

        var parsedRows = ParseRows(worksheet);
        if (parsedRows.Failures.Count > 0 && parsedRows.Rows.Count == 0)
        {
            return new ImportInventoryRestockExcelResult(0, parsedRows.Failures);
        }

        var failures = parsedRows.Failures.ToList();
        var updatedListingIds = new HashSet<Guid>();

        foreach (var row in parsedRows.Rows)
        {
            var listing = await ResolveListingAsync(row, userId, cancellationToken);
            if (listing.IsFailure)
            {
                failures.Add(new InventoryRestockImportFailure(row.RowNumber, row.ListingId, row.Sku, listing.Error.Description));
                continue;
            }

            var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(listing.Value.Id), cancellationToken);
            if (inventory is null)
            {
                failures.Add(new InventoryRestockImportFailure(row.RowNumber, listing.Value.Id, listing.Value.Sku, "Inventory has not been initialized for this listing."));
                continue;
            }

            var restockResult = inventory.Restock(row.Quantity, row.Reason);
            if (restockResult.IsFailure)
            {
                failures.Add(new InventoryRestockImportFailure(row.RowNumber, listing.Value.Id, listing.Value.Sku, restockResult.Error.Description));
                continue;
            }

            _inventoryRepository.Update(inventory);
            updatedListingIds.Add(listing.Value.Id);
        }

        if (updatedListingIds.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new ImportInventoryRestockExcelResult(updatedListingIds.Count, failures);
    }

    private async Task<Result<Domain.Listings.Entities.Listing>> ResolveListingAsync(InventoryRestockImportRow row, string ownerId, CancellationToken cancellationToken)
    {
        Domain.Listings.Entities.Listing? listing = null;

        if (row.ListingId.HasValue)
        {
            listing = await _listingRepository.GetByIdAsync(row.ListingId.Value, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(row.Sku))
        {
            listing = await _listingRepository.GetByOwnerAndSkuAsync(ownerId, row.Sku, cancellationToken);
        }

        if (listing is null)
        {
            return Error.Failure("Listing.NotFound", "Listing could not be found from ListingId or SKU.");
        }

        if (!string.Equals(listing.CreatedBy, ownerId, StringComparison.OrdinalIgnoreCase))
        {
            return ListingErrors.Unauthorized;
        }

        return listing;
    }

    private static (IReadOnlyList<InventoryRestockImportRow> Rows, IReadOnlyList<InventoryRestockImportFailure> Failures) ParseRows(IXLWorksheet worksheet)
    {
        var failures = new List<InventoryRestockImportFailure>();
        var rows = new List<InventoryRestockImportRow>();

        var headerRow = worksheet.FirstRowUsed();
        if (headerRow is null)
        {
            failures.Add(new InventoryRestockImportFailure(0, null, null, "Worksheet is empty."));
            return (rows, failures);
        }

        var headerMap = headerRow.CellsUsed()
            .ToDictionary(
                cell => NormalizeHeader(cell.GetString()),
                cell => cell.Address.ColumnNumber,
                StringComparer.OrdinalIgnoreCase);

        if (!headerMap.TryGetValue("quantity", out var quantityColumn))
        {
            failures.Add(new InventoryRestockImportFailure(0, null, null, "The worksheet must contain a Quantity column."));
            return (rows, failures);
        }

        headerMap.TryGetValue("listingid", out var listingIdColumn);
        headerMap.TryGetValue("sku", out var skuColumn);
        headerMap.TryGetValue("reason", out var reasonColumn);

        if (listingIdColumn == 0 && skuColumn == 0)
        {
            failures.Add(new InventoryRestockImportFailure(0, null, null, "The worksheet must contain either ListingId or SKU column."));
            return (rows, failures);
        }

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? headerRow.RowNumber();
        for (var rowNumber = headerRow.RowNumber() + 1; rowNumber <= lastRow; rowNumber++)
        {
            var row = worksheet.Row(rowNumber);
            if (row.CellsUsed().All(cell => string.IsNullOrWhiteSpace(cell.GetString())))
            {
                continue;
            }

            var listingIdText = listingIdColumn > 0 ? row.Cell(listingIdColumn).GetString().Trim() : string.Empty;
            var sku = skuColumn > 0 ? row.Cell(skuColumn).GetString().Trim() : string.Empty;
            var quantityText = row.Cell(quantityColumn).GetString().Trim();
            var reason = reasonColumn > 0 ? row.Cell(reasonColumn).GetString().Trim() : null;

            Guid? listingId = null;
            if (!string.IsNullOrWhiteSpace(listingIdText))
            {
                if (!Guid.TryParse(listingIdText, out var parsedListingId))
                {
                    failures.Add(new InventoryRestockImportFailure(rowNumber, null, sku, "ListingId must be a valid GUID."));
                    continue;
                }

                listingId = parsedListingId;
            }

            if (listingId is null && string.IsNullOrWhiteSpace(sku))
            {
                failures.Add(new InventoryRestockImportFailure(rowNumber, null, null, "Either ListingId or SKU is required."));
                continue;
            }

            if (!int.TryParse(quantityText, out var quantity) || quantity <= 0)
            {
                failures.Add(new InventoryRestockImportFailure(rowNumber, listingId, sku, "Quantity must be a positive whole number."));
                continue;
            }

            rows.Add(new InventoryRestockImportRow(rowNumber, listingId, string.IsNullOrWhiteSpace(sku) ? null : sku, quantity, string.IsNullOrWhiteSpace(reason) ? null : reason));
        }

        return (rows, failures);
    }

    private static string NormalizeHeader(string value)
    {
        return new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
    }
}