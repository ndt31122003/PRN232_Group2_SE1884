using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Commands.ReplaceExemptBuyers;

public sealed record ReplaceExemptBuyersCommand(
    Guid UserId,
    IReadOnlyCollection<string>? ExemptBuyerIdentifiers) : ICommand<BuyerListDto>;

public sealed class ReplaceExemptBuyersCommandValidator : AbstractValidator<ReplaceExemptBuyersCommand>
{
    public ReplaceExemptBuyersCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleForEach(x => x.ExemptBuyerIdentifiers)
            .Must(identifier => identifier is null || identifier.Trim().Length <= SellerPreference.MaxBuyerIdentifierLength)
            .WithMessage($"Buyer identifier cannot exceed {SellerPreference.MaxBuyerIdentifierLength} characters.");
    }
}

public sealed class ReplaceExemptBuyersCommandHandler : ICommandHandler<ReplaceExemptBuyersCommand, BuyerListDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplaceExemptBuyersCommandHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BuyerListDto>> Handle(ReplaceExemptBuyersCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<BuyerListDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

        var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
        var isNew = false;
        if (preference is null)
        {
            preference = SellerPreference.CreateDefault(userId);
            _sellerPreferenceRepository.Add(preference);
            isNew = true;
        }

        var identifiers = request.ExemptBuyerIdentifiers ?? Array.Empty<string>();
        var result = preference.ReplaceExemptBuyers(identifiers);
        if (result.IsFailure)
        {
            return Result.Failure<BuyerListDto>(result.Error);
        }

        if (!isNew)
        {
            _sellerPreferenceRepository.Update(preference);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(preference.ToExemptBuyerListDto());
    }
}
