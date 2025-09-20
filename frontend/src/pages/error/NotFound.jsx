import { useNavigate } from 'react-router-dom';
import { Box, Container, Typography, Button, Stack, Paper } from '@mui/material';
import { styled } from '@mui/material/styles';
import HomeIcon from '@mui/icons-material/Home';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';

// Styled components
const NotFoundContainer = styled(Box)(({ theme }) => ({
  minHeight: '100vh',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  backgroundColor: theme.palette.grey[50],
  padding: theme.spacing(2)
}));

const NotFoundPaper = styled(Paper)(({ theme }) => ({
  padding: theme.spacing(6, 4),
  textAlign: 'center',
  maxWidth: 600,
  width: '100%',
  boxShadow: theme.shadows[10],
  borderRadius: theme.spacing(2)
}));

const NotFoundNumber = styled(Typography)(({ theme }) => ({
  fontSize: '8rem',
  fontWeight: 'bold',
  color: theme.palette.primary.main,
  lineHeight: 1,
  textShadow: `2px 2px 4px ${theme.palette.grey[300]}`,
  [theme.breakpoints.down('sm')]: {
    fontSize: '6rem'
  }
}));

const NotFoundTitle = styled(Typography)(({ theme }) => ({
  fontSize: '1.5rem',
  fontWeight: 600,
  color: theme.palette.text.primary,
  marginBottom: theme.spacing(2),
  [theme.breakpoints.down('sm')]: {
    fontSize: '1.25rem'
  }
}));

const NotFoundDescription = styled(Typography)(({ theme }) => ({
  color: theme.palette.text.secondary,
  marginBottom: theme.spacing(4),
  fontSize: '1rem',
  lineHeight: 1.6
}));

export default function NotFound() {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate('/');
  };

  const handleGoBack = () => {
    navigate(-1);
  };

  return (
    <NotFoundContainer>
      <Container maxWidth="sm">
        <NotFoundPaper elevation={0}>
          <Box sx={{ mb: 4 }}>
            <NotFoundNumber variant="h1">404</NotFoundNumber>
          </Box>

          <Box sx={{ mb: 4 }}>
            <NotFoundTitle variant="h4">Page Not Found</NotFoundTitle>
            <NotFoundDescription variant="body1">
              The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.
            </NotFoundDescription>
          </Box>

          <Stack
            direction={{ xs: 'column', sm: 'row' }}
            spacing={2}
            justifyContent="center"
            alignItems="center"
          >
            <Button
              variant="contained"
              size="large"
              startIcon={<HomeIcon />}
              onClick={handleGoHome}
              sx={{
                minWidth: { xs: '100%', sm: 'auto' },
                px: 4,
                py: 1.5
              }}
            >
              Go to Home
            </Button>
            <Button
              variant="outlined"
              size="large"
              startIcon={<ArrowBackIcon />}
              onClick={handleGoBack}
              sx={{
                minWidth: { xs: '100%', sm: 'auto' },
                px: 4,
                py: 1.5
              }}
            >
              Go Back
            </Button>
          </Stack>
        </NotFoundPaper>
      </Container>
    </NotFoundContainer>
  );
}