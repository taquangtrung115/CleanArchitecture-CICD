import React, { useEffect, useState } from 'react';
import { Container, Typography, Box, Button, TextField, List, ListItem, ListItemText, Paper, CircularProgress, Snackbar, Alert } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { getProducts, createProduct } from '../api/productApi';

function ProductManager() {
  const [products, setProducts] = useState<any[]>([]);
  const [name, setName] = useState('');
  const [price, setPrice] = useState<number>(0);
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const data = await getProducts();
      setProducts(data);
    } catch (err: any) {
      setError(err.message || 'Error fetching products');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await createProduct({ name, price, description });
      setSuccess('Product added successfully!');
      setName('');
      setPrice(0);
      setDescription('');
      fetchProducts();
    } catch (err: any) {
      setError(err.message || 'Error adding product');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>Product Manager</Typography>
      <Paper sx={{ p: 2, mb: 3 }}>
        <form onSubmit={handleAdd} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>
          <TextField label="Name" value={name} onChange={e => setName(e.target.value)} required />
          <TextField label="Price" type="number" value={price} onChange={e => setPrice(Number(e.target.value))} required />
          <TextField label="Description" value={description} onChange={e => setDescription(e.target.value)} required />
          <Button type="submit" variant="contained" startIcon={<AddIcon />} disabled={loading}>
            Add Product
          </Button>
        </form>
      </Paper>
      <Typography variant="h6" gutterBottom>Product List</Typography>
      {loading ? <CircularProgress /> : (
        <List>
          {products.map((p) => (
            <ListItem key={p.id} divider>
              <ListItemText primary={p.name} secondary={`Price: ${p.price} | ${p.description}`} />
            </ListItem>
          ))}
        </List>
      )}
      <Snackbar open={!!error} autoHideDuration={4000} onClose={() => setError(null)}>
        <Alert severity="error" onClose={() => setError(null)}>{error}</Alert>
      </Snackbar>
      <Snackbar open={!!success} autoHideDuration={3000} onClose={() => setSuccess(null)}>
        <Alert severity="success" onClose={() => setSuccess(null)}>{success}</Alert>
      </Snackbar>
    </Container>
  );
}

export default ProductManager;
