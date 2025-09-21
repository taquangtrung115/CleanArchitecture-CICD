// src/api/product.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const PRODUCT_ENDPOINT = '/api/v1/products';

// 1. Tạo sản phẩm mới
export const createProduct = async (payload) => {
  try {
    const response = await axiosInstance.post(PRODUCT_ENDPOINT, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 2. Lấy danh sách sản phẩm
export const getProducts = async (page = 1, pageSize = 10, searchTerm = '', sortColumn = null, sortOrder = null) => {
  try {
    const params = new URLSearchParams({
      pageIndex: page,
      pageSize: pageSize
    });
    if (searchTerm) params.append('serchTerm', searchTerm); // Note: keeping original typo from backend
    if (sortColumn) params.append('sortColumn', sortColumn);
    if (sortOrder) params.append('sortOrder', sortOrder);

    const response = await axiosInstance.get(`${PRODUCT_ENDPOINT}?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 3. Lấy chi tiết sản phẩm
export const getProductById = async (productId) => {
  try {
    const response = await axiosInstance.get(`${PRODUCT_ENDPOINT}/${productId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 4. Cập nhật sản phẩm
export const updateProduct = async (productId, payload) => {
  try {
    const response = await axiosInstance.put(`${PRODUCT_ENDPOINT}/${productId}`, payload);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 5. Xóa sản phẩm
export const deleteProduct = async (productId) => {
  try {
    const response = await axiosInstance.delete(`${PRODUCT_ENDPOINT}/${productId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};
