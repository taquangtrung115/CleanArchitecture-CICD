import React from 'react';
import Typography from '@mui/material/Typography';

export default function SearchPage() {
  return (
    <div>
      <Typography variant="h4" color="primary" sx={{ fontWeight: 700, mb: 2 }}>
        Search MotoGP
      </Typography>
      <Typography>Search and filter MotoGP data here.</Typography>
    </div>
  );
}
