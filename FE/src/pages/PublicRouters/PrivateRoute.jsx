import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { isAuthenticated } from "../../utils/auth";

const PrivateRoute = ({ children }) => {
  const location = useLocation();

  if (!isAuthenticated()) {
    const target = `${location.pathname}${location.search}${location.hash}`;
    const shouldAppendRedirect = target && target !== "/";
    const redirectPath = shouldAppendRedirect
      ? `/login?redirect=${encodeURIComponent(target)}`
      : "/login";

    return <Navigate to={redirectPath} replace />;
  }

  return children;
};

export default PrivateRoute;
