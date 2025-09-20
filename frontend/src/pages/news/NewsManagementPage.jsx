import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Grid,
  Card,
  CardContent,
  Button,
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Switch,
  FormControlLabel,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  IconButton,
  Tooltip,
  Alert,
  Tabs,
  Tab,
  Divider,
  Stack,
  Container,
  CircularProgress
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as VisibilityIcon,
  Publish as PublishIcon,
  Archive as ArchiveIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Article as ArticleIcon,
  Note as DraftIcon
} from '@mui/icons-material';
import {
  getNews,
  createNews,
  updateNews,
  deleteNews,
  publishNews,
  archiveNews,
  NewsCategories
} from '../../api/news';
import Pagination from '../../components/Shared/Pagination';

const NewsManagementPage = () => {
  const [news, setNews] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [tabValue, setTabValue] = useState(0);
  const [selectedFilter, setSelectedFilter] = useState('');

  // Dialog states
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [selectedNews, setSelectedNews] = useState(null);

  // Form state
  const [formData, setFormData] = useState({
    title: '',
    summary: '',
    content: '',
    category: 'General',
    slug: '',
    featuredImage: '',
    imageCaption: '',
    isFeatured: false,
    isBreaking: false,
    tags: ''
  });

  const pageSize = 10;

  useEffect(() => {
    fetchNews();
  }, [currentPage, selectedFilter]);

  const fetchNews = async () => {
    try {
      setLoading(true);
      const params = {
        pageIndex: currentPage,
        pageSize,
        category: selectedFilter || null
      };
      
      const response = await getNews(params);
      if (response.isSuccess) {
        setNews(response.value.items || []);
        setTotalCount(response.value.totalCount || 0);
      } else {
        setError('Failed to fetch news');
      }
    } catch (err) {
      setError('Error fetching news: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setFormData({
      title: '',
      summary: '',
      content: '',
      category: 'General',
      slug: '',
      featuredImage: '',
      imageCaption: '',
      isFeatured: false,
      isBreaking: false,
      tags: ''
    });
    setIsCreateDialogOpen(true);
  };

  const handleEdit = (newsItem) => {
    setSelectedNews(newsItem);
    setFormData({
      title: newsItem.title || '',
      summary: newsItem.summary || '',
      content: newsItem.content || '',
      category: newsItem.category || 'General',
      slug: newsItem.slug || '',
      featuredImage: newsItem.featuredImage || '',
      imageCaption: newsItem.imageCaption || '',
      isFeatured: newsItem.isFeatured || false,
      isBreaking: newsItem.isBreaking || false,
      tags: newsItem.tags ? newsItem.tags.join(', ') : ''
    });
    setIsEditDialogOpen(true);
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      setError('');
      
      const newsData = {
        ...formData,
        tags: formData.tags ? formData.tags.split(',').map(tag => tag.trim()).filter(tag => tag) : []
      };

      let response;
      if (selectedNews) {
        response = await updateNews(selectedNews.id, newsData);
      } else {
        response = await createNews(newsData);
      }

      if (response.isSuccess) {
        setSuccess(`News ${selectedNews ? 'updated' : 'created'} successfully!`);
        setIsCreateDialogOpen(false);
        setIsEditDialogOpen(false);
        setSelectedNews(null);
        fetchNews();
      } else {
        setError(response.error || `Failed to ${selectedNews ? 'update' : 'create'} news`);
      }
    } catch (err) {
      setError(`Error ${selectedNews ? 'updating' : 'creating'} news: ` + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this news?')) return;
    
    try {
      setLoading(true);
      const response = await deleteNews(id);
      if (response.isSuccess) {
        setSuccess('News deleted successfully!');
        fetchNews();
      } else {
        setError('Failed to delete news');
      }
    } catch (err) {
      setError('Error deleting news: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handlePublish = async (id) => {
    try {
      setLoading(true);
      const response = await publishNews(id);
      if (response.isSuccess) {
        setSuccess('News published successfully!');
        fetchNews();
      } else {
        setError('Failed to publish news');
      }
    } catch (err) {
      setError('Error publishing news: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleArchive = async (id) => {
    try {
      setLoading(true);
      const response = await archiveNews(id);
      if (response.isSuccess) {
        setSuccess('News archived successfully!');
        fetchNews();
      } else {
        setError('Failed to archive news');
      }
    } catch (err) {
      setError('Error archiving news: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'Published':
        return 'success';
      case 'Draft':
        return 'warning';
      case 'Archived':
        return 'default';
      default:
        return 'default';
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const generateSlugFromTitle = (title) => {
    return title
      .toLowerCase()
      .replace(/[^a-z0-9\s-]/g, '')
      .replace(/\s+/g, '-')
      .replace(/-+/g, '-')
      .trim();
  };

  const handleTitleChange = (value) => {
    setFormData({
      ...formData,
      title: value,
      slug: formData.slug || generateSlugFromTitle(value)
    });
  };

  const NewsDialog = ({ open, onClose, title }) => (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>
        <Grid container spacing={2} sx={{ mt: 1 }}>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Title"
              value={formData.title}
              onChange={(e) => handleTitleChange(e.target.value)}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Summary"
              value={formData.summary}
              onChange={(e) => setFormData({ ...formData, summary: e.target.value })}
              multiline
              rows={2}
              required
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Content"
              value={formData.content}
              onChange={(e) => setFormData({ ...formData, content: e.target.value })}
              multiline
              rows={6}
              required
            />
          </Grid>
          <Grid item xs={6}>
            <FormControl fullWidth>
              <InputLabel>Category</InputLabel>
              <Select
                value={formData.category}
                onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                label="Category"
              >
                {Object.entries(NewsCategories).map(([key, value]) => (
                  <MenuItem key={key} value={value}>{value}</MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={6}>
            <TextField
              fullWidth
              label="Slug"
              value={formData.slug}
              onChange={(e) => setFormData({ ...formData, slug: e.target.value })}
              required
              helperText="URL-friendly identifier"
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Featured Image URL"
              value={formData.featuredImage}
              onChange={(e) => setFormData({ ...formData, featuredImage: e.target.value })}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Image Caption"
              value={formData.imageCaption}
              onChange={(e) => setFormData({ ...formData, imageCaption: e.target.value })}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Tags (comma separated)"
              value={formData.tags}
              onChange={(e) => setFormData({ ...formData, tags: e.target.value })}
              helperText="Separate multiple tags with commas"
            />
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={formData.isFeatured}
                  onChange={(e) => setFormData({ ...formData, isFeatured: e.target.checked })}
                />
              }
              label="Featured"
            />
          </Grid>
          <Grid item xs={6}>
            <FormControlLabel
              control={
                <Switch
                  checked={formData.isBreaking}
                  onChange={(e) => setFormData({ ...formData, isBreaking: e.target.checked })}
                />
              }
              label="Breaking News"
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} startIcon={<CancelIcon />}>
          Cancel
        </Button>
        <Button 
          onClick={handleSubmit} 
          variant="contained" 
          startIcon={<SaveIcon />}
          disabled={loading}
        >
          {loading ? <CircularProgress size={20} /> : (selectedNews ? 'Update' : 'Create')}
        </Button>
      </DialogActions>
    </Dialog>
  );

  return (
    <Container maxWidth="xl">
      <Box sx={{ flexGrow: 1, p: 3 }}>
        {/* Header */}
        <Box sx={{ display: 'flex', justifyContent: 'between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h4" component="h1" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <ArticleIcon sx={{ fontSize: 40, color: 'primary.main' }} />
            News Management
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleCreate}
            sx={{ ml: 'auto' }}
          >
            Create News
          </Button>
        </Box>

        {/* Alerts */}
        {error && (
          <Alert severity="error" sx={{ mb: 3 }} onClose={() => setError('')}>
            {error}
          </Alert>
        )}
        {success && (
          <Alert severity="success" sx={{ mb: 3 }} onClose={() => setSuccess('')}>
            {success}
          </Alert>
        )}

        {/* Filters */}
        <Card sx={{ mb: 3 }}>
          <CardContent>
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={12} sm={6} md={4}>
                <FormControl fullWidth>
                  <InputLabel>Filter by Category</InputLabel>
                  <Select
                    value={selectedFilter}
                    onChange={(e) => setSelectedFilter(e.target.value)}
                    label="Filter by Category"
                  >
                    <MenuItem value="">All Categories</MenuItem>
                    {Object.entries(NewsCategories).map(([key, value]) => (
                      <MenuItem key={key} value={value}>{value}</MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>
            </Grid>
          </CardContent>
        </Card>

        {/* News Table */}
        <Card>
          <CardContent>
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Title</TableCell>
                    <TableCell>Category</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Published Date</TableCell>
                    <TableCell>Views</TableCell>
                    <TableCell>Actions</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {loading ? (
                    <TableRow>
                      <TableCell colSpan={6} align="center">
                        <CircularProgress />
                      </TableCell>
                    </TableRow>
                  ) : news.length === 0 ? (
                    <TableRow>
                      <TableCell colSpan={6} align="center">
                        No news found
                      </TableCell>
                    </TableRow>
                  ) : (
                    news.map((newsItem) => (
                      <TableRow key={newsItem.id}>
                        <TableCell>
                          <Box>
                            <Typography variant="subtitle2">
                              {newsItem.title}
                            </Typography>
                            <Typography variant="caption" color="text.secondary">
                              {newsItem.slug}
                            </Typography>
                            {newsItem.isFeatured && (
                              <Chip size="small" label="Featured" color="primary" sx={{ ml: 1 }} />
                            )}
                            {newsItem.isBreaking && (
                              <Chip size="small" label="Breaking" color="error" sx={{ ml: 1 }} />
                            )}
                          </Box>
                        </TableCell>
                        <TableCell>
                          <Chip 
                            label={newsItem.category} 
                            variant="outlined" 
                            size="small" 
                          />
                        </TableCell>
                        <TableCell>
                          <Chip 
                            label={newsItem.status} 
                            color={getStatusColor(newsItem.status)}
                            size="small"
                          />
                        </TableCell>
                        <TableCell>
                          {formatDate(newsItem.publishedDate || newsItem.createdDate)}
                        </TableCell>
                        <TableCell>{newsItem.viewCount || 0}</TableCell>
                        <TableCell>
                          <Stack direction="row" spacing={1}>
                            <Tooltip title="Edit">
                              <IconButton
                                size="small"
                                onClick={() => handleEdit(newsItem)}
                                color="primary"
                              >
                                <EditIcon />
                              </IconButton>
                            </Tooltip>
                            
                            {newsItem.status === 'Draft' && (
                              <Tooltip title="Publish">
                                <IconButton
                                  size="small"
                                  onClick={() => handlePublish(newsItem.id)}
                                  color="success"
                                >
                                  <PublishIcon />
                                </IconButton>
                              </Tooltip>
                            )}
                            
                            {newsItem.status === 'Published' && (
                              <Tooltip title="Archive">
                                <IconButton
                                  size="small"
                                  onClick={() => handleArchive(newsItem.id)}
                                  color="warning"
                                >
                                  <ArchiveIcon />
                                </IconButton>
                              </Tooltip>
                            )}
                            
                            <Tooltip title="Delete">
                              <IconButton
                                size="small"
                                onClick={() => handleDelete(newsItem.id)}
                                color="error"
                              >
                                <DeleteIcon />
                              </IconButton>
                            </Tooltip>
                          </Stack>
                        </TableCell>
                      </TableRow>
                    ))
                  )}
                </TableBody>
              </Table>
            </TableContainer>

            {/* Pagination */}
            {totalCount > pageSize && (
              <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination
                  currentPage={currentPage}
                  totalCount={totalCount}
                  pageSize={pageSize}
                  onPageChange={setCurrentPage}
                />
              </Box>
            )}
          </CardContent>
        </Card>

        {/* Dialogs */}
        <NewsDialog
          open={isCreateDialogOpen}
          onClose={() => setIsCreateDialogOpen(false)}
          title="Create News"
        />
        <NewsDialog
          open={isEditDialogOpen}
          onClose={() => setIsEditDialogOpen(false)}
          title="Edit News"
        />
      </Box>
    </Container>
  );
};

export default NewsManagementPage;