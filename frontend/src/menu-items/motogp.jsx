// assets
import { TrophyOutlined, TeamOutlined } from '@ant-design/icons';

const icons = {
    TrophyOutlined,
    TeamOutlined
};

const motogp = {
    id: 'motogp-admin',
    title: 'MotoGP Management',
    type: 'group',
    children: [
        {
            id: 'admin-riders',
            title: 'Manage Riders',
            type: 'item',
            url: '/admin/riders',
            icon: icons.TrophyOutlined
        },
        {
            id: 'admin-teams',
            title: 'Manage Teams',
            type: 'item',
            url: '/admin/teams',
            icon: icons.TeamOutlined
        }
    ]
};

export default motogp;
