import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getRiderById } from '../../../api/riders';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Chip from '@mui/material/Chip';
import CircularProgress from '@mui/material/CircularProgress';
import Alert from '@mui/material/Alert';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { styled } from '@mui/material/styles';

const HeroSection = styled(Box)(({ theme }) => ({
  background: 'linear-gradient(135deg, #000 0%, #1a1a1a 50%, #000 100%)',
  color: '#fff',
  padding: '60px 0',
  position: 'relative',
  overflow: 'hidden'
}));

const StatsCard = styled(Card)(({ theme }) => ({
  background: 'linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%)',
  color: '#fff',
  border: '1px solid rgba(225, 6, 0, 0.3)',
  height: '100%'
}));

const RiderNumber = styled(Typography)(({ theme }) => ({
  fontSize: '8rem',
  fontWeight: 900,
  color: 'rgba(225, 6, 0, 0.2)',
  lineHeight: 1,
  position: 'absolute',
  top: 20,
  right: 20,
  zIndex: 1
}));

export default function RiderDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [rider, setRider] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchRider = async () => {
      try {
        setLoading(true);
        const token = localStorage.getItem('accessToken');
        const response = await getRiderById(id, token);
        setRider(response.data);
        setError(null);
      } catch (err) {
        console.error('Error fetching rider:', err);
        setError('Failed to load rider details. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchRider();
    }
  }, [id]);

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress size={60} />
      </Box>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 4 }}>
          {error}
        </Alert>
        <Button
          variant="contained"
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate('/riders')}
        >
          Back to Riders
        </Button>
      </Container>
    );
  }

  if (!rider) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="warning" sx={{ mb: 4 }}>
          Rider not found
        </Alert>
        <Button
          variant="contained"
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate('/riders')}
        >
          Back to Riders
        </Button>
      </Container>
    );
  }

  return (
    <>
      <HeroSection>
        <Container maxWidth="lg" sx={{ position: 'relative', zIndex: 2 }}>
          <Button
            startIcon={<ArrowBackIcon />}
            onClick={() => navigate('/riders')}
            sx={{ 
              color: '#fff', 
              mb: 3,
              '&:hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.1)'
              }
            }}
          >
            Back to Riders
          </Button>
          
          <RiderNumber>{rider.racingNumber}</RiderNumber>
          
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
            <Typography variant="h2" component="h1" sx={{ fontWeight: 900, mr: 2 }}>
              {rider.fullName}
            </Typography>
            <Chip
              label={rider.isActive ? 'Active' : 'Retired'}
              color={rider.isActive ? 'success' : 'error'}
              size="medium"
              sx={{ fontWeight: 600 }}
            />
          </Box>
          
          {rider.nickname && (
            <Typography variant="h5" sx={{ color: '#e10600', fontStyle: 'italic', mb: 3 }}>
              "{rider.nickname}"
            </Typography>
          )}
          
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
            <Typography variant="h6" sx={{ fontSize: '2rem' }}>
              {rider.countryFlag}
            </Typography>
            <Typography variant="h6">
              {rider.countryName}
            </Typography>
          </Box>
          
          <Typography variant="h6" sx={{ color: '#ccc' }}>
            Current Team: {rider.currentTeamName || 'Free Agent'}
          </Typography>
        </Container>
      </HeroSection>

      <Container maxWidth="lg" sx={{ py: 6 }}>
        <Typography variant="h4" sx={{ mb: 4, fontWeight: 700 }}>
          Rider Statistics
        </Typography>
        
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', lg: '1fr 1fr 1fr 1fr' }, gap: 3, mb: 6 }}>
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#e10600', fontWeight: 700 }}>
                #{rider.racingNumber}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Racing Number
              </Typography>
            </CardContent>
          </StatsCard>
          
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#4caf50', fontWeight: 700 }}>
                {rider.age}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Years Old
              </Typography>
            </CardContent>
          </StatsCard>
          
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#2196f3', fontWeight: 700 }}>
                {rider.height}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Height (cm)
              </Typography>
            </CardContent>
          </StatsCard>
          
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#ff9800', fontWeight: 700 }}>
                {rider.weight}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Weight (kg)
              </Typography>
            </CardContent>
          </StatsCard>
        </Box>

        {rider.birthDate && (
          <Card sx={{ mb: 4 }}>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: 700 }}>
                Personal Information
              </Typography>
              <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' }, gap: 2 }}>
                <Box>
                  <Typography variant="body2" color="textSecondary">
                    Birth Date
                  </Typography>
                  <Typography variant="body1">
                    {new Date(rider.birthDate).toLocaleDateString()}
                  </Typography>
                </Box>
                <Box>
                  <Typography variant="body2" color="textSecondary">
                    Nationality
                  </Typography>
                  <Typography variant="body1">
                    {rider.countryFlag} {rider.countryName}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        )}

        {rider.currentTeamName && (
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: 700 }}>
                Team Information
              </Typography>
              <Typography variant="body1">
                Currently racing for: <strong>{rider.currentTeamName}</strong>
              </Typography>
            </CardContent>
          </Card>
        )}
      </Container>
    </>
  );
}