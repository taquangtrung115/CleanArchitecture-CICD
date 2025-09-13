import api from './axiosClient';

export const getPermissions = async () => {
  const res = await api.get('/api/carter/v1/permissions');
  return res.data;
};
