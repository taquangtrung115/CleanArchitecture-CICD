import api from './axiosClient';

export const getUsers = async (params: { page?: number; pageSize?: number; searchTerm?: string }) => {
  const res = await api.get('/api/carter/v1/users', { params });
  return res.data;
};

export const getUserById = async (userId: string) => {
  const res = await api.get(`/api/carter/v1/users/${userId}`);
  return res.data;
};

export const createUser = async (data: any) => {
  const res = await api.post('/api/carter/v1/users', data);
  return res.data;
};

export const updateUser = async (userId: string, data: any) => {
  const res = await api.put(`/api/carter/v1/users/${userId}`, data);
  return res.data;
};

export const deleteUser = async (userId: string) => {
  const res = await api.delete(`/api/carter/v1/users/${userId}`);
  return res.data;
};

export const getUserRoles = async (userId: string) => {
  const res = await api.get(`/api/carter/v1/users/${userId}/roles`);
  return res.data;
};

export const assignUserToRole = async (userId: string, roleId: string) => {
  const res = await api.post(`/api/carter/v1/users/${userId}/roles/${roleId}`);
  return res.data;
};

export const removeUserFromRole = async (userId: string, roleId: string) => {
  const res = await api.delete(`/api/carter/v1/users/${userId}/roles/${roleId}`);
  return res.data;
};
