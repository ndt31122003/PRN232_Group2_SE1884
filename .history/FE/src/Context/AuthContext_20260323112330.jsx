import React, { createContext, useContext, useEffect, useState } from 'react'
import AuthService from '../services/AuthService'
import STORAGE, { deleteStorage, getStorage, setStorage } from '../lib/storage'
import Notice from '../components/Common/CustomNotification'

const AuthContext = createContext()
export const useAuth = () => useContext(AuthContext);

const { login: loginRequest } = AuthService;

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [token, setToken] = useState("")
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  useEffect(() => {
    const storedUser = getStorage(STORAGE.USER_INFO) || localStorage.getItem(STORAGE.USER_INFO);
    const storedToken = getStorage(STORAGE.TOKEN) || localStorage.getItem(STORAGE.TOKEN);
    if (storedUser && storedToken) {
      setUser(typeof storedUser === 'string' ? JSON.parse(storedUser) : storedUser)
      setToken(storedToken)
      setIsAuthenticated(true)
    }
  }, [])


  const loginUser = async (userData) => {
    const res = await loginRequest(userData);
    const data = res?.data;

    if (res?.status === 200 && data?.accessToken && data?.refreshToken) {
      setStorage(STORAGE.TOKEN, data.accessToken);
      setStorage(STORAGE.REFRESH_TOKEN, data.refreshToken);
      setToken(data.accessToken);
      setUser(userData);
      setIsAuthenticated(true);
      window.location.href = "/";
      return res;
    }

    Notice({
      msg: "Unable to log in",
      desc: res?.object || "Invalid credentials",
      isSuccess: false,
      place: "bottomRight",
    });

    return res;
  }



  const logoutUser = () => {
    setUser(null);
    setToken(null);
    setIsAuthenticated(false);
    deleteStorage(STORAGE.TOKEN);
    deleteStorage(STORAGE.REFRESH_TOKEN);
    deleteStorage(STORAGE.USER_INFO);
    localStorage.removeItem(STORAGE.USER_INFO);
    localStorage.removeItem(STORAGE.TOKEN);
    window.location.href = "/";

  }

  return (
    <AuthContext.Provider value={{ user, token, isAuthenticated, loginUser, logoutUser }}>
      {children}
    </AuthContext.Provider>
  )
}

