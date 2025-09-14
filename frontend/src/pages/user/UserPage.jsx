import { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import MainCard from 'components/MainCard';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { createUser, getUsers } from 'api/user';
import LinearProgress from '@mui/material/LinearProgress';

export default function UserPage() {
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
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const fetchUsers = async () => {
        setLoading(true);
        const res = await getUsers();
        if (res.data && res.data.value && Array.isArray(res.data.value.users)) {
            setUsers(res.data.value.users);
        } else {
            setUsers([]);
        }
        setLoading(false);
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setForm((prev) => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (loading) return; // Block nếu đang loading
        setError('');
        setLoading(true);
        const res = await createUser(form);
        if (res.data) {
            setForm({
                userName: '', email: '', password: '', firstName: '', lastName: '', dayOfBirth: '', isDirector: false, isHeadOfDepartment: false, managerId: '', positionId: ''
            });
            await fetchUsers();
            setLoading(false);
        } else {
            setError(res.error?.message || 'Tạo user thất bại');
            setLoading(false);
        }
    };

    return (
        <MainCard title="User Management">
            {loading && <LinearProgress sx={{ mb: 2 }} />}
            <Typography variant="h6" sx={{ mb: 2 }}>Thêm User mới</Typography>
            <form onSubmit={handleSubmit} style={{ marginBottom: 24 }}>
                <Grid container spacing={2}>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="User Name" name="userName" value={form.userName} onChange={handleChange} fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Email" name="email" value={form.email} onChange={handleChange} fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Password" name="password" value={form.password} onChange={handleChange} type="password" fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="First Name" name="firstName" value={form.firstName} onChange={handleChange} fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Last Name" name="lastName" value={form.lastName} onChange={handleChange} fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Day of Birth" name="dayOfBirth" value={form.dayOfBirth} onChange={handleChange} type="date" InputLabelProps={{ shrink: true }} fullWidth required disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <FormControlLabel control={<Checkbox checked={form.isDirector} onChange={handleChange} name="isDirector" disabled={loading} />} label="Is Director" />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <FormControlLabel control={<Checkbox checked={form.isHeadOfDepartment} onChange={handleChange} name="isHeadOfDepartment" disabled={loading} />} label="Is Head Of Department" />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Manager ID" name="managerId" value={form.managerId} onChange={handleChange} fullWidth disabled={loading} />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <TextField label="Position ID" name="positionId" value={form.positionId} onChange={handleChange} fullWidth disabled={loading} />
                    </Grid>
                    <Grid item xs={12}>
                        <Button type="submit" variant="contained" disabled={loading}>Tạo User</Button>
                    </Grid>
                </Grid>
                {error && <Typography color="error" sx={{ mt: 1 }}>{error}</Typography>}
            </form>
            <Typography variant="h6" sx={{ mb: 1 }}>Danh sách User</Typography>
            <TableContainer component={Paper}>
                <Table size="small">
                    <TableHead>
                        <TableRow>
                            <TableCell>User Name</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell>Full Name</TableCell>
                            <TableCell>Is Locked</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {loading ? (
                            <TableRow><TableCell colSpan={4}>Đang tải...</TableCell></TableRow>
                        ) : users.length === 0 ? (
                            <TableRow><TableCell colSpan={4}>Không có user nào</TableCell></TableRow>
                        ) : users.map((u) => (
                            <TableRow key={u.userId || u.id}>
                                <TableCell>{u.userName}</TableCell>
                                <TableCell>{u.email}</TableCell>
                                <TableCell>{u.fullName}</TableCell>
                                <TableCell>{u.isLocked ? 'Đã khóa' : 'Hoạt động'}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </MainCard>
    );
}
