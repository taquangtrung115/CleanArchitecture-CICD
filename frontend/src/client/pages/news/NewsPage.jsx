import React from 'react';
import Typography from '@mui/material/Typography';

export default function NewsPage() {
    return (
        <div>
            <Typography variant="h4" color="primary" sx={{ fontWeight: 700, mb: 2 }}>
                MotoGP News
            </Typography>
            <Typography>Latest MotoGP news and articles will be displayed here.</Typography>
        </div>
    );
}
