import api from './axiosClient';

export const getExample = async () => {
  const response = await api.get('/api/example');
  return response.data;
};

// Th�m c�c h�m g?i API kh�c ? ?�y
