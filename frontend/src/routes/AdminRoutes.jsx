import { lazy } from 'react';

// project imports
import Loadable from 'components/Loadable';
import DashboardLayout from 'layout/Dashboard';

// admin pages
const DashboardDefault = Loadable(lazy(() => import('pages/dashboard/default')));
const Typography = Loadable(lazy(() => import('pages/component-overview/typography')));
const Color = Loadable(lazy(() => import('pages/component-overview/color')));
const Shadow = Loadable(lazy(() => import('pages/component-overview/shadows')));
const SamplePage = Loadable(lazy(() => import('pages/extra-pages/sample-page')));
const UserPage = Loadable(lazy(() => import('pages/user/UserPage')));
const RolePage = Loadable(lazy(() => import('pages/role/RolePage')));
const PermissionPage = Loadable(lazy(() => import('pages/permission/PermissionPage')));
const ChatPage = Loadable(lazy(() => import('pages/chat/ChatPage')));
const RidersPage = Loadable(lazy(() => import('pages/RidersPage')));
const TeamsPage = Loadable(lazy(() => import('pages/TeamsPage')));
const ProductPage = Loadable(lazy(() => import('pages/product/ProductPage')));
const VideoManagementPage = Loadable(lazy(() => import('pages/VideoManagementPage')));
const NewsManagementPage = Loadable(lazy(() => import('pages/news/NewsManagementPage')));

// ==============================|| ADMIN ROUTING ||============================== //

const AdminRoutes = [
  {
    path: '',
    element: <DashboardLayout />,
    children: [
      {
        path: '',
        element: <DashboardDefault />
      },
      {
        path: 'dashboard',
        element: <DashboardDefault />
      },
      {
        path: 'typography',
        element: <Typography />
      },
      {
        path: 'color',
        element: <Color />
      },
      {
        path: 'shadow',
        element: <Shadow />
      },
      {
        path: 'sample-page',
        element: <SamplePage />
      },
      {
        path: 'riders',
        element: <RidersPage />
      },
      {
        path: 'teams',
        element: <TeamsPage />
      },
      {
        path: 'users',
        element: <UserPage />
      },
      {
        path: 'roles',
        element: <RolePage />
      },
      {
        path: 'permissions',
        element: <PermissionPage />
      },
      {
        path: 'chat',
        element: <ChatPage />
      },
      {
        path: 'products',
        element: <ProductPage />
      },
      {
        path: 'videos',
        element: <VideoManagementPage />
      },
      {
        path: 'news',
        element: <NewsManagementPage />
      }
    ]
  }
];

export default AdminRoutes;
