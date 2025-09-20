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
import VpnKeyIcon from '@mui/icons-material/VpnKey';

// project imports
import { createPermission } from 'api/permission';

// ==============================|| PERMISSION FORM MODAL ||============================== //

export default function PermissionFormModal({ open, onClose, onSuccess }) {
  const [form, setForm] = useState({
    roleId: '',
    functionId: '',
    actionId: ''
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
      const res = await createPermission(form);
      if (res.data) {
        // Reset form
        setForm({
          roleId: '',
          functionId: '',
          actionId: ''
        });
        setLoading(false);
        onSuccess();
        onClose();
      } else {
        setError(res.error?.message || 'Tạo permission thất bại');
        setLoading(false);
      }
    } catch (err) {
      setError('Có lỗi xảy ra khi tạo permission');
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
      maxWidth="sm"
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
            <VpnKeyIcon color="primary" />
            <Typography variant="h4" component="div">
              Tạo Permission Mới
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
            <Grid item xs={12}>
              <TextField
                label="Role ID"
                name="roleId"
                value={form.roleId}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Nhập Role ID"
              />
            </Grid>
            
            <Grid item xs={12}>
              <TextField
                label="Function ID"
                name="functionId"
                value={form.functionId}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Nhập Function ID"
              />
            </Grid>
            
            <Grid item xs={12}>
              <TextField
                label="Action ID"
                name="actionId"
                value={form.actionId}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Nhập Action ID"
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
            startIcon={loading ? <CircularProgress size={16} /> : <VpnKeyIcon />}
            color="primary"
            sx={{ minWidth: 130 }}
          >
            {loading ? 'Đang tạo...' : 'Tạo Permission'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

PermissionFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func.isRequired
};