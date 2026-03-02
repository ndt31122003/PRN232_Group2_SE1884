using PRN232_EbayClone.Application.Abstractions.Identity;

namespace PRN232_EbayClone.Infrastructure.Identity;

public sealed class OtpGenerator : IOtpGenerator
{
    public string GenerateOtp(int length = 6)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
        }
        var random = new Random();
        var otp = new char[length];
        for (int i = 0; i < length; i++)
        {
            otp[i] = (char)('0' + random.Next(0, 10));
        }
        return new string(otp);
    }
}
