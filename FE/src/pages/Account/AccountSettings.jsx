import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import Notice from "../../components/Common/CustomNotification";
import IdentityService from "../../services/IdentityService";
import UserService from "../../services/UserService";
import { getStoredUser, mapUserProfile, storeUserInfo } from "../../utils/auth";
import OTP_TYPES from "../../constants/otpTypes";
import "./AccountSettings.scss";

const resolveUserId = (user) => {
  if (!user) {
    return null;
  }

  const candidates = [
    user?.id,
    user?.Id,
    user?.userId,
    user?.UserId,
    user?.idValue
  ];

  for (const candidate of candidates) {
    if (candidate == null) {
      continue;
    }

    if (typeof candidate === "object") {
      const valueLike = candidate?.value ?? candidate?.Value ?? null;
      if (valueLike) {
        return valueLike;
      }
      continue;
    }

    return typeof candidate === "string" ? candidate : String(candidate);
  }

  return null;
};

const AccountSettingsPage = () => {
  const navigate = useNavigate();
  const [profile, setProfile] = useState(() => {
    const stored = getStoredUser();
    return stored ? mapUserProfile(stored, stored) : null;
  });
  const [isLoading, setIsLoading] = useState(!profile);
  const [busy, setBusy] = useState({ email: false, payment: false });
  const [paymentCard, setPaymentCard] = useState({ holderName: "", number: "", expiry: "", cvv: "" });

  useEffect(() => {
    let ignore = false;
    const stored = getStoredUser();

    const storedId = resolveUserId(stored);

    if (!storedId) {
      setIsLoading(false);
      Notice({ msg: "Không thể tải thông tin tài khoản.", isSuccess: false });
      navigate("/login", { replace: true });
      return () => {};
    }

    const loadProfile = async () => {
      setIsLoading(true);
      try {
  const result = await UserService.getById(storedId);
        if (ignore) {
          return;
        }
        const normalized = mapUserProfile(result ?? {}, stored);
        if (normalized) {
          setProfile(normalized);
          storeUserInfo(normalized, { notify: true });
        }
      } catch (error) {
        if (!ignore) {
          Notice({ msg: "Không thể tải thông tin tài khoản.", isSuccess: false });
        }
      } finally {
        if (!ignore) {
          setIsLoading(false);
        }
      }
    };

    loadProfile();

    return () => {
      ignore = true;
    };
  }, [navigate]);

  useEffect(() => {
    const handleProfileBroadcast = () => {
      const stored = getStoredUser();
      if (stored) {
        setProfile(mapUserProfile(stored, stored));
      }
    };

    if (typeof window !== "undefined") {
      window.addEventListener("user-info-updated", handleProfileBroadcast);
    }

    return () => {
      if (typeof window !== "undefined") {
        window.removeEventListener("user-info-updated", handleProfileBroadcast);
      }
    };
  }, []);

  const emailStatus = useMemo(() => (
    profile?.isEmailVerified
      ? { label: "Đã xác minh", tone: "positive" }
      : { label: "Chưa xác minh", tone: "warning" }
  ), [profile?.isEmailVerified]);

  const paymentStatus = useMemo(() => (
    profile?.isPaymentVerified
      ? { label: "Đã xác minh", tone: "positive" }
      : { label: "Chưa xác minh", tone: "warning" }
  ), [profile?.isPaymentVerified]);

  const updateProfile = (updater) => {
    setProfile((prev) => {
      const next = typeof updater === "function" ? updater(prev) : updater;
      if (next) {
        storeUserInfo(next, { notify: true });
      }
      return next;
    });
  };

  const setBusyFlag = (key, value) => {
    setBusy((prev) => ({ ...prev, [key]: value }));
  };

  const sanitizeCardNumber = (value) => value.replace(/[^0-9]/g, "");

  const formatCardNumber = (value) => {
    const digits = sanitizeCardNumber(value).slice(0, 19);
    return digits.replace(/(.{4})/g, "$1 ").trim();
  };

  const formatExpiryInput = (value) => {
    const digits = value.replace(/[^0-9]/g, "").slice(0, 6);
    if (digits.length <= 2) {
      return digits;
    }
    if (digits.length <= 4) {
      return `${digits.slice(0, 2)}/${digits.slice(2)}`;
    }
    return `${digits.slice(0, 2)}/${digits.slice(2, 6)}`;
  };

  const extractExpiryParts = (value) => {
    const digits = value.replace(/[^0-9]/g, "").slice(0, 6);
    const month = digits.slice(0, 2);
    const year = digits.slice(2);
    return { month, year };
  };

  const normalizeCardPayload = (card, contextLabel) => {
    const holder = card.holderName?.trim() ?? "";
    if (!holder) {
      Notice({ msg: `Vui lòng nhập tên chủ thẻ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    const digits = sanitizeCardNumber(card.number);
    if (digits.length < 12 || digits.length > 19) {
      Notice({ msg: `Số thẻ không hợp lệ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    const expiry = extractExpiryParts(card.expiry);
    if (!expiry.month || !expiry.year) {
      Notice({ msg: `Vui lòng nhập thời hạn thẻ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    if (expiry.year.length < 2) {
      Notice({ msg: `Năm hết hạn không hợp lệ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    const monthValue = Number(expiry.month);
    if (!Number.isInteger(monthValue) || monthValue < 1 || monthValue > 12) {
      Notice({ msg: `Tháng hết hạn không hợp lệ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    const cvvDigits = card.cvv.replace(/[^0-9]/g, "");
    if (cvvDigits.length < 3 || cvvDigits.length > 4) {
      Notice({ msg: `CVV không hợp lệ (${contextLabel}).`, isSuccess: false });
      return null;
    }

    return {
      cardHolderName: holder,
      cardNumber: digits,
      expiryMonth: expiry.month.padStart(2, "0"),
      expiryYear: expiry.year,
      cvv: cvvDigits
    };
  };

  const handlePaymentCardChange = (field) => (event) => {
    const { value } = event.target;
    setPaymentCard((prev) => {
      if (field === "number") {
        return { ...prev, number: formatCardNumber(value) };
      }
      if (field === "expiry") {
        return { ...prev, expiry: formatExpiryInput(value) };
      }
      if (field === "cvv") {
        return { ...prev, cvv: value.replace(/[^0-9]/g, "").slice(0, 4) };
      }
      return { ...prev, [field]: value };
    });
  };

  const isCardFilled = (card) => {
    const digits = sanitizeCardNumber(card.number);
    return Boolean(
      card.holderName?.trim()
      && digits.length >= 12
      && card.expiry?.trim().length >= 4
      && card.cvv?.trim().length >= 3
    );
  };

  const paymentCardReady = isCardFilled(paymentCard);

  const handleVerifyEmail = async () => {
    if (!profile?.id || profile.isEmailVerified || !profile.email) {
      return;
    }

    setBusyFlag("email", true);
    try {
      const delivery = await IdentityService.requestOtp({
        email: profile.email,
        type: OTP_TYPES.VERIFY_EMAIL
      });

      const issuedCode = delivery?.code ?? "";

      if (!issuedCode) {
        Notice({ msg: "Không thể gửi mã xác minh email.", isSuccess: false });
        return;
      }

      const expiresAtIso = delivery?.expiresOnUtc ?? delivery?.ExpiresOnUtc;
      const expiresLabel = expiresAtIso ? new Date(expiresAtIso).toLocaleTimeString() : null;

      if (delivery?.isSimulation) {
        const descParts = [`Sử dụng mã ${issuedCode}`];
        if (expiresLabel) {
          descParts.push(`Hết hạn lúc ${expiresLabel}`);
        }

        Notice({
          msg: "Mã xác minh email (mô phỏng)",
          desc: `${descParts.join(". ")}.`,
          isSuccess: true
        });
      }

      const enteredCode = window.prompt(
        "Nhập mã xác minh email đã nhận được:",
        issuedCode
      );

      const sanitizedCode = enteredCode?.trim();
      if (!sanitizedCode) {
        return;
      }

      await IdentityService.verifyEmail({
        email: profile.email,
        code: sanitizedCode
      });

      updateProfile((prev) => (prev ? { ...prev, isEmailVerified: true } : prev));
      Notice({
        msg: "Email đã được xác minh.",
        desc: expiresLabel ? `Đã xác thực trước thời điểm hết hạn ${expiresLabel}.` : undefined,
        isSuccess: true
      });
    } catch (error) {
      Notice({ msg: "Không thể xác minh email.", isSuccess: false });
    } finally {
      setBusyFlag("email", false);
    }
  };

  const handleVerifyPayment = async () => {
    if (!profile?.id || profile.isPaymentVerified || !profile.email) {
      return;
    }

    if (!profile.isEmailVerified) {
      Notice({ msg: "Vui lòng xác minh email trước.", isSuccess: false });
      return;
    }

    const normalizedCard = normalizeCardPayload(paymentCard, "xác minh thanh toán");
    if (!normalizedCard) {
      return;
    }

    setBusyFlag("payment", true);
    try {
      const delivery = await IdentityService.requestOtp({
        email: profile.email,
        type: OTP_TYPES.VERIFY_PAYMENT
      });

      const issuedCode = delivery?.code ?? "";
      if (!issuedCode) {
        Notice({ msg: "Không thể gửi mã xác minh thanh toán.", isSuccess: false });
        return;
      }

      const expiresAtIso = delivery?.expiresOnUtc ?? delivery?.ExpiresOnUtc;
      const expiresLabel = expiresAtIso ? new Date(expiresAtIso).toLocaleTimeString() : null;

      if (delivery?.isSimulation) {
        const descParts = [`Nhập mã ${issuedCode} để hoàn tất xác minh`];
        if (expiresLabel) {
          descParts.push(`Mã hết hạn lúc ${expiresLabel}`);
        }

        Notice({
          msg: "Mã xác minh thanh toán (mô phỏng)",
          desc: `${descParts.join(". ")}.`,
          isSuccess: true
        });
      }

      const enteredCode = window.prompt(
        "Nhập mã xác minh thanh toán mà bạn nhận được:",
        issuedCode
      );

      const sanitizedCode = enteredCode?.trim();
      if (!sanitizedCode) {
        return;
      }

      const receipt = await UserService.verifyPayment({
        ...normalizedCard,
        code: sanitizedCode
      });

      updateProfile((prev) => (prev ? { ...prev, isPaymentVerified: true } : prev));

      const reference = receipt?.providerReference ?? receipt?.ProviderReference;
      const verifiedOnIso = receipt?.verifiedOnUtc ?? receipt?.VerifiedOnUtc;
      const verifiedLabel = verifiedOnIso ? new Date(verifiedOnIso).toLocaleTimeString() : null;
      const brand = receipt?.cardBrand ?? receipt?.CardBrand;
      const masked = receipt?.maskedCardNumber ?? receipt?.MaskedCardNumber;
      const last4 = receipt?.cardLast4 ?? receipt?.CardLast4;
      const expMonth = receipt?.expiryMonth ?? receipt?.ExpiryMonth;
      const expYear = receipt?.expiryYear ?? receipt?.ExpiryYear;

      const descParts = [];
      if (reference) {
        descParts.push(`Mã tham chiếu giao dịch: ${reference}`);
      }
      if (verifiedLabel) {
        descParts.push(`Xác minh hoàn tất lúc ${verifiedLabel}`);
      }
      if (masked || last4 || brand) {
        const cardTokens = [];
        if (brand) {
          cardTokens.push(brand);
        }
        if (masked) {
          cardTokens.push(masked);
        } else if (last4) {
          cardTokens.push(`•••• ${last4}`);
        }
        if (cardTokens.length > 0) {
          descParts.push(`Thẻ sử dụng: ${cardTokens.join(" ")}`);
        }
      }
      if (expMonth && expYear) {
        const monthLabel = String(expMonth).padStart(2, "0");
        descParts.push(`Hiệu lực đến ${monthLabel}/${expYear}`);
      }

      Notice({
        msg: "Phương thức thanh toán đã được xác minh.",
        desc: descParts.length ? `${descParts.join(". ")}.` : undefined,
        isSuccess: true
      });

      setPaymentCard((prev) => ({ ...prev, cvv: "" }));
    } catch (error) {
      Notice({ msg: "Không thể xác minh phương thức thanh toán.", isSuccess: false });
    } finally {
      setBusyFlag("payment", false);
    }
  };

  return (
    <section className="account-settings">
      <div className="account-settings__container">
        <header className="account-settings__header">
          <h1 className="account-settings__title">Account settings</h1>
          <p className="account-settings__subtitle">
            Hoàn tất các bước xác minh để bắt đầu đăng bán và nhận thanh toán.
          </p>
        </header>

        <div className="account-settings__card">
          <div className="account-settings__card-body">
            <div className="account-settings__section">
              <h2 className="account-settings__section-title">Các bước xác minh</h2>
              <p className="account-settings__section-desc">
                Thực hiện lần lượt từng bước để mở khóa toàn bộ tính năng tài khoản.
              </p>
            </div>

            <div className="account-settings__verifications">
              <div className="account-settings__verification-item">
                <div className="account-settings__verification-heading">
                  <h3 className="account-settings__verification-title">Xác minh email</h3>
                  <div className={`account-settings__status account-settings__status--${emailStatus.tone}`}>
                    <span className="account-settings__status-indicator" aria-hidden="true" />
                    <span>{emailStatus.label}</span>
                  </div>
                </div>
                <p className="account-settings__verification-desc">
                  Xác nhận địa chỉ email để đảm bảo bạn nhận được thông báo và cập nhật quan trọng.
                </p>
                <div className="account-settings__verification-actions">
                  <button
                    type="button"
                    className="account-settings__primary-btn"
                    onClick={handleVerifyEmail}
                    disabled={isLoading || busy.email || profile?.isEmailVerified}
                  >
                    {profile?.isEmailVerified ? "Đã xác minh"
                      : busy.email ? "Đang xác minh..."
                        : "Xác minh email"}
                  </button>
                </div>
              </div>

              <div className="account-settings__verification-item">
                <div className="account-settings__verification-heading">
                  <h3 className="account-settings__verification-title">Xác minh thanh toán</h3>
                  <div className={`account-settings__status account-settings__status--${paymentStatus.tone}`}>
                    <span className="account-settings__status-indicator" aria-hidden="true" />
                    <span>{paymentStatus.label}</span>
                  </div>
                </div>
                <p className="account-settings__verification-desc">
                  Liên kết và xác nhận phương thức thanh toán để eBay có thể chuyển tiền cho bạn.
                </p>
                <div className="account-settings__card-form">
                  <div className="account-settings__field">
                    <label htmlFor="payment-card-holder">Tên chủ thẻ</label>
                    <input
                      id="payment-card-holder"
                      type="text"
                      value={paymentCard.holderName}
                      onChange={handlePaymentCardChange("holderName")}
                      disabled={isLoading || busy.payment || profile?.isPaymentVerified}
                      placeholder="Ví dụ: Nguyen Van A"
                      autoComplete="cc-name"
                    />
                  </div>
                  <div className="account-settings__field">
                    <label htmlFor="payment-card-number">Số thẻ</label>
                    <input
                      id="payment-card-number"
                      type="text"
                      inputMode="numeric"
                      value={paymentCard.number}
                      onChange={handlePaymentCardChange("number")}
                      disabled={isLoading || busy.payment || profile?.isPaymentVerified}
                      placeholder="1234 5678 9012 3456"
                      autoComplete="cc-number"
                    />
                  </div>
                  <div className="account-settings__field-row">
                    <div className="account-settings__field account-settings__field--half">
                      <label htmlFor="payment-card-expiry">Hết hạn</label>
                      <input
                        id="payment-card-expiry"
                        type="text"
                        inputMode="numeric"
                        value={paymentCard.expiry}
                        onChange={handlePaymentCardChange("expiry")}
                        disabled={isLoading || busy.payment || profile?.isPaymentVerified}
                        placeholder="MM/YY"
                        autoComplete="cc-exp"
                      />
                    </div>
                    <div className="account-settings__field account-settings__field--half">
                      <label htmlFor="payment-card-cvv">CVV</label>
                      <input
                        id="payment-card-cvv"
                        type="password"
                        inputMode="numeric"
                        value={paymentCard.cvv}
                        onChange={handlePaymentCardChange("cvv")}
                        disabled={isLoading || busy.payment || profile?.isPaymentVerified}
                        placeholder="3-4 số"
                        autoComplete="cc-csc"
                      />
                    </div>
                  </div>
                  {!paymentCardReady && !profile?.isPaymentVerified && (
                    <p className="account-settings__helper-text account-settings__helper-text--muted">
                      Điền đầy đủ thông tin thẻ để tiếp tục xác minh.
                    </p>
                  )}
                </div>
                <div className="account-settings__verification-actions">
                  <button
                    type="button"
                    className="account-settings__primary-btn"
                    onClick={handleVerifyPayment}
                    disabled={
                      isLoading
                      || busy.payment
                      || profile?.isPaymentVerified
                      || !profile?.isEmailVerified
                      || !paymentCardReady
                    }
                  >
                    {profile?.isPaymentVerified ? "Đã xác minh"
                      : busy.payment ? "Đang xác minh..."
                        : profile?.isEmailVerified ? "Xác minh thanh toán" : "Chờ xác minh email"}
                  </button>
                  {!profile?.isEmailVerified && (
                    <p className="account-settings__helper-text">Hoàn tất xác minh email trước khi tiếp tục.</p>
                  )}
                </div>
              </div>
            </div>

            <dl className="account-settings__details">
              <div className="account-settings__detail-item">
                <dt>Họ và tên</dt>
                <dd>{profile?.name ?? "--"}</dd>
              </div>
              <div className="account-settings__detail-item">
                <dt>Email</dt>
                <dd>{profile?.email ?? "--"}</dd>
              </div>
            </dl>
          </div>

          <footer className="account-settings__footer">
            <button
              type="button"
              className="account-settings__secondary-btn"
              onClick={() => navigate(-1)}
              disabled={busy.email || busy.payment}
            >
              Quay lại
            </button>
          </footer>
        </div>

        {isLoading && (
          <p className="account-settings__loading">Đang tải thông tin tài khoản...</p>
        )}
      </div>
    </section>
  );
};

export default AccountSettingsPage;
