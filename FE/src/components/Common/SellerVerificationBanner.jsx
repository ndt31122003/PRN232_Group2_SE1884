import React from "react";
import { useNavigate } from "react-router-dom";
import { getStoredUser } from "../../utils/auth";
import { HiExclamationTriangle, HiCheckCircle, HiXCircle } from "react-icons/hi2";

const VerificationStep = ({ label, verified }) => (
  <div className="flex items-center gap-2">
    {verified ? (
      <HiCheckCircle className="text-green-500 w-5 h-5 flex-shrink-0" />
    ) : (
      <HiXCircle className="text-red-500 w-5 h-5 flex-shrink-0" />
    )}
    <span className={verified ? "text-green-700" : "text-red-700"}>{label}</span>
  </div>
);

const SellerVerificationBanner = ({ className = "" }) => {
  const navigate = useNavigate();
  const user = getStoredUser();

  if (!user || user.isSellerVerified) {
    return null;
  }

  return (
    <div className={`bg-amber-50 border border-amber-300 rounded-lg p-6 ${className}`}>
      <div className="flex items-start gap-3 mb-4">
        <HiExclamationTriangle className="text-amber-500 w-6 h-6 flex-shrink-0 mt-0.5" />
        <div>
          <h3 className="text-lg font-semibold text-amber-800">
            Seller Verification Required
          </h3>
          <p className="text-amber-700 mt-1">
            You must complete all verification steps before you can create listings or open a store.
          </p>
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 mb-5 ml-9">
        <VerificationStep label="Email Verified" verified={user.isEmailVerified} />
        <VerificationStep label="Phone Verified" verified={user.isPhoneVerified} />
        <VerificationStep label="Business Verified" verified={user.isBusinessVerified} />
      </div>

      <div className="ml-9">
        <button
          onClick={() => navigate("/register")}
          className="px-5 py-2.5 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 transition-colors"
        >
          Complete Verification
        </button>
      </div>
    </div>
  );
};

export default SellerVerificationBanner;
