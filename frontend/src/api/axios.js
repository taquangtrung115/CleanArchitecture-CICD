// src/api/axios.js
// Tạo instance axios dùng chung cho toàn bộ project
import axios from 'axios';
import { handleRefreshToken } from '../utils/auth';

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '/',
  timeout: 60000, // 60 giây
  headers: {
    'Content-Type': 'application/json'
  }
});

// Flag to prevent multiple refresh attempts
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

// Interceptor: Tự động thêm token vào header nếu có, trừ login/register
axiosInstance.interceptors.request.use(
    (config) => {
        // Không thêm Authorization cho login/register
        const isAuthApi = config.url?.includes('/auth/login') || config.url?.includes('/auth/register');
        if (!isAuthApi) {
            const token = localStorage.getItem('token');
            if (token) {
                config.headers['Authorization'] = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Interceptor: Xử lý lỗi trả về
axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Xử lý lỗi 401 (Unauthorized)
    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      // Don't try to refresh token for auth endpoints
      const isAuthApi =
        originalRequest.url?.includes('/auth/login') ||
        originalRequest.url?.includes('/auth/register') ||
        originalRequest.url?.includes('/auth/refresh-token');

      if (isAuthApi) {
        return Promise.reject(error);
      }

      if (isRefreshing) {
        // If already refreshing, queue this request
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers['Authorization'] = 'Bearer ' + token;
            return axiosInstance(originalRequest);
          })
          .catch((err) => {
            return Promise.reject(err);
          });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const newToken = await handleRefreshToken();
        if (newToken) {
          processQueue(null, newToken);
          originalRequest.headers['Authorization'] = 'Bearer ' + newToken;
          return axiosInstance(originalRequest);
        } else {
          processQueue(error, null);
          return Promise.reject(error);
        }
      } catch (refreshError) {
        processQueue(refreshError, null);
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    // Xử lý lỗi khác nếu cần
    return Promise.reject(error);
  }
);

// Helper methods để tái sử dụng dễ dàng ở các file api khác
export const apiGet = (url, config) => axiosInstance.get(url, config);
export const apiPost = (url, data, config) => axiosInstance.post(url, data, config);
export const apiPut = (url, data, config) => axiosInstance.put(url, data, config);
export const apiDelete = (url, config) => axiosInstance.delete(url, config);
export const apiPatch = (url, data, config) => axiosInstance.patch(url, data, config);

export default axiosInstance;
