import React from 'react';
import { CssBaseline, ThemeProvider, createTheme, Container, Tabs, Tab, Box } from '@mui/material';
import LoginForm from './components/LoginForm';
import UserList from './components/UserList';
import RoleList from './components/RoleList';
import PermissionList from './components/PermissionList';

const theme = createTheme();

function App() {
  const [tab, setTab] = React.useState(0);
  const token = localStorage.getItem('token');
  if (!token) return <LoginForm />;

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Tabs value={tab} onChange={(_, v) => setTab(v)} centered>
          <Tab label="Users" />
          <Tab label="Roles" />
          <Tab label="Permissions" />
        </Tabs>
        <Box mt={3}>
          {tab === 0 && <UserList />}
          {tab === 1 && <RoleList />}
          {tab === 2 && <PermissionList />}
        </Box>
      </Container>
    </ThemeProvider>
  );
}

export default App;
