using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record CreateDisputeCommand(
    Guid ListingId,
    string Reason
) : ICommand<Guid>;

public sealed class CreateDisputeCommandValidator : AbstractValidator<CreateDisputeCommand>
{
    public CreateDisputeCommandValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty().WithMessage("Listing ID là bắt buộc");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Lý do khiếu nại là bắt buộc")
            .MaximumLength(2000).WithMessage("Lý do không được vượt quá 2000 ký tự");
    }
}

public sealed class CreateDisputeCommandHandler : ICommandHandler<CreateDisputeCommand, Guid>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDisputeCommandHandler(
        IDisputeRepository disputeRepository,
        IListingRepository listingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateDisputeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return DisputeErrors.Unauthorized;
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
        {
            return DisputeErrors.ListingNotFound;
        }

        // Check if dispute already exists for this listing
        var existingDisputes = await _disputeRepository.GetDisputesByListingIdAsync(request.ListingId, cancellationToken);
        if (existingDisputes.Any(d => !d.IsClosed))
        {
            return DisputeErrors.AlreadyExists;
        }

        var disputeResult = Dispute.Create(
            request.ListingId,
            _userContext.UserId,
            request.Reason);

        if (disputeResult.IsFailure)
        {
            return disputeResult.Error;
        }

        var dispute = disputeResult.Value;
        _disputeRepository.Add(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(dispute.Id);
    }
}
