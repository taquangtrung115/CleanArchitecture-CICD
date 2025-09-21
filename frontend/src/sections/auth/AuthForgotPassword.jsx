import PropTypes from 'prop-types';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { forgotPassword } from 'api/auth';
import Loader from 'components/Loader';
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

// ============================|| FORGOT PASSWORD ||============================ //

export default function AuthForgotPassword({ isDemo = false }) {
  const [loading, setLoading] = React.useState(false);
  const [formError, setFormError] = React.useState('');
  const [success, setSuccess] = React.useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (values, { setSubmitting }) => {
    if (loading) return;
    setFormError('');
    setLoading(true);

    try {
      const res = await forgotPassword(values.email);
      if (res.error) {
        setFormError(res.error.detail || 'Failed to send reset email');
      } else {
        setSuccess(true);
        // Redirect to check mail page after 2 seconds
        setTimeout(() => {
          navigate('/auth/check-mail');
        }, 2000);
      }
    } catch (error) {
      setFormError('An unexpected error occurred');
    }

    setSubmitting(false);
    setLoading(false);
  };

  if (success) {
    return (
      <Stack spacing={2}>
        <Alert severity="success">
          A password reset link has been sent to your email address.
        </Alert>
        <Typography variant="body2" color="text.secondary" align="center">
          Redirecting to check mail page...
        </Typography>
      </Stack>
    );
  }

  return (
    <>
      {loading && <Loader />}
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
        {({ errors, handleBlur, handleChange, touched, values, isSubmitting }) => (
          <form
            noValidate
            onSubmit={(e) => {
              e.preventDefault();
              handleSubmit(values, { setSubmitting: () => {} });
            }}
          >
            <Grid container spacing={3}>
              <Grid size={12}>
                <Stack spacing={1}>
                  <Typography variant="h4" gutterBottom>
                    Forgot Password
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Enter your email address and we'll send you a link to reset your password.
                  </Typography>
                </Stack>
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
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
                {formError && <FormHelperText error>{formError}</FormHelperText>}
                <AnimateButton>
                  <Button fullWidth size="large" variant="contained" color="primary" type="submit" disabled={isSubmitting}>
                    Send Reset Link
                  </Button>
                </AnimateButton>
              </Grid>
              <Grid size={12}>
                <Stack direction="row" justifyContent="center" alignItems="center">
                  <Typography variant="body2">
                    Remember your password?{' '}
                    <Link variant="h6" component={RouterLink} to="/login" color="text.primary">
                      Back to Login
                    </Link>
                  </Typography>
                </Stack>
              </Grid>
            </Grid>
          </form>
        )}
      </Formik>
    </>
  );
}

AuthForgotPassword.propTypes = { isDemo: PropTypes.bool };