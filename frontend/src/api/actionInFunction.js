// src/api/actionInFunction.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const ACTION_IN_FUNCTION_ENDPOINT = '/api/v1/action-in-functions';

// Tạo mới ActionInFunction
export const createActionInFunction = async ({ actionId, functionId }) => {
  try {
    const response = await axiosInstance.post(ACTION_IN_FUNCTION_ENDPOINT, { actionId, functionId });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy danh sách ActionInFunction (có phân trang)
export const getActionInFunctions = async (page = 1, pageSize = 20, searchTerm = null) => {
  try {
    const params = { page, pageSize };
    if (searchTerm) params.searchTerm = searchTerm;

    const response = await axiosInstance.get(ACTION_IN_FUNCTION_ENDPOINT, { params });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Lấy chi tiết ActionInFunction
export const getActionInFunctionDetail = async (actionId, functionId) => {
  try {
    const response = await axiosInstance.get(`${ACTION_IN_FUNCTION_ENDPOINT}/${actionId}/${functionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// Xóa ActionInFunction
export const deleteActionInFunction = async (actionId, functionId) => {
  try {
    const response = await axiosInstance.delete(`${ACTION_IN_FUNCTION_ENDPOINT}/${actionId}/${functionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};
