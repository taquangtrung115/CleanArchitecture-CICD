import React, { useState, useEffect } from 'react';
import { getTeams } from '../../../api/teams';
import TeamCard from '../../components/TeamCard';
import { mockTeams } from '../../data/mockData';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import InputAdornment from '@mui/material/InputAdornment';
import SearchIcon from '@mui/icons-material/Search';
import CircularProgress from '@mui/material/CircularProgress';
import Alert from '@mui/material/Alert';
import Chip from '@mui/material/Chip';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import { styled } from '@mui/material/styles';

const HeroSection = styled(Box)(({ theme }) => ({
  background: 'linear-gradient(135deg, #000 0%, #1a1a1a 50%, #000 100%)',
  color: '#fff',
  padding: '80px 0 60px',
  position: 'relative',
  overflow: 'hidden',
  '&::before': {
    content: '""',
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    background: 'url("data:image/svg+xml,%3Csvg width="60" height="60" viewBox="0 0 60 60" xmlns="http://www.w3.org/2000/svg"%3E%3Cg fill="none" fill-rule="evenodd"%3E%3Cg fill="%23e10600" fill-opacity="0.05"%3E%3Ccircle cx="30" cy="30" r="2"/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")',
    zIndex: 1
  }
}));

const SearchSection = styled(Box)(({ theme }) => ({
  backgroundColor: '#f5f5f5',
  padding: '40px 0',
  borderBottom: '1px solid #eee'
}));

const ContentSection = styled(Box)(({ theme }) => ({
  backgroundColor: '#fff',
  minHeight: '60vh',
  padding: '40px 0'
}));

export default function TeamPage() {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');
  const [filteredTeams, setFilteredTeams] = useState([]);
  const [usingMockData, setUsingMockData] = useState(false);

  useEffect(() => {
    const fetchTeams = async () => {
      try {
        setLoading(true);
        // Try to get token from localStorage, but don't require it for public viewing
        const token = localStorage.getItem('accessToken');
        const response = await getTeams({}, token);
        setTeams(response.data.items || []);
        setError(null);
        setUsingMockData(false);
      } catch (err) {
        console.error('Error fetching teams:', err);
        // Fall back to mock data for demonstration
        setTeams(mockTeams);
        setError(null);
        setUsingMockData(true);
      } finally {
        setLoading(false);
      }
    };

    fetchTeams();
  }, []);

  useEffect(() => {
    let filtered = teams;

    // Filter by search term
    if (searchTerm) {
      filtered = filtered.filter(team =>
        team.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        team.shortName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        team.countryName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
        team.description?.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    // Filter by status
    if (statusFilter === 'active') {
      filtered = filtered.filter(team => team.isActive);
    } else if (statusFilter === 'inactive') {
      filtered = filtered.filter(team => !team.isActive);
    }

    setFilteredTeams(filtered);
  }, [teams, searchTerm, statusFilter]);

  const handleStatusFilterChange = (event, newFilter) => {
    if (newFilter !== null) {
      setStatusFilter(newFilter);
    }
  };

  const activeTeams = teams.filter(team => team.isActive).length;
  const inactiveTeams = teams.filter(team => !team.isActive).length;

  return (
    <>
      <HeroSection>
        <Container maxWidth="lg" sx={{ position: 'relative', zIndex: 2 }}>
          <Typography 
            variant="h2" 
            component="h1" 
            sx={{ 
              fontWeight: 900, 
              mb: 2,
              fontSize: { xs: '2.5rem', md: '3.5rem' },
              background: 'linear-gradient(45deg, #fff, #e10600)',
              backgroundClip: 'text',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent'
            }}
          >
            MotoGP Teams
          </Typography>
          <Typography 
            variant="h5" 
            sx={{ 
              color: '#ccc', 
              mb: 4,
              fontSize: { xs: '1.1rem', md: '1.3rem' }
            }}
          >
            The constructors and teams competing in the world's premier motorcycle championship
          </Typography>
          
          <Box sx={{ display: 'flex', gap: 3, flexWrap: 'wrap' }}>
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="h4" sx={{ color: '#e10600', fontWeight: 700 }}>
                {teams.length}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Total Teams
              </Typography>
            </Box>
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="h4" sx={{ color: '#4caf50', fontWeight: 700 }}>
                {activeTeams}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Active
              </Typography>
            </Box>
            <Box sx={{ textAlign: 'center' }}>
              <Typography variant="h4" sx={{ color: '#f44336', fontWeight: 700 }}>
                {inactiveTeams}
              </Typography>
              <Typography variant="body1" sx={{ color: '#ccc' }}>
                Inactive
              </Typography>
            </Box>
          </Box>
        </Container>
      </HeroSection>

      <SearchSection>
        <Container maxWidth="lg">
          <Box sx={{ display: 'flex', gap: 3, alignItems: 'center', flexWrap: 'wrap' }}>
            <TextField
              placeholder="Search teams..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon />
                  </InputAdornment>
                ),
              }}
              sx={{ 
                minWidth: 300,
                '& .MuiOutlinedInput-root': {
                  backgroundColor: '#fff'
                }
              }}
            />
            
            <ToggleButtonGroup
              value={statusFilter}
              exclusive
              onChange={handleStatusFilterChange}
              aria-label="team status filter"
            >
              <ToggleButton value="all" aria-label="all teams">
                All
              </ToggleButton>
              <ToggleButton value="active" aria-label="active teams">
                Active
              </ToggleButton>
              <ToggleButton value="inactive" aria-label="inactive teams">
                Inactive
              </ToggleButton>
            </ToggleButtonGroup>

            <Chip 
              label={`${filteredTeams.length} teams found`} 
              color="primary" 
              variant="outlined"
            />
          </Box>
        </Container>
      </SearchSection>

      <ContentSection>
        <Container maxWidth="lg">
          {usingMockData && (
            <Alert severity="info" sx={{ mb: 4 }}>
              Demo mode: Showing sample data. Connect to API to see live data.
            </Alert>
          )}

          {loading && (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
              <CircularProgress size={60} />
            </Box>
          )}

          {error && (
            <Box sx={{ mb: 4 }}>
              <Alert severity="error">{error}</Alert>
            </Box>
          )}

          {!loading && !error && (
            <Grid container spacing={3}>
              {filteredTeams.map((team) => (
                <Grid item key={team.id} xs={12} sm={6} md={4} lg={3}>
                  <TeamCard team={team} />
                </Grid>
              ))}
            </Grid>
          )}

          {!loading && !error && filteredTeams.length === 0 && (
            <Box sx={{ textAlign: 'center', py: 8 }}>
              <Typography variant="h6" color="textSecondary">
                No teams found matching your search criteria
              </Typography>
            </Box>
          )}
        </Container>
      </ContentSection>
    </>
  );
}
