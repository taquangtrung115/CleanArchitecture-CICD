import api from './axiosClient';

export const getExample = async () => {
  const response = await api.get('/api/example');
  return response.data;
};

// Thêm các hàm g?i API khác ? ?ây
