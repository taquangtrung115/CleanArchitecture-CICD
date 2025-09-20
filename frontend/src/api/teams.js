import axiosInstance from './axios';
import { handleApiError } from '../utils/errorHandler';

const TEAMS_ENDPOINT = '/api/v1/motogp/teams';

export const getTeams = async (params = {}, token = null) => {
  try {
    const response = await axiosInstance.get(TEAMS_ENDPOINT, { params });
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const getTeamById = async (id, token = null) => {
  try {
    const response = await axiosInstance.get(`${TEAMS_ENDPOINT}/${id}`);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const getTeamWithRiders = async (id, token = null) => {
  try {
    const response = await axiosInstance.get(`${TEAMS_ENDPOINT}/${id}/with-riders`);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const createTeam = async (data, token = null) => {
  try {
    const response = await axiosInstance.post(TEAMS_ENDPOINT, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};

export const updateTeam = async (id, data, token = null) => {
  try {
    const response = await axiosInstance.put(`${TEAMS_ENDPOINT}/${id}`, data);
    return response;
  } catch (error) {
    throw error; // Maintain backward compatibility by throwing
  }
};
