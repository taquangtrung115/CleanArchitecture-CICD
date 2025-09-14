// src/routes/ProtectedRoute.jsx
// Route bảo vệ: chỉ cho phép truy cập nếu đã đăng nhập
import { Navigate, Outlet } from 'react-router-dom';

export default function ProtectedRoute() {
    const token = localStorage.getItem('token');
    return token ? <Outlet /> : <Navigate to="/login" replace />;
}
