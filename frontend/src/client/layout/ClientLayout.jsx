import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import Box from '@mui/material/Box';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';

export default function ClientLayout() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <Box sx={{ p: 0 }}>
                <Outlet />
            </Box>
        </Box>
    );
}
