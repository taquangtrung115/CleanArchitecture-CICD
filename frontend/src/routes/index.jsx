import { createBrowserRouter } from 'react-router-dom';
import ProtectedRoute from './ProtectedRoute';
import ClientRoutes from './ClientRoutes';
import AdminRoutes from './AdminRoutes';
import AuthRoutes from './AuthRoutes';
import { lazy } from 'react';
import Loadable from 'components/Loadable';

// 404 Error Page
const NotFound = Loadable(lazy(() => import('pages/error/NotFound')));

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
    },
    // 404 catch-all route (must be last)
    {
      path: '*',
      element: <NotFound />
    }
  ],
  { basename: '/' }
);

export default router;
