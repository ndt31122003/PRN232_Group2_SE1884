import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import LogoEbay from "../../assets/images/ebay_logo.png";
import IdentityService from "../../services/IdentityService";
import Notice from "../../components/Common/CustomNotification";
import OTP_TYPES from "../../constants/otpTypes";

export default function PaymentVerificationPage() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [otpData, setOtpData] = useState(null);
  const [code, setCode] = useState("");
  const [loading, setLoading] = useState(false);
  const [receipt, setReceipt] = useState(null);
  const [form, setForm] = useState({
    cardHolderName: "",
    cardNumber: "",
    expiryMonth: "",
    expiryYear: "",
    cvv: "",
  });
  const [errors, setErrors] = useState({});

  const handleRequestOtp = async () => {
    try {
      setLoading(true);
      const res = await IdentityService.requestOtp({
        type: OTP_TYPES.VERIFY_PAYMENT,
      });
      const data = res?.data ?? res;
      setOtpData(data);
      Notice({
        msg: "OTP sent!",
        desc: "Check your email for the verification code.",
        isSuccess: true,
        place: "bottomRight",
      });
      setStep(2);
    } catch (err) {
      const msg =
        err?.response?.data?.detail ||
        err?.response?.data?.title ||
        "Failed to send OTP";
      Notice({ msg: "Error", desc: msg, isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (field) => (e) => {
    let value = e.target.value;
    if (field === "cardNumber") {
      value = value.replace(/[^\d\s]/g, "");
    }
    if (field === "expiryMonth" || field === "expiryYear" || field === "cvv") {
      value = value.replace(/\D/g, "");
    }
    setForm((prev) => ({ ...prev, [field]: value }));
    if (errors[field]) setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  const validate = () => {
    const errs = {};
    if (!form.cardHolderName) errs.cardHolderName = "Cardholder name is required";
    if (!form.cardNumber) errs.cardNumber = "Card number is required";
    else if (form.cardNumber.replace(/\s/g, "").length < 12)
      errs.cardNumber = "Card number must be at least 12 digits";
    if (!form.expiryMonth) errs.expiryMonth = "Required";
    else {
      const m = parseInt(form.expiryMonth, 10);
      if (m < 1 || m > 12) errs.expiryMonth = "1-12";
    }
    if (!form.expiryYear) errs.expiryYear = "Required";
    else {
      const y = parseInt(form.expiryYear, 10);
      if (y < new Date().getFullYear() % 100 + 2000)
        errs.expiryYear = "Card expired";
    }
    if (!form.cvv) errs.cvv = "CVV is required";
    else if (form.cvv.length < 3) errs.cvv = "Min 3 digits";
    if (!code) errs.code = "OTP code is required";
    else if (code.length !== 6) errs.code = "Must be 6 digits";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleVerify = async (e) => {
    e.preventDefault();
    if (!validate()) return;
    try {
      setLoading(true);
      const res = await IdentityService.verifyPayment({
        code,
        cardHolderName: form.cardHolderName,
        cardNumber: form.cardNumber.replace(/\s/g, ""),
        expiryMonth: form.expiryMonth,
        expiryYear: form.expiryYear,
        cvv: form.cvv,
      });
      const data = res?.data ?? res;
      setReceipt(data);
      setStep(3);
      Notice({
        msg: "Payment verified!",
        desc: "Your payment method has been successfully verified.",
        isSuccess: true,
        place: "bottomRight",
      });
    } catch (err) {
      const msg =
        err?.response?.data?.detail ||
        err?.response?.data?.title ||
        "Verification failed";
      Notice({ msg: "Error", desc: msg, isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const inputClass =
    "w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent";

  return (
    <div className="min-h-screen bg-white">
      <header className="border-b border-gray-200 px-6 py-4">
        <div className="flex items-center justify-between max-w-7xl mx-auto">
          <svg className="w-48 h-20" viewBox="0 0 200 80" fill="none">
            <image href={LogoEbay} width="200" height="80" />
          </svg>
          <button
            type="button"
            className="text-sm text-gray-600 hover:text-blue-600"
            onClick={() => navigate("/")}
          >
            Back to home
          </button>
        </div>
      </header>

      <main className="max-w-md mx-auto px-6 py-10">
        {/* Step 1: Request OTP */}
        {step === 1 && (
          <div className="space-y-4 text-center">
            <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto">
              <svg className="w-8 h-8 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
              </svg>
            </div>
            <h2 className="text-2xl font-medium">Verify payment method</h2>
            <p className="text-gray-500 text-sm">
              To start selling, you need to verify a payment method. We'll send a
              verification code to your email.
            </p>
            <button
              onClick={handleRequestOtp}
              disabled={loading}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
            >
              {loading ? "Sending..." : "Send verification code"}
            </button>
          </div>
        )}

        {/* Step 2: Card form + OTP */}
        {step === 2 && (
          <form onSubmit={handleVerify} className="space-y-4">
            <h2 className="text-2xl font-medium text-center mb-2">
              Enter card details
            </h2>
            <p className="text-gray-500 text-center text-sm mb-4">
              Enter your card information and the OTP code sent to your email.
            </p>

            {otpData?.isSimulation && otpData?.code && (
              <div className="bg-yellow-50 border border-yellow-200 rounded-md p-3 text-center">
                <span className="text-sm text-yellow-700">Dev mode OTP: </span>
                <span className="font-mono font-bold text-yellow-900">{otpData.code}</span>
              </div>
            )}

            <div>
              <label className="text-sm text-gray-600 mb-1 block">OTP Code</label>
              <input
                type="text"
                maxLength={6}
                placeholder="000000"
                value={code}
                onChange={(e) => {
                  setCode(e.target.value.replace(/\D/g, ""));
                  if (errors.code) setErrors((prev) => ({ ...prev, code: undefined }));
                }}
                className={`${inputClass} text-center text-xl tracking-[0.4em] font-mono`}
              />
              {errors.code && <p className="text-red-500 text-sm mt-1">{errors.code}</p>}
            </div>

            <div>
              <label className="text-sm text-gray-600 mb-1 block">Cardholder Name</label>
              <input
                type="text"
                placeholder="NGUYEN VAN A"
                value={form.cardHolderName}
                onChange={handleChange("cardHolderName")}
                className={inputClass}
              />
              {errors.cardHolderName && (
                <p className="text-red-500 text-sm mt-1">{errors.cardHolderName}</p>
              )}
            </div>

            <div>
              <label className="text-sm text-gray-600 mb-1 block">Card Number</label>
              <input
                type="text"
                placeholder="4242 4242 4242 4242"
                maxLength={23}
                value={form.cardNumber}
                onChange={handleChange("cardNumber")}
                className={inputClass}
              />
              {errors.cardNumber && (
                <p className="text-red-500 text-sm mt-1">{errors.cardNumber}</p>
              )}
            </div>

            <div className="grid grid-cols-3 gap-3">
              <div>
                <label className="text-sm text-gray-600 mb-1 block">Month</label>
                <input
                  type="text"
                  placeholder="MM"
                  maxLength={2}
                  value={form.expiryMonth}
                  onChange={handleChange("expiryMonth")}
                  className={inputClass}
                />
                {errors.expiryMonth && (
                  <p className="text-red-500 text-sm mt-1">{errors.expiryMonth}</p>
                )}
              </div>
              <div>
                <label className="text-sm text-gray-600 mb-1 block">Year</label>
                <input
                  type="text"
                  placeholder="YYYY"
                  maxLength={4}
                  value={form.expiryYear}
                  onChange={handleChange("expiryYear")}
                  className={inputClass}
                />
                {errors.expiryYear && (
                  <p className="text-red-500 text-sm mt-1">{errors.expiryYear}</p>
                )}
              </div>
              <div>
                <label className="text-sm text-gray-600 mb-1 block">CVV</label>
                <input
                  type="text"
                  placeholder="123"
                  maxLength={4}
                  value={form.cvv}
                  onChange={handleChange("cvv")}
                  className={inputClass}
                />
                {errors.cvv && (
                  <p className="text-red-500 text-sm mt-1">{errors.cvv}</p>
                )}
              </div>
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700 mt-2"
            >
              {loading ? "Verifying..." : "Verify payment method"}
            </button>
          </form>
        )}

        {/* Step 3: Success receipt */}
        {step === 3 && receipt && (
          <div className="text-center space-y-4 py-6">
            <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto">
              <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
            </div>
            <h2 className="text-2xl font-medium">Payment verified!</h2>
            <p className="text-gray-500 text-sm">
              Your payment method has been successfully verified.
            </p>

            <div className="bg-gray-50 rounded-lg p-4 text-left space-y-2 mt-4">
              <div className="flex justify-between text-sm">
                <span className="text-gray-500">Card</span>
                <span className="font-medium">
                  {receipt.cardBrand} •••• {receipt.cardLast4}
                </span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-500">Cardholder</span>
                <span className="font-medium">{receipt.cardholderName}</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-500">Expiry</span>
                <span className="font-medium">
                  {String(receipt.expiryMonth).padStart(2, "0")}/{receipt.expiryYear}
                </span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-500">Reference</span>
                <span className="font-mono text-xs">{receipt.providerReference}</span>
              </div>
            </div>

            <button
              onClick={() => navigate("/")}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition hover:bg-blue-700 mt-4"
            >
              Continue
            </button>
          </div>
        )}
      </main>
    </div>
  );
}
