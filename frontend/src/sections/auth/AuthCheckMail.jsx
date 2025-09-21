import { Link as RouterLink } from 'react-router-dom';

// material-ui
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import Alert from '@mui/material/Alert';

// assets
import MailOutlined from '@ant-design/icons/MailOutlined';

// ============================|| CHECK MAIL ||============================ //

export default function AuthCheckMail() {
  return (
    <Paper sx={{ p: 4, textAlign: 'center', maxWidth: 400, mx: 'auto', mt: 8 }}>
      <Stack spacing={3} alignItems="center">
        <Box
          sx={{
            width: 80,
            height: 80,
            borderRadius: '50%',
            backgroundColor: 'success.light',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            fontSize: '2rem',
            color: 'success.main'
          }}
        >
          <MailOutlined />
        </Box>
        
        <Stack spacing={1} alignItems="center">
          <Typography variant="h4" gutterBottom>
            Check Your Email
          </Typography>
          <Typography variant="body1" color="text.secondary" textAlign="center">
            We've sent a password reset link to your email address. Please check your inbox and follow the instructions to reset your password.
          </Typography>
        </Stack>

        <Alert severity="info" sx={{ width: '100%' }}>
          <Typography variant="body2">
            Didn't receive the email? Check your spam folder or try requesting a new reset link.
          </Typography>
        </Alert>

        <Stack spacing={2} sx={{ width: '100%' }}>
          <Button 
            component={RouterLink} 
            to="/forgot-password"
            variant="outlined" 
            fullWidth
          >
            Resend Email
          </Button>
          <Button 
            component={RouterLink} 
            to="/login"
            variant="contained" 
            fullWidth
          >
            Back to Login
          </Button>
        </Stack>
      </Stack>
    </Paper>
  );
}