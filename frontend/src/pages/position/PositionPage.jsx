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
import IconButton from '@mui/material/IconButton';
import Chip from '@mui/material/Chip';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';
import { getPositions, createPosition, updatePosition, deletePosition } from 'api/position';
import PositionFormModal from 'components/forms/PositionFormModal';

export default function PositionPage() {
  const [positions, setPositions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editPosition, setEditPosition] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [positionToDelete, setPositionToDelete] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [pagination, setPagination] = useState({
    page: 1,
    pageSize: 10,
    totalCount: 0
  });

  const fetchPositions = async (page = 1, searchTerm = '') => {
    setLoading(true);
    try {
      const res = await getPositions(page, pagination.pageSize, searchTerm);
      if (res.data && res.data.value) {
        setPositions(res.data.value.Positions || []);
        setPagination(prev => ({
          ...prev,
          page: res.data.value.Page || 1,
          totalCount: res.data.value.TotalCount || 0
        }));
      } else {
        setPositions([]);
      }
    } catch (error) {
      console.error('Error fetching positions:', error);
      setPositions([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchPositions(1, searchTerm);
  }, [searchTerm]);

  const handleModalSuccess = () => {
    fetchPositions(pagination.page, searchTerm);
    setModalOpen(false);
    setEditPosition(null);
  };

  const handleEditPosition = (position) => {
    setEditPosition(position);
    setModalOpen(true);
  };

  const handleDeleteClick = (position) => {
    setPositionToDelete(position);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!positionToDelete) return;
    
    try {
      const res = await deletePosition(positionToDelete.PositionId);
      if (res.error) {
        alert('Không thể xóa position: ' + (res.error.message || 'Có lỗi xảy ra'));
      } else {
        fetchPositions(pagination.page, searchTerm);
      }
    } catch (error) {
      alert('Có lỗi xảy ra khi xóa position');
    }
    
    setDeleteDialogOpen(false);
    setPositionToDelete(null);
  };

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  return (
    <MainCard
      title="Position Management"
      secondary={
        <Button 
          variant="contained" 
          startIcon={<AddIcon />} 
          onClick={() => setModalOpen(true)} 
          color="primary" 
          size="medium"
        >
          Thêm Position Mới
        </Button>
      }
    >
      {loading && <LinearProgress sx={{ mb: 2 }} />}

      <Box sx={{ mb: 3 }}>
        <Typography variant="h6" sx={{ mb: 1 }}>
          Danh sách Position
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
          Quản lý các vị trí công việc trong hệ thống
        </Typography>
        
        <TextField
          placeholder="Tìm kiếm position..."
          variant="outlined"
          size="small"
          value={searchTerm}
          onChange={handleSearchChange}
          InputProps={{
            startAdornment: <SearchIcon sx={{ color: 'text.secondary', mr: 1 }} />
          }}
          sx={{ maxWidth: 300 }}
        />
      </Box>

      <TableContainer component={Paper} sx={{ boxShadow: 1 }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 600 }}>Tên Position</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Mã Code</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Level</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Mô tả</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Trạng thái</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Thao tác</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={6} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2">Đang tải...</Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : positions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={6} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2" color="text.secondary">
                      Không có position nào
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              positions.map((position) => (
                <TableRow key={position.PositionId} hover>
                  <TableCell>{position.Name}</TableCell>
                  <TableCell>
                    <Chip 
                      label={position.Code} 
                      size="small" 
                      variant="outlined" 
                    />
                  </TableCell>
                  <TableCell>{position.Level}</TableCell>
                  <TableCell sx={{ maxWidth: 200 }}>
                    <Typography variant="body2" noWrap>
                      -
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={position.IsActive ? 'Hoạt động' : 'Không hoạt động'}
                      color={position.IsActive ? 'success' : 'default'}
                      size="small"
                    />
                  </TableCell>
                  <TableCell>
                    <Stack direction="row" spacing={1}>
                      <IconButton
                        size="small"
                        onClick={() => handleEditPosition(position)}
                        color="primary"
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                      <IconButton
                        size="small"
                        onClick={() => handleDeleteClick(position)}
                        color="error"
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Stack>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Position Form Modal */}
      <PositionFormModal
        open={modalOpen}
        onClose={() => {
          setModalOpen(false);
          setEditPosition(null);
        }}
        onSuccess={handleModalSuccess}
        editPosition={editPosition}
      />

      {/* Delete Confirmation Dialog */}
      <Dialog open={deleteDialogOpen} onClose={() => setDeleteDialogOpen(false)}>
        <DialogTitle>Xác nhận xóa Position</DialogTitle>
        <DialogContent>
          <Typography>
            Bạn có chắc chắn muốn xóa position "{positionToDelete?.Name}"? 
            Hành động này không thể hoàn tác.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Hủy</Button>
          <Button onClick={handleDeleteConfirm} color="error" variant="contained">
            Xóa
          </Button>
        </DialogActions>
      </Dialog>
    </MainCard>
  );
}