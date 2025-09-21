import React, { useState } from 'react';

// material-ui
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Divider from '@mui/material/Divider';

// material-ui icons
import GroupsIcon from '@mui/icons-material/Groups';
import AddIcon from '@mui/icons-material/Add';

// project imports
import MainCard from 'components/MainCard';
import TeamList from '../components/Team/TeamList';
import TeamDetail from '../components/Team/TeamDetail';
import SearchBar from '../components/Shared/SearchBar';
import Pagination from '../components/Shared/Pagination';
import FilterPanel from '../components/Shared/FilterPanel';
import TeamFormModal from '../components/forms/TeamFormModal';

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

const TeamsPage = ({ token }) => {
  const [filters, setFilters] = useState({ pageIndex: 1, pageSize: 10 });
  const [selected, setSelected] = useState(null);
  const [showFormModal, setShowFormModal] = useState(false);
  const [refresh, setRefresh] = useState(0);

  const handleSearch = (q) => setFilters((f) => ({ ...f, ...q, pageIndex: 1 }));
  const handleFilter = (name, value) => setFilters((f) => ({ ...f, [name]: value, pageIndex: 1 }));
  const handlePage = (page, size) => setFilters((f) => ({ ...f, pageIndex: page, pageSize: size }));

  const handleFormSuccess = () => {
    setRefresh((r) => r + 1);
  };

  return (
    <MainCard
      title={
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <GroupsIcon color="primary" sx={{ fontSize: 32 }} />
          <Typography variant="h3" component="h1">
            Quản Lý Teams MotoGP
          </Typography>
        </Box>
      }
      secondary={
        <Button 
          variant="contained" 
          startIcon={<AddIcon />} 
          onClick={() => setShowFormModal(true)}
          color="primary"
          size="medium"
          sx={{
            borderRadius: 2,
            textTransform: 'none',
            fontWeight: 600
          }}
        >
          Thêm Team Mới
        </Button>
      }
    >
      {/* Search and Filter Section */}
      <Box sx={{ mb: 3 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <SearchBar onSearch={handleSearch} />
          </Grid>
          <Grid item xs={12} md={6}>
            <FilterPanel filters={filters} onChange={handleFilter} options={countryOptions} />
          </Grid>
        </Grid>
      </Box>

      <Divider sx={{ mb: 3 }} />

      {/* Content Area */}
      <Grid container spacing={3}>
        {/* Teams List */}
        <Grid item xs={12} lg={selected ? 8 : 12}>
          <Card sx={{ minHeight: 400 }}>
            <CardContent>
              <TeamList 
                key={refresh} 
                token={token} 
                filters={filters} 
                onSelect={setSelected} 
              />
              <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination 
                  page={filters.pageIndex} 
                  pageSize={filters.pageSize} 
                  total={100} 
                  onChange={handlePage} 
                />
              </Box>
            </CardContent>
          </Card>
        </Grid>

        {/* Team Detail */}
        {selected && (
          <Grid item xs={12} lg={4}>
            <Card sx={{ minHeight: 400, position: 'sticky', top: 20 }}>
              <CardContent>
                <Typography variant="h5" gutterBottom>
                  Chi Tiết Team
                </Typography>
                <Divider sx={{ mb: 2 }} />
                <TeamDetail team={selected} />
              </CardContent>
            </Card>
          </Grid>
        )}
      </Grid>

      {/* Team Form Modal */}
      <TeamFormModal 
        open={showFormModal} 
        onClose={() => setShowFormModal(false)} 
        onSuccess={handleFormSuccess}
        token={token}
      />
    </MainCard>
  );
};

export default TeamsPage;
