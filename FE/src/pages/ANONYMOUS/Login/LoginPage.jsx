import React, { useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

import LogoEbay from "../../../assets/images/ebay_logo.png";
import AuthService from "../../../services/AuthService";
import STORAGE, { setStorage } from "../../../lib/storage";
import Notice from "../../../components/Common/CustomNotification";
import { isAuthenticated, persistAuthSession } from "../../../utils/auth";

const { login } = AuthService;

export default function LoginPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const redirectTarget = useMemo(() => {
    const params = new URLSearchParams(location.search);
    const redirect = params.get("redirect");
    if (!redirect) {
      return "/";
    }
    return redirect.startsWith("/") ? redirect : "/";
  }, [location.search]);
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    if (isAuthenticated()) {
      navigate(redirectTarget, { replace: true });
    }
  }, [navigate, redirectTarget]);
  const onLogin = async () => {
    try {
      setLoading(true);
      const values = { username: email, password };
      const res = await login(values);
      const data = res?.data;
      if (res.status === 200 && data?.accessToken && data?.refreshToken) {
        setStorage(STORAGE.REMEMBER_LOGIN, staySignedIn);
        persistAuthSession(data.accessToken, data.refreshToken);
        navigate(redirectTarget, { replace: true });
      } else {
        Notice({
          msg: "Unable to log in",
          desc: res?.object || "Invalid credentials",
          isSuccess: false,
          place: "bottomRight",
        });
      }
    } catch (err) {
      console.error("Login error:", err);
      Notice({
        msg: "Login failed",
        desc: err?.messages?.[0] || err?.message || "Please check your credentials and try again.",
        isSuccess: false,
      });
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!email || !password || loading) {
      return;
    }
    await onLogin();
  };

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [staySignedIn, setStaySignedIn] = useState(true);
  const handleSocialLogin = async (provider) => {
    if (provider === 'Google') {
      const apiBaseUrl = "https://propval.io.vn";
      const callbackUrl = `${window.location.origin}/auth/callback?redirect=${encodeURIComponent(redirectTarget)}`;
      window.location.href = `${apiBaseUrl}/api/identity/google/login?returnUrl=${encodeURIComponent(callbackUrl)}`;
    }
  };

  return (
    <>
      <div className="min-h-screen bg-white">
        {/* Header */}
        <header className="border-b border-gray-200 px-6 py-4">
          <div className="flex items-center justify-between max-w-7xl mx-auto">
            <svg className="w-48 h-20" viewBox="0 0 200 80" fill="none">
              <image href={LogoEbay} width="200" height="80" />
            </svg>
            <button
              type="button"
              className="text-sm text-gray-600 hover:text-blue-600"
              onClick={() => window.open("https://www.ebay.com/help", "_blank", "noopener")}
            >
              Tell us what you think
            </button>
          </div>
        </header>

        {/* Main Content */}
        <main className="max-w-md mx-auto px-6 py-12">
          <div className="text-center mb-8">
            <h1 className="text-3xl font-medium mb-6">Sign in to your account</h1>

            <div className="flex items-center justify-center gap-3 mb-8">
              <span className="text-gray-700">New to eBay?</span>
              <button
                type="button"
                onClick={() => navigate("/register")}
                className="px-6 py-2 border-2 border-gray-900 rounded-full text-sm font-medium hover:bg-gray-50 transition"
              >
                Create account
              </button>
            </div>
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            <input
              type="text"
              name="username"
              placeholder="Email or username"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-blue-600 text-white py-3 rounded-full font-medium transition disabled:opacity-60 disabled:cursor-not-allowed hover:bg-blue-700"
            >
              {loading ? "Signing in..." : "Sign in"}
            </button>
          </form>

          <div className="flex items-center my-6">
            <div className="flex-1 border-t border-gray-300"></div>
            <span className="px-4 text-sm text-gray-500">or</span>
            <div className="flex-1 border-t border-gray-300"></div>
          </div>

          <div className="space-y-3">
            <button
              onClick={() => handleSocialLogin('Google')}
              disabled={loading}
              className="w-full flex items-center justify-center gap-3 px-4 py-3 border-2 border-gray-300 rounded-full hover:bg-gray-50 transition disabled:opacity-60 disabled:cursor-not-allowed"
            >
              <svg className="w-5 h-5" viewBox="0 0 24 24">
                <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" />
                <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" />
                <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" />
                <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" />
              </svg>
              <span className="font-medium text-gray-700">Continue with Google</span>
            </button>

            <button
              onClick={() => handleSocialLogin('Facebook')}
              className="w-full flex items-center justify-center gap-3 px-4 py-3 border-2 border-gray-300 rounded-full hover:bg-gray-50 transition"
            >
              <svg className="w-5 h-5" fill="#1877F2" viewBox="0 0 24 24">
                <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
              </svg>
              <span className="font-medium text-gray-700">Continue with Facebook</span>
            </button>

            <button
              onClick={() => handleSocialLogin('Apple')}
              className="w-full flex items-center justify-center gap-3 px-4 py-3 border-2 border-gray-300 rounded-full hover:bg-gray-50 transition"
            >
              <svg className="w-5 h-5" fill="#000000" viewBox="0 0 24 24">
                <path d="M17.05 20.28c-.98.95-2.05.8-3.08.35-1.09-.46-2.09-.48-3.24 0-1.44.62-2.2.44-3.06-.35C2.79 15.25 3.51 7.59 9.05 7.31c1.35.07 2.29.74 3.08.8 1.18-.24 2.31-.93 3.57-.84 1.51.12 2.65.72 3.4 1.8-3.12 1.87-2.38 5.98.48 7.13-.57 1.5-1.31 2.99-2.54 4.09l.01-.01zM12.03 7.25c-.15-2.23 1.66-4.07 3.74-4.25.29 2.58-2.34 4.5-3.74 4.25z" />
              </svg>
              <span className="font-medium text-gray-700">Continue with Apple</span>
            </button>
          </div>

          <div className="flex items-center justify-center gap-2 mt-6">
            <input
              type="checkbox"
              id="staySignedIn"
              checked={staySignedIn}
              onChange={(e) => setStaySignedIn(e.target.checked)}
              className="w-4 h-4 accent-blue-600"
            />
            <label htmlFor="staySignedIn" className="text-sm text-gray-700">
              Stay signed in
            </label>
          </div>
        </main>
      </div>
    </>
  );
}
