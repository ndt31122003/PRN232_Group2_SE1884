import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import LogoEbay from "../../../assets/images/ebay_logo.png";
import IdentityService from "../../../services/IdentityService";
import OTP_TYPES from "../../../constants/otpTypes";
import { isAuthenticated, persistAuthSession } from "../../../utils/auth";
import Notice from "../../../components/Common/CustomNotification";
import ReCaptcha from "../../../components/Common/ReCaptcha";
import { useCaptcha } from "../../../hooks/useCaptcha";

const REGISTER_CAPTCHA_ACTION = "identity.register";
const RESEND_OTP_CAPTCHA_ACTION = "identity.resend-otp";

const STEPS = [
  { key: "basic", label: "Create Account" },
  { key: "email", label: "Verify Email" },
  { key: "phone", label: "Verify Phone" },
  { key: "business", label: "Business Info" },
];

function StepIndicator({ currentStep }) {
  return (
    <div className="flex items-center justify-center gap-2 mb-8">
      {STEPS.map((step, idx) => {
        const isActive = idx === currentStep;
        const isCompleted = idx < currentStep;
        return (
          <React.Fragment key={step.key}>
            {idx > 0 && (
              <div
                className={`h-0.5 w-8 ${
                  isCompleted ? "bg-blue-600" : "bg-gray-300"
                }`}
              />
            )}
            <div className="flex flex-col items-center">
              <div
                className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-medium ${
                  isCompleted
                    ? "bg-blue-600 text-white"
                    : isActive
                    ? "bg-blue-600 text-white"
                    : "bg-gray-200 text-gray-500"
                }`}
              >
                {isCompleted ? (
                  <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                  </svg>
                ) : (
                  idx + 1
                )}
              </div>
              <span
                className={`text-xs mt-1 ${
                  isActive ? "text-blue-600 font-medium" : "text-gray-400"
                }`}
              >
                {step.label}
              </span>
            </div>
          </React.Fragment>
        );
      })}
    </div>
  );
}

function BasicInfoStep({
  onSubmit,
  loading,
  captchaRequired,
  captchaToken,
  captchaRef,
  onCaptchaChange,
}) {
  const [form, setForm] = useState({
    email: "",
    fullName: "",
    password: "",
    confirmPassword: "",
  });
  const [errors, setErrors] = useState({});

  const validate = () => {
    const errs = {};
    if (!form.email) errs.email = "Email is required";
    else if (!/\S+@\S+\.\S+/.test(form.email)) errs.email = "Invalid email format";
    if (!form.fullName) errs.fullName = "Full name is required";
    if (!form.password) errs.password = "Password is required";
    else if (form.password.length < 8) errs.password = "Min 8 characters";
    else if (!/[A-Z]/.test(form.password)) errs.password = "Must contain uppercase";
    else if (!/[a-z]/.test(form.password)) errs.password = "Must contain lowercase";
    else if (!/[0-9]/.test(form.password)) errs.password = "Must contain a number";
    else if (!/[^a-zA-Z0-9]/.test(form.password)) errs.password = "Must contain a special character";
    if (form.password !== form.confirmPassword) errs.confirmPassword = "Passwords do not match";
    if (captchaRequired && !captchaToken) errs.captcha = "Please complete CAPTCHA";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) {
      onSubmit({ email: form.email, fullName: form.fullName, password: form.password });
    }
  };

  const handleChange = (field) => (e) => {
    setForm((prev) => ({ ...prev, [field]: e.target.value }));
    if (errors[field]) setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <h2 className="text-2xl font-medium text-center mb-6">Create your account</h2>
      <div>
        <input
          type="text"
          placeholder="Full name"
          value={form.fullName}
          onChange={handleChange("fullName")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.fullName && <p className="text-red-500 text-sm mt-1">{errors.fullName}</p>}
      </div>
      <div>
        <input
          type="email"
          placeholder="Email address"
          value={form.email}
          onChange={handleChange("email")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email}</p>}
      </div>
      <div>
        <input
          type="password"
          placeholder="Password"
          value={form.password}
          onChange={handleChange("password")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password}</p>}
      </div>
      <div>
        <input
          type="password"
          placeholder="Confirm password"
          value={form.confirmPassword}
          onChange={handleChange("confirmPassword")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.confirmPassword && <p className="text-red-500 text-sm mt-1">{errors.confirmPassword}</p>}
      </div>
      <button
        type="submit"
        disabled={loading}
        className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
      >
        {loading ? "Creating account..." : "Create account"}
      </button>

      {captchaRequired && (
        <div className="pt-2">
          <ReCaptcha ref={captchaRef} onChange={onCaptchaChange} />
          {errors.captcha && <p className="text-red-500 text-sm mt-1">{errors.captcha}</p>}
        </div>
      )}
    </form>
  );
}

function OtpVerifyStep({
  title,
  description,
  onVerify,
  onResend,
  loading,
  otpData,
  showResendCaptcha,
  resendCaptchaRef,
  onResendCaptchaChange,
}) {
  const [code, setCode] = useState("");
  const [countdown, setCountdown] = useState(0);

  useEffect(() => {
    if (otpData?.expiresOnUtc) {
      const expiresAt = new Date(otpData.expiresOnUtc).getTime();
      const remaining = Math.max(0, Math.floor((expiresAt - Date.now()) / 1000));
      setCountdown(remaining);
    }
  }, [otpData]);

  useEffect(() => {
    if (countdown <= 0) return;
    const timer = setInterval(() => {
      setCountdown((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
    return () => clearInterval(timer);
  }, [countdown]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (code.length === 6) onVerify(code);
  };

  const handleResend = async () => {
    const result = await onResend();
    if (result?.expiresOnUtc) {
      const expiresAt = new Date(result.expiresOnUtc).getTime();
      setCountdown(Math.max(0, Math.floor((expiresAt - Date.now()) / 1000)));
    }
    setCode("");
  };

  const formatTime = (s) => `${Math.floor(s / 60)}:${String(s % 60).padStart(2, "0")}`;

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <h2 className="text-2xl font-medium text-center mb-2">{title}</h2>
      <p className="text-gray-500 text-center text-sm mb-6">{description}</p>

      {otpData?.isSimulation && otpData?.code && (
        <div className="bg-yellow-50 border border-yellow-200 rounded-md p-3 text-center">
          <span className="text-sm text-yellow-700">Dev mode OTP: </span>
          <span className="font-mono font-bold text-yellow-900">{otpData.code}</span>
        </div>
      )}

      <div className="flex justify-center">
        <input
          type="text"
          maxLength={6}
          value={code}
          onChange={(e) => setCode(e.target.value.replace(/\D/g, ""))}
          placeholder="000000"
          className="w-48 text-center text-2xl tracking-[0.5em] px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent font-mono"
        />
      </div>

      <button
        type="submit"
        disabled={loading || code.length !== 6}
        className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
      >
        {loading ? "Verifying..." : "Verify"}
      </button>

      <div className="text-center">
        {countdown > 0 ? (
          <p className="text-sm text-gray-500">
            Resend available in {formatTime(countdown)}
          </p>
        ) : (
          <>
            {showResendCaptcha && (
              <div className="flex justify-center mb-3">
                <ReCaptcha ref={resendCaptchaRef} onChange={onResendCaptchaChange} />
              </div>
            )}
            <button
              type="button"
              onClick={handleResend}
              disabled={loading}
              className="text-sm text-blue-600 hover:underline disabled:opacity-50"
            >
              Resend code
            </button>
          </>
        )}
      </div>
    </form>
  );
}

function PhoneStep({
  onSubmit,
  onVerify,
  onResend,
  loading,
  otpData,
  showResendCaptcha,
  resendCaptchaRef,
  onResendCaptchaChange,
}) {
  const [phoneNumber, setPhoneNumber] = useState("");
  const [phoneSent, setPhoneSent] = useState(false);
  const [phoneError, setPhoneError] = useState("");

  const handleSendOtp = async (e) => {
    e.preventDefault();
    setPhoneError("");
    if (!/^\+?[1-9]\d{6,14}$/.test(phoneNumber)) {
      setPhoneError("Enter a valid phone number (e.g. +84912345678)");
      return;
    }
    const result = await onSubmit(phoneNumber);
    if (result) setPhoneSent(true);
  };

  if (!phoneSent) {
    return (
      <form onSubmit={handleSendOtp} className="space-y-4">
        <h2 className="text-2xl font-medium text-center mb-2">Verify your phone</h2>
        <p className="text-gray-500 text-center text-sm mb-6">
          We'll send a verification code via SMS.
        </p>
        <div>
          <input
            type="tel"
            placeholder="+84912345678"
            value={phoneNumber}
            onChange={(e) => {
              setPhoneNumber(e.target.value);
              if (phoneError) setPhoneError("");
            }}
            className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {phoneError && <p className="text-red-500 text-sm mt-1">{phoneError}</p>}
        </div>
        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
        >
          {loading ? "Sending..." : "Send verification code"}
        </button>
      </form>
    );
  }

  return (
    <OtpVerifyStep
      title="Enter phone verification code"
      description={`We sent a 6-digit code to ${phoneNumber}`}
      onVerify={onVerify}
      onResend={onResend}
      loading={loading}
      otpData={otpData}
      showResendCaptcha={showResendCaptcha}
      resendCaptchaRef={resendCaptchaRef}
      onResendCaptchaChange={onResendCaptchaChange}
    />
  );
}

function BusinessInfoStep({ onSubmit, loading }) {
  const [form, setForm] = useState({
    businessName: "",
    street: "",
    city: "",
    state: "",
    zipCode: "",
    country: "",
  });
  const [errors, setErrors] = useState({});

  const validate = () => {
    const errs = {};
    if (!form.businessName) errs.businessName = "Business name is required";
    if (!form.street) errs.street = "Street address is required";
    if (!form.city) errs.city = "City is required";
    if (!form.state) errs.state = "State is required";
    if (!form.zipCode) errs.zipCode = "Zip code is required";
    if (!form.country) errs.country = "Country is required";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validate()) onSubmit(form);
  };

  const handleChange = (field) => (e) => {
    setForm((prev) => ({ ...prev, [field]: e.target.value }));
    if (errors[field]) setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <h2 className="text-2xl font-medium text-center mb-2">Business information</h2>
      <p className="text-gray-500 text-center text-sm mb-6">
        Tell us about your business to start selling.
      </p>
      <div>
        <input
          type="text"
          placeholder="Business name"
          value={form.businessName}
          onChange={handleChange("businessName")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.businessName && <p className="text-red-500 text-sm mt-1">{errors.businessName}</p>}
      </div>
      <div>
        <input
          type="text"
          placeholder="Street address"
          value={form.street}
          onChange={handleChange("street")}
          className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
        {errors.street && <p className="text-red-500 text-sm mt-1">{errors.street}</p>}
      </div>
      <div className="grid grid-cols-2 gap-3">
        <div>
          <input
            type="text"
            placeholder="City"
            value={form.city}
            onChange={handleChange("city")}
            className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {errors.city && <p className="text-red-500 text-sm mt-1">{errors.city}</p>}
        </div>
        <div>
          <input
            type="text"
            placeholder="State / Province"
            value={form.state}
            onChange={handleChange("state")}
            className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {errors.state && <p className="text-red-500 text-sm mt-1">{errors.state}</p>}
        </div>
      </div>
      <div className="grid grid-cols-2 gap-3">
        <div>
          <input
            type="text"
            placeholder="Zip code"
            value={form.zipCode}
            onChange={handleChange("zipCode")}
            className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {errors.zipCode && <p className="text-red-500 text-sm mt-1">{errors.zipCode}</p>}
        </div>
        <div>
          <input
            type="text"
            placeholder="Country"
            value={form.country}
            onChange={handleChange("country")}
            className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
          {errors.country && <p className="text-red-500 text-sm mt-1">{errors.country}</p>}
        </div>
      </div>
      <button
        type="submit"
        disabled={loading}
        className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
      >
        {loading ? "Submitting..." : "Complete registration"}
      </button>
    </form>
  );
}

function SuccessStep() {
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => navigate("/"), 3000);
    return () => clearTimeout(timer);
  }, [navigate]);

  return (
    <div className="text-center space-y-4 py-8">
      <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto">
        <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
        </svg>
      </div>
      <h2 className="text-2xl font-medium">Registration complete!</h2>
      <p className="text-gray-500">Your seller account is set up. Redirecting to dashboard...</p>
      <button
        onClick={() => navigate("/")}
        className="text-blue-600 hover:underline text-sm"
      >
        Go to dashboard now
      </button>
    </div>
  );
}

export default function RegisterPage() {
  const navigate = useNavigate();
  const [currentStep, setCurrentStep] = useState(0);
  const [loading, setLoading] = useState(false);
  const [registeredEmail, setRegisteredEmail] = useState("");
  const [emailOtpData, setEmailOtpData] = useState(null);
  const [phoneOtpData, setPhoneOtpData] = useState(null);
  const registerCaptcha = useCaptcha();
  const resendOtpCaptcha = useCaptcha();

  const isRegisterCaptchaRequired = registerCaptcha.shouldRequireCaptcha(
    REGISTER_CAPTCHA_ACTION
  );
  const isResendOtpCaptchaRequired = resendOtpCaptcha.shouldRequireCaptcha(
    RESEND_OTP_CAPTCHA_ACTION
  );

  useEffect(() => {
    if (isAuthenticated() && currentStep === 0) {
      navigate("/", { replace: true });
    }
  }, [navigate, currentStep]);

  const handleRegister = async (data) => {
    if (isRegisterCaptchaRequired && !registerCaptcha.token) {
      Notice({
        msg: "CAPTCHA required",
        desc: "Please complete CAPTCHA before creating account.",
        isSuccess: false,
        place: "bottomRight",
      });
      return;
    }

    try {
      setLoading(true);
      const payload = registerCaptcha.withCaptchaPayload(
        data,
        REGISTER_CAPTCHA_ACTION
      );
      const res = await IdentityService.register(payload);
      const tokens = res?.data ?? res;
      if (tokens?.accessToken && tokens?.refreshToken) {
        registerCaptcha.resetCaptcha();
        persistAuthSession(tokens.accessToken, tokens.refreshToken);
        setRegisteredEmail(data.email);
        setCurrentStep(1);
      }
    } catch (err) {
      registerCaptcha.resetCaptcha();
      const msg =
        err?.response?.data?.detail ||
        err?.response?.data?.message ||
        "Registration failed";
      Notice({ msg: "Registration failed", desc: msg, isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const handleVerifyEmail = async (code) => {
    try {
      setLoading(true);
      await IdentityService.verifyEmail({ email: registeredEmail, code });
      Notice({ msg: "Email verified!", desc: "Your email has been verified.", isSuccess: true, place: "bottomRight" });
      setCurrentStep(2);
    } catch (err) {
      Notice({ msg: "Verification failed", desc: "Invalid or expired code.", isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const handleResendEmailOtp = useCallback(async () => {
    if (isResendOtpCaptchaRequired && !resendOtpCaptcha.token) {
      Notice({
        msg: "CAPTCHA required",
        desc: "Please complete CAPTCHA before requesting another code.",
        isSuccess: false,
        place: "bottomRight",
      });
      return null;
    }

    try {
      const payload = resendOtpCaptcha.withCaptchaPayload({
        email: registeredEmail,
        type: OTP_TYPES.VERIFY_EMAIL,
      }, RESEND_OTP_CAPTCHA_ACTION);

      const res = await IdentityService.requestOtp(payload);
      resendOtpCaptcha.resetCaptcha();
      setEmailOtpData(res);
      Notice({ msg: "Code resent", desc: "A new code has been sent to your email.", isSuccess: true, place: "bottomRight" });
      return res;
    } catch {
      resendOtpCaptcha.resetCaptcha();
      Notice({ msg: "Failed to resend", desc: "Could not resend code.", isSuccess: false, place: "bottomRight" });
    }
  }, [isResendOtpCaptchaRequired, registeredEmail, resendOtpCaptcha]);

  const handleSetPhone = async (phoneNumber) => {
    try {
      setLoading(true);
      const res = await IdentityService.setPhoneNumber({ phoneNumber });
      const data = res?.data ?? res;
      setPhoneOtpData(data);
      Notice({ msg: "Code sent", desc: `Verification code sent to ${phoneNumber}`, isSuccess: true, place: "bottomRight" });
      return data;
    } catch (err) {
      const msg = err?.response?.data?.detail || "Failed to send SMS";
      Notice({ msg: "Failed", desc: msg, isSuccess: false, place: "bottomRight" });
      return null;
    } finally {
      setLoading(false);
    }
  };

  const handleVerifyPhone = async (code) => {
    try {
      setLoading(true);
      await IdentityService.verifyPhone({ code });
      Notice({ msg: "Phone verified!", desc: "Your phone has been verified.", isSuccess: true, place: "bottomRight" });
      setCurrentStep(3);
    } catch (err) {
      Notice({ msg: "Verification failed", desc: "Invalid or expired code.", isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const handleResendPhoneOtp = useCallback(async () => {
    if (isResendOtpCaptchaRequired && !resendOtpCaptcha.token) {
      Notice({
        msg: "CAPTCHA required",
        desc: "Please complete CAPTCHA before requesting another SMS.",
        isSuccess: false,
        place: "bottomRight",
      });
      return null;
    }

    try {
      const payload = resendOtpCaptcha.withCaptchaPayload({
        email: registeredEmail,
        type: OTP_TYPES.VERIFY_PHONE,
      }, RESEND_OTP_CAPTCHA_ACTION);

      const res = await IdentityService.requestOtp(payload);
      resendOtpCaptcha.resetCaptcha();
      setPhoneOtpData(res);
      Notice({ msg: "Code resent", desc: "A new SMS code has been sent.", isSuccess: true, place: "bottomRight" });
      return res;
    } catch {
      resendOtpCaptcha.resetCaptcha();
      Notice({ msg: "Failed to resend", desc: "Could not resend SMS.", isSuccess: false, place: "bottomRight" });
    }
  }, [isResendOtpCaptchaRequired, registeredEmail, resendOtpCaptcha]);

  const handleSubmitBusiness = async (data) => {
    try {
      setLoading(true);
      await IdentityService.submitBusiness(data);
      Notice({ msg: "Business verified!", desc: "Your business info has been submitted.", isSuccess: true, place: "bottomRight" });
      setCurrentStep(4);
    } catch (err) {
      const msg = err?.response?.data?.detail || "Submission failed";
      Notice({ msg: "Failed", desc: msg, isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
    }
  };

  const handleSkip = () => {
    if (currentStep === 2) setCurrentStep(3);
    else if (currentStep === 3) setCurrentStep(4);
  };

  const renderStep = () => {
    switch (currentStep) {
      case 0:
        return (
          <BasicInfoStep
            onSubmit={handleRegister}
            loading={loading}
            captchaRequired={isRegisterCaptchaRequired}
            captchaToken={registerCaptcha.token}
            captchaRef={registerCaptcha.captchaRef}
            onCaptchaChange={registerCaptcha.onChange}
          />
        );
      case 1:
        return (
          <OtpVerifyStep
            title="Verify your email"
            description={`We sent a 6-digit code to ${registeredEmail}`}
            onVerify={handleVerifyEmail}
            onResend={handleResendEmailOtp}
            loading={loading}
            otpData={emailOtpData}
            showResendCaptcha={isResendOtpCaptchaRequired}
            resendCaptchaRef={resendOtpCaptcha.captchaRef}
            onResendCaptchaChange={resendOtpCaptcha.onChange}
          />
        );
      case 2:
        return (
          <PhoneStep
            onSubmit={handleSetPhone}
            onVerify={handleVerifyPhone}
            onResend={handleResendPhoneOtp}
            loading={loading}
            otpData={phoneOtpData}
            showResendCaptcha={isResendOtpCaptchaRequired}
            resendCaptchaRef={resendOtpCaptcha.captchaRef}
            onResendCaptchaChange={resendOtpCaptcha.onChange}
          />
        );
      case 3:
        return <BusinessInfoStep onSubmit={handleSubmitBusiness} loading={loading} />;
      case 4:
        return <SuccessStep />;
      default:
        return null;
    }
  };

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
            onClick={() => navigate("/login")}
          >
            Already have an account? Sign in
          </button>
        </div>
      </header>

      <main className="max-w-md mx-auto px-6 py-10">
        {currentStep < 4 && <StepIndicator currentStep={currentStep} />}

        {renderStep()}

        {(currentStep === 2 || currentStep === 3) && (
          <div className="text-center mt-4">
            <button
              type="button"
              onClick={handleSkip}
              className="text-sm text-gray-500 hover:text-gray-700 hover:underline"
            >
              Skip for now
            </button>
          </div>
        )}
      </main>
    </div>
  );
}
