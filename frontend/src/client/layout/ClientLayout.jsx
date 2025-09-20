import { Outlet } from 'react-router-dom';
import Box from '@mui/material/Box';
import ClientHeader from '../components/ClientHeader';
import ClientFooter from '../components/ClientFooter';

export default function ClientLayout() {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <ClientHeader />
      <Box sx={{ flex: 1, p: 0 }}>
        <Outlet />
      </Box>
      <ClientFooter />
    </Box>
  );
}
