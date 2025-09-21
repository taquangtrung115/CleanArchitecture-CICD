// src/api/chat.js
// API service để tương tác với AI Chat Agent backend
// Tuân thủ convention của dự án và error handling pattern

import axiosInstance from './axios';

const CHAT_ENDPOINT = '/api/v1/chat';

/**
 * Gửi tin nhắn chat đến AI Agent
 * Hỗ trợ các lệnh quản lý role và permission bằng ngôn ngữ tự nhiên
 * @param {string} message - Tin nhắn từ người dùng (tiếng Việt hoặc tiếng Anh)
 * @param {string|null} userId - ID của người dùng (optional, để tracking)
 * @returns {Object} Response object với data, status và error
 */
export const sendChatMessage = async (message, userId = null) => {
  try {
    const response = await axiosInstance.post(`${CHAT_ENDPOINT}/message`, {
      message,
      userId
    });
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

/**
 * Lấy lịch sử chat của người dùng
 * Hỗ trợ phân trang để hiển thị tin nhắn cũ
 * @param {string|null} userId - ID của người dùng (optional)
 * @param {number} page - Số trang (mặc định = 1)
 * @param {number} pageSize - Số tin nhắn mỗi trang (mặc định = 20)
 * @returns {Object} Response object với data, status và error
 */
export const getChatHistory = async (userId = null, page = 1, pageSize = 20) => {
  try {
    const params = new URLSearchParams({ page, pageSize });
    if (userId) params.append('userId', userId);
    const response = await axiosInstance.get(`${CHAT_ENDPOINT}/history?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};
