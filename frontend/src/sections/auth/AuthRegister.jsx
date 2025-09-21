import Stack from '@mui/material/Stack';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import FormHelperText from '@mui/material/FormHelperText';
import FormControl from '@mui/material/FormControl';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import Button from '@mui/material/Button';
import AnimateButton from 'components/@extended/AnimateButton';
import { useEffect, useState } from 'react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { register } from 'api/auth';
import { getErrorMessage } from '../../utils/errorHandler';
import Loader from 'components/Loader';
import { Formik } from 'formik';
import * as Yup from 'yup';
import Grid from '@mui/material/Grid';
import { strengthColor, strengthIndicator } from 'utils/password-strength';

// assets
import EyeOutlined from '@ant-design/icons/EyeOutlined';
import EyeInvisibleOutlined from '@ant-design/icons/EyeInvisibleOutlined';

// ============================|| JWT - REGISTER ||============================ //
export default function AuthRegister() {
  const [level, setLevel] = useState();
  const [showPassword, setShowPassword] = useState(false);
  const [formError, setFormError] = useState('');
  const [formSuccess, setFormSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const changePassword = (value) => {
    const temp = strengthIndicator(value);
    setLevel(strengthColor(temp));
  };

  useEffect(() => {
    changePassword('');
  }, []);

  const handleSubmit = async (values, { setSubmitting }) => {
    if (loading) return; // Block nếu đang loading
    setFormError('');
    setFormSuccess('');
    setLoading(true);
    const payload = {
      userName: values.userName,
      email: values.email,
      password: values.password,
      firstName: values.firstName,
      lastName: values.lastName,
      dayOfBirth: values.dayOfBirth
    };
    const res = await register(payload);
    if (res.data) {
      setFormSuccess('Đăng ký thành công! Vui lòng đăng nhập.');
      setTimeout(() => navigate('/login'), 1500);
    } else {
      setFormError(getErrorMessage(res) || 'Đăng ký thất bại');
    }
    setSubmitting(false);
    setLoading(false);
  };

  return (
    <>
      {loading && <Loader />}
      <Formik
        initialValues={{
          userName: '',
          email: '',
          password: '',
          firstName: '',
          lastName: '',
          dayOfBirth: '',
          submit: null
        }}
        validationSchema={Yup.object().shape({
          firstname: Yup.string().max(255).required('First Name is required'),
          lastname: Yup.string().max(255).required('Last Name is required'),
          email: Yup.string().email('Must be a valid email').max(255).required('Email is required'),
          password: Yup.string()
            .required('Password is required')
            .test('no-leading-trailing-whitespace', 'Password cannot start or end with spaces', (value) => value === value.trim())
            .max(255, 'Password must be less than 255 characters')
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
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="userName-signup">User Name*</InputLabel>
                  <OutlinedInput
                    id="userName-signup"
                    type="text"
                    value={values.userName}
                    name="userName"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="User name"
                    fullWidth
                    error={Boolean(touched.userName && errors.userName)}
                  />
                </Stack>
                {touched.userName && errors.userName && (
                  <FormHelperText error id="helper-text-userName-signup">
                    {errors.userName}
                  </FormHelperText>
                )}
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="firstName-signup">First Name*</InputLabel>
                  <OutlinedInput
                    id="firstName-signup"
                    type="text"
                    value={values.firstName}
                    name="firstName"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="First name"
                    fullWidth
                    error={Boolean(touched.firstName && errors.firstName)}
                  />
                </Stack>
                {touched.firstName && errors.firstName && (
                  <FormHelperText error id="helper-text-firstName-signup">
                    {errors.firstName}
                  </FormHelperText>
                )}
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="lastName-signup">Last Name*</InputLabel>
                  <OutlinedInput
                    id="lastName-signup"
                    type="text"
                    value={values.lastName}
                    name="lastName"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="Last name"
                    fullWidth
                    error={Boolean(touched.lastName && errors.lastName)}
                  />
                </Stack>
                {touched.lastName && errors.lastName && (
                  <FormHelperText error id="helper-text-lastName-signup">
                    {errors.lastName}
                  </FormHelperText>
                )}
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="email-signup">Email Address*</InputLabel>
                  <OutlinedInput
                    fullWidth
                    error={Boolean(touched.email && errors.email)}
                    id="email-signup"
                    type="email"
                    value={values.email}
                    name="email"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    placeholder="demo@company.com"
                  />
                </Stack>
                {touched.email && errors.email && (
                  <FormHelperText error id="helper-text-email-signup">
                    {errors.email}
                  </FormHelperText>
                )}
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="password-signup">Password*</InputLabel>
                  <OutlinedInput
                    fullWidth
                    error={Boolean(touched.password && errors.password)}
                    id="password-signup"
                    type={showPassword ? 'text' : 'password'}
                    value={values.password}
                    name="password"
                    onBlur={handleBlur}
                    onChange={(e) => {
                      handleChange(e);
                      changePassword(e.target.value);
                    }}
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
                    placeholder="******"
                  />
                </Stack>
                {touched.password && errors.password && (
                  <FormHelperText error id="helper-text-password-signup">
                    {errors.password}
                  </FormHelperText>
                )}
                <FormControl fullWidth sx={{ mt: 2 }}>
                  <Grid container spacing={2} alignItems="center">
                    <Grid>
                      <Box sx={{ bgcolor: level?.color, width: 85, height: 8, borderRadius: '7px' }} />
                    </Grid>
                    <Grid>
                      <Typography variant="subtitle1" fontSize="0.75rem">
                        {level?.label}
                      </Typography>
                    </Grid>
                  </Grid>
                </FormControl>
              </Grid>
              <Grid size={12}>
                <Stack sx={{ gap: 1 }}>
                  <InputLabel htmlFor="dayOfBirth-signup">Day of Birth*</InputLabel>
                  <OutlinedInput
                    id="dayOfBirth-signup"
                    type="date"
                    value={values.dayOfBirth}
                    name="dayOfBirth"
                    onBlur={handleBlur}
                    onChange={handleChange}
                    fullWidth
                    error={Boolean(touched.dayOfBirth && errors.dayOfBirth)}
                  />
                </Stack>
                {touched.dayOfBirth && errors.dayOfBirth && (
                  <FormHelperText error id="helper-text-dayOfBirth-signup">
                    {errors.dayOfBirth}
                  </FormHelperText>
                )}
              </Grid>
              <Grid size={12}>
                <Typography variant="body2">
                  By Signing up, you agree to our &nbsp;
                  <Link variant="subtitle2" component={RouterLink} to="#">
                    Terms of Service
                  </Link>
                  &nbsp; and &nbsp;
                  <Link variant="subtitle2" component={RouterLink} to="#">
                    Privacy Policy
                  </Link>
                </Typography>
              </Grid>
              {errors.submit && (
                <Grid size={12}>
                  <FormHelperText error>{errors.submit}</FormHelperText>
                </Grid>
              )}
              <Grid size={12}>
                {formError && <FormHelperText error>{formError}</FormHelperText>}
                {formSuccess && <FormHelperText sx={{ color: 'success.main' }}>{formSuccess}</FormHelperText>}
                <AnimateButton>
                  <Button fullWidth size="large" variant="contained" color="primary" type="submit" disabled={isSubmitting}>
                    Create Account
                  </Button>
                </AnimateButton>
              </Grid>
            </Grid>
          </form>
        )}
      </Formik>
    </>
  );
}
