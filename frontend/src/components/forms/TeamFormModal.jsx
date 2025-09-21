import { useState } from 'react';
import PropTypes from 'prop-types';

// material-ui
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';
import CircularProgress from '@mui/material/CircularProgress';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import GroupsIcon from '@mui/icons-material/Groups';

// project imports
import { createTeam } from '../../api/teams';

// ==============================|| TEAM FORM MODAL ||============================== //

export default function TeamFormModal({ open, onClose, onSuccess, token }) {
  const [form, setForm] = useState({
    name: '',
    shortName: '',
    countryCode: '',
    countryName: '',
    countryFlag: '',
    foundedYear: '',
    description: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;

    setError('');
    setLoading(true);

    try {
      const result = await createTeam(form, token);
      if (result && !result.error) {
        // Reset form
        setForm({
          name: '',
          shortName: '',
          countryCode: '',
          countryName: '',
          countryFlag: '',
          foundedYear: '',
          description: ''
        });
        setLoading(false);
        onSuccess();
        onClose();
      } else {
        setError(result.error?.message || 'Tạo team thất bại');
        setLoading(false);
      }
    } catch (err) {
      setError(err.response?.data?.detail || 'Có lỗi xảy ra khi tạo team');
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (!loading) {
      setError('');
      setForm({
        name: '',
        shortName: '',
        countryCode: '',
        countryName: '',
        countryFlag: '',
        foundedYear: '',
        description: ''
      });
      onClose();
    }
  };

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      maxWidth="md"
      fullWidth
      PaperProps={{
        sx: {
          borderRadius: 2,
          boxShadow: (theme) => theme.customShadows?.z16 || 16
        }
      }}
    >
      <DialogTitle>
        <Stack direction="row" alignItems="center" justifyContent="space-between">
          <Stack direction="row" alignItems="center" spacing={2}>
            <GroupsIcon color="primary" />
            <Typography variant="h4" component="div">
              Tạo Team MotoGP Mới
            </Typography>
          </Stack>
          <IconButton
            aria-label="close"
            onClick={handleClose}
            disabled={loading}
            sx={{
              color: (theme) => theme.palette.grey[500]
            }}
          >
            <CloseIcon />
          </IconButton>
        </Stack>
      </DialogTitle>

      <Divider />

      <form onSubmit={handleSubmit}>
        <DialogContent sx={{ py: 3 }}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <TextField
                label="Tên Team"
                name="name"
                value={form.name}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Nhập tên đầy đủ của team"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                label="Tên Viết Tắt"
                name="shortName"
                value={form.shortName}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: YAM, HON, DUC"
              />
            </Grid>

            <Grid item xs={12} md={4}>
              <TextField
                label="Mã Quốc Gia"
                name="countryCode"
                value={form.countryCode}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: JP, IT, ES"
                inputProps={{ maxLength: 3, style: { textTransform: 'uppercase' } }}
              />
            </Grid>

            <Grid item xs={12} md={8}>
              <TextField
                label="Tên Quốc Gia"
                name="countryName"
                value={form.countryName}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: Japan, Italy, Spain"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                label="Cờ Quốc Gia"
                name="countryFlag"
                value={form.countryFlag}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: 🇯🇵, 🇮🇹, 🇪🇸"
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <TextField
                label="Năm Thành Lập"
                name="foundedYear"
                value={form.foundedYear}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                type="date"
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Mô Tả"
                name="description"
                value={form.description}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                multiline
                rows={4}
                placeholder="Nhập thông tin mô tả về team (tùy chọn)"
              />
            </Grid>

            {error && (
              <Grid item xs={12}>
                <Typography color="error" variant="body2" sx={{ mt: 1 }}>
                  {error}
                </Typography>
              </Grid>
            )}
          </Grid>
        </DialogContent>

        <Divider />

        <DialogActions sx={{ p: 3 }}>
          <Button onClick={handleClose} disabled={loading} color="inherit" variant="outlined">
            Hủy
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={loading || !form.name || !form.shortName || !form.countryCode || !form.countryName || !form.countryFlag || !form.foundedYear}
            startIcon={loading ? <CircularProgress size={16} /> : <GroupsIcon />}
            color="primary"
            sx={{ minWidth: 120 }}
          >
            {loading ? 'Đang tạo...' : 'Tạo Team'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

TeamFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func.isRequired,
  token: PropTypes.string.isRequired
};