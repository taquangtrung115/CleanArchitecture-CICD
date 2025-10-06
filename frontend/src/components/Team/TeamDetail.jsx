import React from 'react';

// material-ui
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';

// material-ui icons
import FlagIcon from '@mui/icons-material/Flag';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import PeopleIcon from '@mui/icons-material/People';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';

const TeamDetail = ({ team }) => {
  if (!team) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Typography variant="body1" color="text.secondary">
          Chọn một team để xem chi tiết
        </Typography>
      </Box>
    );
  }

  return (
    <Box>
      {/* Team Header */}
      <Box sx={{ mb: 3 }}>
        <Typography variant="h4" gutterBottom sx={{ fontWeight: 'bold' }}>
          {team.name}
        </Typography>
        <Typography variant="h6" color="text.secondary" sx={{ mb: 2 }}>
          ({team.shortName})
        </Typography>
      </Box>

      <Divider sx={{ mb: 3 }} />

      {/* Team Information */}
      <Stack spacing={3}>
        {/* Country */}
        <Box>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Quốc Gia
          </Typography>
          <Stack direction="row" alignItems="center" spacing={1}>
            <Typography variant="h6" sx={{ fontSize: '1.5rem' }}>
              {team.countryFlag}
            </Typography>
            <Typography variant="body1" sx={{ fontWeight: 500 }}>
              {team.countryName}
            </Typography>
            <Chip label={team.countryCode} size="small" variant="outlined" icon={<FlagIcon />} />
          </Stack>
        </Box>

        {/* Founded Year */}
        <Box>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Năm Thành Lập
          </Typography>
          <Stack direction="row" alignItems="center" spacing={1}>
            <CalendarTodayIcon color="action" fontSize="small" />
            <Typography variant="body1">{team.foundedYear ? new Date(team.foundedYear).getFullYear() : 'Không có thông tin'}</Typography>
          </Stack>
        </Box>

        {/* Status */}
        <Box>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Trạng Thái
          </Typography>
          <Chip
            icon={team.isActive ? <CheckCircleIcon /> : <CancelIcon />}
            label={team.isActive ? 'Đang hoạt động' : 'Không hoạt động'}
            color={team.isActive ? 'success' : 'error'}
            variant="outlined"
          />
        </Box>

        {/* Capacity */}
        <Box>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            Sức Chứa
          </Typography>
          <Stack direction="row" alignItems="center" spacing={1}>
            <PeopleIcon color="action" fontSize="small" />
            <Typography variant="body1">{team.capacity || 2} riders</Typography>
          </Stack>
        </Box>

        {/* Description */}
        {team.description && (
          <Box>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              Mô Tả
            </Typography>
            <Typography variant="body2" sx={{ lineHeight: 1.6 }}>
              {team.description}
            </Typography>
          </Box>
        )}
      </Stack>
    </Box>
  );
};

export default TeamDetail;
