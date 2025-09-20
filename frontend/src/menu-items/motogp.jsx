// assets
import { TrophyOutlined, TeamOutlined, VideoCameraOutlined, FileTextOutlined } from '@ant-design/icons';

const icons = {
  TrophyOutlined,
  TeamOutlined,
  VideoCameraOutlined,
  FileTextOutlined
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
    },
    {
      id: 'admin-videos',
      title: 'Manage Videos',
      type: 'item',
      url: '/admin/videos',
      icon: icons.VideoCameraOutlined
    },
    {
      id: 'admin-news',
      title: 'Manage News',
      type: 'item',
      url: '/admin/news',
      icon: icons.FileTextOutlined
    }
  ]
};

export default motogp;
