import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// material-ui
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import Stack from '@mui/material/Stack';
import Divider from '@mui/material/Divider';
import CircularProgress from '@mui/material/CircularProgress';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import VpnKeyIcon from '@mui/icons-material/VpnKey';

// project imports
import { createPermission } from 'api/permission';
import { getRolesForDropdown } from 'api/role';
import { getActiveActions, getActiveFunctions } from 'api/action';

// ==============================|| PERMISSION FORM MODAL ||============================== //

export default function PermissionFormModal({ open, onClose, onSuccess }) {
  const [form, setForm] = useState({
    roleId: '',
    functionId: '',
    actionId: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Dropdown data
  const [roles, setRoles] = useState([]);
  const [functions, setFunctions] = useState([]);
  const [actions, setActions] = useState([]);
  const [loadingOptions, setLoadingOptions] = useState(false);

  // Load dropdown options when modal opens
  useEffect(() => {
    if (open) {
      loadOptions();
    }
  }, [open]);

  const loadOptions = async () => {
    setLoadingOptions(true);
    try {
      const [rolesRes, functionsRes, actionsRes] = await Promise.all([
        getRolesForDropdown(),
        getActiveFunctions(),
        getActiveActions()
      ]);

      if (rolesRes.data && rolesRes.data.value) {
        setRoles(rolesRes.data.value.roles || []);
      }

      if (functionsRes.data && functionsRes.data.value) {
        setFunctions(functionsRes.data.value.functions || []);
      }

      if (actionsRes.data && actionsRes.data.value) {
        setActions(actionsRes.data.value.actions || []);
      }
    } catch (error) {
      console.error('Error loading options:', error);
    }
    setLoadingOptions(false);
  };

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
              <FormControl fullWidth required disabled={loading || loadingOptions}>
                <InputLabel id="role-select-label">Role *</InputLabel>
                <Select
                  labelId="role-select-label"
                  name="roleId"
                  value={form.roleId}
                  onChange={handleChange}
                  label="Role *"
                >
                  {roles.map((role) => (
                    <MenuItem key={role.roleId} value={role.roleId}>
                      {role.name} ({role.roleCode})
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12}>
              <FormControl fullWidth required disabled={loading || loadingOptions}>
                <InputLabel id="function-select-label">Function *</InputLabel>
                <Select
                  labelId="function-select-label"
                  name="functionId"
                  value={form.functionId}
                  onChange={handleChange}
                  label="Function *"
                >
                  {functions.map((func) => (
                    <MenuItem key={func.id} value={func.id}>
                      {func.name}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12}>
              <FormControl fullWidth required disabled={loading || loadingOptions}>
                <InputLabel id="action-select-label">Action *</InputLabel>
                <Select
                  labelId="action-select-label"
                  name="actionId"
                  value={form.actionId}
                  onChange={handleChange}
                  label="Action *"
                >
                  {actions.map((action) => (
                    <MenuItem key={action.id} value={action.id}>
                      {action.name}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            {loadingOptions && (
              <Grid item xs={12}>
                <Stack direction="row" alignItems="center" spacing={1}>
                  <CircularProgress size={16} />
                  <Typography variant="body2" color="text.secondary">
                    Đang tải danh sách...
                  </Typography>
                </Stack>
              </Grid>
            )}

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
