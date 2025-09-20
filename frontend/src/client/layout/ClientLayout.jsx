import React from 'react';
import { Outlet } from 'react-router-dom';
import Box from '@mui/material/Box';
import ClientHeader from '../components/ClientHeader';

export default function ClientLayout() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <ClientHeader />
            <Box sx={{ p: 0 }}>
                <Outlet />
            </Box>
        </Box>
    );
}
