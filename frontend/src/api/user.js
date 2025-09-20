// src/api/user.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const USER_ENDPOINT = '/api/v1/users';

// 1. Tạo user mới
export const createUser = async (payload) => {
  try {
    const response = await axiosInstance.post(USER_ENDPOINT, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 2. Lấy danh sách user
export const getUsers = async (page = 1, pageSize = 20, searchTerm = '') => {
  try {
    const params = new URLSearchParams({ page, pageSize });
    if (searchTerm) params.append('searchTerm', searchTerm);
    const response = await axiosInstance.get(`${USER_ENDPOINT}?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 3. Lấy chi tiết user
export const getUserDetail = async (userId) => {
  try {
    const response = await axiosInstance.get(`${USER_ENDPOINT}/${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 3.1. Lấy profile của user hiện tại
export const getCurrentUserProfile = async () => {
  try {
    const response = await axiosInstance.get(`${USER_ENDPOINT}/profile/me`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 3.2. Cập nhật profile của user hiện tại
export const updateCurrentUserProfile = async (profileData) => {
  try {
    const response = await axiosInstance.put(`${USER_ENDPOINT}/profile/me`, profileData);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 4. Cập nhật user
export const updateUser = async (userId, payload) => {
  try {
    const response = await axiosInstance.put(`${USER_ENDPOINT}/${userId}`, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 5. Xóa user
export const deleteUser = async (userId) => {
  try {
    const response = await axiosInstance.delete(`${USER_ENDPOINT}/${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 6. Đổi mật khẩu
export const changePassword = async (userId, { currentPassword, newPassword }) => {
  try {
    const response = await axiosInstance.post(`${USER_ENDPOINT}/${userId}/change-password`, { userId, currentPassword, newPassword });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 7. Reset mật khẩu
export const resetPassword = async (userId, newPassword) => {
  try {
    const response = await axiosInstance.post(`${USER_ENDPOINT}/${userId}/reset-password`, { userId, newPassword });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 8. Khóa user
export const lockUser = async (userId) => {
  try {
    const response = await axiosInstance.post(`${USER_ENDPOINT}/${userId}/lock`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 9. Mở khóa user
export const unlockUser = async (userId) => {
  try {
    const response = await axiosInstance.post(`${USER_ENDPOINT}/${userId}/unlock`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 10. Lấy danh sách vai trò của user
export const getUserRoles = async (userId) => {
  try {
    const response = await axiosInstance.get(`${USER_ENDPOINT}/${userId}/roles`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 11. Gán user vào vai trò
export const addUserToRole = async (userId, roleId) => {
  try {
    const response = await axiosInstance.post(`${USER_ENDPOINT}/${userId}/roles/${roleId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 12. Xóa user khỏi vai trò
export const removeUserFromRole = async (userId, roleId) => {
  try {
    const response = await axiosInstance.delete(`${USER_ENDPOINT}/${userId}/roles/${roleId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};
