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
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import Alert from '@mui/material/Alert';

// material-ui icons
import CloseIcon from '@mui/icons-material/Close';
import PersonAddIcon from '@mui/icons-material/PersonAdd';

// project imports
import { createUser } from 'api/user';
import { getActivePositions } from 'api/position';
import { getManagerOptions } from 'api/user';

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
    positionId: '',
    // Profile fields
    phone: '',
    address: '',
    city: '',
    country: '',
    bio: '',
    website: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [positions, setPositions] = useState([]);
  const [managers, setManagers] = useState([]);
  const [loadingOptions, setLoadingOptions] = useState(false);
  const [optionsError, setOptionsError] = useState('');

  // Load positions and managers when modal opens
  useEffect(() => {
    if (open) {
      loadOptions();
    }
  }, [open]);

  const loadOptions = async () => {
    setLoadingOptions(true);
    setOptionsError('');
    try {
      const [positionsRes, managersRes] = await Promise.all([getActivePositions(), getManagerOptions()]);

      // Check for API errors
      if (positionsRes.error) {
        console.error('Error loading positions:', positionsRes.error);
        setOptionsError('Không thể tải danh sách vị trí. Vui lòng thử lại.');
      } else if (positionsRes.data && positionsRes.data.value) {
        setPositions(positionsRes.data.value.positions || []);
      }

      if (managersRes.error) {
        console.error('Error loading managers:', managersRes.error);
        setOptionsError((prev) =>
          prev ? `${prev} Không thể tải danh sách manager.` : 'Không thể tải danh sách manager. Vui lòng thử lại.'
        );
      } else if (managersRes.data && managersRes.data.value) {
        setManagers(managersRes.data.value.users || []);
      }
    } catch (error) {
      console.error('Error loading options:', error);
      setOptionsError('Có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại.');
    }
    setLoadingOptions(false);
  };

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
          positionId: '',
          // Profile fields
          phone: '',
          address: '',
          city: '',
          country: '',
          bio: '',
          website: ''
        });
        setLoading(false);
        onSuccess();
        onClose();
      } else {
        setError(res.error?.message || 'Tạo user thất bại');
        setLoading(false);
      }
    } catch (error) {
      console.error('Error creating user:', error);
      setError('Có lỗi xảy ra khi tạo user');
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (!loading && !loadingOptions) {
      setError('');
      setOptionsError('');
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
        positionId: '',
        // Profile fields
        phone: '',
        address: '',
        city: '',
        country: '',
        bio: '',
        website: ''
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
            <PersonAddIcon color="primary" />
            <Typography variant="h4" component="div">
              Tạo User Mới
            </Typography>
          </Stack>
          <IconButton
            aria-label="close"
            onClick={handleClose}
            disabled={loading || loadingOptions}
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
          {optionsError && (
            <Alert
              severity="warning"
              sx={{ mb: 3 }}
              action={
                <Button color="inherit" size="small" onClick={loadOptions} disabled={loadingOptions}>
                  Thử lại
                </Button>
              }
            >
              {optionsError}
            </Alert>
          )}

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
              <FormControl fullWidth disabled={loading || loadingOptions} variant="outlined" size="medium">
                <InputLabel>Manager</InputLabel>
                <Select
                  name="managerId"
                  value={form.managerId}
                  onChange={handleChange}
                  label="Manager"
                  endAdornment={loadingOptions ? <CircularProgress size={20} sx={{ mr: 2 }} /> : null}
                >
                  {loadingOptions ? (
                    <MenuItem disabled>
                      <em>Đang tải danh sách manager...</em>
                    </MenuItem>
                  ) : (
                    <>
                      <MenuItem value="">
                        <em>Không có manager</em>
                      </MenuItem>
                      {managers.map((manager) => (
                        <MenuItem key={manager.userId} value={manager.userId}>
                          {manager.fullName} ({manager.email})
                        </MenuItem>
                      ))}
                    </>
                  )}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12} sm={6}>
              <FormControl fullWidth disabled={loading || loadingOptions} variant="outlined" size="medium" required>
                <InputLabel>Position *</InputLabel>
                <Select
                  name="positionId"
                  value={form.positionId}
                  onChange={handleChange}
                  label="Position *"
                  endAdornment={loadingOptions ? <CircularProgress size={20} sx={{ mr: 2 }} /> : null}
                >
                  {loadingOptions ? (
                    <MenuItem disabled>
                      <em>Đang tải danh sách vị trí...</em>
                    </MenuItem>
                  ) : (
                    positions.map((position) => (
                      <MenuItem key={position.positionId} value={position.positionId}>
                        {position.name} (Level {position.level})
                      </MenuItem>
                    ))
                  )}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 1, fontWeight: 600 }}>
                Quyền hạn
              </Typography>
              <Stack direction="row" spacing={3}>
                <FormControlLabel
                  control={
                    <Checkbox checked={form.isDirector} onChange={handleChange} name="isDirector" disabled={loading} color="primary" />
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

            {/* Profile Information Section */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, fontWeight: 600 }}>
                Thông tin cá nhân
              </Typography>
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Số điện thoại"
                name="phone"
                value={form.phone}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Website"
                name="website"
                value={form.website}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                placeholder="https://example.com"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Thành phố"
                name="city"
                value={form.city}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <TextField
                label="Quốc gia"
                name="country"
                value={form.country}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Địa chỉ"
                name="address"
                value={form.address}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                multiline
                rows={2}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Tiểu sử"
                name="bio"
                value={form.bio}
                onChange={handleChange}
                fullWidth
                disabled={loading}
                variant="outlined"
                size="medium"
                multiline
                rows={3}
                placeholder="Mô tả ngắn về bản thân..."
              />
            </Grid>

            {error && (
              <Grid item xs={12}>
                <Alert severity="error" sx={{ mt: 1 }}>
                  {error}
                </Alert>
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
