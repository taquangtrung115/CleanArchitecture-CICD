import { Outlet } from 'react-router-dom';
import Box from '@mui/material/Box';
import ClientHeader from '../components/ClientHeader';
import ClientFooter from '../components/ClientFooter';

export default function ClientLayout() {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh', overflow: 'hidden' }}>
      <ClientHeader />
      <Box sx={{ flex: 1, overflow: 'hidden' }}>
        <Outlet />
      </Box>
      <ClientFooter />
    </Box>
  );
}
