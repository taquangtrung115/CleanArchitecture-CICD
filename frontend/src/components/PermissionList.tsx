import React, { useEffect, useState } from 'react';
import { getPermissions } from '../api/permissionApi';
import { List, ListItem, ListItemText, Typography, Box } from '@mui/material';

function PermissionList() {
  const [permissions, setPermissions] = useState<any[]>([]);

  useEffect(() => {
    getPermissions().then(data => setPermissions(data.value?.items || []));
  }, []);

  return (
    <Box>
      <Typography variant="h5" gutterBottom>Permission Management</Typography>
      <List>
        {permissions.map((p, idx) => (
          <ListItem key={idx}>
            <ListItemText primary={p.actionId} secondary={`Role: ${p.roleId} | Function: ${p.functionId}`} />
          </ListItem>
        ))}
      </List>
    </Box>
  );
}

export default PermissionList;
