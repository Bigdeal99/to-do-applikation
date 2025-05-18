import React, { createContext, useState, useEffect } from 'react';
import api from '../api/axios';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = async (username, password) => {
    try {
      const res = await api.post('/Auth/login', { username, password });
      localStorage.setItem('token', res.data.token);
      localStorage.setItem('refreshToken', res.data.refreshToken);
      setUser({ username: res.data.username });
      return { success: true };
    } catch (err) {
      return { success: false, message: err.response?.data?.message || 'Login failed' };
    }
  };

  const register = async (username, password) => {
    try {
      await api.post('/Auth/register', { username, password });
      return { success: true };
    } catch (err) {
      return { success: false, message: err.response?.data?.message || 'Register failed' };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    setUser(null);
  };

  const refreshToken = async () => {
    const token = localStorage.getItem('refreshToken');
    if (!token) return logout();

    try {
      const res = await api.post('/Auth/refresh-token', { refreshToken: token });
      localStorage.setItem('token', res.data.token);
      localStorage.setItem('refreshToken', res.data.refreshToken);
    } catch (err) {
      logout();
    }
  };

  
  useEffect(() => {
    const token = localStorage.getItem('token');
    const username = token ? 'user' : null; 
    if (token && username) {
      setUser({ username });
    }
  }, []);

  return (
    <AuthContext.Provider value={{ user, login, register, logout, refreshToken }}>
      {children}
    </AuthContext.Provider>
  );
};
