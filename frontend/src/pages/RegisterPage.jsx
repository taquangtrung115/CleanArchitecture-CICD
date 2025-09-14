import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../api/auth';

const RegisterPage = () => {
    const [form, setForm] = useState({ userName: '', email: '', password: '', confirm: '' });
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleChange = e => {
        const { name, value } = e.target;
        setForm(f => ({ ...f, [name]: value }));
    };

    const handleSubmit = async e => {
        e.preventDefault();
        if (!form.userName || !form.email || !form.password || !form.confirm) {
            setError('Please fill all fields');
            return;
        }
        if (form.password !== form.confirm) {
            setError('Passwords do not match');
            return;
        }
        setError(null);
        setLoading(true);
        const res = await register({
            userName: form.userName,
            email: form.email,
            password: form.password,
            firstName: '',
            lastName: '',
            dayOfBirth: ''
        });
        setLoading(false);
        if (res.status === 200 || res.status === 201) {
            navigate('/login');
        } else {
            setError(res.error?.message || 'Register failed');
        }
    };

    return (
        <div style={{ maxWidth: 320, margin: '40px auto', padding: 24, border: '1px solid #ccc', borderRadius: 8 }}>
            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <input name="userName" value={form.userName} onChange={handleChange} placeholder="Username" style={{ width: '100%', marginBottom: 12 }} />
                <input name="email" value={form.email} onChange={handleChange} placeholder="Email" style={{ width: '100%', marginBottom: 12 }} />
                <input name="password" value={form.password} onChange={handleChange} placeholder="Password" type="password" style={{ width: '100%', marginBottom: 12 }} />
                <input name="confirm" value={form.confirm} onChange={handleChange} placeholder="Confirm Password" type="password" style={{ width: '100%', marginBottom: 12 }} />
                <button type="submit" style={{ width: '100%' }} disabled={loading}>{loading ? 'Registering...' : 'Register'}</button>
                {error && <div style={{ color: 'red', marginTop: 8 }}>{error}</div>}
            </form>
        </div>
    );
};

export default RegisterPage;
