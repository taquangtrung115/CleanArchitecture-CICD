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
import LinkIcon from '@mui/icons-material/Link';

// project imports
import { createActionInFunction } from 'api/actionInFunction';
import { getActiveActions } from 'api/action';
import { getActiveFunctions } from 'api/action'; // Functions are fetched from the same API

// ==============================|| ACTION IN FUNCTION FORM MODAL ||============================== //

export default function ActionInFunctionFormModal({ open, onClose, onSuccess }) {
  const [form, setForm] = useState({
    actionId: '',
    functionId: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [actions, setActions] = useState([]);
  const [functions, setFunctions] = useState([]);
  const [loadingDropdowns, setLoadingDropdowns] = useState(false);

  // Load dropdown data
  useEffect(() => {
    if (open) {
      loadDropdownData();
    }
  }, [open]);

  const loadDropdownData = async () => {
    setLoadingDropdowns(true);
    try {
      const [actionsResult, functionsResult] = await Promise.all([
        getActiveActions(),
        getActiveFunctions()
      ]);

      if (actionsResult.error) {
        console.error('Error loading actions:', actionsResult.error);
      } else {
        setActions(actionsResult.data?.actions || []);
      }

      if (functionsResult.error) {
        console.error('Error loading functions:', functionsResult.error);
      } else {
        setFunctions(functionsResult.data?.functions || []);
      }
    } catch (err) {
      console.error('Error loading dropdown data:', err);
    } finally {
      setLoadingDropdowns(false);
    }
  };

  // Reset form when dialog opens/closes
  useEffect(() => {
    if (open) {
      setForm({
        actionId: '',
        functionId: ''
      });
      setError('');
    }
  }, [open]);

  const handleInputChange = (field) => (event) => {
    setForm(prev => ({
      ...prev,
      [field]: event.target.value
    }));
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!form.actionId?.trim() || !form.functionId?.trim()) {
      setError('Vui lòng chọn Action và Function');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const result = await createActionInFunction({
        actionId: form.actionId.trim(),
        functionId: form.functionId.trim()
      });

      if (result.error) {
        setError(result.error.message || 'Có lỗi xảy ra khi tạo ActionInFunction');
        return;
      }

      onSuccess?.();
      handleClose();
    } catch (err) {
      setError('Có lỗi xảy ra khi tạo ActionInFunction');
      console.error('ActionInFunction creation error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    if (!loading) {
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
        sx: { borderRadius: 2 }
      }}
    >
      <DialogTitle>
        <Stack direction="row" justifyContent="space-between" alignItems="center">
          <Stack direction="row" spacing={1} alignItems="center">
            <LinkIcon color="primary" />
            <Typography variant="h6">Tạo ActionInFunction</Typography>
          </Stack>
          <IconButton 
            onClick={handleClose}
            disabled={loading}
            size="small"
          >
            <CloseIcon />
          </IconButton>
        </Stack>
      </DialogTitle>
      
      <Divider />
      
      <form onSubmit={handleSubmit}>
        <DialogContent sx={{ pt: 3 }}>
          <Grid container spacing={3}>
            {/* Action Selection */}
            <Grid item xs={12}>
              <FormControl fullWidth disabled={loadingDropdowns}>
                <InputLabel>Action *</InputLabel>
                <Select
                  value={form.actionId}
                  onChange={handleInputChange('actionId')}
                  label="Action *"
                >
                  {actions.map((action) => (
                    <MenuItem key={action.id} value={action.id}>
                      {action.name} ({action.id})
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            {/* Function Selection */}
            <Grid item xs={12}>
              <FormControl fullWidth disabled={loadingDropdowns}>
                <InputLabel>Function *</InputLabel>
                <Select
                  value={form.functionId}
                  onChange={handleInputChange('functionId')}
                  label="Function *"
                >
                  {functions.map((func) => (
                    <MenuItem key={func.id} value={func.id}>
                      {func.name} ({func.id})
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            {/* Loading indicator for dropdowns */}
            {loadingDropdowns && (
              <Grid item xs={12}>
                <Stack direction="row" spacing={1} alignItems="center" justifyContent="center">
                  <CircularProgress size={16} />
                  <Typography variant="body2" color="text.secondary">
                    Đang tải dữ liệu...
                  </Typography>
                </Stack>
              </Grid>
            )}

            {/* Error message */}
            {error && (
              <Grid item xs={12}>
                <Typography variant="body2" color="error" sx={{ textAlign: 'center' }}>
                  {error}
                </Typography>
              </Grid>
            )}
          </Grid>
        </DialogContent>

        <DialogActions sx={{ px: 3, pb: 3 }}>
          <Button 
            onClick={handleClose}
            disabled={loading}
            color="inherit"
          >
            Hủy
          </Button>
          <Button 
            type="submit"
            variant="contained"
            disabled={loading || loadingDropdowns}
            startIcon={loading && <CircularProgress size={16} color="inherit" />}
            sx={{ minWidth: 100 }}
          >
            {loading ? 'Đang tạo...' : 'Tạo'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
}

ActionInFunctionFormModal.propTypes = {
  open: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSuccess: PropTypes.func,
};