import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const RIDERS_ENDPOINT = '/api/v1/motogp/riders';

export const getRiders = async (params = {}, token = null) => {
  try {
    const response = await axiosInstance.get(RIDERS_ENDPOINT, { params });
    return response;
  } catch (error) {
    const errorResult = handleApiError(error);
    throw error; // Maintain backward compatibility by throwing
  }
};

export const getRiderById = async (id, token = null) => {
  try {
    const response = await axiosInstance.get(`${RIDERS_ENDPOINT}/${id}`);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const getRiderByNumber = async (number, token = null) => {
  try {
    const response = await axiosInstance.get(`${RIDERS_ENDPOINT}/racing-number/${number}`);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const createRider = async (data, token = null) => {
  try {
    const response = await axiosInstance.post(RIDERS_ENDPOINT, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const updateRiderPersonalInfo = async (id, data, token = null) => {
  try {
    const response = await axiosInstance.put(`${RIDERS_ENDPOINT}/${id}/personal-info`, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const transferRider = async (id, data, token = null) => {
  try {
    const response = await axiosInstance.put(`${RIDERS_ENDPOINT}/${id}/transfer`, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const retireRider = async (id, data, token = null) => {
  try {
    const response = await axiosInstance.put(`${RIDERS_ENDPOINT}/${id}/retire`, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const comebackRider = async (id, token = null) => {
  try {
    const response = await axiosInstance.put(`${RIDERS_ENDPOINT}/${id}/comeback`, {});
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const deleteRider = async (id, token = null) => {
  try {
    const response = await axiosInstance.delete(`${RIDERS_ENDPOINT}/${id}`);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};
