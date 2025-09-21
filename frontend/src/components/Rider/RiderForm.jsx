import React, { useState } from 'react';
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
import Alert from '@mui/material/Alert';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import PersonAddIcon from '@mui/icons-material/PersonAdd';
import SportsMotorsportsIcon from '@mui/icons-material/SportsMotorsports';

const initialState = {
  firstName: '',
  lastName: '',
  racingNumber: '',
  countryCode: '',
  countryName: '',
  countryFlag: '',
  dateOfBirth: '',
  height: '',
  weight: '',
  nickname: ''
};

const RiderFormModal = ({ open, onClose, onSubmit, initial = initialState, loading, error }) => {
  const [form, setForm] = useState(initial);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (loading) return;
    onSubmit(form);
  };

  const handleClose = () => {
    if (!loading) {
      setForm(initialState);
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
        sx: { borderRadius: 2 }
      }}
    >
      <DialogTitle sx={{ pb: 2 }}>
        <Stack direction="row" alignItems="center" justifyContent="space-between">
          <Stack direction="row" alignItems="center" spacing={2}>
            <SportsMotorsportsIcon color="primary" />
            <Typography variant="h4" component="div">
              Tạo Rider Mới
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
          {error && (
            <Alert severity="error" sx={{ mb: 3 }}>
              {error}
            </Alert>
          )}
          
          <Grid container spacing={3}>
            <Grid item xs={12} sm={6}>
              <TextField
                label="Tên"
                name="firstName"
                value={form.firstName}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Họ"
                name="lastName"
                value={form.lastName}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Số đua"
                name="racingNumber"
                type="number"
                value={form.racingNumber}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                inputProps={{ min: 1, max: 99 }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Mã quốc gia"
                name="countryCode"
                value={form.countryCode}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: IT, ES, JP"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Tên quốc gia"
                name="countryName"
                value={form.countryName}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Cờ quốc gia"
                name="countryFlag"
                value={form.countryFlag}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="VD: 🇮🇹, 🇪🇸, 🇯🇵"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Ngày sinh"
                name="dateOfBirth"
                type="date"
                value={form.dateOfBirth}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Chiều cao (cm)"
                name="height"
                type="number"
                value={form.height}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                inputProps={{ min: 100 }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Cân nặng (kg)"
                name="weight"
                type="number"
                value={form.weight}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                inputProps={{ min: 30 }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Biệt danh"
                name="nickname"
                value={form.nickname}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Tùy chọn"
              />
            </Grid>
          </Grid>
        </DialogContent>

        <Divider />

        <DialogActions sx={{ p: 3 }}>
          <Button 
            onClick={handleClose} 
            disabled={loading} 
            color="inherit" 
            variant="outlined"
          >
            Hủy
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={loading}
            startIcon={loading ? <CircularProgress size={16} /> : <PersonAddIcon />}
            color="primary"
            sx={{ minWidth: 120 }}
          >
            {loading ? 'Đang lưu...' : 'Lưu'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

RiderFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  initial: PropTypes.object,
  loading: PropTypes.bool,
  error: PropTypes.string
};

export default RiderFormModal;
