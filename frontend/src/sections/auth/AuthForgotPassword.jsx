import PropTypes from 'prop-types';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { forgotPassword } from 'api/auth';
import { Link as RouterLink } from 'react-router-dom';

// material-ui
import Button from '@mui/material/Button';
import FormHelperText from '@mui/material/FormHelperText';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';

// third-party
import * as Yup from 'yup';
import { Formik } from 'formik';

// project imports
import AnimateButton from 'components/@extended/AnimateButton';

// ============================|| JWT - FORGOT PASSWORD ||============================ //

export default function AuthForgotPassword({ isDemo = false }) {
  const [loading, setLoading] = React.useState(false);
  const [success, setSuccess] = React.useState(false);
  const [errorMessage, setErrorMessage] = React.useState('');
  const navigate = useNavigate();

  const handleSubmit = async (values, { setSubmitting }) => {
    if (loading) return;
    
    setLoading(true);
    setErrorMessage('');
    
    const res = await forgotPassword(values.email);
    
    if (res.error) {
      setErrorMessage(res.error.detail || 'An error occurred. Please try again.');
    } else {
      setSuccess(true);
    }
    
    setLoading(false);
    setSubmitting(false);
  };

  if (success) {
    return (
      <Grid container spacing={3}>
        <Grid size={12}>
          <Alert severity="success" sx={{ mb: 2 }}>
            <Typography variant="h6" sx={{ mb: 1 }}>
              Reset Instructions Sent
            </Typography>
            <Typography variant="body2">
              We've sent password reset instructions to your email address. Please check your inbox and follow the instructions to reset your password.
            </Typography>
          </Alert>
        </Grid>
        <Grid size={12}>
          <Stack direction="row" spacing={2}>
            <AnimateButton>
              <Button
                component={RouterLink}
                to="/login"
                variant="contained"
                color="primary"
                fullWidth
              >
                Back to Login
              </Button>
            </AnimateButton>
            <Button
              variant="outlined"
              onClick={() => setSuccess(false)}
              fullWidth
            >
              Send Again
            </Button>
          </Stack>
        </Grid>
      </Grid>
    );
  }

  return (
    <Formik
      initialValues={{
        email: '',
        submit: null
      }}
      validationSchema={Yup.object().shape({
        email: Yup.string().email('Must be a valid email').max(255).required('Email is required')
      })}
      onSubmit={handleSubmit}
    >
      {({ errors, handleBlur, handleChange, handleSubmit, isSubmitting, touched, values }) => (
        <form noValidate onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid size={12}>
              <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
                Enter your email address and we'll send you instructions to reset your password.
              </Typography>
            </Grid>
            <Grid size={12}>
              <Stack spacing={1}>
                <InputLabel htmlFor="email-forgot">Email Address</InputLabel>
                <OutlinedInput
                  id="email-forgot"
                  type="email"
                  value={values.email}
                  name="email"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  placeholder="Enter your email address"
                  fullWidth
                  error={Boolean(touched.email && errors.email)}
                />
              </Stack>
              {touched.email && errors.email && (
                <FormHelperText error id="standard-weight-helper-text-email-forgot">
                  {errors.email}
                </FormHelperText>
              )}
            </Grid>
            <Grid size={12}>
              {errorMessage && (
                <FormHelperText error sx={{ mb: 1 }}>
                  {errorMessage}
                </FormHelperText>
              )}
              <AnimateButton>
                <Button
                  disableElevation
                  disabled={isSubmitting || loading}
                  fullWidth
                  size="large"
                  type="submit"
                  variant="contained"
                  color="primary"
                >
                  {loading ? 'Sending...' : 'Send Reset Instructions'}
                </Button>
              </AnimateButton>
            </Grid>
            <Grid size={12}>
              <Stack direction="row" spacing={1} sx={{ justifyContent: 'center' }}>
                <Typography variant="body2">
                  Remember your password?
                </Typography>
                <Link variant="body2" component={RouterLink} to="/login" color="primary">
                  Sign in
                </Link>
              </Stack>
            </Grid>
          </Grid>
        </form>
      )}
    </Formik>
  );
}

AuthForgotPassword.propTypes = { isDemo: PropTypes.bool };