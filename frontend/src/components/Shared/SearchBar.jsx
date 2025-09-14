import React, { useState } from 'react';

const SearchBar = ({ onSearch, placeholder = 'Search...', initial = '' }) => {
    const [value, setValue] = useState(initial);
    const handleSubmit = e => {
        e.preventDefault();
        onSearch({ searchTerm: value });
    };
    return (
        <form onSubmit={handleSubmit} style={{ marginBottom: 16 }}>
            <input
                value={value}
                onChange={e => setValue(e.target.value)}
                placeholder={placeholder}
                style={{ padding: 8, minWidth: 200 }}
            />
            <button type="submit" style={{ marginLeft: 8 }}>Search</button>
        </form>
    );
};

export default SearchBar;
