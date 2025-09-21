import { Link, useSearchParams } from 'react-router-dom';

// material-ui
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

// project imports
import AuthWrapper from 'sections/auth/AuthWrapper';
import AuthResetPassword from 'sections/auth/AuthResetPassword';

// ================================|| JWT - RESET PASSWORD ||================================ //

export default function ResetPassword() {
  const [searchParams] = useSearchParams();
  const email = searchParams.get('email');
  const token = searchParams.get('token');

  return (
    <AuthWrapper>
      <Grid container spacing={3}>
        <Grid size={12}>
          <Stack direction="row" sx={{ alignItems: 'baseline', justifyContent: 'space-between', mb: { xs: -0.5, sm: 0.5 } }}>
            <Typography variant="h3">Reset Password</Typography>
            <Typography component={Link} to="/login" variant="body1" sx={{ textDecoration: 'none' }} color="primary">
              Back to Login
            </Typography>
          </Stack>
        </Grid>
        <Grid size={12}>
          <AuthResetPassword email={email} token={token} />
        </Grid>
      </Grid>
    </AuthWrapper>
  );
}