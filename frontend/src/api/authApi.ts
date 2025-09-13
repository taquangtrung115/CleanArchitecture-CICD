import api from './axiosClient';

export const login = async (data: { userName: string; password: string }) => {
  const res = await api.post('/api/carter/v1/auth/login', data);
  return res.data;
};
