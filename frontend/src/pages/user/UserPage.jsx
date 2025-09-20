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
import PersonAddIcon from '@mui/icons-material/PersonAdd';
import { getUsers } from 'api/user';
import UserFormModal from 'components/forms/UserFormModal';

export default function UserPage() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  const fetchUsers = async () => {
    setLoading(true);
    const res = await getUsers();
    if (res.data && res.data.value && Array.isArray(res.data.value.users)) {
      setUsers(res.data.value.users);
    } else {
      setUsers([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const handleModalSuccess = () => {
    fetchUsers();
  };

  return (
    <MainCard 
      title="User Management"
      secondary={
        <Button
          variant="contained"
          startIcon={<PersonAddIcon />}
          onClick={() => setModalOpen(true)}
          color="primary"
          size="medium"
        >
          Thêm User Mới
        </Button>
      }
    >
      {loading && <LinearProgress sx={{ mb: 2 }} />}
      
      <Box sx={{ mb: 3 }}>
        <Typography variant="h6" sx={{ mb: 1 }}>
          Danh sách User
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Quản lý thông tin người dùng trong hệ thống
        </Typography>
      </Box>

      <TableContainer component={Paper} sx={{ boxShadow: 1 }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 600 }}>User Name</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Email</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Full Name</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Status</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={4} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2">Đang tải...</Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : users.length === 0 ? (
              <TableRow>
                <TableCell colSpan={4} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2" color="text.secondary">
                      Không có user nào
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              users.map((u) => (
                <TableRow key={u.userId || u.id} hover>
                  <TableCell>{u.userName}</TableCell>
                  <TableCell>{u.email}</TableCell>
                  <TableCell>{u.fullName}</TableCell>
                  <TableCell>
                    <Typography 
                      variant="caption" 
                      sx={{ 
                        px: 1.5, 
                        py: 0.5, 
                        borderRadius: 1,
                        bgcolor: u.isLocked ? 'error.light' : 'success.light',
                        color: u.isLocked ? 'error.dark' : 'success.dark'
                      }}
                    >
                      {u.isLocked ? 'Đã khóa' : 'Hoạt động'}
                    </Typography>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <UserFormModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSuccess={handleModalSuccess}
      />
    </MainCard>
  );
}
