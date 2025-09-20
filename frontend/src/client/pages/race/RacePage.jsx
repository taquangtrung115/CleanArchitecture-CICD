import React from 'react';
import Typography from '@mui/material/Typography';

export default function RacePage() {
  return (
    <div>
      <Typography variant="h4" color="primary" sx={{ fontWeight: 700, mb: 2 }}>
        MotoGP Races
      </Typography>
      <Typography>List of all MotoGP races will be displayed here.</Typography>
    </div>
  );
}
