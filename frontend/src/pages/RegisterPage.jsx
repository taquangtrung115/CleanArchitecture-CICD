import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../api/auth';
import { getErrorMessage, formatValidationErrors, isValidationError } from '../utils/errorHandler';

const RegisterPage = () => {
  const [form, setForm] = useState({ userName: '', email: '', password: '', confirm: '' });
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
    // Clear errors when user starts typing
    if (error) setError(null);
    if (validationErrors.length > 0) setValidationErrors([]);
  };

  const handleSubmit = async (e) => {
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
    setValidationErrors([]);
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
      // Handle different types of errors
      if (isValidationError(res)) {
        setValidationErrors(formatValidationErrors(res.error.errors));
        setError(res.error.detail || 'Validation errors occurred');
      } else {
        setError(getErrorMessage(res));
      }
    }
  };

  return (
    <div style={{ maxWidth: 320, margin: '40px auto', padding: 24, border: '1px solid #ccc', borderRadius: 8 }}>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <input
          name="userName"
          value={form.userName}
          onChange={handleChange}
          placeholder="Username"
          style={{ width: '100%', marginBottom: 12 }}
        />
        <input name="email" value={form.email} onChange={handleChange} placeholder="Email" style={{ width: '100%', marginBottom: 12 }} />
        <input
          name="password"
          value={form.password}
          onChange={handleChange}
          placeholder="Password"
          type="password"
          style={{ width: '100%', marginBottom: 12 }}
        />
        <input
          name="confirm"
          value={form.confirm}
          onChange={handleChange}
          placeholder="Confirm Password"
          type="password"
          style={{ width: '100%', marginBottom: 12 }}
        />
        <button type="submit" style={{ width: '100%' }} disabled={loading}>
          {loading ? 'Registering...' : 'Register'}
        </button>
        
        {/* Error Messages */}
        {error && (
          <div style={{ 
            color: 'red', 
            marginTop: 8, 
            padding: 8,
            backgroundColor: '#ffebee',
            border: '1px solid #f44336',
            borderRadius: 4
          }}>
            {error}
          </div>
        )}
        
        {/* Validation Errors */}
        {validationErrors.length > 0 && (
          <div style={{ 
            color: '#e65100', 
            marginTop: 8, 
            padding: 8,
            backgroundColor: '#fff3e0',
            border: '1px solid #ff9800',
            borderRadius: 4
          }}>
            <strong>Validation Errors:</strong>
            <ul style={{ margin: '4px 0', paddingLeft: 20 }}>
              {validationErrors.map((errorMsg, index) => (
                <li key={index}>{errorMsg}</li>
              ))}
            </ul>
          </div>
        )}
      </form>
    </div>
  );
};

export default RegisterPage;
