// material-ui
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';

// project imports
import AuthBackground from 'sections/auth/AuthBackground';
import AuthForgotPassword from 'sections/auth/AuthForgotPassword';
import Logo from 'components/logo';

// ================================|| FORGOT PASSWORD ||================================ //

export default function ForgotPassword() {
  return (
    <Grid container sx={{ minHeight: '100vh' }}>
      <AuthBackground />
      <Grid
        item
        xs={12}
        sm={6}
        md={5}
        lg={4}
        sx={{
          position: 'relative',
          alignSelf: 'center',
          justifySelf: 'center'
        }}
      >
        <Stack
          direction="column"
          justifyContent="center"
          sx={{
            minHeight: { xs: 'calc(100vh - 210px)', sm: 'calc(100vh - 134px)', md: 'calc(100vh - 112px)' },
            p: 4
          }}
        >
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <Stack direction="row" justifyContent="space-between" alignItems="baseline" sx={{ mb: { xs: -0.5, sm: 0.5 } }}>
                <Typography variant="h3">Forgot Password</Typography>
                <Typography component="span" variant="h6" sx={{ textDecoration: 'none' }}>
                  <Logo />
                </Typography>
              </Stack>
            </Grid>
            <Grid item xs={12}>
              <AuthForgotPassword />
            </Grid>
          </Grid>
        </Stack>
      </Grid>
      <Grid item xs={12} sm={6} md={7} lg={8} sx={{ position: 'relative', alignSelf: 'stretch', display: { xs: 'none', sm: 'block' } }}>
        <AuthBackground />
      </Grid>
    </Grid>
  );
}