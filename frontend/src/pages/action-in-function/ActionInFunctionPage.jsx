import { useState, useEffect, useCallback } from 'react';

// material-ui
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';
import Chip from '@mui/material/Chip';
import LinearProgress from '@mui/material/LinearProgress';
import Pagination from '@mui/material/Pagination';
import Stack from '@mui/material/Stack';
import Tooltip from '@mui/material/Tooltip';

// material-ui icons
import AddIcon from '@mui/icons-material/Add';
import SearchIcon from '@mui/icons-material/Search';
import DeleteIcon from '@mui/icons-material/Delete';
import LinkIcon from '@mui/icons-material/Link';

// project imports
import MainCard from 'components/MainCard';
import { getActionInFunctions, deleteActionInFunction } from 'api/actionInFunction';
import ActionInFunctionFormModal from 'components/forms/ActionInFunctionFormModal';

// utils
import { showNotification } from 'utils/notification';

export default function ActionInFunctionPage() {
  const [actionInFunctions, setActionInFunctions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [totalCount, setTotalCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [showForm, setShowForm] = useState(false);
  const pageSize = 20;

  // Load ActionInFunctions
  const loadActionInFunctions = useCallback(async (page = 1, search = searchTerm) => {
    setLoading(true);
    try {
      const result = await getActionInFunctions(page, pageSize, search || null);
      
      if (result.error) {
        showNotification('error', 'Có lỗi xảy ra khi tải danh sách ActionInFunction');
        console.error('Load ActionInFunctions error:', result.error);
        return;
      }

      setActionInFunctions(result.data?.actionInFunctions || []);
      setTotalCount(result.data?.totalCount || 0);
      setCurrentPage(page);
    } catch (error) {
      showNotification('error', 'Có lỗi xảy ra khi tải danh sách ActionInFunction');
      console.error('Load ActionInFunctions error:', error);
    } finally {
      setLoading(false);
    }
  }, [searchTerm]);

  // Initial load
  useEffect(() => {
    loadActionInFunctions(1);
  }, []);

  // Handle search
  const handleSearch = (event) => {
    const value = event.target.value;
    setSearchTerm(value);
    
    // Debounce search
    const timeoutId = setTimeout(() => {
      loadActionInFunctions(1, value);
    }, 500);

    return () => clearTimeout(timeoutId);
  };

  // Handle pagination
  const handlePageChange = (event, page) => {
    loadActionInFunctions(page);
  };

  // Handle add
  const handleAdd = () => {
    setShowForm(true);
  };

  // Handle delete
  const handleDelete = async (actionId, functionId) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa ActionInFunction này?')) {
      return;
    }

    try {
      const result = await deleteActionInFunction(actionId, functionId);
      
      if (result.error) {
        showNotification('error', 'Có lỗi xảy ra khi xóa ActionInFunction');
        console.error('Delete ActionInFunction error:', result.error);
        return;
      }

      showNotification('success', 'Xóa ActionInFunction thành công');
      loadActionInFunctions(currentPage);
    } catch (error) {
      showNotification('error', 'Có lỗi xảy ra khi xóa ActionInFunction');
      console.error('Delete ActionInFunction error:', error);
    }
  };

  // Handle form success
  const handleFormSuccess = () => {
    loadActionInFunctions(1); // Reload list after successful create
    showNotification('success', 'Tạo ActionInFunction thành công');
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <MainCard>
      {/* Header */}
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <Box>
          <Typography variant="h6" sx={{ mb: 1 }}>
            Quản lý ActionInFunction
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Quản lý mối quan hệ giữa Action và Function trong hệ thống
          </Typography>
        </Box>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAdd}
          sx={{ minWidth: 180 }}
        >
          Thêm ActionInFunction
        </Button>
      </Box>

      {/* Search */}
      <Box sx={{ mb: 3 }}>
        <TextField
          placeholder="Tìm kiếm action, function..."
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
              <TableCell sx={{ fontWeight: 600 }}>Action ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Action Name</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Function ID</TableCell>
              <TableCell sx={{ fontWeight: 600 }}>Function Name</TableCell>
              <TableCell sx={{ fontWeight: 600, textAlign: 'center' }}>Hành động</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {actionInFunctions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={5} sx={{ textAlign: 'center', py: 4 }}>
                  <Stack spacing={1} alignItems="center">
                    <LinkIcon sx={{ fontSize: 48, color: 'text.disabled' }} />
                    <Typography variant="body2" color="text.secondary">
                      {loading ? 'Đang tải...' : 'Không có dữ liệu ActionInFunction'}
                    </Typography>
                  </Stack>
                </TableCell>
              </TableRow>
            ) : (
              actionInFunctions.map((item) => (
                <TableRow key={`${item.actionId}-${item.functionId}`} hover>
                  <TableCell>
                    <Chip label={item.actionId} variant="outlined" size="small" />
                  </TableCell>
                  <TableCell>{item.actionName}</TableCell>
                  <TableCell>
                    <Chip label={item.functionId} variant="outlined" size="small" color="primary" />
                  </TableCell>
                  <TableCell>{item.functionName}</TableCell>
                  <TableCell sx={{ textAlign: 'center' }}>
                    <Tooltip title="Xóa">
                      <IconButton
                        onClick={() => handleDelete(item.actionId, item.functionId)}
                        color="error"
                        size="small"
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Pagination */}
      {totalPages > 1 && (
        <Stack direction="row" justifyContent="center" sx={{ mt: 3 }}>
          <Pagination
            count={totalPages}
            page={currentPage}
            onChange={handlePageChange}
            color="primary"
            showFirstButton
            showLastButton
          />
        </Stack>
      )}

      {/* Summary */}
      {actionInFunctions.length > 0 && (
        <Typography variant="caption" color="text.secondary" sx={{ mt: 2, display: 'block', textAlign: 'center' }}>
          Hiển thị {actionInFunctions.length} / {totalCount} ActionInFunction
        </Typography>
      )}

      {/* ActionInFunction Form Modal */}
      <ActionInFunctionFormModal
        open={showForm}
        onClose={() => setShowForm(false)}
        onSuccess={handleFormSuccess}
      />
    </MainCard>
  );
}