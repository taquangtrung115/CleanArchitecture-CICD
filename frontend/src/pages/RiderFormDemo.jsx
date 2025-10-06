import React, { useState } from 'react';

// material-ui
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import Stack from '@mui/material/Stack';
import Alert from '@mui/material/Alert';

// material-ui icons
import SportsMotorsportsIcon from '@mui/icons-material/SportsMotorsports';
import VisibilityIcon from '@mui/icons-material/Visibility';

// project imports
import RiderFormModal from '../components/Rider/RiderForm';

const RiderFormDemo = () => {
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState(null);

  const handleSubmit = async (data) => {
    setLoading(true);
    setMessage(null);

    // Simulate API call
    setTimeout(() => {
      setLoading(false);
      setShowForm(false);
      setMessage({
        type: 'success',
        text: `Rider "${data.firstName} ${data.lastName}" đã được tạo thành công! (Demo Mode)`
      });
    }, 2000);
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      {/* Header */}
      <Paper sx={{ p: 4, mb: 4, textAlign: 'center' }}>
        <Stack direction="row" alignItems="center" justifyContent="center" spacing={2} sx={{ mb: 2 }}>
          <SportsMotorsportsIcon sx={{ fontSize: 50, color: 'primary.main' }} />
          <Typography variant="h3" component="h1" fontWeight="bold">
            Demo: Giao diện Rider Form mới
          </Typography>
        </Stack>
        <Typography variant="h6" color="text.secondary" sx={{ mb: 3 }}>
          Giao diện đã được cải thiện với Material-UI components
        </Typography>

        <Button variant="contained" size="large" startIcon={<VisibilityIcon />} onClick={() => setShowForm(true)} sx={{ minWidth: 200 }}>
          Xem Form Mới
        </Button>
      </Paper>

      {/* Features */}
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" fontWeight="bold" sx={{ mb: 3 }}>
          Cải tiến đã thực hiện:
        </Typography>

        <Stack spacing={2}>
          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Form Modal:</strong> Thay thế form inline bằng modal dialog chuyên nghiệp
            </Typography>
          </Alert>

          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Material-UI Components:</strong> Sử dụng TextField, Dialog, Grid layout
            </Typography>
          </Alert>

          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Vietnamese Labels:</strong> Giao diện tiếng Việt thân thiện
            </Typography>
          </Alert>

          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Responsive Design:</strong> Tự động điều chỉnh theo kích thước màn hình
            </Typography>
          </Alert>

          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Better UX:</strong> Loading states, error handling, success feedback
            </Typography>
          </Alert>

          <Alert severity="info">
            <Typography variant="body1">
              ✅ <strong>Improved Layout:</strong> RidersPage với Grid layout và Paper containers
            </Typography>
          </Alert>
        </Stack>
      </Paper>

      {/* Success Message */}
      {message && (
        <Box sx={{ mt: 3 }}>
          <Alert severity={message.type} onClose={() => setMessage(null)}>
            {message.text}
          </Alert>
        </Box>
      )}

      {/* Form Modal */}
      <RiderFormModal open={showForm} onClose={() => setShowForm(false)} onSubmit={handleSubmit} loading={loading} />
    </Container>
  );
};

export default RiderFormDemo;
