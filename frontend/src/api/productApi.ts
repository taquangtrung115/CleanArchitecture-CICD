import api from './axiosClient';

export const getProducts = async () => {
  const res = await api.get('/api/carter/v1/products');

  ///api/carter/v1/products
  return res.data;
};

export const createProduct = async (data: { name: string; price: number; description: string }) => {
  const res = await api.post('/api/carter/v1/products', data);
  return res.data;
};

// Có thể bổ sung các hàm updateProduct, deleteProduct nếu cần
