import React, { useEffect, useState } from 'react';
import { getRoles } from '../api/roleApi';
import { List, ListItem, ListItemText, Typography, Box } from '@mui/material';

function RoleList() {
  const [roles, setRoles] = useState<any[]>([]);

  useEffect(() => {
    getRoles().then(data => setRoles(data.value?.items || []));
  }, []);

  return (
    <Box>
      <Typography variant="h5" gutterBottom>Role Management</Typography>
      <List>
        {roles.map((role) => (
          <ListItem key={role.id}>
            <ListItemText primary={role.name} secondary={role.description} />
          </ListItem>
        ))}
      </List>
    </Box>
  );
}

export default RoleList;
