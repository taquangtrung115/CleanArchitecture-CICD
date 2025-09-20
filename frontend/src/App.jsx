import { RouterProvider } from 'react-router-dom';
import { useEffect } from 'react';

// project imports
import router from 'routes';
import ThemeCustomization from 'themes';
import { handleRefreshToken } from 'utils/auth';

import ScrollTop from 'components/ScrollTop';

// ==============================|| APP - THEME, ROUTER, LOCAL ||============================== //

export default function App() {
  useEffect(() => {
    // Initialize authentication state on app load (handles page refresh)
    const initializeAuth = async () => {
      const token = localStorage.getItem('token');
      const refreshToken = localStorage.getItem('refreshToken');

      // If we have a refresh token but no access token, try to refresh
      if (!token && refreshToken) {
        try {
          await handleRefreshToken();
        } catch (error) {
          console.error('Failed to refresh token on app initialization:', error);
        }
      }
    };

    initializeAuth();
  }, []);

  return (
    <ThemeCustomization>
      <ScrollTop>
        <RouterProvider router={router} />
      </ScrollTop>
    </ThemeCustomization>
  );
}
