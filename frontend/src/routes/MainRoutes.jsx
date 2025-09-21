import { lazy } from 'react';

// project imports
import Loadable from 'components/Loadable';
import DashboardLayout from 'layout/Dashboard';

// render- Dashboard
const DashboardDefault = Loadable(lazy(() => import('pages/dashboard/default')));

// render - color
const Color = Loadable(lazy(() => import('pages/component-overview/color')));
const Typography = Loadable(lazy(() => import('pages/component-overview/typography')));
const Shadow = Loadable(lazy(() => import('pages/component-overview/shadows')));

// render - sample page
const SamplePage = Loadable(lazy(() => import('pages/extra-pages/sample-page')));
const UserPage = Loadable(lazy(() => import('pages/user/UserPage')));
const RolePage = Loadable(lazy(() => import('pages/role/RolePage')));
const PermissionPage = Loadable(lazy(() => import('pages/permission/PermissionPage')));
const ActionPage = Loadable(lazy(() => import('pages/action/ActionPage')));
const ChatPage = Loadable(lazy(() => import('pages/chat/ChatPage')));
const UserChatPage = Loadable(lazy(() => import('pages/chat/UserChatPage')));
const ProfileViewPage = Loadable(lazy(() => import('pages/profile/ProfileViewPage')));

// ==============================|| MAIN ROUTING ||============================== //

import ClientApp from '../client/index';
import RidersPage from 'pages/RidersPage';
import TeamsPage from 'pages/TeamsPage';

const MainRoutes = [
  {
    path: '/*',
    element: <ClientApp />
  },
  {
    path: '/admin',
    element: <DashboardLayout />,
    children: [
      {
        path: '',
        element: <DashboardDefault />
      },
      {
        path: 'dashboard',
        children: [
          {
            path: 'default',
            element: <DashboardDefault />
          }
        ]
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
        path: 'actions',
        element: <ActionPage />
      },
      {
        path: 'chat',
        element: <ChatPage />
      },
      {
        path: 'user-chat',
        element: <UserChatPage />
      },
      {
        path: 'profile',
        element: <ProfileViewPage />
      }
    ]
  }
];

export default MainRoutes;
