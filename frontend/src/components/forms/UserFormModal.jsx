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
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';
import CircularProgress from '@mui/material/CircularProgress';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import PersonAddIcon from '@mui/icons-material/PersonAdd';

// project imports
import { createUser } from 'api/user';

// ==============================|| USER FORM MODAL ||============================== //

export default function UserFormModal({ open, onClose, onSuccess }) {
  const [form, setForm] = useState({
    userName: '',
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    dayOfBirth: '',
    isDirector: false,
    isHeadOfDepartment: false,
    managerId: '',
    positionId: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;
    
    setError('');
    setLoading(true);
    
    try {
      const res = await createUser(form);
      if (res.data) {
        // Reset form
        setForm({
          userName: '',
          email: '',
          password: '',
          firstName: '',
          lastName: '',
          dayOfBirth: '',
          isDirector: false,
          isHeadOfDepartment: false,
          managerId: '',
          positionId: ''
        });
        setLoading(false);
        onSuccess();
        onClose();
      } else {
        setError(res.error?.message || 'Tạo user thất bại');
        setLoading(false);
      }
    } catch (err) {
      setError('Có lỗi xảy ra khi tạo user');
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (!loading) {
      setError('');
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
            <PersonAddIcon color="primary" />
            <Typography variant="h4" component="div">
              Tạo User Mới
            </Typography>
          </Stack>
          <IconButton
            aria-label="close"
            onClick={handleClose}
            disabled={loading}
            sx={{
              color: (theme) => theme.palette.grey[500],
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
            <Grid item xs={12} sm={6}>
              <TextField
                label="User Name"
                name="userName"
                value={form.userName}
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
                label="Email"
                name="email"
                type="email"
                value={form.email}
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
                label="Password"
                name="password"
                type="password"
                value={form.password}
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
                label="Day of Birth"
                name="dayOfBirth"
                type="date"
                value={form.dayOfBirth}
                onChange={handleChange}
                InputLabelProps={{ shrink: true }}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>
            
            <Grid item xs={12} sm={6}>
              <TextField
                label="First Name"
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
                label="Last Name"
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
                label="Manager ID"
                name="managerId"
                value={form.managerId}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>
            
            <Grid item xs={12} sm={6}>
              <TextField
                label="Position ID"
                name="positionId"
                value={form.positionId}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>
            
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 1, fontWeight: 600 }}>
                Quyền hạn
              </Typography>
              <Stack direction="row" spacing={3}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={form.isDirector}
                      onChange={handleChange}
                      name="isDirector"
                      disabled={loading}
                      color="primary"
                    />
                  }
                  label="Is Director"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={form.isHeadOfDepartment}
                      onChange={handleChange}
                      name="isHeadOfDepartment"
                      disabled={loading}
                      color="primary"
                    />
                  }
                  label="Is Head Of Department"
                />
              </Stack>
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
            {loading ? 'Đang tạo...' : 'Tạo User'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

UserFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func.isRequired
};