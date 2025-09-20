import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Container,
  Alert,
  CircularProgress,
  Fade,
  Grid,
  Card,
  CardContent,
  Button,
  Tabs,
  Tab,
  Divider
} from '@mui/material';
import { styled } from '@mui/material/styles';
import { keyframes } from '@mui/system';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';
import SpeedIcon from '@mui/icons-material/Speed';
import FlagIcon from '@mui/icons-material/Flag';
import RaceClassificationTable from '../../components/RaceClassificationTable';
import RaceSelector from '../../components/RaceSelector';
import { raceService } from '../../../services/motogpService';

// MotoGP styled components
const PageContainer = styled(Box)(({ theme }) => ({
  backgroundColor: '#101014',
  minHeight: '100vh',
  padding: theme.spacing(3, 0),
  fontFamily: 'Oswald, Arial Black, sans-serif',
}));

const StyledCard = styled(Card)(({ theme }) => ({
  backgroundColor: '#1a1a1a',
  border: '2px solid #333',
  borderRadius: '12px',
  transition: 'all 0.3s ease',
  '&:hover': {
    borderColor: '#e10600',
    boxShadow: '0 8px 32px rgba(225, 6, 0, 0.3)',
  },
}));

const HeaderCard = styled(Card)(({ theme }) => ({
  background: 'linear-gradient(135deg, #e10600 0%, #b50000 100%)',
  borderRadius: '16px',
  marginBottom: theme.spacing(3),
  position: 'relative',
  overflow: 'hidden',
  '&::before': {
    content: '""',
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    background: 'url("data:image/svg+xml,%3Csvg xmlns=\'http://www.w3.org/2000/svg\' viewBox=\'0 0 100 100\'%3E%3Cpath d=\'M0 0h100v100H0z\' fill=\'%23000\' fill-opacity=\'0.05\'/%3E%3C/svg%3E")',
    backgroundSize: '20px 20px',
  },
}));

const StatsCard = styled(Card)(({ theme }) => ({
  backgroundColor: '#1a1a1a',
  border: '2px solid #e10600',
  borderRadius: '12px',
  textAlign: 'center',
  transition: 'all 0.3s ease',
  '&:hover': {
    transform: 'translateY(-4px)',
    boxShadow: '0 12px 40px rgba(225, 6, 0, 0.4)',
  },
}));

const pulse = keyframes`
  0% { box-shadow: 0 0 0 0 rgba(225, 6, 0, 0.7); }
  70% { box-shadow: 0 0 0 10px rgba(225, 6, 0, 0); }
  100% { box-shadow: 0 0 0 0 rgba(225, 6, 0, 0); }
`;

const LiveIndicator = styled(Box)(({ theme }) => ({
  display: 'inline-flex',
  alignItems: 'center',
  gap: theme.spacing(1),
  backgroundColor: '#ff1744',
  color: '#fff',
  padding: theme.spacing(0.5, 1),
  borderRadius: '20px',
  fontSize: '12px',
  fontWeight: 700,
  animation: `${pulse} 2s infinite`,
}));

export default function RacePage() {
  const [races, setRaces] = useState([]);
  const [selectedRaceId, setSelectedRaceId] = useState(null);
  const [raceResults, setRaceResults] = useState(null);
  const [loading, setLoading] = useState(true);
  const [resultsLoading, setResultsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [tabValue, setTabValue] = useState(0);
  const [show, setShow] = useState(false);

  useEffect(() => {
    setShow(true);
    fetchRaces();
  }, []);

  useEffect(() => {
    if (selectedRaceId) {
      fetchRaceResults(selectedRaceId);
    }
  }, [selectedRaceId]);

  const fetchRaces = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // For demo purposes, create mock data since we don't have real API yet
      const mockRaces = [
        {
          id: '1',
          name: 'San Marino and Rimini Riviera Grand Prix',
          circuitName: 'Misano Adriatico',
          countryCode: 'IT',
          countryName: 'Italy',
          raceDate: '2025-09-07T14:00:00Z',
          status: 'Completed',
          roundNumber: 13,
          circuitLength: 4.226,
          numberOfLaps: 27,
          description: 'The Misano World Circuit Marco Simoncelli'
        },
        {
          id: '2',
          name: 'Emilia Romagna Grand Prix',
          circuitName: 'Misano Adriatico',
          countryCode: 'IT',
          countryName: 'Italy',
          raceDate: '2025-09-21T14:00:00Z',
          status: 'Completed',
          roundNumber: 14,
          circuitLength: 4.226,
          numberOfLaps: 27,
          description: 'Return to Misano for the second Italian round'
        },
        {
          id: '3',
          name: 'Indonesian Grand Prix',
          circuitName: 'Mandalika International Circuit',
          countryCode: 'ID',
          countryName: 'Indonesia',
          raceDate: '2025-09-28T09:00:00Z',
          status: 'Upcoming',
          roundNumber: 15,
          circuitLength: 4.310,
          numberOfLaps: 27,
          description: 'The spectacular Mandalika circuit'
        }
      ];
      
      setRaces(mockRaces);
      if (mockRaces.length > 0) {
        setSelectedRaceId(mockRaces[0].id);
      }
      
      // Uncomment when real API is available:
      // const response = await raceService.getCompletedRaces({ pageSize: 20 });
      // setRaces(response.items || []);
      // if (response.items && response.items.length > 0) {
      //   setSelectedRaceId(response.items[0].id);
      // }
    } catch (err) {
      console.error('Error fetching races:', err);
      setError('Failed to load races. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const fetchRaceResults = async (raceId) => {
    try {
      setResultsLoading(true);
      setError(null);
      
      // Mock race results data
      const mockResults = [
        {
          id: '1',
          raceId: raceId,
          riderId: '1',
          riderName: 'Francesco Bagnaia',
          riderNumber: 1,
          teamId: '1',
          teamName: 'Ducati Lenovo Team',
          bikeId: '1',
          bikeModel: 'Ducati Desmosedici GP24',
          startingPosition: 1,
          finishPosition: 1,
          finishTime: '00:41:52.083',
          bestLapTime: '00:01:31.064',
          pointsEarned: 25,
          isFinisher: true,
          notes: null
        },
        {
          id: '2',
          raceId: raceId,
          riderId: '2',
          riderName: 'Jorge Martin',
          riderNumber: 89,
          teamId: '2',
          teamName: 'Prima Pramac Racing',
          bikeId: '2',
          bikeModel: 'Ducati Desmosedici GP24',
          startingPosition: 2,
          finishPosition: 2,
          finishTime: '00:41:52.506',
          bestLapTime: '00:01:31.156',
          pointsEarned: 20,
          isFinisher: true,
          notes: null
        },
        {
          id: '3',
          raceId: raceId,
          riderId: '3',
          riderName: 'Enea Bastianini',
          riderNumber: 23,
          teamId: '1',
          teamName: 'Ducati Lenovo Team',
          bikeId: '3',
          bikeModel: 'Ducati Desmosedici GP24',
          startingPosition: 3,
          finishPosition: 3,
          finishTime: '00:41:53.281',
          bestLapTime: '00:01:31.201',
          pointsEarned: 16,
          isFinisher: true,
          notes: null
        },
        {
          id: '4',
          raceId: raceId,
          riderId: '4',
          riderName: 'Marc Marquez',
          riderNumber: 93,
          teamId: '3',
          teamName: 'Gresini Racing MotoGP',
          bikeId: '4',
          bikeModel: 'Ducati Desmosedici GP23',
          startingPosition: 4,
          finishPosition: 4,
          finishTime: '00:41:54.190',
          bestLapTime: '00:01:31.289',
          pointsEarned: 13,
          isFinisher: true,
          notes: null
        },
        {
          id: '5',
          raceId: raceId,
          riderId: '5',
          riderName: 'Pedro Acosta',
          riderNumber: 31,
          teamId: '4',
          teamName: 'Red Bull GASGAS Tech3',
          bikeId: '5',
          bikeModel: 'KTM RC16',
          startingPosition: 5,
          finishPosition: 5,
          finishTime: '00:41:55.012',
          bestLapTime: '00:01:31.445',
          pointsEarned: 11,
          isFinisher: true,
          notes: null
        },
        {
          id: '6',
          raceId: raceId,
          riderId: '6',
          riderName: 'Fabio Quartararo',
          riderNumber: 20,
          teamId: '5',
          teamName: 'Monster Energy Yamaha MotoGP',
          bikeId: '6',
          bikeModel: 'Yamaha YZR-M1',
          startingPosition: 8,
          finishPosition: null,
          finishTime: null,
          bestLapTime: '00:01:31.789',
          pointsEarned: 0,
          isFinisher: false,
          notes: 'DNF - Technical'
        }
      ];
      
      setRaceResults(mockResults);
      
      // Uncomment when real API is available:
      // const response = await raceService.getRaceWithResults(raceId);
      // setRaceResults(response.results || []);
    } catch (err) {
      console.error('Error fetching race results:', err);
      setError('Failed to load race results. Please try again later.');
    } finally {
      setResultsLoading(false);
    }
  };

  const handleRaceChange = (raceId) => {
    setSelectedRaceId(raceId);
  };

  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  const selectedRace = races.find(race => race.id === selectedRaceId);
  const raceStats = raceResults ? {
    totalRiders: raceResults.length,
    finishers: raceResults.filter(r => r.isFinisher).length,
    dnf: raceResults.filter(r => !r.isFinisher && r.notes?.includes('DNF')).length,
  } : null;

  return (
    <Fade in={show} timeout={900}>
      <PageContainer>
        <Container maxWidth="xl">
          {/* Header */}
          <HeaderCard>
            <CardContent sx={{ position: 'relative', zIndex: 1, py: 4 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
                <EmojiEventsIcon sx={{ fontSize: 40, color: '#fff' }} />
                <Box>
                  <Typography
                    variant="h3"
                    sx={{
                      fontWeight: 900,
                      color: '#fff',
                      fontFamily: 'Oswald, Arial Black, sans-serif',
                      letterSpacing: 1,
                      textShadow: '2px 2px 4px rgba(0,0,0,0.5)',
                    }}
                  >
                    MotoGP Race Classification
                  </Typography>
                  <Typography variant="h6" sx={{ color: '#ffeb3b', fontWeight: 600 }}>
                    Official Race Results & Championship Points
                  </Typography>
                </Box>
              </Box>
              
              {selectedRace?.status === 'InProgress' && (
                <LiveIndicator>
                  <SpeedIcon sx={{ fontSize: 16 }} />
                  LIVE
                </LiveIndicator>
              )}
            </CardContent>
          </HeaderCard>

          {error && (
            <Alert severity="error" sx={{ mb: 3, backgroundColor: '#2a1a1a', color: '#fff' }}>
              {error}
            </Alert>
          )}

          {/* Race Selector */}
          <RaceSelector
            races={races}
            selectedRaceId={selectedRaceId}
            onRaceChange={handleRaceChange}
            loading={loading}
          />

          {/* Race Statistics */}
          {selectedRace && raceStats && (
            <Grid container spacing={3} sx={{ mb: 3 }}>
              <Grid item xs={12} md={3}>
                <StatsCard>
                  <CardContent>
                    <Typography variant="h4" sx={{ color: '#e10600', fontWeight: 900 }}>
                      {raceStats.totalRiders}
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#ccc' }}>
                      Total Riders
                    </Typography>
                  </CardContent>
                </StatsCard>
              </Grid>
              <Grid item xs={12} md={3}>
                <StatsCard>
                  <CardContent>
                    <Typography variant="h4" sx={{ color: '#4CAF50', fontWeight: 900 }}>
                      {raceStats.finishers}
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#ccc' }}>
                      Finishers
                    </Typography>
                  </CardContent>
                </StatsCard>
              </Grid>
              <Grid item xs={12} md={3}>
                <StatsCard>
                  <CardContent>
                    <Typography variant="h4" sx={{ color: '#f44336', fontWeight: 900 }}>
                      {raceStats.dnf}
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#ccc' }}>
                      DNF
                    </Typography>
                  </CardContent>
                </StatsCard>
              </Grid>
              <Grid item xs={12} md={3}>
                <StatsCard>
                  <CardContent>
                    <Typography variant="h4" sx={{ color: '#ff9800', fontWeight: 900 }}>
                      {selectedRace.numberOfLaps}
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#ccc' }}>
                      Total Laps
                    </Typography>
                  </CardContent>
                </StatsCard>
              </Grid>
            </Grid>
          )}

          {/* Race Results Table */}
          {selectedRace && (
            <StyledCard>
              <CardContent sx={{ p: 0 }}>
                <Box sx={{ p: 3, pb: 0 }}>
                  <Typography
                    variant="h5"
                    sx={{
                      fontWeight: 900,
                      color: '#e10600',
                      fontFamily: 'Oswald, Arial, sans-serif',
                      letterSpacing: 1,
                      mb: 2,
                    }}
                  >
                    <FlagIcon sx={{ mr: 1, verticalAlign: 'middle' }} />
                    Race Classification
                  </Typography>
                  
                  <Tabs
                    value={tabValue}
                    onChange={handleTabChange}
                    sx={{
                      mb: 2,
                      '& .MuiTab-root': {
                        color: '#ccc',
                        fontWeight: 600,
                      },
                      '& .Mui-selected': {
                        color: '#e10600 !important',
                      },
                      '& .MuiTabs-indicator': {
                        backgroundColor: '#e10600',
                      },
                    }}
                  >
                    <Tab label="Race Results" />
                    <Tab label="Lap Times" disabled />
                    <Tab label="Sector Times" disabled />
                  </Tabs>
                </Box>
                
                <Divider sx={{ borderColor: '#333' }} />
                
                <Box sx={{ p: 3 }}>
                  {tabValue === 0 && (
                    <RaceClassificationTable
                      raceResults={raceResults}
                      loading={resultsLoading}
                    />
                  )}
                </Box>
              </CardContent>
            </StyledCard>
          )}

          {/* Loading State */}
          {loading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
              <CircularProgress sx={{ color: '#e10600' }} size={60} />
            </Box>
          )}
        </Container>
      </PageContainer>
    </Fade>
  );
}
