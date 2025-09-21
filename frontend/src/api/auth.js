// src/api/auth.js
// API functions for authentication (Login & Logout)
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

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
    return handleApiError(error);
  }
};
// Refresh token
export const refreshToken = async (refreshToken) => {
  try {
    // Include both tokens as required by the backend API
    const accessToken = localStorage.getItem('token');
    const payload = {
      accessToken: accessToken || '',
      refreshToken
    };
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/refresh-token`, payload);
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return handleApiError(error);
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
        error: {
          title: 'Login Failed',
          detail: res.error?.message || 'Login failed',
          errors: null
        }
      };
    }
  } catch (error) {
    return handleApiError(error);
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
    return handleApiError(error);
  }
};


export const forgotPassword = async (email) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/forgot-password`, { email });
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return handleApiError(error);
  }
};

// Reset Password with Token
export const resetPasswordWithToken = async (email, token, newPassword) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/reset-password`, { 
      email, 
      token, 
      newPassword 
    });

export const verifyResetCode = async (email, resetCode) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/verify-reset-code`, { email, resetCode });
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return handleApiError(error);
  }
};

export const resetPasswordWithCode = async (email, resetCode, newPassword) => {
  try {
    const response = await axiosInstance.post(`${AUTH_ENDPOINT}/reset-password`, { email, resetCode, newPassword });
    return {
      data: response.data,
      status: response.status,
      error: null
    };
  } catch (error) {
    return handleApiError(error);
  }
};

