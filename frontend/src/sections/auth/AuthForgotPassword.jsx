import PropTypes from 'prop-types';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { forgotPassword } from 'api/auth';
import { getErrorMessage } from '../../utils/errorHandler';

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

// ============================|| JWT - FORGOT PASSWORD ||============================ //

export default function AuthForgotPassword({ isDemo = false }) {
  const [loading, setLoading] = React.useState(false);
  const [formError, setFormError] = React.useState('');
  const [success, setSuccess] = React.useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (values, { setSubmitting }) => {
    if (loading) return;
    
    setLoading(true);
    setFormError('');

    try {
      const result = await forgotPassword(values.email);
      if (result.data) {
        setSuccess(true);
        setTimeout(() => {
          navigate('/login');
        }, 2000);
      } else {
        setFormError(getErrorMessage(result) || 'Failed to send reset email');
      }
    } catch (err) {
      setFormError('An error occurred while sending reset email');
    }
    setLoading(false);
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
          <Stack spacing={2}>
            <AnimateButton>
              <Button
                component={RouterLink}
                to="/login"
                variant="contained"
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

  if (loading) return <Loader />;

  return (
    <>
      <Formik
        initialValues={{
          email: '',
          submit: null
        }}
        validationSchema={Yup.object().shape({
          email: Yup.string().email('Must be a valid email').max(255).required('Email is required')
        })}
        onSubmit={async (values, { setErrors, setStatus, setSubmitting }) => {
          setLoading(true);
          setErrorMessage('');

          if (isDemo) {
            setTimeout(() => {
              setStatus({ success: true });
              setLoading(false);
              setSuccess(true);
              setSubmitting(false);
            }, 500);
          } else {
            try {
              const result = await forgotPassword(values.email);
              if (result.error) {
                const errorMsg = getErrorMessage(result) || 'Failed to send reset email';
                setErrorMessage(errorMsg);
                setErrors({ submit: errorMsg });
              } else {
                setStatus({ success: true });
                setSuccess(true);
              }
            } catch (err) {
              setErrorMessage('An error occurred while sending reset email');
              setErrors({ submit: 'An error occurred while sending reset email' });
            }
            setLoading(false);
            setSubmitting(false);
          }
        }}
      >
        {({ errors, handleBlur, handleChange, handleSubmit, isSubmitting, touched, values }) => (
          <form noValidate onSubmit={handleSubmit}>
            <Grid container spacing={3}>
              <Grid size={12}>
                <Stack spacing={1}>
                  <InputLabel htmlFor="email-forgot">Email Address</InputLabel>
                  <OutlinedInput
                    fullWidth
                    error={Boolean(touched.email && errors.email)}
                    id="email-forgot"
                    type="email"
                    value={values.email}
                    name="email"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="Enter email address"
                    inputProps={{}}
                  />
                  {touched.email && errors.email && (
                    <FormHelperText error id="helper-text-email-forgot">
                      {errors.email}
                    </FormHelperText>
                  )}
                </Stack>
              </Grid>
              {formError && (
                <Grid size={12}>
                  <FormHelperText error>{formError}</FormHelperText>
                </Grid>
              )}
              <Grid size={12} sx={{ mb: -1 }}>
                <Typography variant="body2">
                  Do not forgot to check SPAM box.
                </Typography>
              </Grid>
              <Grid size={12}>
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
                    Send Password Reset Email
                  </Button>
                </AnimateButton>
              </Grid>
              {errors.submit && (
                <Grid size={12}>
                  <FormHelperText error>{errors.submit}</FormHelperText>
                </Grid>
              )}
            </Grid>
          </form>
        )}
      </Formik>
      {success && (
        <Stack spacing={1} sx={{ mt: 3 }}>
          <Alert severity="success">Check your email for reset instructions</Alert>
        </Stack>
      )}
    </>
  );
}

AuthForgotPassword.propTypes = {
  isDemo: PropTypes.bool
};
