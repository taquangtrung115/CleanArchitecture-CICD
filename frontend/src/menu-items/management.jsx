// assets
import {
  UserOutlined,
  TeamOutlined,
  SafetyOutlined,
  RobotOutlined,
  IdcardOutlined,
  MessageOutlined,
  TrophyOutlined,
  SettingOutlined,
  LinkOutlined
} from '@ant-design/icons';

const icons = {
  UserOutlined,
  TeamOutlined,
  SafetyOutlined,
  RobotOutlined,
  IdcardOutlined,
  MessageOutlined,
  TrophyOutlined,
  SettingOutlined,
  LinkOutlined
};

const management = {
  id: 'management',
  title: 'Management',
  type: 'group',
  children: [
    {
      id: 'profile',
      title: 'Profile',
      type: 'item',
      url: '/admin/profile',
      icon: icons.IdcardOutlined
    },
    {
      id: 'user-management',
      title: 'Users',
      type: 'item',
      url: '/admin/users',
      icon: icons.UserOutlined
    },
    {
      id: 'role-management',
      title: 'Roles',
      type: 'item',
      url: '/admin/roles',
      icon: icons.TeamOutlined
    },
    {
      id: 'position-management',
      title: 'Positions',
      type: 'item',
      url: '/admin/positions',
      icon: icons.TrophyOutlined
    },
    {
      id: 'permission-management',
      title: 'Permissions',
      type: 'item',
      url: '/admin/permissions',
      icon: icons.SafetyOutlined
    },
    {
      id: 'action-management',
      title: 'Actions',
      type: 'item',
      url: '/admin/actions',
      icon: icons.SettingOutlined
    },
    {
      id: 'action-in-function-management',
      title: 'Action in Functions',
      type: 'item',
      url: '/admin/action-in-functions',
      icon: icons.LinkOutlined
    },
    {
      id: 'ai-chat',
      title: 'AI Assistant',
      type: 'item',
      url: '/admin/chat',
      icon: icons.RobotOutlined
    },
    {
      id: 'user-chat',
      title: 'User Chat',
      type: 'item',
      url: '/admin/user-chat',
      icon: icons.MessageOutlined
    }
  ]
};

export default management;
