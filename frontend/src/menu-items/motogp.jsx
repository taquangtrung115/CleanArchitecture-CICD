// assets
import { TrophyOutlined, TeamOutlined } from '@ant-design/icons';

const icons = {
    TrophyOutlined,
    TeamOutlined
};

const motogp = {
    id: 'motogp',
    title: 'MotoGP',
    type: 'group',
    children: [
        {
            id: 'motogp-riders',
            title: 'Riders',
            type: 'item',
            url: '/riders',
            icon: icons.TrophyOutlined
        },
        {
            id: 'motogp-teams',
            title: 'Teams',
            type: 'item',
            url: '/teams',
            icon: icons.TeamOutlined
        }
    ]
};

export default motogp;
