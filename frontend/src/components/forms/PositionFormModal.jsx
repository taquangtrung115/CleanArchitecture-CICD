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
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';
import CircularProgress from '@mui/material/CircularProgress';
import Alert from '@mui/material/Alert';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import WorkIcon from '@mui/icons-material/Work';

// project imports
import { createPosition, updatePosition } from 'api/position';
import { getErrorMessage } from '../../utils/errorHandler';

// ==============================|| POSITION FORM MODAL ||============================== //

export default function PositionFormModal({ open, onClose, onSuccess, editPosition }) {
  const [form, setForm] = useState({
    name: '',
    description: '',
    code: '',
    level: 1,
    isActive: true
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Populate form when editing
  useEffect(() => {
    if (editPosition) {
      setForm({
        name: editPosition.Name || '',
        description: editPosition.Description || '',
        code: editPosition.Code || '',
        level: editPosition.Level || 1,
        isActive: editPosition.IsActive ?? true
      });
    } else {
      setForm({
        name: '',
        description: '',
        code: '',
        level: 1,
        isActive: true
      });
    }
    setError('');
  }, [editPosition, open]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : type === 'number' ? Number(value) : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;

    setError('');
    setLoading(true);

    try {
      let res;
      if (editPosition) {
        res = await updatePosition(editPosition.positionId, form);
      } else {
        res = await createPosition(form);
      }

      if (res.data) {
        // Reset form
        setForm({
          name: '',
          description: '',
          code: '',
          level: 1,
          isActive: true
        });
        onSuccess();
        onClose();
      } else if (res.error) {
        setError(getErrorMessage(res) || `Có lỗi xảy ra khi ${editPosition ? 'cập nhật' : 'tạo'} position`);
      }
    } catch (error) {
      setError(`Có lỗi xảy ra khi ${editPosition ? 'cập nhật' : 'tạo'} position`);
    }

    setLoading(false);
  };

  const isFormValid = form.name.trim() && form.code.trim() && form.level > 0;

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle sx={{ m: 0, p: 2, display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
        <Stack direction="row" spacing={1} alignItems="center">
          <WorkIcon color="primary" />
          <Typography variant="h6">
            {editPosition ? 'Chỉnh sửa Position' : 'Thêm Position Mới'}
          </Typography>
        </Stack>
        <IconButton onClick={onClose} size="small">
          <CloseIcon />
        </IconButton>
      </DialogTitle>

      <Divider />

      <form onSubmit={handleSubmit}>
        <DialogContent sx={{ p: 3 }}>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Grid container spacing={3}>
            <Grid item xs={12} sm={6}>
              <TextField
                label="Tên Position *"
                name="name"
                value={form.name}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                helperText="Tên hiển thị của vị trí công việc"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Mã Code *"
                name="code"
                value={form.code}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                helperText="Mã định danh duy nhất (chỉ chữ in hoa, số và dấu gạch dưới)"
                inputProps={{ style: { textTransform: 'uppercase' } }}
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Level *"
                name="level"
                type="number"
                value={form.level}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                inputProps={{ min: 1 }}
                helperText="Cấp độ của position (số càng nhỏ càng cao cấp)"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <FormControlLabel
                control={
                  <Checkbox 
                    checked={form.isActive} 
                    onChange={handleChange} 
                    name="isActive" 
                    disabled={loading} 
                    color="primary" 
                  />
                }
                label="Trạng thái hoạt động"
                sx={{ mt: 2 }}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Mô tả"
                name="description"
                value={form.description}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                multiline
                rows={3}
                helperText="Mô tả chi tiết về vị trí công việc này"
              />
            </Grid>
          </Grid>
        </DialogContent>

        <Divider />

        <DialogActions sx={{ p: 2, justifyContent: 'space-between' }}>
          <Button 
            onClick={onClose} 
            disabled={loading}
            color="inherit"
          >
            Hủy
          </Button>
          <Button
            type="submit"
            variant="contained"
            disabled={loading || !isFormValid}
            startIcon={loading ? <CircularProgress size={16} color="inherit" /> : <WorkIcon />}
            color="primary"
          >
            {loading 
              ? (editPosition ? 'Đang cập nhật...' : 'Đang tạo...') 
              : (editPosition ? 'Cập nhật Position' : 'Tạo Position')
            }
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

PositionFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func.isRequired,
  editPosition: PropTypes.object
};