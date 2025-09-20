// src/utils/auth.js
// Tiện ích xử lý auth: logout, refreshToken
import { logout as logoutApi, refreshToken as refreshTokenApi } from 'api/auth';

let isLoggingOut = false;
export const logout = async () => {
  if (isLoggingOut) return;
  isLoggingOut = true;
  try {
    await logoutApi();
  } catch (error) {
    // Ignore logout API errors
    console.error('Logout API error:', error);
  }
  // Clear all authentication-related tokens
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('refreshTokenExpiryTime');
  window.location.href = '/login';
  setTimeout(() => {
    isLoggingOut = false;
  }, 2000); // reset flag phòng trường hợp reload không kịp
};

export const handleRefreshToken = async () => {
  const refreshToken = localStorage.getItem('refreshToken');
  if (!refreshToken) return null;

  try {
    const res = await refreshTokenApi(refreshToken);
    // Check if the response follows the API format with isSuccess and value
    if (res.data && res.data.isSuccess && res.data.value) {
      const { accessToken, refreshToken: newRefreshToken, refreshTokenExpiryTime } = res.data.value;
      localStorage.setItem('token', accessToken);
      if (newRefreshToken) {
        localStorage.setItem('refreshToken', newRefreshToken);
      }
      if (refreshTokenExpiryTime) {
        localStorage.setItem('refreshTokenExpiryTime', refreshTokenExpiryTime);
      }
      return accessToken;
    }
    // Fallback for direct token response (if API format is different)
    else if (res.data && res.data.accessToken) {
      localStorage.setItem('token', res.data.accessToken);
      if (res.data.refreshToken) {
        localStorage.setItem('refreshToken', res.data.refreshToken);
      }
      if (res.data.refreshTokenExpiryTime) {
        localStorage.setItem('refreshTokenExpiryTime', res.data.refreshTokenExpiryTime);
      }
      return res.data.accessToken;
    } else {
      logout();
      return null;
    }
  } catch (error) {
    console.error('Token refresh failed:', error);
    logout();
    return null;
  }
};
