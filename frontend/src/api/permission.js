// src/api/permission.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const PERMISSION_ENDPOINT = '/api/v1/permissions';

// Tạo mới permission
export const createPermission = async ({ roleId, functionId, actionId }) => {
  try {
    const response = await axiosInstance.post(PERMISSION_ENDPOINT, { roleId, functionId, actionId });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy danh sách permission (có phân trang)
export const getPermissions = async (page = 1, pageSize = 20) => {
  try {
    const response = await axiosInstance.get(`${PERMISSION_ENDPOINT}?page=${page}&pageSize=${pageSize}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy chi tiết permission
export const getPermissionDetail = async (roleId, functionId, actionId) => {
  try {
    const response = await axiosInstance.get(`${PERMISSION_ENDPOINT}/${roleId}/${functionId}/${actionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Xóa permission
export const deletePermission = async (roleId, functionId, actionId) => {
  try {
    const response = await axiosInstance.delete(`${PERMISSION_ENDPOINT}/${roleId}/${functionId}/${actionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};
