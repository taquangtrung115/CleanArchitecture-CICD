

import { createBrowserRouter } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';
import MainRoutes from './MainRoutes';
import LoginRoutes from './LoginRoutes';

// Chuẩn GitHub: gom các route public/private rõ ràng

// Tách riêng /admin mới cần login, client không cần login
const routes = [
    // Public routes
    ...LoginRoutes.children[0].children.map(r => ({ ...r })),
    // Client routes (không cần login)
    ...MainRoutes.filter(r => r.path !== '/admin'),
    // Admin routes (bảo vệ bằng ProtectedRoute)
    {
        path: '/admin',
        element: <ProtectedRoute />,
        children: MainRoutes.find(r => r.path === '/admin')?.children || []
    }
];

const router = createBrowserRouter(routes, { basename: '/' });

export default router;
