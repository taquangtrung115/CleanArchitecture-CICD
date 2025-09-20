import React from 'react';
import { useNavigate } from 'react-router-dom';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Chip from '@mui/material/Chip';
import { styled } from '@mui/material/styles';

const StyledCard = styled(Card)(({ theme }) => ({
  height: '100%',
  cursor: 'pointer',
  transition: 'all 0.3s ease-in-out',
  background: 'linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%)',
  color: '#fff',
  border: '1px solid rgba(225, 6, 0, 0.3)',
  position: 'relative',
  overflow: 'hidden',
  '&::before': {
    content: '""',
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    height: '4px',
    background: 'linear-gradient(90deg, #e10600, #ff4d4d)',
    zIndex: 1
  },
  '&:hover': {
    transform: 'translateY(-8px)',
    boxShadow: '0 20px 40px rgba(225, 6, 0, 0.3)',
    border: '1px solid #e10600',
    '& .team-logo': {
      transform: 'scale(1.1)',
    },
    '& .team-image': {
      transform: 'scale(1.05)'
    }
  }
}));

const TeamShortName = styled(Typography)(({ theme }) => ({
  position: 'absolute',
  top: 16,
  right: 16,
  fontSize: '2rem',
  fontWeight: 900,
  color: 'rgba(255, 255, 255, 0.15)',
  lineHeight: 1,
  zIndex: 2
}));

const StatusChip = styled(Chip)(({ active }) => ({
  backgroundColor: active ? '#4caf50' : '#f44336',
  color: '#fff',
  fontWeight: 600,
  fontSize: '0.75rem'
}));

const CountryFlag = styled('span')({
  fontSize: '1.5rem',
  marginRight: '8px'
});

export default function TeamCard({ team }) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/teams/${team.id}`);
  };

  // Placeholder image - in real app this would come from the API
  const teamImage = `/api/images/teams/${team.id}.jpg`;

  return (
    <StyledCard onClick={handleClick}>
      <TeamShortName className="team-logo">
        {team.shortName}
      </TeamShortName>
      
      <CardMedia
        component="img"
        height="200"
        image={teamImage}
        alt={team.name}
        className="team-image"
        sx={{
          objectFit: 'cover',
          transition: 'transform 0.3s ease',
          filter: 'contrast(1.1) brightness(0.9)'
        }}
        onError={(e) => {
          e.target.src = 'https://via.placeholder.com/300x200/333/fff?text=Team+Logo';
        }}
      />
      
      <CardContent sx={{ p: 3, position: 'relative' }}>
        <Box sx={{ mb: 2 }}>
          <Typography 
            variant="h6" 
            component="h3" 
            sx={{ 
              fontWeight: 700, 
              fontSize: '1.1rem',
              mb: 0.5,
              color: '#fff'
            }}
          >
            {team.name}
          </Typography>
          <Typography 
            variant="body2" 
            sx={{ 
              color: '#e10600', 
              fontWeight: 600,
              fontSize: '0.9rem'
            }}
          >
            {team.shortName}
          </Typography>
        </Box>

        <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
          <CountryFlag>{team.countryFlag}</CountryFlag>
          <Typography variant="body2" sx={{ color: '#ccc' }}>
            {team.countryName}
          </Typography>
        </Box>

        <Box sx={{ mb: 2 }}>
          <Typography variant="body2" sx={{ color: '#ccc' }}>
            Founded: {team.foundedYear ? new Date(team.foundedYear).getFullYear() : 'N/A'}
          </Typography>
        </Box>

        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="body2" sx={{ color: '#ccc' }}>
            Capacity: {team.capacity || 2} riders
          </Typography>
          <StatusChip 
            label={team.isActive ? 'Active' : 'Inactive'} 
            active={team.isActive}
            size="small"
          />
        </Box>

        {team.description && (
          <Typography 
            variant="body2" 
            sx={{ 
              color: '#bbb', 
              fontSize: '0.875rem',
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical'
            }}
          >
            {team.description}
          </Typography>
        )}
      </CardContent>
    </StyledCard>
  );
}