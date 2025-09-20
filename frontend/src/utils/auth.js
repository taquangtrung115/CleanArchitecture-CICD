// src/utils/auth.js
// Tiện ích xử lý auth: logout, refreshToken
import { logout as logoutApi, refreshToken as refreshTokenApi } from 'api/auth';

let isLoggingOut = false;
export const logout = async () => {
  if (isLoggingOut) return;
  isLoggingOut = true;
  try {
    await logoutApi();
  } catch (e) {}
  localStorage.removeItem('token');
  // Có thể xóa thêm các thông tin khác nếu cần
  window.location.href = '/login';
  setTimeout(() => {
    isLoggingOut = false;
  }, 2000); // reset flag phòng trường hợp reload không kịp
};

export const handleRefreshToken = async () => {
  const refreshToken = localStorage.getItem('refreshToken');
  if (!refreshToken) return null;
  const res = await refreshTokenApi(refreshToken);
  if (res.data && res.data.token) {
    localStorage.setItem('token', res.data.token);
    if (res.data.refreshToken) {
      localStorage.setItem('refreshToken', res.data.refreshToken);
    }
    return res.data.token;
  } else {
    logout();
    return null;
  }
};
