import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:5258';

// Create axios instance with default config
const apiClient = axios.create({
  baseURL: `${API_BASE_URL}/api/v1/motogp`,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor to include auth token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error);
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const raceService = {
  // Get all races with pagination and filtering
  getRaces: async (params = {}) => {
    const response = await apiClient.get('/races', { params });
    return response.data;
  },

  // Get race by ID
  getRaceById: async (id) => {
    const response = await apiClient.get(`/races/${id}`);
    return response.data;
  },

  // Get race with results (classification)
  getRaceWithResults: async (id) => {
    const response = await apiClient.get(`/races/${id}/results`);
    return response.data;
  },

  // Get races by season
  getRacesBySeason: async (seasonId, params = {}) => {
    const response = await apiClient.get('/races', { 
      params: { seasonId, ...params } 
    });
    return response.data;
  },

  // Get completed races
  getCompletedRaces: async (params = {}) => {
    const response = await apiClient.get('/races', { 
      params: { status: 'Completed', ...params } 
    });
    return response.data;
  },

  // Get upcoming races
  getUpcomingRaces: async (params = {}) => {
    const response = await apiClient.get('/races', { 
      params: { status: 'Upcoming', ...params } 
    });
    return response.data;
  },
};

export const riderService = {
  // Get all riders
  getRiders: async (params = {}) => {
    const response = await apiClient.get('/riders', { params });
    return response.data;
  },

  // Get rider by ID
  getRiderById: async (id) => {
    const response = await apiClient.get(`/riders/${id}`);
    return response.data;
  },
};

export const teamService = {
  // Get all teams
  getTeams: async (params = {}) => {
    const response = await apiClient.get('/teams', { params });
    return response.data;
  },

  // Get team by ID
  getTeamById: async (id) => {
    const response = await apiClient.get(`/teams/${id}`);
    return response.data;
  },

  // Get team with riders
  getTeamWithRiders: async (id) => {
    const response = await apiClient.get(`/teams/${id}/with-riders`);
    return response.data;
  },
};

export default apiClient;