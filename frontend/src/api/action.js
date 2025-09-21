// src/api/action.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const ACTION_ENDPOINT = '/api/v1/actions';

// Tạo mới action
export const createAction = async ({ id, name, sortOrder, isActive }) => {
  try {
    const response = await axiosInstance.post(ACTION_ENDPOINT, { id, name, sortOrder, isActive });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy danh sách action (có phân trang)
export const getActions = async (page = 1, pageSize = 20, searchTerm = null) => {
  try {
    const params = { page, pageSize };
    if (searchTerm) params.searchTerm = searchTerm;
    
    const response = await axiosInstance.get(ACTION_ENDPOINT, { params });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy danh sách action đang hoạt động
export const getActiveActions = async () => {
  try {
    const response = await axiosInstance.get(`${ACTION_ENDPOINT}/active`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy chi tiết action
export const getActionDetail = async (id) => {
  try {
    const response = await axiosInstance.get(`${ACTION_ENDPOINT}/${id}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Cập nhật action
export const updateAction = async (id, { name, sortOrder, isActive }) => {
  try {
    const response = await axiosInstance.put(`${ACTION_ENDPOINT}/${id}`, { id, name, sortOrder, isActive });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Xóa action
export const deleteAction = async (id) => {
  try {
    const response = await axiosInstance.delete(`${ACTION_ENDPOINT}/${id}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy danh sách function đang hoạt động
export const getActiveFunctions = async () => {
  try {
    const response = await axiosInstance.get('/api/v1/functions/active');
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};