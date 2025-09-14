// assets
import { UserOutlined, TeamOutlined, SafetyOutlined } from '@ant-design/icons';

const icons = {
    UserOutlined,
    TeamOutlined,
    SafetyOutlined
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
            url: '/users',
            icon: icons.UserOutlined
        },
        {
            id: 'role-management',
            title: 'Roles',
            type: 'item',
            url: '/roles',
            icon: icons.TeamOutlined
        },
        {
            id: 'permission-management',
            title: 'Permissions',
            type: 'item',
            url: '/permissions',
            icon: icons.SafetyOutlined
        }
    ]
};

export default management;
