import React, { useState } from 'react';
import TeamList from '../components/Team/TeamList';
import TeamDetail from '../components/Team/TeamDetail';
import TeamForm from '../components/Team/TeamForm';
import SearchBar from '../components/Shared/SearchBar';
import Pagination from '../components/Shared/Pagination';
import FilterPanel from '../components/Shared/FilterPanel';
import ConfirmDialog from '../components/Shared/ConfirmDialog';
import { createTeam } from '../api/teams';

const countryOptions = [
    {
        name: 'countryCode', label: 'Country', choices: [
            { value: 'IT', label: 'Italy' },
            { value: 'ES', label: 'Spain' },
            { value: 'JP', label: 'Japan' },
            // ...add more as needed
        ]
    }
];

const TeamsPage = ({ token }) => {
    const [filters, setFilters] = useState({ pageIndex: 1, pageSize: 10 });
    const [selected, setSelected] = useState(null);
    const [showForm, setShowForm] = useState(false);
    const [error, setError] = useState(null);
    const [refresh, setRefresh] = useState(0);

    const handleSearch = q => setFilters(f => ({ ...f, ...q, pageIndex: 1 }));
    const handleFilter = (name, value) => setFilters(f => ({ ...f, [name]: value, pageIndex: 1 }));
    const handlePage = (page, size) => setFilters(f => ({ ...f, pageIndex: page, pageSize: size }));

    const handleCreate = data => {
        setError(null);
        createTeam(data, token)
            .then(() => { setShowForm(false); setRefresh(r => r + 1); })
            .catch(e => setError(e.response?.data?.detail || 'Error creating team'));
    };

    return (
        <div>
            <h1>MotoGP Teams</h1>
            <SearchBar onSearch={handleSearch} />
            <FilterPanel filters={filters} onChange={handleFilter} options={countryOptions} />
            <button onClick={() => setShowForm(true)}>Add Team</button>
            <TeamList key={refresh} token={token} filters={filters} onSelect={setSelected} />
            <Pagination page={filters.pageIndex} pageSize={filters.pageSize} total={100} onChange={handlePage} />
            {selected && <TeamDetail team={selected} />}
            {showForm && <TeamForm onSubmit={handleCreate} loading={false} error={error} />}
        </div>
    );
};

export default TeamsPage;
