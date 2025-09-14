// src/api/axios.js
// Tạo instance axios dùng chung cho toàn bộ project
import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/',
    timeout: 60000, // 30 giây
    headers: {
        'Content-Type': 'application/json',
    },
});


// Interceptor: Tự động thêm token vào header nếu có, trừ login/register
axiosInstance.interceptors.request.use(
    (config) => {
        // Không thêm Authorization cho login/register
        const isAuthApi =
            config.url?.includes('/auth/login') ||
            config.url?.includes('/auth/register');
        if (!isAuthApi) {
            const token = localStorage.getItem('token');
            if (token) {
                config.headers['Authorization'] = `Bearer ${token}`;
            }
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Interceptor: Xử lý lỗi trả về
axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
        // Xử lý lỗi 401 (Unauthorized)
        if (error.response && error.response.status === 401) {
            // Có thể redirect về trang login hoặc thông báo lỗi
            // window.location.href = '/login';
            // alert('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
        }
        // Xử lý lỗi khác nếu cần
        return Promise.reject(error);
    }
);


// Helper methods để tái sử dụng dễ dàng ở các file api khác
export const apiGet = (url, config) => axiosInstance.get(url, config);
export const apiPost = (url, data, config) => axiosInstance.post(url, data, config);
export const apiPut = (url, data, config) => axiosInstance.put(url, data, config);
export const apiDelete = (url, config) => axiosInstance.delete(url, config);
export const apiPatch = (url, data, config) => axiosInstance.patch(url, data, config);

export default axiosInstance;
