import React, { useEffect, useState } from 'react';
import { getUsers, deleteUser, getUserRoles } from '../api/userApi';
import { Button, List, ListItem, ListItemText, Typography, Box, IconButton, Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import SecurityIcon from '@mui/icons-material/Security';

function UserList() {
  const [users, setUsers] = useState<any[]>([]);
  const [selectedUser, setSelectedUser] = useState<any>(null);
  const [roles, setRoles] = useState<any[]>([]);
  const [openRoles, setOpenRoles] = useState(false);

  const fetchUsers = async () => {
    const data = await getUsers({});
    setUsers(data.value?.items || []);
  };

  const handleDelete = async (userId: string) => {
    await deleteUser(userId);
    fetchUsers();
  };

  const handleShowRoles = async (user: any) => {
    setSelectedUser(user);
    const data = await getUserRoles(user.id);
    setRoles(data.value?.roles || []);
    setOpenRoles(true);
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <Box>
      <Typography variant="h5" gutterBottom>User Management</Typography>
      <List>
        {users.map((user) => (
          <ListItem key={user.id} secondaryAction={
            <>
              <IconButton onClick={() => handleShowRoles(user)}><SecurityIcon /></IconButton>
              <IconButton onClick={() => handleDelete(user.id)}><DeleteIcon /></IconButton>
            </>
          }>
            <ListItemText primary={user.userName} secondary={user.email} />
          </ListItem>
        ))}
      </List>
      <Dialog open={openRoles} onClose={() => setOpenRoles(false)}>
        <DialogTitle>User Roles</DialogTitle>
        <DialogContent>
          <List>
            {roles.map((role: any) => (
              <ListItem key={role.id}>
                <ListItemText primary={role.name} />
              </ListItem>
            ))}
          </List>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenRoles(false)}>Close</Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}

export default UserList;
