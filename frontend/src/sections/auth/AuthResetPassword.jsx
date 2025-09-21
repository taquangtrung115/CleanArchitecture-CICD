import PropTypes from 'prop-types';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { resetPasswordWithToken } from 'api/auth';
import { Link as RouterLink } from 'react-router-dom';

// material-ui
import Button from '@mui/material/Button';
import FormHelperText from '@mui/material/FormHelperText';
import Grid from '@mui/material/Grid';
import Link from '@mui/material/Link';
import InputAdornment from '@mui/material/InputAdornment';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import Stack from '@mui/material/Stack';
import Typography from '@mui/material/Typography';
import Alert from '@mui/material/Alert';

// third-party
import * as Yup from 'yup';
import { Formik } from 'formik';

// project imports
import IconButton from 'components/@extended/IconButton';
import AnimateButton from 'components/@extended/AnimateButton';

// assets
import EyeOutlined from '@ant-design/icons/EyeOutlined';
import EyeInvisibleOutlined from '@ant-design/icons/EyeInvisibleOutlined';

// ============================|| JWT - RESET PASSWORD ||============================ //

export default function AuthResetPassword({ email, token }) {
  const [loading, setLoading] = React.useState(false);
  const [success, setSuccess] = React.useState(false);
  const [errorMessage, setErrorMessage] = React.useState('');
  const [showPassword, setShowPassword] = React.useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = React.useState(false);
  const navigate = useNavigate();

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleClickShowConfirmPassword = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  React.useEffect(() => {
    if (!email || !token) {
      setErrorMessage('Invalid reset link. Please request a new password reset.');
    }
  }, [email, token]);

  const handleSubmit = async (values, { setSubmitting }) => {
    if (loading || !email || !token) return;
    
    setLoading(true);
    setErrorMessage('');
    
    const res = await resetPasswordWithToken(email, token, values.password);
    
    if (res.error) {
      setErrorMessage(res.error.detail || 'Failed to reset password. Please try again.');
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
              Password Reset Successfully
            </Typography>
            <Typography variant="body2">
              Your password has been reset successfully. You can now log in with your new password.
            </Typography>
          </Alert>
        </Grid>
        <Grid size={12}>
          <AnimateButton>
            <Button
              component={RouterLink}
              to="/login"
              variant="contained"
              color="primary"
              fullWidth
              size="large"
            >
              Go to Login
            </Button>
          </AnimateButton>
        </Grid>
      </Grid>
    );
  }

  if (!email || !token) {
    return (
      <Grid container spacing={3}>
        <Grid size={12}>
          <Alert severity="error" sx={{ mb: 2 }}>
            <Typography variant="h6" sx={{ mb: 1 }}>
              Invalid Reset Link
            </Typography>
            <Typography variant="body2">
              This password reset link is invalid or has expired. Please request a new password reset.
            </Typography>
          </Alert>
        </Grid>
        <Grid size={12}>
          <Stack direction="row" spacing={2}>
            <AnimateButton>
              <Button
                component={RouterLink}
                to="/forgot-password"
                variant="contained"
                color="primary"
                fullWidth
              >
                Request New Reset
              </Button>
            </AnimateButton>
            <Button
              component={RouterLink}
              to="/login"
              variant="outlined"
              fullWidth
            >
              Back to Login
            </Button>
          </Stack>
        </Grid>
      </Grid>
    );
  }

  return (
    <Formik
      initialValues={{
        password: '',
        confirmPassword: '',
        submit: null
      }}
      validationSchema={Yup.object().shape({
        password: Yup.string()
          .min(6, 'Password must be at least 6 characters')
          .required('Password is required'),
        confirmPassword: Yup.string()
          .required('Please confirm your password')
          .oneOf([Yup.ref('password')], 'Passwords must match')
      })}
      onSubmit={handleSubmit}
    >
      {({ errors, handleBlur, handleChange, handleSubmit, isSubmitting, touched, values }) => (
        <form noValidate onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid size={12}>
              <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
                Enter your new password below. Make sure it's at least 6 characters long.
              </Typography>
            </Grid>
            <Grid size={12}>
              <Stack spacing={1}>
                <InputLabel htmlFor="password-reset">New Password</InputLabel>
                <OutlinedInput
                  id="password-reset"
                  type={showPassword ? 'text' : 'password'}
                  value={values.password}
                  name="password"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  endAdornment={
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle password visibility"
                        onClick={handleClickShowPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                        color="secondary"
                      >
                        {showPassword ? <EyeOutlined /> : <EyeInvisibleOutlined />}
                      </IconButton>
                    </InputAdornment>
                  }
                  placeholder="Enter new password"
                  fullWidth
                  error={Boolean(touched.password && errors.password)}
                />
              </Stack>
              {touched.password && errors.password && (
                <FormHelperText error id="standard-weight-helper-text-password-reset">
                  {errors.password}
                </FormHelperText>
              )}
            </Grid>
            <Grid size={12}>
              <Stack spacing={1}>
                <InputLabel htmlFor="confirm-password-reset">Confirm New Password</InputLabel>
                <OutlinedInput
                  id="confirm-password-reset"
                  type={showConfirmPassword ? 'text' : 'password'}
                  value={values.confirmPassword}
                  name="confirmPassword"
                  onBlur={handleBlur}
                  onChange={handleChange}
                  endAdornment={
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle confirm password visibility"
                        onClick={handleClickShowConfirmPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                        color="secondary"
                      >
                        {showConfirmPassword ? <EyeOutlined /> : <EyeInvisibleOutlined />}
                      </IconButton>
                    </InputAdornment>
                  }
                  placeholder="Confirm new password"
                  fullWidth
                  error={Boolean(touched.confirmPassword && errors.confirmPassword)}
                />
              </Stack>
              {touched.confirmPassword && errors.confirmPassword && (
                <FormHelperText error id="standard-weight-helper-text-confirm-password-reset">
                  {errors.confirmPassword}
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
                  {loading ? 'Resetting...' : 'Reset Password'}
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

AuthResetPassword.propTypes = { 
  email: PropTypes.string,
  token: PropTypes.string
};