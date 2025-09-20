import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTeamWithRiders } from '../../../api/teams';
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
import Grid from '@mui/material/Grid';
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

const RiderCard = styled(Card)(({ theme }) => ({
  cursor: 'pointer',
  transition: 'all 0.3s ease',
  '&:hover': {
    transform: 'translateY(-4px)',
    boxShadow: '0 8px 20px rgba(0,0,0,0.15)'
  }
}));

const TeamShortName = styled(Typography)(({ theme }) => ({
  fontSize: '6rem',
  fontWeight: 900,
  color: 'rgba(225, 6, 0, 0.15)',
  lineHeight: 1,
  position: 'absolute',
  top: 20,
  right: 20,
  zIndex: 1
}));

export default function TeamDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [team, setTeam] = useState(null);
  const [riders, setRiders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchTeam = async () => {
      try {
        setLoading(true);
        const token = localStorage.getItem('accessToken');
        const response = await getTeamWithRiders(id, token);
        setTeam(response.data);
        setRiders(response.data.riders || []);
        setError(null);
      } catch (err) {
        console.error('Error fetching team:', err);
        setError('Failed to load team details. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchTeam();
    }
  }, [id]);

  const handleRiderClick = (riderId) => {
    navigate(`/riders/${riderId}`);
  };

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
          onClick={() => navigate('/teams')}
        >
          Back to Teams
        </Button>
      </Container>
    );
  }

  if (!team) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="warning" sx={{ mb: 4 }}>
          Team not found
        </Alert>
        <Button
          variant="contained"
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate('/teams')}
        >
          Back to Teams
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
            onClick={() => navigate('/teams')}
            sx={{ 
              color: '#fff', 
              mb: 3,
              '&:hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.1)'
              }
            }}
          >
            Back to Teams
          </Button>
          
          <TeamShortName>{team.shortName}</TeamShortName>
          
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
            <Typography variant="h2" component="h1" sx={{ fontWeight: 900, mr: 2 }}>
              {team.name}
            </Typography>
            <Chip
              label={team.isActive ? 'Active' : 'Inactive'}
              color={team.isActive ? 'success' : 'error'}
              size="medium"
              sx={{ fontWeight: 600 }}
            />
          </Box>
          
          <Typography variant="h5" sx={{ color: '#e10600', mb: 3 }}>
            {team.shortName}
          </Typography>
          
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
            <Typography variant="h6" sx={{ fontSize: '2rem' }}>
              {team.countryFlag}
            </Typography>
            <Typography variant="h6">
              {team.countryName}
            </Typography>
          </Box>
          
          {team.foundedYear && (
            <Typography variant="h6" sx={{ color: '#ccc' }}>
              Founded: {new Date(team.foundedYear).getFullYear()}
            </Typography>
          )}
        </Container>
      </HeroSection>

      <Container maxWidth="lg" sx={{ py: 6 }}>
        <Typography variant="h4" sx={{ mb: 4, fontWeight: 700 }}>
          Team Statistics
        </Typography>
        
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', lg: '1fr 1fr 1fr' }, gap: 3, mb: 6 }}>
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#e10600', fontWeight: 700 }}>
                {riders.length}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Current Riders
              </Typography>
            </CardContent>
          </StatsCard>
          
          <StatsCard>
            <CardContent sx={{ textAlign: 'center' }}>
              <Typography variant="h3" sx={{ color: '#4caf50', fontWeight: 700 }}>
                {team.capacity || 2}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Team Capacity
              </Typography>
            </CardContent>
          </StatsCard>
          
          {team.foundedYear && (
            <StatsCard>
              <CardContent sx={{ textAlign: 'center' }}>
                <Typography variant="h3" sx={{ color: '#2196f3', fontWeight: 700 }}>
                  {new Date().getFullYear() - new Date(team.foundedYear).getFullYear()}
                </Typography>
                <Typography variant="body1" sx={{ color: '#ccc' }}>
                  Years Active
                </Typography>
              </CardContent>
            </StatsCard>
          )}
        </Box>

        {team.description && (
          <Card sx={{ mb: 6 }}>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2, fontWeight: 700 }}>
                About the Team
              </Typography>
              <Typography variant="body1">
                {team.description}
              </Typography>
            </CardContent>
          </Card>
        )}

        {riders.length > 0 && (
          <>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 700 }}>
              Current Riders
            </Typography>
            <Grid container spacing={3}>
              {riders.map((rider) => (
                <Grid item key={rider.id} xs={12} sm={6} md={4}>
                  <RiderCard onClick={() => handleRiderClick(rider.id)}>
                    <CardContent>
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
                        <Typography variant="h6" sx={{ fontWeight: 700 }}>
                          {rider.fullName}
                        </Typography>
                        <Typography 
                          variant="h5" 
                          sx={{ 
                            color: '#e10600', 
                            fontWeight: 900,
                            fontSize: '1.5rem'
                          }}
                        >
                          #{rider.racingNumber}
                        </Typography>
                      </Box>
                      
                      {rider.nickname && (
                        <Typography variant="body2" sx={{ color: '#666', fontStyle: 'italic', mb: 1 }}>
                          "{rider.nickname}"
                        </Typography>
                      )}
                      
                      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
                        <span style={{ fontSize: '1.2rem' }}>{rider.countryFlag}</span>
                        <Typography variant="body2" color="textSecondary">
                          {rider.countryName}
                        </Typography>
                      </Box>
                      
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <Typography variant="body2" color="textSecondary">
                          Age: {rider.age}
                        </Typography>
                        <Chip
                          label={rider.isActive ? 'Active' : 'Retired'}
                          color={rider.isActive ? 'success' : 'error'}
                          size="small"
                        />
                      </Box>
                    </CardContent>
                  </RiderCard>
                </Grid>
              ))}
            </Grid>
          </>
        )}

        {riders.length === 0 && (
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ textAlign: 'center', color: 'textSecondary' }}>
                No riders currently assigned to this team
              </Typography>
            </CardContent>
          </Card>
        )}
      </Container>
    </>
  );
}