using PRN232_EbayClone.Application.Abstractions.Identity;
using PRN232_EbayClone.Application.Abstractions.Mail;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Users.Events;

namespace PRN232_EbayClone.Application.Identity.EventHandlers;

public sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IOtpGenerator _otpGenerator;
    private readonly IOtpRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserRegisteredDomainEventHandler(
        IOtpGenerator otpGenerator,
        IOtpRepository otpRepository,
        IUnitOfWork unitOfWork)
    {
        _otpGenerator = otpGenerator;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var code = _otpGenerator.GenerateOtp();

        var otp = Otp.Create(
            notification.Email,
            code,
            OtpType.VerifyEmail);

        _otpRepository.Add(otp);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
