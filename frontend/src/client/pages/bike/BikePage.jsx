import React from 'react';
import Typography from '@mui/material/Typography';

export default function BikePage() {
  return (
    <div>
      <Typography variant="h4" color="primary" sx={{ fontWeight: 700, mb: 2 }}>
        MotoGP Bikes
      </Typography>
      <Typography>List of all MotoGP bikes will be displayed here.</Typography>
    </div>
  );
}
