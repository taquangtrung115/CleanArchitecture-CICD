import React from 'react';
import {
  Box,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Card,
  CardContent,
  Grid,
  Chip,
  Avatar
} from '@mui/material';
import { styled } from '@mui/material/styles';

const StyledCard = styled(Card)(({ theme }) => ({
  backgroundColor: '#1a1a1a',
  border: '2px solid #333',
  borderRadius: '12px',
  transition: 'all 0.3s ease',
  '&:hover': {
    borderColor: '#e10600',
    boxShadow: '0 8px 32px rgba(225, 6, 0, 0.3)',
    transform: 'translateY(-2px)',
  },
}));

const StyledSelect = styled(Select)(({ theme }) => ({
  backgroundColor: '#2a2a2a',
  borderRadius: '8px',
  '& .MuiOutlinedInput-notchedOutline': {
    borderColor: '#555',
  },
  '&:hover .MuiOutlinedInput-notchedOutline': {
    borderColor: '#e10600',
  },
  '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
    borderColor: '#e10600',
  },
  '& .MuiSelect-select': {
    color: '#fff',
    fontWeight: 600,
  },
  '& .MuiSvgIcon-root': {
    color: '#e10600',
  },
}));

const StatusChip = styled(Chip)(({ status }) => ({
  fontWeight: 700,
  fontSize: '12px',
  backgroundColor: 
    status === 'Completed' ? '#4CAF50' :
    status === 'InProgress' ? '#ff9800' :
    status === 'Upcoming' ? '#2196F3' :
    status === 'Cancelled' ? '#f44336' : '#757575',
  color: '#fff',
}));

export default function RaceSelector({ races, selectedRaceId, onRaceChange, loading = false }) {
  const getCountryFlag = (countryCode) => {
    const flags = {
      'IT': '🇮🇹', 'ES': '🇪🇸', 'FR': '🇫🇷', 'GB': '🇬🇧', 'DE': '🇩🇪',
      'AU': '🇦🇺', 'JP': '🇯🇵', 'US': '🇺🇸', 'BR': '🇧🇷', 'ZA': '🇿🇦',
      'PT': '🇵🇹', 'NL': '🇳🇱', 'AT': '🇦🇹', 'CH': '🇨🇭', 'MY': '🇲🇾',
      'TH': '🇹🇭', 'QA': '🇶🇦', 'AE': '🇦🇪', 'IN': '🇮🇳', 'AR': '🇦🇷'
    };
    return flags[countryCode] || '🏁';
  };

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      day: 'numeric',
      year: 'numeric'
    });
  };

  if (loading) {
    return (
      <StyledCard>
        <CardContent>
          <Typography variant="h6" color="primary" sx={{ mb: 2 }}>
            Loading races...
          </Typography>
        </CardContent>
      </StyledCard>
    );
  }

  if (!races || races.length === 0) {
    return (
      <StyledCard>
        <CardContent>
          <Typography variant="h6" color="text.secondary">
            No races available
          </Typography>
        </CardContent>
      </StyledCard>
    );
  }

  const selectedRace = races.find(race => race.id === selectedRaceId);

  return (
    <Box sx={{ mb: 3 }}>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <FormControl fullWidth>
            <InputLabel 
              sx={{ 
                color: '#ccc',
                '&.Mui-focused': { color: '#e10600' }
              }}
            >
              Select Race
            </InputLabel>
            <StyledSelect
              value={selectedRaceId || ''}
              label="Select Race"
              onChange={(e) => onRaceChange(e.target.value)}
            >
              {races.map((race) => (
                <MenuItem 
                  key={race.id} 
                  value={race.id}
                  sx={{
                    backgroundColor: '#2a2a2a',
                    color: '#fff',
                    '&:hover': {
                      backgroundColor: '#3a3a3a',
                    },
                    '&.Mui-selected': {
                      backgroundColor: '#e10600',
                      '&:hover': {
                        backgroundColor: '#c40500',
                      },
                    },
                  }}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, width: '100%' }}>
                    <span style={{ fontSize: '18px' }}>
                      {getCountryFlag(race.countryCode)}
                    </span>
                    <Box sx={{ flex: 1 }}>
                      <Typography variant="body2" sx={{ fontWeight: 700 }}>
                        Round {race.roundNumber}: {race.name}
                      </Typography>
                      <Typography variant="caption" sx={{ color: '#ccc' }}>
                        {race.circuitName} - {formatDate(race.raceDate)}
                      </Typography>
                    </Box>
                    <StatusChip 
                      label={race.status} 
                      status={race.status} 
                      size="small" 
                    />
                  </Box>
                </MenuItem>
              ))}
            </StyledSelect>
          </FormControl>
        </Grid>
        
        {selectedRace && (
          <Grid item xs={12} md={6}>
            <StyledCard>
              <CardContent>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
                  <span style={{ fontSize: '24px' }}>
                    {getCountryFlag(selectedRace.countryCode)}
                  </span>
                  <Box>
                    <Typography variant="h6" sx={{ 
                      color: '#e10600', 
                      fontWeight: 900,
                      fontFamily: 'Oswald, Arial, sans-serif'
                    }}>
                      {selectedRace.name}
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#ccc' }}>
                      Round {selectedRace.roundNumber} - {selectedRace.circuitName}
                    </Typography>
                  </Box>
                </Box>
                
                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Typography variant="caption" sx={{ color: '#999' }}>
                      Race Date
                    </Typography>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {formatDate(selectedRace.raceDate)}
                    </Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" sx={{ color: '#999' }}>
                      Status
                    </Typography>
                    <Box sx={{ mt: 0.5 }}>
                      <StatusChip 
                        label={selectedRace.status} 
                        status={selectedRace.status} 
                        size="small" 
                      />
                    </Box>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" sx={{ color: '#999' }}>
                      Circuit Length
                    </Typography>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {selectedRace.circuitLength} km
                    </Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" sx={{ color: '#999' }}>
                      Laps
                    </Typography>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {selectedRace.numberOfLaps}
                    </Typography>
                  </Grid>
                </Grid>
                
                {selectedRace.description && (
                  <Typography variant="body2" sx={{ mt: 2, color: '#ccc', fontStyle: 'italic' }}>
                    {selectedRace.description}
                  </Typography>
                )}
              </CardContent>
            </StyledCard>
          </Grid>
        )}
      </Grid>
    </Box>
  );
}