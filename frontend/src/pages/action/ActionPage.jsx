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
import Chip from '@mui/material/Chip';
import IconButton from '@mui/material/IconButton';
import TextField from '@mui/material/TextField';
import InputAdornment from '@mui/material/InputAdornment';
import Pagination from '@mui/material/Pagination';

// icons
import SettingsIcon from '@mui/icons-material/Settings';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';

// project imports
import { getActions, deleteAction } from 'api/action';
import ActionFormModal from 'components/forms/ActionFormModal';

export default function ActionPage() {
  const [actions, setActions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  // Modal states
  const [showForm, setShowForm] = useState(false);
  const [editingAction, setEditingAction] = useState(null);

  // Load actions
  const loadActions = async () => {
    setLoading(true);
    try {
      const response = await getActions(page, pageSize, searchTerm);
      if (response.data && response.data.value) {
        setActions(response.data.value.actions || []);
        setTotalCount(response.data.value.totalCount || 0);
        setTotalPages(Math.ceil((response.data.value.totalCount || 0) / pageSize));
      }
    } catch (error) {
      console.error('Error loading actions:', error);
    }
    setLoading(false);
  };

  useEffect(() => {
    loadActions();
  }, [page, searchTerm]);

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
    setPage(1); // Reset to first page when searching
  };

  const handlePageChange = (event, newPage) => {
    setPage(newPage);
  };

  const handleAdd = () => {
    setEditingAction(null);
    setShowForm(true);
  };

  const handleEdit = (action) => {
    setEditingAction(action);
    setShowForm(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bạn có chắc chắn muốn xóa action này?')) {
      try {
        await deleteAction(id);
        loadActions(); // Reload list
      } catch (error) {
        console.error('Error deleting action:', error);
        alert('Không thể xóa action này');
      }
    }
  };

  const handleFormSuccess = () => {
    loadActions(); // Reload list after successful create/update
  };

  return (
    <MainCard>
      {/* Header */}
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <Box>
          <Typography variant="h6" sx={{ mb: 1 }}>
            Quản lý Actions
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Quản lý các hành động trong hệ thống
          </Typography>
        </Box>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          sx={{ minWidth: 150 }}
        >
          Thêm Action
        </Button>
      </Box>

      {/* Search */}
      <Box sx={{ mb: 3 }}>
        <TextField
          placeholder="Tìm kiếm action..."
          value={searchTerm}
          onChange={handleSearch}
          variant="outlined"
          size="small"
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            ),
          }}
          sx={{ width: 300 }}
        />
      </Box>

      {/* Loading */}
      {loading && <LinearProgress sx={{ mb: 2 }} />}

      {/* Table */}
      <TableContainer component={Paper} sx={{ boxShadow: 1 }}>
        <Table size="small">
          <TableHead>
            <TableRow sx={{ bgcolor: 'grey.50' }}>
              <TableCell sx={{ fontWeight: 600 }}>ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Tên Action</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Thứ tự</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Trạng thái</TableCell>
              <TableCell sx={{ fontWeight: 600, textAlign: 'center' }}>Hành động</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={5} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <Typography variant="body2">Đang tải...</Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : actions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={5} align="center" sx={{ py: 3 }}>
                  <Stack alignItems="center" spacing={1}>
                    <SettingsIcon color="disabled" sx={{ fontSize: 48 }} />
                    <Typography variant="body2" color="text.secondary">
                      Không có action nào
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              actions.map((action, idx) => (
                <TableRow key={action.id || idx} hover>
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
                      {action.id}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" fontWeight={500}>
                      {action.name}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2" color="text.secondary">
                      {action.sortOrder ?? '-'}
                    </Typography>
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={action.isActive ? 'Hoạt động' : 'Không hoạt động'}
                      color={action.isActive ? 'success' : 'default'}
                      size="small"
                      variant="outlined"
                    />
                  </TableCell>
                  <TableCell align="center">
                    <Stack direction="row" spacing={1} justifyContent="center">
                      <IconButton 
                        size="small" 
                        color="primary"
                        onClick={() => handleEdit(action)}
                        title="Chỉnh sửa"
                      >
                        <EditIcon fontSize="small" />
                      </IconButton>
                      <IconButton 
                        size="small" 
                        color="error"
                        onClick={() => handleDelete(action.id)}
                        title="Xóa"
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

      {/* Pagination */}
      {totalPages > 1 && (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
          <Pagination
            count={totalPages}
            page={page}
            onChange={handlePageChange}
            color="primary"
            size="medium"
          />
        </Box>
      )}

      {/* Status */}
      {!loading && (
        <Typography variant="caption" color="text.secondary" sx={{ mt: 2, display: 'block', textAlign: 'center' }}>
          Hiển thị {actions.length} / {totalCount} actions
        </Typography>
      )}

      {/* Action Form Modal */}
      <ActionFormModal
        open={showForm}
        onClose={() => setShowForm(false)}
        onSuccess={handleFormSuccess}
        editingAction={editingAction}
      />
    </MainCard>
  );
}