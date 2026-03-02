using System;
using PRN232_EbayClone.Domain.SellingPreferences.Enums;
using PRN232_EbayClone.Domain.SellingPreferences.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;

public sealed class InvoicePreference
{
    public InvoiceFormat Format { get; private set; }
    public bool SendEmailCopy { get; private set; }
    public bool ApplyCreditsAutomatically { get; private set; }

    private InvoicePreference() { }

    private InvoicePreference(InvoiceFormat format, bool sendEmailCopy, bool applyCreditsAutomatically)
    {
        Format = format;
        SendEmailCopy = sendEmailCopy;
        ApplyCreditsAutomatically = applyCreditsAutomatically;
    }

    public static InvoicePreference CreateDefault() => new(InvoiceFormat.Detailed, true, true);

    public Result Update(InvoiceFormat format, bool sendEmailCopy, bool applyCreditsAutomatically)
    {
        if (!Enum.IsDefined(typeof(InvoiceFormat), format))
        {
            return SellerPreferenceErrors.InvalidInvoiceFormat;
        }

        Format = format;
        SendEmailCopy = sendEmailCopy;
        ApplyCreditsAutomatically = applyCreditsAutomatically;

        return Result.Success();
    }
}
