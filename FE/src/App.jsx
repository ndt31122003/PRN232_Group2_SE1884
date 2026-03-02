import React, { Suspense } from "react";
import "./App.css";
import AppRouter from "./pages/PublicRouters/AppRouter.jsx";
import { StoreProvider } from "./StoreProvider.jsx";
import { LoadingScreen } from "./components/LoadingScreen/LoadingScreen.jsx";
import { AuthProvider } from "./Context/AuthContext.jsx";

const App = () => {
  return (
    <Suspense fallback={<LoadingScreen />}>
      <AuthProvider>
        <StoreProvider>
          <AppRouter />
        </StoreProvider>
      </AuthProvider>
    </Suspense>
  );
};

export default App;
