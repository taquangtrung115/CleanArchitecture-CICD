// src/routes/ProtectedRoute.jsx
// Route bảo vệ: chỉ cho phép truy cập nếu đã đăng nhập
import { Navigate, Outlet } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { handleRefreshToken } from '../utils/auth';

export default function ProtectedRoute() {
  const [isAuthenticated, setIsAuthenticated] = useState(null); // null = checking, true = authenticated, false = not authenticated

  useEffect(() => {
    const checkAuthentication = async () => {
      const token = localStorage.getItem('token');
      const refreshToken = localStorage.getItem('refreshToken');

      if (!token && !refreshToken) {
        setIsAuthenticated(false);
        return;
      }

      if (token) {
        // Token exists, assume valid for now (axios interceptor will handle if it's expired)
        setIsAuthenticated(true);
        return;
      }

      if (refreshToken) {
        // No access token but have refresh token, try to refresh
        try {
          const newToken = await handleRefreshToken();
          setIsAuthenticated(!!newToken);
        } catch (error) {
          console.error('Token refresh failed during route protection:', error);
          setIsAuthenticated(false);
        }
      } else {
        setIsAuthenticated(false);
      }
    };

    checkAuthentication();
  }, []);

  // Show loading or nothing while checking authentication
  if (isAuthenticated === null) {
    return <div>Loading...</div>; // You can replace this with a proper loading component
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}
