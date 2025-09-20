import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  Avatar,
  Grid,
  Card,
  CardContent,
  Skeleton,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Container
} from '@mui/material';
import { styled } from '@mui/material/styles';

// Styled components for MotoGP theme
const StyledTableContainer = styled(TableContainer)(({ theme }) => ({
  backgroundColor: '#1a1a1a',
  borderRadius: '12px',
  border: '2px solid #e10600',
  boxShadow: '0 8px 32px rgba(225, 6, 0, 0.3)',
  '& .MuiTable-root': {
    backgroundColor: 'transparent',
  },
  '& .MuiTableHead-root': {
    backgroundColor: '#e10600',
  },
  '& .MuiTableHead-root .MuiTableCell-root': {
    backgroundColor: '#e10600',
    color: '#fff',
    fontWeight: 900,
    fontSize: '16px',
    fontFamily: 'Oswald, Arial, sans-serif',
    letterSpacing: '1px',
    textTransform: 'uppercase',
    borderBottom: 'none',
  },
  '& .MuiTableBody-root .MuiTableCell-root': {
    backgroundColor: '#1a1a1a',
    color: '#fff',
    borderBottom: '1px solid #333',
    fontSize: '14px',
    fontWeight: 600,
  },
  '& .MuiTableRow-root:hover': {
    backgroundColor: '#2a2a2a',
  },
}));

const StyledCard = styled(Card)(({ theme }) => ({
  backgroundColor: '#1a1a1a',
  border: '2px solid #333',
  borderRadius: '12px',
  transition: 'all 0.3s ease',
  '&:hover': {
    borderColor: '#e10600',
    boxShadow: '0 8px 32px rgba(225, 6, 0, 0.3)',
    transform: 'translateY(-4px)',
  },
}));

const PositionChip = styled(Chip)(({ position }) => ({
  fontWeight: 900,
  fontSize: '14px',
  minWidth: '40px',
  backgroundColor: position === 1 ? '#FFD700' : position === 2 ? '#C0C0C0' : position === 3 ? '#CD7F32' : '#e10600',
  color: position <= 3 ? '#000' : '#fff',
  border: position <= 3 ? '2px solid #000' : 'none',
}));

const StatusChip = styled(Chip)(({ status }) => ({
  fontWeight: 700,
  fontSize: '12px',
  backgroundColor: 
    status === 'Finished' ? '#4CAF50' :
    status === 'DNF' ? '#f44336' :
    status === 'DNS' ? '#ff9800' :
    status === 'DSQ' ? '#9c27b0' : '#757575',
  color: '#fff',
}));

export default function RaceClassificationTable({ raceResults, loading = false }) {
  if (loading) {
    return (
      <Box sx={{ p: 2 }}>
        <Skeleton variant="rectangular" width="100%" height={400} sx={{ borderRadius: 2 }} />
      </Box>
    );
  }

  if (!raceResults || raceResults.length === 0) {
    return (
      <StyledCard>
        <CardContent sx={{ textAlign: 'center', py: 6 }}>
          <Typography variant="h6" color="text.secondary">
            No race results available
          </Typography>
        </CardContent>
      </StyledCard>
    );
  }

  const formatTime = (timespan) => {
    if (!timespan) return '-';
    // Convert C# TimeSpan format to readable time
    const parts = timespan.split(':');
    if (parts.length === 3) {
      const minutes = parseInt(parts[1]);
      const seconds = parseFloat(parts[2]);
      return `${minutes}:${seconds.toFixed(3)}`;
    }
    return timespan;
  };

  const formatGap = (finishTime, winnerTime) => {
    if (!finishTime || !winnerTime) return '-';
    // Calculate gap (this would need proper time calculation)
    return '+0.123'; // Placeholder
  };

  const getRiderFlag = (countryCode) => {
    const flags = {
      'IT': '🇮🇹', 'ES': '🇪🇸', 'FR': '🇫🇷', 'GB': '🇬🇧', 'DE': '🇩🇪',
      'AU': '🇦🇺', 'JP': '🇯🇵', 'US': '🇺🇸', 'BR': '🇧🇷', 'ZA': '🇿🇦',
      'PT': '🇵🇹', 'NL': '🇳🇱', 'AT': '🇦🇹', 'CH': '🇨🇭'
    };
    return flags[countryCode] || '🏁';
  };

  return (
    <StyledTableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell align="center">Pos</TableCell>
            <TableCell align="center">No.</TableCell>
            <TableCell>Rider</TableCell>
            <TableCell>Team</TableCell>
            <TableCell>Bike</TableCell>
            <TableCell align="center">Time/Status</TableCell>
            <TableCell align="center">Gap</TableCell>
            <TableCell align="center">Best Lap</TableCell>
            <TableCell align="center">Points</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {raceResults.map((result, index) => {
            const position = result.finishPosition || index + 1;
            const status = result.isFinisher ? 'Finished' : 
                          result.notes?.includes('DNF') ? 'DNF' :
                          result.notes?.includes('DNS') ? 'DNS' :
                          result.notes?.includes('DSQ') ? 'DSQ' : 'Finished';
            
            return (
              <TableRow key={result.id}>
                <TableCell align="center">
                  <PositionChip
                    label={position}
                    position={position}
                    size="small"
                  />
                </TableCell>
                <TableCell align="center">
                  <Box sx={{ 
                    backgroundColor: '#e10600', 
                    color: '#fff', 
                    borderRadius: '4px', 
                    px: 1, 
                    py: 0.5,
                    fontWeight: 900,
                    minWidth: '30px',
                    display: 'inline-block'
                  }}>
                    {result.riderNumber}
                  </Box>
                </TableCell>
                <TableCell>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <span style={{ fontSize: '18px' }}>🏁</span>
                    <Typography variant="body2" sx={{ fontWeight: 700 }}>
                      {result.riderName}
                    </Typography>
                  </Box>
                </TableCell>
                <TableCell>
                  <Typography variant="body2" sx={{ color: '#e10600', fontWeight: 600 }}>
                    {result.teamName}
                  </Typography>
                </TableCell>
                <TableCell>
                  <Typography variant="body2" sx={{ color: '#ccc' }}>
                    {result.bikeModel}
                  </Typography>
                </TableCell>
                <TableCell align="center">
                  {result.isFinisher ? (
                    <Typography variant="body2" sx={{ fontFamily: 'monospace', color: '#4CAF50' }}>
                      {formatTime(result.finishTime)}
                    </Typography>
                  ) : (
                    <StatusChip label={status} status={status} size="small" />
                  )}
                </TableCell>
                <TableCell align="center">
                  <Typography variant="body2" sx={{ fontFamily: 'monospace', color: '#ff9800' }}>
                    {position === 1 ? '-' : formatGap(result.finishTime, raceResults[0]?.finishTime)}
                  </Typography>
                </TableCell>
                <TableCell align="center">
                  <Typography variant="body2" sx={{ fontFamily: 'monospace', color: '#2196F3' }}>
                    {formatTime(result.bestLapTime)}
                  </Typography>
                </TableCell>
                <TableCell align="center">
                  <Box sx={{ 
                    backgroundColor: result.pointsEarned > 0 ? '#4CAF50' : '#757575', 
                    color: '#fff', 
                    borderRadius: '4px', 
                    px: 1, 
                    py: 0.5,
                    fontWeight: 900,
                    minWidth: '30px',
                    display: 'inline-block'
                  }}>
                    {result.pointsEarned || 0}
                  </Box>
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </StyledTableContainer>
  );
}