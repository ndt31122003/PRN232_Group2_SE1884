import React, { createContext, use, useContext, useEffect, useState } from 'react'
import { loginUser } from '../services/AuthService'
import STORAGE, { deleteStorage, getStorage, setStorage } from '../lib/storage'
import Notice from '../components/Common/CustomNotification'

const AuthContext = createContext()
export const useAuth = () => useContext(AuthContext);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [token, setToken] = useState("")
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  useEffect(() => {
    const storedUser = getStorage(STORAGE.USER) || localStorage.getItem('user');
    const storedToken = getStorage(STORAGE.TOKEN) || localStorage.getItem('token');
    if (storedUser && storedToken) {
      setUser(JSON.parse(storedUser))
      setToken(storedToken)
      setIsAuthenticated(true)
    }
  }, [])


  const loginUser = async (userData) => {
    var res = await loginUser(userData);
    const data = res?.data;
    if (res.status === 200 && data?.accessToken && data?.refreshToken) {
      setStorage(STORAGE.TOKEN, data.accessToken);
      setStorage(STORAGE.REFRESH_TOKEN, data.refreshToken);
      window.location.href = "/";
    } else {
      Notice({
        msg: "Unable to log in",
        desc: res?.object || "Invalid credentials",
        isSuccess: false,
        place: "bottomRight",
      });
    }
    setUser(userData);
    setIsAuthenticated(true);
  }



  const logoutUser = () => {
    setUser(null);
    setToken(null);
    setIsAuthenticated(false);
    deleteStorage(STORAGE.TOKEN);
    deleteStorage(STORAGE.REFRESH_TOKEN);
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    window.location.href = "/";

  }

  return (
    <AuthContext.Provider value={{ user, loginUser, logoutUser }}>
      {children}
    </AuthContext.Provider>
  )
}

