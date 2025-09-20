import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation, Link as RouterLink } from 'react-router-dom';
import { getUserDetail } from '../../api/user';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import LanguageIcon from '@mui/icons-material/Language';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemText from '@mui/material/ListItemText';
import motogpLogo from '../assets/images/EstrellaGalicia_Logo.png';

const navigationItems = [
  { label: 'Home', path: '/' },
  { label: 'Seasons', path: '/seasons' },
  { label: 'Races', path: '/races' },
  { label: 'Teams', path: '/teams' },
  { label: 'Riders', path: '/riders' },
  { label: 'Bikes', path: '/bikes' },
  { label: 'News', path: '/news' },
  { label: 'Media', path: '/media' },
  { label: 'Search', path: '/search' }
];

export default function ClientHeader() {
  const navigate = useNavigate();
  const location = useLocation();
  const [user, setUser] = useState(null);
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  useEffect(() => {
    const accessToken = localStorage.getItem('accessToken');
    const fetchUser = async () => {
      const userId = localStorage.getItem('userId');
      if (accessToken && userId) {
        try {
          const res = await getUserDetail(userId);
          if (res.data && res.data.userName) {
            setUser({ userName: res.data.userName });
          } else {
            setUser({ userName: 'User' });
          }
        } catch (error) {
          setUser(null);
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

  const handleAdminPanel = () => {
    navigate('/admin');
  };

  const toggleMobileMenu = () => {
    setMobileMenuOpen(!mobileMenuOpen);
  };

  const isActive = (path) => {
    if (path === '/') {
      return location.pathname === '/';
    }
    return location.pathname.startsWith(path);
  };

  return (
    <>
      <AppBar position="sticky" sx={{ bgcolor: '#111', boxShadow: '0 2px 16px #000', zIndex: 1201 }}>
        <Toolbar sx={{ minHeight: 72, px: { xs: 1, md: 4 } }}>
          <IconButton
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2, display: { xs: 'flex', md: 'none' } }}
            onClick={toggleMobileMenu}
          >
            <MenuIcon />
          </IconButton>

          <Box sx={{ display: 'flex', alignItems: 'center', flexGrow: 1 }}>
            <Box component={RouterLink} to="/" sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none' }}>
              <img src={motogpLogo} alt="MotoGP Logo" style={{ height: 38, marginRight: 32 }} />
            </Box>

            <Box sx={{ display: { xs: 'none', md: 'flex' }, gap: 1 }}>
              {navigationItems.map((item) => (
                <Button
                  key={item.path}
                  component={RouterLink}
                  to={item.path}
                  color="inherit"
                  sx={{
                    fontWeight: isActive(item.path) ? 800 : 600,
                    fontSize: 16,
                    letterSpacing: 1,
                    textTransform: 'none',
                    borderBottom: isActive(item.path) ? '2px solid #e10600' : '2px solid transparent',
                    borderRadius: 0,
                    px: 2,
                    py: 1,
                    color: isActive(item.path) ? '#e10600' : '#fff',
                    '&:hover': {
                      bgcolor: 'rgba(225, 6, 0, 0.1)',
                      color: '#e10600'
                    }
                  }}
                >
                  {item.label}
                </Button>
              ))}
            </Box>
          </Box>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <IconButton color="inherit">
              <LanguageIcon />
            </IconButton>

            {user ? (
              <>
                <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 16, mx: 1 }}>Welcome, {user.userName}</Typography>
                <Button
                  variant="outlined"
                  size="small"
                  onClick={handleAdminPanel}
                  sx={{
                    borderColor: '#e10600',
                    color: '#e10600',
                    fontWeight: 600,
                    '&:hover': {
                      bgcolor: '#e10600',
                      color: '#fff'
                    }
                  }}
                >
                  Admin
                </Button>
                <Button
                  variant="outlined"
                  size="small"
                  onClick={handleLogout}
                  sx={{
                    borderColor: '#fff',
                    color: '#fff',
                    fontWeight: 600,
                    '&:hover': {
                      bgcolor: '#fff',
                      color: '#111'
                    }
                  }}
                >
                  Logout
                </Button>
              </>
            ) : (
              <>
                <Button
                  variant="outlined"
                  size="small"
                  onClick={handleLogin}
                  sx={{
                    borderColor: '#fff',
                    color: '#fff',
                    fontWeight: 600,
                    '&:hover': {
                      bgcolor: '#fff',
                      color: '#111'
                    }
                  }}
                >
                  Login
                </Button>
                <Button
                  variant="contained"
                  size="small"
                  onClick={handleRegister}
                  sx={{
                    bgcolor: '#e10600',
                    color: '#fff',
                    fontWeight: 600,
                    '&:hover': {
                      bgcolor: '#b00500'
                    }
                  }}
                >
                  Register
                </Button>
              </>
            )}
          </Box>
        </Toolbar>
      </AppBar>

      {/* Mobile Navigation Drawer */}
      <Drawer
        anchor="left"
        open={mobileMenuOpen}
        onClose={toggleMobileMenu}
        sx={{
          display: { xs: 'block', md: 'none' },
          '& .MuiDrawer-paper': {
            bgcolor: '#111',
            color: '#fff',
            width: 280
          }
        }}
      >
        <Box sx={{ p: 2 }}>
          <img src={motogpLogo} alt="MotoGP Logo" style={{ height: 32, marginBottom: 16 }} />
        </Box>
        <List>
          {navigationItems.map((item) => (
            <ListItem key={item.path} disablePadding>
              <ListItemButton
                component={RouterLink}
                to={item.path}
                onClick={toggleMobileMenu}
                sx={{
                  bgcolor: isActive(item.path) ? 'rgba(225, 6, 0, 0.2)' : 'transparent',
                  borderLeft: isActive(item.path) ? '4px solid #e10600' : '4px solid transparent',
                  '&:hover': {
                    bgcolor: 'rgba(225, 6, 0, 0.1)'
                  }
                }}
              >
                <ListItemText
                  primary={item.label}
                  sx={{
                    '& .MuiTypography-root': {
                      color: isActive(item.path) ? '#e10600' : '#fff',
                      fontWeight: isActive(item.path) ? 700 : 500
                    }
                  }}
                />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </Drawer>
    </>
  );
}
