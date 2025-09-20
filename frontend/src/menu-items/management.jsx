// assets
import { UserOutlined, TeamOutlined, SafetyOutlined, RobotOutlined } from '@ant-design/icons';

const icons = {
  UserOutlined,
  TeamOutlined,
  SafetyOutlined,
  RobotOutlined
};

const management = {
  id: 'management',
  title: 'Management',
  type: 'group',
  children: [
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
      id: 'permission-management',
      title: 'Permissions',
      type: 'item',
      url: '/admin/permissions',
      icon: icons.SafetyOutlined
    },
    {
      id: 'ai-chat',
      title: 'AI Assistant',
      type: 'item',
      url: '/admin/chat',
      icon: icons.RobotOutlined
    }
  ]
};

export default management;
