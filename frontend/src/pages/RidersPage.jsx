import React, { useState } from 'react';
import RiderList from '../components/Rider/RiderList';
import RiderDetail from '../components/Rider/RiderDetail';
import RiderForm from '../components/Rider/RiderForm';
import SearchBar from '../components/Shared/SearchBar';
import Pagination from '../components/Shared/Pagination';
import FilterPanel from '../components/Shared/FilterPanel';
import ConfirmDialog from '../components/Shared/ConfirmDialog';
import { createRider, deleteRider } from '../api/riders';

const countryOptions = [
  {
    name: 'countryCode',
    label: 'Country',
    choices: [
      { value: 'IT', label: 'Italy' },
      { value: 'ES', label: 'Spain' },
      { value: 'JP', label: 'Japan' }
      // ...add more as needed
    ]
  }
];

const RidersPage = ({ token }) => {
  const [filters, setFilters] = useState({ pageIndex: 1, pageSize: 10 });
  const [selected, setSelected] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [error, setError] = useState(null);
  const [refresh, setRefresh] = useState(0);

  const handleSearch = (q) => setFilters((f) => ({ ...f, ...q, pageIndex: 1 }));
  const handleFilter = (name, value) => setFilters((f) => ({ ...f, [name]: value, pageIndex: 1 }));
  const handlePage = (page, size) => setFilters((f) => ({ ...f, pageIndex: page, pageSize: size }));

  const handleCreate = (data) => {
    setError(null);
    createRider(data, token)
      .then(() => {
        setShowForm(false);
        setRefresh((r) => r + 1);
      })
      .catch((e) => setError(e.response?.data?.detail || 'Error creating rider'));
  };

  const handleDelete = () => {
    if (!selected) return;
    deleteRider(selected.id, token)
      .then(() => {
        setConfirmDelete(false);
        setSelected(null);
        setRefresh((r) => r + 1);
      })
      .catch((e) => setError(e.response?.data?.detail || 'Error deleting rider'));
  };

  return (
    <div>
      <h1>MotoGP Riders</h1>
      <SearchBar onSearch={handleSearch} />
      <FilterPanel filters={filters} onChange={handleFilter} options={countryOptions} />
      <button onClick={() => setShowForm(true)}>Add Rider</button>
      <RiderList key={refresh} token={token} filters={filters} onSelect={setSelected} />
      <Pagination page={filters.pageIndex} pageSize={filters.pageSize} total={100} onChange={handlePage} />
      {selected && <RiderDetail rider={selected} />}
      {showForm && <RiderForm onSubmit={handleCreate} loading={false} error={error} />}
      <ConfirmDialog
        open={confirmDelete}
        title="Delete Rider"
        message="Are you sure?"
        onConfirm={handleDelete}
        onCancel={() => setConfirmDelete(false)}
      />
      {selected && <button onClick={() => setConfirmDelete(true)}>Delete Selected Rider</button>}
    </div>
  );
};

export default RidersPage;
