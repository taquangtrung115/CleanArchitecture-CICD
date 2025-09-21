// src/api/chat.js
import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const CHAT_ENDPOINT = '/api/v1/chat';

// 1. Send chat message
export const sendChatMessage = async (message, userId = null) => {
  try {
    const response = await axiosInstance.post(`${CHAT_ENDPOINT}/message`, {
      message,
      userId
    });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};

// 2. Get chat history
export const getChatHistory = async (userId = null, page = 1, pageSize = 20) => {
  try {
    const params = new URLSearchParams({ page, pageSize });
    if (userId) params.append('userId', userId);
    const response = await axiosInstance.get(`${CHAT_ENDPOINT}/history?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};
