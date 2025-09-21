// src/api/position.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const POSITION_ENDPOINT = '/api/v1/positions';

// 1. Create position
export const createPosition = async (payload) => {
  try {
    const response = await axiosInstance.post(POSITION_ENDPOINT, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 2. Get all positions with pagination and search
export const getPositions = async (page = 1, pageSize = 10, searchTerm = '') => {
  try {
    const params = new URLSearchParams({ page, pageSize });
    if (searchTerm) params.append('searchTerm', searchTerm);
    const response = await axiosInstance.get(`${POSITION_ENDPOINT}?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 3. Get active positions (for dropdowns)
export const getActivePositions = async () => {
  try {
    const response = await axiosInstance.get(`${POSITION_ENDPOINT}/active`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 4. Get position by ID
export const getPositionById = async (positionId) => {
  try {
    const response = await axiosInstance.get(`${POSITION_ENDPOINT}/${positionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 5. Update position
export const updatePosition = async (positionId, payload) => {
  try {
    const response = await axiosInstance.put(`${POSITION_ENDPOINT}/${positionId}`, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// 6. Delete position
export const deletePosition = async (positionId) => {
  try {
    const response = await axiosInstance.delete(`${POSITION_ENDPOINT}/${positionId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};