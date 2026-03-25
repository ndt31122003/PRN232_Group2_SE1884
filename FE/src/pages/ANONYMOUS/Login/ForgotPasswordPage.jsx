import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import LogoEbay from "../../../assets/images/ebay_logo.png";
import IdentityService from "../../../services/IdentityService";
import Notice from "../../../components/Common/CustomNotification";

export default function ForgotPasswordPage() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [email, setEmail] = useState("");
  const [code, setCode] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({});

  const validateEmail = () => {
    const errs = {};
    if (!email) errs.email = "Email is required";
    else if (!/\S+@\S+\.\S+/.test(email)) errs.email = "Invalid email format";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const validateReset = () => {
    const errs = {};
    if (!code) errs.code = "OTP code is required";
    else if (code.length !== 6) errs.code = "OTP must be 6 digits";
    if (!newPassword) errs.newPassword = "New password is required";
    else if (newPassword.length < 8)
      errs.newPassword = "Min 8 characters";
    else if (!/[A-Z]/.test(newPassword))
      errs.newPassword = "Must contain uppercase";
    else if (!/[a-z]/.test(newPassword))
      errs.newPassword = "Must contain lowercase";
    else if (!/[0-9]/.test(newPassword))
      errs.newPassword = "Must contain a number";
    else if (!/[^a-zA-Z0-9]/.test(newPassword))
      errs.newPassword = "Must contain a special character";
    if (newPassword !== confirmPassword) errs.confirmPassword = "Passwords do not match";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSendOtp = async (e) => {
    e.preventDefault();
    if (!validateEmail()) return;
    try {
      setLoading(true);
      await IdentityService.forgotPassword(email);
      Notice({
        msg: "OTP sent!",
        desc: `We sent a verification code to ${email}`,
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

  const handleResetPassword = async (e) => {
    e.preventDefault();
    if (!validateReset()) return;
    try {
      setLoading(true);
      await IdentityService.resetPassword({
        email,
        code,
        newPassword,
      });
      Notice({
        msg: "Password reset successful!",
        desc: "You can now login with your new password.",
        isSuccess: true,
        place: "bottomRight",
      });
      navigate("/login");
    } catch (err) {
      const msg =
        err?.response?.data?.detail ||
        err?.response?.data?.title ||
        "Failed to reset password";
      Notice({ msg: "Error", desc: msg, isSuccess: false, place: "bottomRight" });
    } finally {
      setLoading(false);
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
            Back to sign in
          </button>
        </div>
      </header>

      <main className="max-w-md mx-auto px-6 py-10">
        {step === 1 ? (
          <form onSubmit={handleSendOtp} className="space-y-4">
            <h2 className="text-2xl font-medium text-center mb-2">
              Reset your password
            </h2>
            <p className="text-gray-500 text-center text-sm mb-6">
              Enter your email address and we'll send you a verification code.
            </p>
            <div>
              <input
                type="email"
                placeholder="Email address"
                value={email}
                onChange={(e) => {
                  setEmail(e.target.value);
                  if (errors.email) setErrors((prev) => ({ ...prev, email: undefined }));
                }}
                className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
              {errors.email && (
                <p className="text-red-500 text-sm mt-1">{errors.email}</p>
              )}
            </div>
            <button
              type="submit"
              disabled={loading}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
            >
              {loading ? "Sending..." : "Send verification code"}
            </button>
          </form>
        ) : (
          <form onSubmit={handleResetPassword} className="space-y-4">
            <h2 className="text-2xl font-medium text-center mb-2">
              Enter verification code
            </h2>
            <p className="text-gray-500 text-center text-sm mb-6">
              We sent a 6-digit code to <strong>{email}</strong>
            </p>
            <div>
              <input
                type="text"
                maxLength={6}
                placeholder="000000"
                value={code}
                onChange={(e) => {
                  setCode(e.target.value.replace(/\D/g, ""));
                  if (errors.code) setErrors((prev) => ({ ...prev, code: undefined }));
                }}
                className="w-full text-center text-2xl tracking-[0.5em] px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent font-mono"
              />
              {errors.code && (
                <p className="text-red-500 text-sm mt-1">{errors.code}</p>
              )}
            </div>
            <div>
              <input
                type="password"
                placeholder="New password"
                value={newPassword}
                onChange={(e) => {
                  setNewPassword(e.target.value);
                  if (errors.newPassword)
                    setErrors((prev) => ({ ...prev, newPassword: undefined }));
                }}
                className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
              {errors.newPassword && (
                <p className="text-red-500 text-sm mt-1">{errors.newPassword}</p>
              )}
            </div>
            <div>
              <input
                type="password"
                placeholder="Confirm new password"
                value={confirmPassword}
                onChange={(e) => {
                  setConfirmPassword(e.target.value);
                  if (errors.confirmPassword)
                    setErrors((prev) => ({ ...prev, confirmPassword: undefined }));
                }}
                className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
              {errors.confirmPassword && (
                <p className="text-red-500 text-sm mt-1">{errors.confirmPassword}</p>
              )}
            </div>
            <button
              type="submit"
              disabled={loading || code.length !== 6}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
            >
              {loading ? "Resetting..." : "Reset password"}
            </button>
            <div className="text-center">
              <button
                type="button"
                onClick={() => setStep(1)}
                className="text-sm text-blue-600 hover:underline"
              >
                Use a different email
              </button>
            </div>
          </form>
        )}
      </main>
    </div>
  );
}
