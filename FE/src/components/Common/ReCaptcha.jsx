import ReCAPTCHA from "react-google-recaptcha";
import { forwardRef } from "react";

const ReCaptcha = forwardRef(({ onChange, className, size = "normal" }, ref) => {
  return (
    <div className={className}>
      <ReCAPTCHA
        ref={ref}
        sitekey={process.env.REACT_APP_RECAPTCHA_SITE_KEY}
        onChange={onChange}
        size={size}
      />
    </div>
  );
});

export default ReCaptcha;