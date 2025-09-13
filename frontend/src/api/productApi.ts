import api from './axiosClient';

export const getProducts = async () => {
  const res = await api.get('/api/v1/products');
  return res.data;
};

export const createProduct = async (data: { name: string; price: number; description: string }) => {
  const res = await api.post('/api/v1/products', data);
  return res.data;
};

// C� th? b? sung c�c h�m updateProduct, deleteProduct n?u c?n
