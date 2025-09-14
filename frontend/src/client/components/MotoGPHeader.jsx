import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { getUserDetail } from '../../api/user';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import LanguageIcon from '@mui/icons-material/Language';
import motogpLogo from '../assets/images/EstrellaGalicia_Logo.png';
export default function MotoGPHeader() {
    const navigate = useNavigate();
    const location = useLocation();
    const [user, setUser] = useState(null);

    useEffect(() => {
        // Giả sử user info lưu trong localStorage sau khi login
        const accessToken = localStorage.getItem('accessToken');
        const userName = localStorage.getItem('userName');
        const fetchUser = async () => {
            const userId = localStorage.getItem('userId');
            if (accessToken && userId) {
                const res = await getUserDetail(userId);
                if (res.data && res.data.userName) {
                    setUser({ userName: res.data.userName });
                } else {
                    setUser({ userName: 'User' });
                }
            } else {
                setUser(null);
            }
        };
        fetchUser();
    }, []);

    const handleLogin = () => navigate('/login');
    const handleRegister = () => navigate('/register');
    const handleLogout = () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userId');
        setUser(null);
        navigate('/');
    };
    return (
        <AppBar position="sticky" sx={{ bgcolor: '#111', boxShadow: '0 2px 16px #000', zIndex: 1201 }}>
            <Toolbar sx={{ minHeight: 72, px: { xs: 1, md: 4 } }}>
                <IconButton edge="start" color="inherit" aria-label="menu" sx={{ mr: 2, display: { xs: 'flex', md: 'none' } }}>
                    <MenuIcon />
                </IconButton>
                <Box sx={{ display: 'flex', alignItems: 'center', flexGrow: 1 }}>
                    <img src={motogpLogo} alt="MotoGP Logo" style={{ height: 38, marginRight: 32 }} />
                    <Box sx={{ display: { xs: 'none', md: 'flex' }, gap: 3 }}>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>Calendar</Button>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>Results & Standings</Button>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>Riders & Teams</Button>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>VideoPass</Button>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>Videos</Button>
                        <Button color="inherit" sx={{ fontWeight: 700, fontSize: 18, letterSpacing: 1, textTransform: 'none' }}>News</Button>
                    </Box>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                    <Button variant="contained" sx={{ bgcolor: '#e10600', color: '#fff', fontWeight: 900, fontSize: 16, borderRadius: 8, px: 3, boxShadow: '0 2px 12px #e10600', '&:hover': { bgcolor: '#b00500' } }}>SUBSCRIBE</Button>
                    <IconButton color="inherit">
                        <LanguageIcon />
                    </IconButton>
                    {user ? (
                        <>
                            <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, mx: 1 }}>
                                {user.userName}
                            </Typography>
                            <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, mx: 1, cursor: 'pointer' }} onClick={handleLogout}>
                                Logout
                            </Typography>
                        </>
                    ) : (
                        <>
                            <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, mx: 1, cursor: 'pointer' }} onClick={handleLogin}>Login</Typography>
                            <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, cursor: 'pointer' }}>|</Typography>
                            <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, mx: 1, cursor: 'pointer' }} onClick={handleRegister}>Register</Typography>
                        </>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    );
}
