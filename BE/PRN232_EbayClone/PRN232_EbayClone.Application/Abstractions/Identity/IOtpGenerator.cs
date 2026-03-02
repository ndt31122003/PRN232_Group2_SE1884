namespace PRN232_EbayClone.Application.Abstractions.Identity;

public interface IOtpGenerator
{
    string GenerateOtp(int length = 6);
}
