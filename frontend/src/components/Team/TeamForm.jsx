import React, { useState } from 'react';

const initialState = {
    name: '',
    shortName: '',
    countryCode: '',
    countryName: '',
    countryFlag: '',
    foundedYear: '',
    description: ''
};

const TeamForm = ({ onSubmit, initial = initialState, loading, error }) => {
    const [form, setForm] = useState(initial);

    const handleChange = e => {
        const { name, value } = e.target;
        setForm(f => ({ ...f, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        onSubmit(form);
    };

    return (
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
            <input name="name" value={form.name} onChange={handleChange} placeholder="Team Name" required />
            <input name="shortName" value={form.shortName} onChange={handleChange} placeholder="Short Name" required />
            <input name="countryCode" value={form.countryCode} onChange={handleChange} placeholder="Country Code (e.g. JP)" required />
            <input name="countryName" value={form.countryName} onChange={handleChange} placeholder="Country Name" required />
            <input name="countryFlag" value={form.countryFlag} onChange={handleChange} placeholder="Country Flag (e.g. 🇯🇵)" required />
            <input name="foundedYear" value={form.foundedYear} onChange={handleChange} placeholder="Founded Year" type="date" required />
            <input name="description" value={form.description} onChange={handleChange} placeholder="Description" />
            <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Save'}</button>
            {error && <div style={{ color: 'red' }}>{error}</div>}
        </form>
    );
};

export default TeamForm;
