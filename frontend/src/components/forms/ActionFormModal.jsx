import { useState, useEffect } from 'react';
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
import FormControlLabel from '@mui/material/FormControlLabel';
import Switch from '@mui/material/Switch';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import SettingsIcon from '@mui/icons-material/Settings';

// project imports
import { createAction, updateAction } from 'api/action';

// ==============================|| ACTION FORM MODAL ||============================== //

export default function ActionFormModal({ open, onClose, onSuccess, editingAction = null }) {
  const [form, setForm] = useState({
    id: '',
    name: '',
    sortOrder: '',
    isActive: true
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Initialize form when editing
  useEffect(() => {
    if (editingAction) {
      setForm({
        id: editingAction.id || '',
        name: editingAction.name || '',
        sortOrder: editingAction.sortOrder?.toString() || '',
        isActive: editingAction.isActive !== false
      });
    } else {
      setForm({
        id: '',
        name: '',
        sortOrder: '',
        isActive: true
      });
    }
    setError('');
  }, [editingAction, open]);

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
      const payload = {
        id: form.id,
        name: form.name,
        sortOrder: form.sortOrder ? parseInt(form.sortOrder, 10) : null,
        isActive: form.isActive
      };

      let res;
      if (editingAction) {
        res = await updateAction(editingAction.id, payload);
      } else {
        res = await createAction(payload);
      }

      if (res.data) {
        setLoading(false);
        onSuccess();
        onClose();
      } else {
        setError(res.error?.message || `${editingAction ? 'Cập nhật' : 'Tạo'} action thất bại`);
        setLoading(false);
      }
    } catch (err) {
      setError(`Có lỗi xảy ra khi ${editingAction ? 'cập nhật' : 'tạo'} action`);
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (!loading) {
      setError('');
      onClose();
    }
  };

  const isEditing = Boolean(editingAction);

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
            <SettingsIcon color="primary" />
            <Typography variant="h4" component="div">
              {isEditing ? 'Cập nhật Action' : 'Tạo Action Mới'}
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
            <Grid item xs={12}>
              <TextField
                label="Action ID *"
                name="id"
                value={form.id}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading || isEditing} // Cannot edit ID when updating
                variant="outlined"
                size="medium"
                placeholder="Nhập ID action (vd: CREATE, UPDATE, DELETE)"
                helperText={isEditing ? "ID không thể thay đổi khi cập nhật" : "ID duy nhất cho action"}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Tên Action *"
                name="name"
                value={form.name}
                onChange={handleChange}
                fullWidth
                required
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="Nhập tên action (vd: Tạo mới, Cập nhật, Xóa)"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Thứ tự sắp xếp"
                name="sortOrder"
                value={form.sortOrder}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                type="number"
                placeholder="Nhập thứ tự sắp xếp (tuỳ chọn)"
                helperText="Số thứ tự để sắp xếp action trong danh sách"
              />
            </Grid>

            <Grid item xs={12}>
              <FormControlLabel
                control={
                  <Switch
                    checked={form.isActive}
                    onChange={handleChange}
                    name="isActive"
                    disabled={loading}
                    color="primary"
                  />
                }
                label="Kích hoạt action"
              />
              <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 0.5 }}>
                Action chỉ có thể sử dụng khi được kích hoạt
              </Typography>
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
            disabled={loading}
            startIcon={loading ? <CircularProgress size={16} /> : <SettingsIcon />}
            color="primary"
            sx={{ minWidth: 130 }}
          >
            {loading ? (isEditing ? 'Đang cập nhật...' : 'Đang tạo...') : (isEditing ? 'Cập nhật' : 'Tạo Action')}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

ActionFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func.isRequired,
  editingAction: PropTypes.object
};