// src/api/userChat.js
import axiosInstance from './axios';

const USER_CHAT_ENDPOINT = '/api/v1/user-chat';

// Message operations
export const sendMessage = async (senderId, receiverId, roomId, content, type = 'Text') => {
  try {
    const response = await axiosInstance.post(`${USER_CHAT_ENDPOINT}/messages`, {
      senderId,
      receiverId,
      roomId,
      content,
      type
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

export const getChatHistory = async (userId, otherUserId = null, roomId = null, page = 1, pageSize = 50) => {
  try {
    const params = new URLSearchParams({ 
      userId: userId.toString(), 
      page: page.toString(), 
      pageSize: pageSize.toString() 
    });
    if (otherUserId) params.append('otherUserId', otherUserId.toString());
    if (roomId) params.append('roomId', roomId.toString());
    
    const response = await axiosInstance.get(`${USER_CHAT_ENDPOINT}/messages/history?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const markMessageAsRead = async (messageId, userId) => {
  try {
    const response = await axiosInstance.patch(`${USER_CHAT_ENDPOINT}/messages/${messageId}/read?userId=${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const getUnreadCount = async (userId) => {
  try {
    const response = await axiosInstance.get(`${USER_CHAT_ENDPOINT}/messages/unread-count?userId=${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// Room operations
export const createChatRoom = async (name, description = null, type = 'Group', memberIds = null) => {
  try {
    const response = await axiosInstance.post(`${USER_CHAT_ENDPOINT}/rooms`, {
      name,
      description,
      type,
      memberIds
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

export const getChatRooms = async (userId, page = 1, pageSize = 20) => {
  try {
    const params = new URLSearchParams({ 
      userId: userId.toString(), 
      page: page.toString(), 
      pageSize: pageSize.toString() 
    });
    const response = await axiosInstance.get(`${USER_CHAT_ENDPOINT}/rooms?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const joinChatRoom = async (roomId, userId) => {
  try {
    const response = await axiosInstance.post(`${USER_CHAT_ENDPOINT}/rooms/${roomId}/join?userId=${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const leaveChatRoom = async (roomId, userId) => {
  try {
    const response = await axiosInstance.post(`${USER_CHAT_ENDPOINT}/rooms/${roomId}/leave?userId=${userId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

export const getRoomMembers = async (roomId, page = 1, pageSize = 50) => {
  try {
    const params = new URLSearchParams({ 
      page: page.toString(), 
      pageSize: pageSize.toString() 
    });
    const response = await axiosInstance.get(`${USER_CHAT_ENDPOINT}/rooms/${roomId}/members?${params.toString()}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};

// User operations
export const getOnlineUsers = async (currentUserId) => {
  try {
    const response = await axiosInstance.get(`${USER_CHAT_ENDPOINT}/users/online?currentUserId=${currentUserId}`);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return {
      data: null,
      status: error.response ? error.response.status : 500,
      error: error.response ? error.response.data : error.message
    };
  }
};