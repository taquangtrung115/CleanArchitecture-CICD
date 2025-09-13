import api from './axiosClient';

export const getRoles = async () => {
  const res = await api.get('/api/carter/v1/roles');
  return res.data;
};
