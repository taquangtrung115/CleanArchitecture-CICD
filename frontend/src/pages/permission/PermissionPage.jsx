import { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import MainCard from 'components/MainCard';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { createPermission, getPermissions } from 'api/permission';
import LinearProgress from '@mui/material/LinearProgress';

export default function PermissionPage() {
  const [form, setForm] = useState({
    roleId: '',
    functionId: '',
    actionId: ''
  });
  const [permissions, setPermissions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchPermissions = async () => {
    setLoading(true);
    const res = await getPermissions();
    if (res.data && res.data.value && Array.isArray(res.data.value.permissions)) {
      setPermissions(res.data.value.permissions);
    } else {
      setPermissions([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchPermissions();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    const res = await createPermission(form);
    if (res.data) {
      setForm({ roleId: '', functionId: '', actionId: '' });
      await fetchPermissions();
    } else {
      setError(res.error?.message || 'Tạo permission thất bại');
      setLoading(false);
    }
  };

  return (
    <MainCard title="Permission Management">
      {loading && <LinearProgress sx={{ mb: 2 }} />}
      <Typography variant="h6" sx={{ mb: 2 }}>
        Thêm Permission mới
      </Typography>
      <form onSubmit={handleSubmit} style={{ marginBottom: 24 }}>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} md={4}>
            <TextField label="Role ID" name="roleId" value={form.roleId} onChange={handleChange} fullWidth required disabled={loading} />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Function ID"
              name="functionId"
              value={form.functionId}
              onChange={handleChange}
              fullWidth
              required
              disabled={loading}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Action ID"
              name="actionId"
              value={form.actionId}
              onChange={handleChange}
              fullWidth
              required
              disabled={loading}
            />
          </Grid>
          <Grid item xs={12}>
            <Button type="submit" variant="contained" disabled={loading}>
              Tạo Permission
            </Button>
          </Grid>
        </Grid>
        {error && (
          <Typography color="error" sx={{ mt: 1 }}>
            {error}
          </Typography>
        )}
      </form>
      <Typography variant="h6" sx={{ mb: 1 }}>
        Danh sách Permission
      </Typography>
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Role ID</TableCell>
              <TableCell>Function ID</TableCell>
              <TableCell>Action ID</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={3}>Đang tải...</TableCell>
              </TableRow>
            ) : permissions.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3}>Không có permission nào</TableCell>
              </TableRow>
            ) : (
              permissions.map((p, idx) => (
                <TableRow key={p.permissionId || idx}>
                  <TableCell>{p.roleId}</TableCell>
                  <TableCell>{p.functionId}</TableCell>
                  <TableCell>{p.actionId}</TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </MainCard>
  );
}
