import { createBrowserRouter } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';
import ClientRoutes from './ClientRoutes';
import AdminRoutes from './AdminRoutes';
import AuthRoutes from './AuthRoutes';

// Clean route separation: client (public) and admin (protected)
const router = createBrowserRouter(
  [
    // Auth routes (login, register)
    ...AuthRoutes,
    // Client routes (public, no login required)
    ...ClientRoutes,
    // Admin routes (protected, login required)
    {
      path: '/admin',
      element: <ProtectedRoute />,
      children: AdminRoutes
    }
  ],
  { basename: '/' }
);

export default router;
