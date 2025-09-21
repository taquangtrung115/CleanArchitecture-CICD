import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Formik } from 'formik';
import * as Yup from 'yup';

// material-ui
import {
  Button,
  FormHelperText,
  Grid,
  InputLabel,
  OutlinedInput,
  Stack,
  Typography,
  Box,
  Alert
} from '@mui/material';

// third party
import AnimateButton from 'components/@extended/AnimateButton';

// API
import { forgotPassword } from 'api/auth';

// assets
import MailOutlined from '@ant-design/icons/MailOutlined';
import ArrowLeftOutlined from '@ant-design/icons/ArrowLeftOutlined';

const ForgotPasswordPage = () => {
  const navigate = useNavigate();
  const [success, setSuccess] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (values, { setSubmitting, setFieldError }) => {
    setLoading(true);
    try {
      const response = await forgotPassword(values.email);
      
      if (response.error) {
        setFieldError('email', response.error.detail || 'An error occurred');
      } else {
        setSuccess(true);
        // Navigate to verification page after 2 seconds
        setTimeout(() => {
          navigate('/auth/verify-code', { state: { email: values.email } });
        }, 2000);
      }
    } catch (error) {
      setFieldError('email', 'An unexpected error occurred');
    } finally {
      setLoading(false);
      setSubmitting(false);
    }
  };

  const validationSchema = Yup.object().shape({
    email: Yup.string().email('Must be a valid email').max(255).required('Email is required')
  });

  if (success) {
    return (
      <Box sx={{ textAlign: 'center', p: 3 }}>
        <MailOutlined style={{ fontSize: '48px', color: '#1976d2', marginBottom: '16px' }} />
        <Typography variant="h4" gutterBottom>
          Check Your Email
        </Typography>
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          We've sent a verification code to your email address. Please check your inbox and follow the instructions.
        </Typography>
        <Alert severity="success" sx={{ mb: 2 }}>
          If you don't see the email, please check your spam folder.
        </Alert>
        <Typography variant="body2" color="text.secondary">
          Redirecting to verification page...
        </Typography>
      </Box>
    );
  }

  return (
    <>
      <Formik
        initialValues={{
          email: ''
        }}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ errors, handleBlur, handleChange, handleSubmit, isSubmitting, touched, values }) => (
          <form noValidate onSubmit={handleSubmit}>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <Stack spacing={1}>
                  <Button
                    variant="text"
                    startIcon={<ArrowLeftOutlined />}
                    onClick={() => navigate('/login')}
                    sx={{ alignSelf: 'flex-start', mb: 2 }}
                  >
                    Back to Login
                  </Button>
                  
                  <Typography variant="h3" gutterBottom>
                    Forgot Password
                  </Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
                    Enter your email address and we'll send you a verification code to reset your password.
                  </Typography>
                </Stack>
              </Grid>
              
              <Grid item xs={12}>
                <Stack spacing={1}>
                  <InputLabel htmlFor="email-forgot">Email Address*</InputLabel>
                  <OutlinedInput
                    id="email-forgot"
                    type="email"
                    value={values.email}
                    name="email"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="Enter email address"
                    fullWidth
                    error={Boolean(touched.email && errors.email)}
                  />
                  {touched.email && errors.email && (
                    <FormHelperText error>{errors.email}</FormHelperText>
                  )}
                </Stack>
              </Grid>

              <Grid item xs={12}>
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
                    {loading ? 'Sending...' : 'Send Verification Code'}
                  </Button>
                </AnimateButton>
              </Grid>
            </Grid>
          </form>
        )}
      </Formik>
    </>
  );
};

export default ForgotPasswordPage;