import React, { useState } from 'react';
import { login } from '../api/authApi';
import { Button, TextField, Box, Typography, Alert } from '@mui/material';

function LoginForm() {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    try {
      const data = await login({ userName, password });
      localStorage.setItem('token', data.value?.accessToken);
      window.location.reload();
    } catch (err: any) {
      setError('Login failed');
    }
  };

  return (
    <Box maxWidth={400} mx="auto" mt={8}>
      <Typography variant="h5" gutterBottom>Login</Typography>
      <form onSubmit={handleSubmit}>
        <TextField label="Username" value={userName} onChange={e => setUserName(e.target.value)} fullWidth margin="normal" />
        <TextField label="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} fullWidth margin="normal" />
        <Button type="submit" variant="contained" fullWidth>Login</Button>
        {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
      </form>
    </Box>
  );
}

export default LoginForm;
