using PRN232_EbayClone.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Domain.Shared.Errors
{
    public static class MoneyErrors
    {
        private readonly static Error InvalidAmount = Error.Failure(
            "Money.InvalidAmount",
            "Số tiền không hợp lệ");
        public readonly static Error NegativeAmount = Error.Failure(
            "Money.NegativeAmount",
            "Số tiền không được âm");
        public readonly static Error ZeroAmount = Error.Failure(
            "Money.ZeroAmount",
            "Số tiền phải lớn hơn 0");
        public readonly static Error CurrencyRequired = Error.Failure(
            "Money.CurrencyRequired",
            "Loại tiền tệ là bắt buộc");
        public readonly static Error DifferentCurrency = Error.Failure(
            "Money.DifferentCurrency",
            "Không thể thực hiện thao tác trên các loại tiền tệ khác nhau");


        public static Error InvalidCurrency => Error.Failure(
            "Money.InvalidCurrency",
            $"Loại tiền tệ không hợp lệ");

        public static Error DivideByZero => Error.Failure(
            "Money.DivideByZero",
            "Không thể chia cho 0");

    }
}
