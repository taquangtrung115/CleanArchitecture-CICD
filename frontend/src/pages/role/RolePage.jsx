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
import { createRole, getRoles } from 'api/role';
import LinearProgress from '@mui/material/LinearProgress';

export default function RolePage() {
  const [form, setForm] = useState({
    name: '',
    description: '',
    roleCode: ''
  });
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchRoles = async () => {
    setLoading(true);
    const res = await getRoles();
    if (res.data && res.data.value && Array.isArray(res.data.value.roles)) {
      setRoles(res.data.value.roles);
    } else {
      setRoles([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchRoles();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    const res = await createRole(form);
    if (res.data) {
      setForm({ name: '', description: '', roleCode: '' });
      await fetchRoles();
    } else {
      setError(res.error?.message || 'Tạo role thất bại');
      setLoading(false);
    }
  };

  return (
    <MainCard title="Role Management">
      {loading && <LinearProgress sx={{ mb: 2 }} />}
      <Typography variant="h6" sx={{ mb: 2 }}>
        Thêm Role mới
      </Typography>
      <form onSubmit={handleSubmit} style={{ marginBottom: 24 }}>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} md={4}>
            <TextField label="Role Name" name="name" value={form.name} onChange={handleChange} fullWidth required disabled={loading} />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Description"
              name="description"
              value={form.description}
              onChange={handleChange}
              fullWidth
              disabled={loading}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Role Code"
              name="roleCode"
              value={form.roleCode}
              onChange={handleChange}
              fullWidth
              required
              disabled={loading}
            />
          </Grid>
          <Grid item xs={12}>
            <Button type="submit" variant="contained" disabled={loading}>
              Tạo Role
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
        Danh sách Role
      </Typography>
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Role Name</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Role Code</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={3}>Đang tải...</TableCell>
              </TableRow>
            ) : roles.length === 0 ? (
              <TableRow>
                <TableCell colSpan={3}>Không có role nào</TableCell>
              </TableRow>
            ) : (
              roles.map((r) => (
                <TableRow key={r.roleId || r.id}>
                  <TableCell>{r.name}</TableCell>
                  <TableCell>{r.description}</TableCell>
                  <TableCell>{r.roleCode}</TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </MainCard>
  );
}
