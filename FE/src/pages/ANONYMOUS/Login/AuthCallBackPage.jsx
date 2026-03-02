import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { persistAuthSession } from "../../../utils/auth";

export default function AuthCallbackPage() {
  const navigate = useNavigate();

  useEffect(() => {
    const hash = window.location.hash.substring(1); // bỏ dấu #
    const params = new URLSearchParams(hash);

    const accessToken = params.get("access_token");
    const refreshToken = params.get("refresh_token");

    if (accessToken && refreshToken) {
      persistAuthSession(accessToken, refreshToken);

      const queryParams = new URLSearchParams(window.location.search);
      const redirectTarget = queryParams.get("redirect") || "/";
      navigate(redirectTarget.startsWith("/") ? redirectTarget : "/");
    } else {
      navigate("/login");
    }
  }, [navigate]);

  return <div className="text-center mt-20">Đang đăng nhập bằng Google...</div>;
}
