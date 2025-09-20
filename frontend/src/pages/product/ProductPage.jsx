import { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import MainCard from 'components/MainCard';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import IconButton from '@mui/material/IconButton';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { createProduct, getProducts, updateProduct, deleteProduct } from 'api/product';
import LinearProgress from '@mui/material/LinearProgress';

export default function ProductPage() {
  const [form, setForm] = useState({
    name: '',
    price: '',
    description: ''
  });
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [editingProduct, setEditingProduct] = useState(null);
  const [page] = useState(1);
  const [pageSize] = useState(10);

  const fetchProducts = async () => {
    setLoading(true);
    const res = await getProducts(page, pageSize);
    if (res.data && res.data.value) {
      // Handle both paginated and direct array responses
      const productData = res.data.value.items || res.data.value || [];
      setProducts(Array.isArray(productData) ? productData : []);
    } else {
      setProducts([]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchProducts();
  }, [page, pageSize]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (loading) return;
    setError('');
    setLoading(true);

    const productData = {
      name: form.name,
      price: parseFloat(form.price),
      description: form.description
    };

    let res;
    if (editingProduct) {
      res = await updateProduct(editingProduct.id, productData);
    } else {
      res = await createProduct(productData);
    }

    if (res.data) {
      setForm({
        name: '',
        price: '',
        description: ''
      });
      setEditingProduct(null);
      await fetchProducts();
      setLoading(false);
    } else {
      setError(res.error?.message || `${editingProduct ? 'Cập nhật' : 'Tạo'} sản phẩm thất bại`);
      setLoading(false);
    }
  };

  const handleEdit = (product) => {
    setEditingProduct(product);
    setForm({
      name: product.name || '',
      price: product.price?.toString() || '',
      description: product.description || ''
    });
  };

  const handleCancelEdit = () => {
    setEditingProduct(null);
    setForm({
      name: '',
      price: '',
      description: ''
    });
  };

  const handleDelete = async (productId) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa sản phẩm này?')) {
      return;
    }

    setLoading(true);
    const res = await deleteProduct(productId);
    if (res.data) {
      await fetchProducts();
    } else {
      setError(res.error?.message || 'Xóa sản phẩm thất bại');
    }
    setLoading(false);
  };

  return (
    <MainCard title="Product Management">
      {loading && <LinearProgress sx={{ mb: 2 }} />}
      <Typography variant="h6" sx={{ mb: 2 }}>
        {editingProduct ? 'Cập nhật sản phẩm' : 'Thêm sản phẩm mới'}
      </Typography>
      <form onSubmit={handleSubmit} style={{ marginBottom: 24 }}>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} md={4}>
            <TextField label="Tên sản phẩm" name="name" value={form.name} onChange={handleChange} fullWidth required disabled={loading} />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Giá"
              name="price"
              value={form.price}
              onChange={handleChange}
              type="number"
              inputProps={{ step: '0.01', min: '0' }}
              fullWidth
              required
              disabled={loading}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={4}>
            <TextField
              label="Mô tả"
              name="description"
              value={form.description}
              onChange={handleChange}
              fullWidth
              disabled={loading}
              multiline
              maxRows={3}
            />
          </Grid>
          <Grid item xs={12}>
            <Button type="submit" variant="contained" disabled={loading} sx={{ mr: 1 }}>
              {editingProduct ? 'Cập nhật' : 'Tạo sản phẩm'}
            </Button>
            {editingProduct && (
              <Button variant="outlined" onClick={handleCancelEdit} disabled={loading}>
                Hủy
              </Button>
            )}
          </Grid>
        </Grid>
        {error && (
          <Typography color="error" sx={{ mt: 1 }}>
            {error}
          </Typography>
        )}
      </form>
      <Typography variant="h6" sx={{ mb: 1 }}>
        Danh sách sản phẩm
      </Typography>
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Tên sản phẩm</TableCell>
              <TableCell>Giá</TableCell>
              <TableCell>Mô tả</TableCell>
              <TableCell>Thao tác</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={4}>Đang tải...</TableCell>
              </TableRow>
            ) : products.length === 0 ? (
              <TableRow>
                <TableCell colSpan={4}>Không có sản phẩm nào</TableCell>
              </TableRow>
            ) : (
              products.map((product) => (
                <TableRow key={product.id || product.productId}>
                  <TableCell>{product.name}</TableCell>
                  <TableCell>{product.price?.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</TableCell>
                  <TableCell>{product.description}</TableCell>
                  <TableCell>
                    <IconButton size="small" onClick={() => handleEdit(product)} disabled={loading}>
                      <EditIcon />
                    </IconButton>
                    <IconButton size="small" onClick={() => handleDelete(product.id || product.productId)} disabled={loading}>
                      <DeleteIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </MainCard>
  );
}
