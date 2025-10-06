import React, { useState } from 'react';

// material-ui
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import Grid from '@mui/material/Grid';
import Alert from '@mui/material/Alert';

// material-ui icons
import AddIcon from '@mui/icons-material/Add';
import SportsMotorsportsIcon from '@mui/icons-material/SportsMotorsports';

// project imports
import RiderList from '../components/Rider/RiderList';
import RiderDetail from '../components/Rider/RiderDetail';
import RiderFormModal from '../components/Rider/RiderForm';
import SearchBar from '../components/Shared/SearchBar';
import Pagination from '../components/Shared/Pagination';
import FilterPanel from '../components/Shared/FilterPanel';
import ConfirmDialog from '../components/Shared/ConfirmDialog';
import { createRider, deleteRider } from '../api/riders';

const countryOptions = [
  {
    name: 'countryCode',
    label: 'Quốc gia',
    choices: [
      { value: 'IT', label: 'Italy' },
      { value: 'ES', label: 'Spain' },
      { value: 'JP', label: 'Japan' },
      { value: 'VN', label: 'Vietnam' },
      { value: 'FR', label: 'France' },
      { value: 'GB', label: 'Great Britain' },
      { value: 'DE', label: 'Germany' },
      { value: 'AU', label: 'Australia' }
    ]
  }
];

const RidersPage = ({ token }) => {
  const [filters, setFilters] = useState({ pageIndex: 1, pageSize: 10 });
  const [selected, setSelected] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [refresh, setRefresh] = useState(0);
  const [loading, setLoading] = useState(false);

  const handleSearch = (q) => setFilters((f) => ({ ...f, ...q, pageIndex: 1 }));
  const handleFilter = (name, value) => setFilters((f) => ({ ...f, [name]: value, pageIndex: 1 }));
  const handlePage = (page, size) => setFilters((f) => ({ ...f, pageIndex: page, pageSize: size }));

  const handleCreate = async (data) => {
    setError(null);
    setSuccess(null);
    setLoading(true);

    try {
      await createRider(data, token);
      setShowForm(false);
      setRefresh((r) => r + 1);
      setSuccess('Rider đã được tạo thành công!');
    } catch (e) {
      setError(e.response?.data?.detail || 'Lỗi khi tạo rider');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!selected) return;

    setError(null);
    setSuccess(null);

    try {
      await deleteRider(selected.id, token);
      setConfirmDelete(false);
      setSelected(null);
      setRefresh((r) => r + 1);
      setSuccess('Rider đã được xóa thành công!');
    } catch (e) {
      setError(e.response?.data?.detail || 'Lỗi khi xóa rider');
    }
  };

  const handleCloseAlerts = () => {
    setError(null);
    setSuccess(null);
  };

  return (
    <Container maxWidth="xl" sx={{ py: 3 }}>
      {/* Header */}
      <Box sx={{ mb: 4 }}>
        <Stack direction="row" alignItems="center" spacing={2} sx={{ mb: 2 }}>
          <SportsMotorsportsIcon sx={{ fontSize: 40, color: 'primary.main' }} />
          <Typography variant="h3" component="h1" fontWeight="bold">
            Quản lý Riders MotoGP
          </Typography>
        </Stack>
        <Typography variant="body1" color="text.secondary">
          Quản lý thông tin các tay đua MotoGP
        </Typography>
      </Box>

      {/* Alerts */}
      {error && (
        <Alert severity="error" onClose={handleCloseAlerts} sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}
      {success && (
        <Alert severity="success" onClose={handleCloseAlerts} sx={{ mb: 3 }}>
          {success}
        </Alert>
      )}

      {/* Controls */}
      <Paper sx={{ p: 3, mb: 3 }}>
        <Grid container spacing={3} alignItems="center">
          <Grid item xs={12} md={4}>
            <SearchBar onSearch={handleSearch} />
          </Grid>
          <Grid item xs={12} md={4}>
            <FilterPanel filters={filters} onChange={handleFilter} options={countryOptions} />
          </Grid>
          <Grid item xs={12} md={4}>
            <Stack direction="row" justifyContent="flex-end">
              <Button variant="contained" startIcon={<AddIcon />} onClick={() => setShowForm(true)} size="large" sx={{ minWidth: 150 }}>
                Thêm Rider
              </Button>
            </Stack>
          </Grid>
        </Grid>
      </Paper>

      {/* Content Grid */}
      <Grid container spacing={3}>
        {/* Rider List */}
        <Grid item xs={12} lg={selected ? 8 : 12}>
          <Paper sx={{ p: 3 }}>
            <RiderList key={refresh} token={token} filters={filters} onSelect={setSelected} />
            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
              <Pagination page={filters.pageIndex} pageSize={filters.pageSize} total={100} onChange={handlePage} />
            </Box>
          </Paper>
        </Grid>

        {/* Rider Detail */}
        {selected && (
          <Grid item xs={12} lg={4}>
            <Paper sx={{ p: 3 }}>
              <RiderDetail rider={selected} />
              <Box sx={{ mt: 3 }}>
                <Button variant="outlined" color="error" fullWidth onClick={() => setConfirmDelete(true)}>
                  Xóa Rider
                </Button>
              </Box>
            </Paper>
          </Grid>
        )}
      </Grid>

      {/* Form Modal */}
      <RiderFormModal open={showForm} onClose={() => setShowForm(false)} onSubmit={handleCreate} loading={loading} error={error} />

      {/* Confirm Delete Dialog */}
      <ConfirmDialog
        open={confirmDelete}
        title="Xóa Rider"
        message={`Bạn có chắc chắn muốn xóa rider "${selected?.firstName} ${selected?.lastName}" không?`}
        onConfirm={handleDelete}
        onCancel={() => setConfirmDelete(false)}
      />
    </Container>
  );
};

export default RidersPage;
