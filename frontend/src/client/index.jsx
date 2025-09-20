// MotoGP Client App Entry
import React from 'react';
import { Routes, Route } from 'react-router-dom';
import ClientLayout from './layout/ClientLayout';
import HomePage from './pages/HomePage';
import SeasonPage from './pages/season/SeasonPage';
import RacePage from './pages/race/RacePage';
import TeamPage from './pages/team/TeamPage';
import TeamDetailPage from '../pages/TeamDetailPage';
import RiderPage from './pages/rider/RiderPage';
import RiderDetailPage from '../pages/RiderDetailPage';
import BikePage from './pages/bike/BikePage';
import NewsPage from './pages/news/NewsPage';
import MediaPage from './pages/media/MediaPage';
import SearchPage from './pages/search/SearchPage';
import LoginPage from '../pages/LoginPage';
import RegisterPage from '../pages/RegisterPage';

export default function ClientApp() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/" element={<ClientLayout />}>
        <Route index element={<HomePage />} />
        <Route path="seasons" element={<SeasonPage />} />
        <Route path="races" element={<RacePage />} />
        <Route path="teams" element={<TeamPage />} />
        <Route path="teams/:id" element={<TeamDetailPage />} />
        <Route path="riders" element={<RiderPage />} />
        <Route path="riders/:id" element={<RiderDetailPage />} />
        <Route path="bikes" element={<BikePage />} />
        <Route path="news" element={<NewsPage />} />
        <Route path="media" element={<MediaPage />} />
        <Route path="search" element={<SearchPage />} />
      </Route>
    </Routes>
  );
}
