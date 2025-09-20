import { lazy } from 'react';

// project imports
import Loadable from 'components/Loadable';
import ClientLayout from 'client/layout/ClientLayout';

// client pages
const HomePage = Loadable(lazy(() => import('client/pages/HomePage')));
const SeasonPage = Loadable(lazy(() => import('client/pages/season/SeasonPage')));
const RacePage = Loadable(lazy(() => import('client/pages/race/RacePage')));
const TeamPage = Loadable(lazy(() => import('client/pages/team/TeamPage')));
const TeamDetailPage = Loadable(lazy(() => import('pages/TeamDetailPage')));
const RiderPage = Loadable(lazy(() => import('client/pages/rider/RiderPage')));
const RiderDetailPage = Loadable(lazy(() => import('pages/RiderDetailPage')));
const BikePage = Loadable(lazy(() => import('client/pages/bike/BikePage')));
const NewsPage = Loadable(lazy(() => import('client/pages/news/NewsPage')));
const MediaPage = Loadable(lazy(() => import('client/pages/media/MediaPage')));
const SearchPage = Loadable(lazy(() => import('client/pages/search/SearchPage')));

// ==============================|| CLIENT ROUTING ||============================== //

const ClientRoutes = [
  {
    path: '/',
    element: <ClientLayout />,
    children: [
      {
        path: '',
        element: <HomePage />
      },
      {
        path: 'seasons',
        element: <SeasonPage />
      },
      {
        path: 'races',
        element: <RacePage />
      },
      {
        path: 'teams',
        element: <TeamPage />
      },
      {
        path: 'teams/:id',
        element: <TeamDetailPage />
      },
      {
        path: 'riders',
        element: <RiderPage />
      },
      {
        path: 'riders/:id',
        element: <RiderDetailPage />
      },
      {
        path: 'bikes',
        element: <BikePage />
      },
      {
        path: 'news',
        element: <NewsPage />
      },
      {
        path: 'media',
        element: <MediaPage />
      },
      {
        path: 'search',
        element: <SearchPage />
      }
    ]
  }
];

export default ClientRoutes;
