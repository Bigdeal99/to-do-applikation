import React, { createContext, useState, useEffect, useContext } from 'react';
import api from '../api/axios';

export const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const login = async (username, password) => {
    try {
      const res = await api.post('/Auth/login', { username, password });
      localStorage.setItem('token', res.data.token);
      localStorage.setItem('refreshToken', res.data.refreshToken);
      setUser({ username: res.data.username });
      return { success: true };
    } catch (err) {
      const errorMessage = err.response?.data?.message || 'Login failed';
      if (errorMessage.includes('locked')) {
        return { 
          success: false, 
          message: errorMessage,
          isLocked: true
        };
      }
      return { success: false, message: errorMessage };
    }
  };

  const register = async (username, password) => {
    try {
      const res = await api.post('/Auth/register', { 
        username, 
        password,
        confirmPassword: password 
      });
      return { success: true };
    } catch (err) {
      const errorData = err.response?.data;
      if (errorData?.errors) {
        return { 
          success: false, 
          message: 'Registration failed',
          errors: errorData.errors
        };
      }
      return { 
        success: false, 
        message: errorData?.message || 'Registration failed' 
      };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    setUser(null);
  };

  const refreshToken = async () => {
    const token = localStorage.getItem('refreshToken');
    if (!token) {
      logout();
      return false;
    }

    try {
      const res = await api.post('/Auth/refresh-token', { refreshToken: token });
      localStorage.setItem('token', res.data.token);
      localStorage.setItem('refreshToken', res.data.refreshToken);
      return true;
    } catch (err) {
      logout();
      return false;
    }
  };

  const checkAuth = async () => {
    const token = localStorage.getItem('token');
    if (!token) {
      setIsLoading(false);
      return;
    }

    try {
      await api.get('/Auth/verify');
      const username = localStorage.getItem('username');
      setUser({ username });
    } catch (err) {
      const refreshed = await refreshToken();
      if (!refreshed) {
        logout();
      }
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ 
      user, 
      login, 
      register, 
      logout, 
      refreshToken,
      isLoading 
    }}>
      {children}
    </AuthContext.Provider>
  );
};
