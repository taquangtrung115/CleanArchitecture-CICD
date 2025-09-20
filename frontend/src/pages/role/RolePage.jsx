import { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import MainCard from 'components/MainCard';
import Button from '@mui/material/Button';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import LinearProgress from '@mui/material/LinearProgress';
import SecurityIcon from '@mui/icons-material/Security';
import { getRoles } from 'api/role';
import RoleFormModal from 'components/forms/RoleFormModal';

export default function RolePage() {
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchRoles = async () => {
    setLoading(true);
    const res = await getRoles();
    if (res.data && res.data.value && Array.isArray(res.data.value.roles)) {
      setRoles(res.data.value.roles);
    } else {
      setRoles([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchRoles();
  }, []);

  const handleModalSuccess = () => {
    fetchRoles();
  };

  return (
    <MainCard
      title="Role Management"
      secondary={
        <Button variant="contained" startIcon={<SecurityIcon />} onClick={() => setModalOpen(true)} color="primary" size="medium">
          Thêm Role Mới
        </Button>
      }
    >
      {loading && <LinearProgress sx={{ mb: 2 }} />}

      <Box sx={{ mb: 3 }}>
        <Typography variant="h6" sx={{ mb: 1 }}>
          Danh sách Role
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Quản lý các vai trò và quyền hạn trong hệ thống
        </Typography>
      </Box>

      <TableContainer component={Paper} sx={{ boxShadow: 1 }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 600 }}>Role Name</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Role Code</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Description</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={3} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2">Đang tải...</Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : roles.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2" color="text.secondary">
                      Không có role nào
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              roles.map((r) => (
                <TableRow key={r.roleId || r.id} hover>
                  <TableCell sx={{ fontWeight: 500 }}>{r.name}</TableCell>
                  <TableCell>
                    <Typography
                      variant="caption"
                      sx={{
                        px: 1.5,
                        py: 0.5,
                        borderRadius: 1,
                        bgcolor: 'primary.light',
                        color: 'primary.dark',
                        fontFamily: 'monospace'
                      }}
                    >
                      {r.roleCode}
                    </Typography>
                  </TableCell>
                  <TableCell>{r.description || '-'}</TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <RoleFormModal open={modalOpen} onClose={() => setModalOpen(false)} onSuccess={handleModalSuccess} />
    </MainCard>
  );
}
