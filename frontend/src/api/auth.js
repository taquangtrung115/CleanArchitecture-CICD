// src/api/auth.js
// API functions for authentication (Login & Logout)
import axiosInstance from './axios';

const AUTH_ENDPOINT = '/api/v1/auth';
// Đăng ký tài khoản mới
export const register = async ({ userName, email, password, firstName, lastName, dayOfBirth }) => {
  try {
    const payload = { userName, email, password, firstName, lastName, dayOfBirth };
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/register`, payload);
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};
// Refresh token
export const refreshToken = async (refreshToken) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/refresh-token`, { refreshToken });
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const login = async (userName, password) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/login`, { userName, password });
    const res = response.data;
    if (res.isSuccess && res.value) {
      return {
        data: {
          accessToken: res.value.accessToken,
          refreshToken: res.value.refreshToken,
          refreshTokenExpiryTime: res.value.refreshTokenExpiryTime
        },
        status: response.status,
        error: null
      };
    } else {
      return {
        data: null,
        status: response.status,
        error: res.error?.message || 'Login failed'
      };
    }
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const logout = async () => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/logout`);
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};
