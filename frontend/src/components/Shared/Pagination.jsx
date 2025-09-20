import React from 'react';
import { Box, Button, Typography, IconButton } from '@mui/material';
import { ChevronLeft, ChevronRight } from '@mui/icons-material';

const Pagination = ({ page, pageSize, total, onChange }) => {
  const totalPages = Math.ceil(total / pageSize);
  if (totalPages <= 1) return null;

  const getVisiblePages = () => {
    const delta = 2;
    const range = [];
    const rangeWithDots = [];

    for (let i = Math.max(2, page - delta); i <= Math.min(totalPages - 1, page + delta); i++) {
      range.push(i);
    }

    if (page - delta > 2) {
      rangeWithDots.push(1, '...');
    } else {
      rangeWithDots.push(1);
    }

    rangeWithDots.push(...range);

    if (page + delta < totalPages - 1) {
      rangeWithDots.push('...', totalPages);
    } else if (totalPages > 1) {
      rangeWithDots.push(totalPages);
    }

    return rangeWithDots;
  };

  const visiblePages = getVisiblePages();

  return (
    <Box
      sx={{
        display: 'flex',
        alignItems: 'center',
        gap: 1,
        justifyContent: 'center',
        flexWrap: 'wrap',
        p: 2
      }}
    >
      {/* Previous Button */}
      <IconButton
        onClick={() => onChange(page - 1, pageSize)}
        disabled={page <= 1}
        sx={{
          bgcolor: 'rgba(255,255,255,0.05)',
          color: page <= 1 ? '#666' : '#e10600',
          border: '1px solid',
          borderColor: page <= 1 ? '#333' : '#e10600',
          '&:hover': {
            bgcolor: page <= 1 ? 'rgba(255,255,255,0.05)' : 'rgba(225,6,0,0.1)',
            borderColor: page <= 1 ? '#333' : '#ff0000'
          },
          '&:disabled': {
            color: '#666',
            borderColor: '#333'
          }
        }}
      >
        <ChevronLeft />
      </IconButton>

      {/* Page Numbers */}
      {visiblePages.map((pageNum, index) => (
        <React.Fragment key={index}>
          {pageNum === '...' ? (
            <Typography sx={{ color: '#888', px: 1 }}>...</Typography>
          ) : (
            <Button
              onClick={() => onChange(pageNum, pageSize)}
              variant={page === pageNum ? 'contained' : 'outlined'}
              sx={{
                minWidth: 40,
                height: 40,
                bgcolor: page === pageNum ? '#e10600' : 'transparent',
                color: page === pageNum ? 'white' : '#e10600',
                borderColor: '#e10600',
                fontWeight: 600,
                '&:hover': {
                  bgcolor: page === pageNum ? '#ff0000' : 'rgba(225,6,0,0.1)',
                  borderColor: '#ff0000'
                }
              }}
            >
              {pageNum}
            </Button>
          )}
        </React.Fragment>
      ))}

      {/* Next Button */}
      <IconButton
        onClick={() => onChange(page + 1, pageSize)}
        disabled={page >= totalPages}
        sx={{
          bgcolor: 'rgba(255,255,255,0.05)',
          color: page >= totalPages ? '#666' : '#e10600',
          border: '1px solid',
          borderColor: page >= totalPages ? '#333' : '#e10600',
          '&:hover': {
            bgcolor: page >= totalPages ? 'rgba(255,255,255,0.05)' : 'rgba(225,6,0,0.1)',
            borderColor: page >= totalPages ? '#333' : '#ff0000'
          },
          '&:disabled': {
            color: '#666',
            borderColor: '#333'
          }
        }}
      >
        <ChevronRight />
      </IconButton>

      {/* Page Info */}
      <Typography
        variant="body2"
        sx={{
          color: '#888',
          ml: 2,
          display: { xs: 'none', sm: 'block' }
        }}
      >
        Page {page} of {totalPages} ({total} total items)
      </Typography>
    </Box>
  );
};

export default Pagination;
