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
import VpnKeyIcon from '@mui/icons-material/VpnKey';
import { getPermissions } from 'api/permission';
import PermissionFormModal from 'components/forms/PermissionFormModal';

export default function PermissionPage() {
  const [permissions, setPermissions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchPermissions = async () => {
    setLoading(true);
    const res = await getPermissions();
    if (res.data && res.data.value && Array.isArray(res.data.value.permissions)) {
      setPermissions(res.data.value.permissions);
    } else {
      setPermissions([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchPermissions();
  }, []);

  const handleModalSuccess = () => {
    fetchPermissions();
  };

  return (
    <MainCard 
      title="Permission Management"
      secondary={
        <Button
          variant="contained"
          startIcon={<VpnKeyIcon />}
          onClick={() => setModalOpen(true)}
          color="primary"
          size="medium"
        >
          Thêm Permission Mới
        </Button>
      }
    >
      {loading && <LinearProgress sx={{ mb: 2 }} />}
      
      <Box sx={{ mb: 3 }}>
        <Typography variant="h6" sx={{ mb: 1 }}>
          Danh sách Permission
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Quản lý các quyền truy cập chức năng trong hệ thống
        </Typography>
      </Box>

      <TableContainer component={Paper} sx={{ boxShadow: 1 }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 600 }}>Role ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Function ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Action ID</TableCell>
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
            ) : permissions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2" color="text.secondary">
                      Không có permission nào
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              permissions.map((p, idx) => (
                <TableRow key={p.permissionId || idx} hover>
                  <TableCell>
                    <Typography 
                      variant="caption" 
                      sx={{ 
                        px: 1.5, 
                        py: 0.5, 
                        borderRadius: 1,
                        bgcolor: 'info.light',
                        color: 'info.dark',
                        fontFamily: 'monospace'
                      }}
                    >
                      {p.roleId}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography 
                      variant="caption" 
                      sx={{ 
                        px: 1.5, 
                        py: 0.5, 
                        borderRadius: 1,
                        bgcolor: 'secondary.light',
                        color: 'secondary.dark',
                        fontFamily: 'monospace'
                      }}
                    >
                      {p.functionId}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography 
                      variant="caption" 
                      sx={{ 
                        px: 1.5, 
                        py: 0.5, 
                        borderRadius: 1,
                        bgcolor: 'warning.light',
                        color: 'warning.dark',
                        fontFamily: 'monospace'
                      }}
                    >
                      {p.actionId}
                    </Typography>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <PermissionFormModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSuccess={handleModalSuccess}
      />
    </MainCard>
  );
}
