import { useRef, useState } from "react";

const CAPTCHA_ATTEMPTS_STORAGE_KEY = "captcha_failed_attempts_v1";

const CAPTCHA_REQUIREMENT = {
  NONE: "none",
  ALWAYS: "always",
  CONDITIONAL: "conditional",
};

const CAPTCHA_RULES = {
  "identity.login": {
    requirement: CAPTCHA_REQUIREMENT.CONDITIONAL,
    threshold: 3,
  },
  "identity.register": {
    requirement: CAPTCHA_REQUIREMENT.ALWAYS,
    threshold: 0,
  },
  "identity.forgot-password": {
    requirement: CAPTCHA_REQUIREMENT.ALWAYS,
    threshold: 0,
  },
  "identity.reset-password": {
    requirement: CAPTCHA_REQUIREMENT.ALWAYS,
    threshold: 0,
  },
  "identity.resend-otp": {
    requirement: CAPTCHA_REQUIREMENT.ALWAYS,
    threshold: 0,
  },
};

const getRule = (actionName) =>
  CAPTCHA_RULES[actionName] || {
    requirement: CAPTCHA_REQUIREMENT.NONE,
    threshold: 0,
  };

const loadFailedAttempts = () => {
  try {
    const raw = window.localStorage.getItem(CAPTCHA_ATTEMPTS_STORAGE_KEY);
    if (!raw) {
      return {};
    }

    const parsed = JSON.parse(raw);
    return typeof parsed === "object" && parsed !== null ? parsed : {};
  } catch {
    return {};
  }
};

const persistFailedAttempts = (attemptsByAction) => {
  try {
    window.localStorage.setItem(
      CAPTCHA_ATTEMPTS_STORAGE_KEY,
      JSON.stringify(attemptsByAction)
    );
  } catch {
    // Ignore storage write issues and keep hook operational.
  }
};

export const useCaptcha = () => {
  const captchaRef = useRef(null);
  const [token, setToken] = useState(null);
  const [failedAttemptsByAction, setFailedAttemptsByAction] = useState(() =>
    loadFailedAttempts()
  );

  const onChange = (value) => {
    setToken(value);
  };

  const getFailedAttempts = (actionName) => failedAttemptsByAction[actionName] || 0;

  const shouldRequireCaptcha = (actionName) => {
    const rule = getRule(actionName);

    if (rule.requirement === CAPTCHA_REQUIREMENT.ALWAYS) {
      return true;
    }

    if (rule.requirement === CAPTCHA_REQUIREMENT.CONDITIONAL) {
      return getFailedAttempts(actionName) >= rule.threshold;
    }

    return false;
  };

  const registerFailure = (actionName) => {
    const rule = getRule(actionName);
    if (rule.requirement !== CAPTCHA_REQUIREMENT.CONDITIONAL) {
      return;
    }

    setFailedAttemptsByAction((current) => {
      const next = {
        ...current,
        [actionName]: (current[actionName] || 0) + 1,
      };

      persistFailedAttempts(next);
      return next;
    });
  };

  const registerSuccess = (actionName) => {
    const rule = getRule(actionName);
    if (rule.requirement !== CAPTCHA_REQUIREMENT.CONDITIONAL) {
      return;
    }

    setFailedAttemptsByAction((current) => {
      const next = {
        ...current,
        [actionName]: 0,
      };

      persistFailedAttempts(next);
      return next;
    });
  };

  const withCaptchaPayload = (payload, actionName) => {
    if (!shouldRequireCaptcha(actionName)) {
      return payload;
    }

    return {
      ...payload,
      captchaToken: token,
      captchaAction: actionName,
    };
  };

  const resetCaptcha = () => {
    captchaRef.current?.reset();
    setToken(null);
  };

  return {
    captchaRef,
    token,
    onChange,
    shouldRequireCaptcha,
    getFailedAttempts,
    registerFailure,
    registerSuccess,
    withCaptchaPayload,
    resetCaptcha,
  };
};