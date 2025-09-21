import { lazy } from 'react';

// project imports
import Loadable from 'components/Loadable';

// auth pages
const LoginPage = Loadable(lazy(() => import('pages/auth/Login')));
const RegisterPage = Loadable(lazy(() => import('pages/auth/Register')));
const ForgotPasswordPage = Loadable(lazy(() => import('pages/auth/ForgotPassword')));
const CheckMailPage = Loadable(lazy(() => import('pages/auth/CheckMail')));

// ==============================|| AUTH ROUTING ||============================== //

const AuthRoutes = [
  {
    path: '/login',
    element: <LoginPage />
  },
  {
    path: '/register',
    element: <RegisterPage />
  },
  {
    path: '/forgot-password',
    element: <ForgotPasswordPage />
  },
  {
    path: '/auth/check-mail',
    element: <CheckMailPage />
  }
];

export default AuthRoutes;
