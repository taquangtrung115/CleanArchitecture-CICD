import React, { useState, useRef, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

// material-ui
import {
  Button,
  FormHelperText,
  Grid,
  Stack,
  Typography,
  Box,
  Alert,
  TextField
} from '@mui/material';

// third party
import AnimateButton from 'components/@extended/AnimateButton';

// API
import { verifyResetCode, forgotPassword } from 'api/auth';

// assets
import SafetyOutlined from '@ant-design/icons/SafetyOutlined';
import ArrowLeftOutlined from '@ant-design/icons/ArrowLeftOutlined';

const CodeVerificationPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const email = location.state?.email || 'demo@example.com';

  const [code, setCode] = useState(['', '', '', '', '', '']);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [resending, setResending] = useState(false);
  const [resendSuccess, setResendSuccess] = useState(false);
  const [timeLeft, setTimeLeft] = useState(0);

  const inputRefs = useRef([]);

  useEffect(() => {
    if (!email) {
      // For demo purposes, use a default email
      // In production, redirect to forgot password
      // navigate('/auth/forgot-password');
    }
  }, [email, navigate]);

  useEffect(() => {
    // Start with 60 second countdown for resend
    setTimeLeft(60);
    const timer = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  const handleCodeChange = (index, value) => {
    if (value.length > 1) {
      // Handle paste
      const pastedCode = value.slice(0, 6).split('');
      const newCode = [...code];
      pastedCode.forEach((char, i) => {
        if (index + i < 6 && /^\d$/.test(char)) {
          newCode[index + i] = char;
        }
      });
      setCode(newCode);
      
      // Focus next empty input or last input
      const nextIndex = Math.min(index + pastedCode.length, 5);
      inputRefs.current[nextIndex]?.focus();
    } else if (/^\d$/.test(value) || value === '') {
      const newCode = [...code];
      newCode[index] = value;
      setCode(newCode);

      // Move to next input if value is entered
      if (value && index < 5) {
        inputRefs.current[index + 1]?.focus();
      }
    }
    setError('');
  };

  const handleKeyDown = (index, e) => {
    if (e.key === 'Backspace' && !code[index] && index > 0) {
      inputRefs.current[index - 1]?.focus();
    }
    if (e.key === 'ArrowLeft' && index > 0) {
      inputRefs.current[index - 1]?.focus();
    }
    if (e.key === 'ArrowRight' && index < 5) {
      inputRefs.current[index + 1]?.focus();
    }
  };

  const handleVerify = async () => {
    const verificationCode = code.join('');
    if (verificationCode.length !== 6) {
      setError('Please enter the complete 6-digit code');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const response = await verifyResetCode(email, verificationCode);
      
      if (response.error) {
        setError(response.error.detail || 'Invalid verification code');
      } else if (response.data?.value?.isValid) {
        // Code is valid, navigate to reset password page
        navigate('/auth/reset-password', { 
          state: { 
            email, 
            resetCode: verificationCode 
          } 
        });
      } else {
        setError('Invalid or expired verification code');
      }
    } catch (error) {
      setError('An unexpected error occurred');
    } finally {
      setLoading(false);
    }
  };

  const handleResendCode = async () => {
    setResending(true);
    setResendSuccess(false);
    setError('');

    try {
      const response = await forgotPassword(email);
      
      if (response.error) {
        setError(response.error.detail || 'Failed to resend code');
      } else {
        setResendSuccess(true);
        setTimeLeft(60);
        
        // Start countdown again
        const timer = setInterval(() => {
          setTimeLeft((prev) => {
            if (prev <= 1) {
              clearInterval(timer);
              return 0;
            }
            return prev - 1;
          });
        }, 1000);
        
        // Clear success message after 3 seconds
        setTimeout(() => setResendSuccess(false), 3000);
      }
    } catch (error) {
      setError('An unexpected error occurred');
    } finally {
      setResending(false);
    }
  };

  if (!email || email === 'demo@example.com') {
    // Allow demo mode
  } else {
    return null;
  }

  return (
    <Box sx={{ maxWidth: 400, mx: 'auto', textAlign: 'center' }}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Stack spacing={1}>
            <Button
              variant="text"
              startIcon={<ArrowLeftOutlined />}
              onClick={() => navigate('/auth/forgot-password')}
              sx={{ alignSelf: 'flex-start', mb: 2 }}
            >
              Back
            </Button>
            
            <SafetyOutlined style={{ fontSize: '48px', color: '#1976d2', marginBottom: '16px' }} />
            
            <Typography variant="h3" gutterBottom>
              Verify Your Email
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
              We've sent a 6-digit verification code to
            </Typography>
            <Typography variant="body1" fontWeight="medium" sx={{ mb: 3 }}>
              {email}
            </Typography>
          </Stack>
        </Grid>
        
        <Grid item xs={12}>
          <Stack spacing={2}>
            <Box sx={{ display: 'flex', gap: 1, justifyContent: 'center' }}>
              {code.map((digit, index) => (
                <TextField
                  key={index}
                  inputRef={(el) => (inputRefs.current[index] = el)}
                  value={digit}
                  onChange={(e) => handleCodeChange(index, e.target.value)}
                  onKeyDown={(e) => handleKeyDown(index, e)}
                  onFocus={(e) => e.target.select()}
                  variant="outlined"
                  inputProps={{
                    maxLength: 6,
                    style: {
                      textAlign: 'center',
                      fontSize: '1.5rem',
                      fontWeight: 'bold',
                      padding: '12px',
                      width: '50px',
                      height: '50px'
                    }
                  }}
                  sx={{
                    width: '60px',
                    '& .MuiOutlinedInput-root': {
                      height: '60px'
                    }
                  }}
                />
              ))}
            </Box>
            
            {error && (
              <FormHelperText error sx={{ textAlign: 'center', fontSize: '0.875rem' }}>
                {error}
              </FormHelperText>
            )}
            
            {resendSuccess && (
              <Alert severity="success" sx={{ textAlign: 'left' }}>
                Verification code sent successfully!
              </Alert>
            )}
          </Stack>
        </Grid>

        <Grid item xs={12}>
          <AnimateButton>
            <Button
              fullWidth
              size="large"
              variant="contained"
              color="primary"
              onClick={handleVerify}
              disabled={loading || code.join('').length !== 6}
            >
              {loading ? 'Verifying...' : 'Verify Code'}
            </Button>
          </AnimateButton>
        </Grid>

        <Grid item xs={12}>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Didn't receive the code?
          </Typography>
          
          {timeLeft > 0 ? (
            <Typography variant="body2" color="text.secondary">
              Resend code in {timeLeft}s
            </Typography>
          ) : (
            <Button
              variant="text"
              onClick={handleResendCode}
              disabled={resending}
            >
              {resending ? 'Sending...' : 'Resend Code'}
            </Button>
          )}
        </Grid>
      </Grid>
    </Box>
  );
};

export default CodeVerificationPage;