import React, { useState } from 'react';

const initialState = {
    firstName: '',
    lastName: '',
    racingNumber: '',
    countryCode: '',
    countryName: '',
    countryFlag: '',
    dateOfBirth: '',
    height: '',
    weight: '',
    nickname: ''
};

const RiderForm = ({ onSubmit, initial = initialState, loading, error }) => {
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
            <input name="firstName" value={form.firstName} onChange={handleChange} placeholder="First Name" required />
            <input name="lastName" value={form.lastName} onChange={handleChange} placeholder="Last Name" required />
            <input name="racingNumber" value={form.racingNumber} onChange={handleChange} placeholder="Racing Number" type="number" min="1" max="99" required />
            <input name="countryCode" value={form.countryCode} onChange={handleChange} placeholder="Country Code (e.g. IT)" required />
            <input name="countryName" value={form.countryName} onChange={handleChange} placeholder="Country Name" required />
            <input name="countryFlag" value={form.countryFlag} onChange={handleChange} placeholder="Country Flag (e.g. 🇮🇹)" required />
            <input name="dateOfBirth" value={form.dateOfBirth} onChange={handleChange} placeholder="Date of Birth" type="date" required />
            <input name="height" value={form.height} onChange={handleChange} placeholder="Height (cm)" type="number" min="100" required />
            <input name="weight" value={form.weight} onChange={handleChange} placeholder="Weight (kg)" type="number" min="30" required />
            <input name="nickname" value={form.nickname} onChange={handleChange} placeholder="Nickname (optional)" />
            <button type="submit" disabled={loading}>{loading ? 'Saving...' : 'Save'}</button>
            {error && <div style={{ color: 'red' }}>{error}</div>}
        </form>
    );
};

export default RiderForm;
