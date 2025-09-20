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
  Container
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as VisibilityIcon,
  PlayArrow as PlayIcon,
  Star as StarIcon,
  Publish as PublishIcon,
  VideoLibrary as VideoLibraryIcon,
  AccessTime as TimeIcon,
  CalendarToday as CalendarIcon,
  Save as SaveIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';
import {
  getVideos,
  createVideo,
  updateVideo,
  deleteVideo,
  publishVideo,
  setVideoAsFeatured,
  VideoTypes,
  VideoPlatforms
} from '../api/video';

const VideoManagementPage = () => {
  const [videos, setVideos] = useState([]);
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
  const [selectedVideo, setSelectedVideo] = useState(null);

  // Form state
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    type: 'Highlight',
    videoUrl: '',
    duration: '00:05:00',
    platform: 'YouTube',
    thumbnailUrl: '',
    externalVideoId: '',
    tags: '',
    isFeatured: false
  });

  const pageSize = 10;

  useEffect(() => {
    fetchVideos();
  }, [currentPage, selectedFilter]);

  const fetchVideos = async () => {
    try {
      setLoading(true);
      const params = {
        pageIndex: currentPage,
        pageSize,
        type: selectedFilter || null
      };

      const response = await getVideos(params);
      if (response.isSuccess) {
        setVideos(response.value.items);
        setTotalCount(response.value.totalCount);
      } else {
        setError('Failed to fetch videos');
      }
    } catch (err) {
      setError('Error loading videos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = async () => {
    try {
      setLoading(true);

      // Convert duration string to TimeSpan format for API
      const durationParts = formData.duration.split(':');
      const duration = `${durationParts[0].padStart(2, '0')}:${durationParts[1].padStart(2, '0')}:${durationParts[2] || '00'.padStart(2, '0')}`;

      const videoData = {
        ...formData,
        duration,
        tags: formData.tags ? formData.tags.split(',').map((tag) => tag.trim()) : []
      };

      const response = await createVideo(videoData);
      if (response.isSuccess) {
        setSuccess('Video created successfully');
        setIsCreateDialogOpen(false);
        resetForm();
        fetchVideos();
      } else {
        setError('Failed to create video');
      }
    } catch (err) {
      setError('Error creating video');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = async () => {
    try {
      setLoading(true);

      const durationParts = formData.duration.split(':');
      const duration = `${durationParts[0].padStart(2, '0')}:${durationParts[1].padStart(2, '0')}:${durationParts[2] || '00'.padStart(2, '0')}`;

      const videoData = {
        id: selectedVideo.id,
        ...formData,
        duration,
        tags: formData.tags ? formData.tags.split(',').map((tag) => tag.trim()) : []
      };

      const response = await updateVideo(selectedVideo.id, videoData);
      if (response.isSuccess) {
        setSuccess('Video updated successfully');
        setIsEditDialogOpen(false);
        resetForm();
        fetchVideos();
      } else {
        setError('Failed to update video');
      }
    } catch (err) {
      setError('Error updating video');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (videoId) => {
    if (window.confirm('Are you sure you want to delete this video?')) {
      try {
        setLoading(true);
        const response = await deleteVideo(videoId);
        if (response.isSuccess) {
          setSuccess('Video deleted successfully');
          fetchVideos();
        } else {
          setError('Failed to delete video');
        }
      } catch (err) {
        setError('Error deleting video');
        console.error(err);
      } finally {
        setLoading(false);
      }
    }
  };

  const handlePublish = async (videoId) => {
    try {
      setLoading(true);
      const response = await publishVideo(videoId);
      if (response.isSuccess) {
        setSuccess('Video published successfully');
        fetchVideos();
      } else {
        setError('Failed to publish video');
      }
    } catch (err) {
      setError('Error publishing video');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleToggleFeatured = async (videoId) => {
    try {
      setLoading(true);
      const response = await setVideoAsFeatured(videoId);
      if (response.isSuccess) {
        setSuccess('Video feature status updated');
        fetchVideos();
      } else {
        setError('Failed to update video feature status');
      }
    } catch (err) {
      setError('Error updating feature status');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const openEditDialog = (video) => {
    setSelectedVideo(video);
    setFormData({
      title: video.title,
      description: video.description,
      type: video.type,
      videoUrl: video.videoUrl,
      duration: video.formattedDuration || '00:05:00',
      platform: video.platform || 'YouTube',
      thumbnailUrl: video.thumbnailUrl || '',
      externalVideoId: video.externalVideoId || '',
      tags: video.tags ? video.tags.join(', ') : '',
      isFeatured: video.isFeatured
    });
    setIsEditDialogOpen(true);
  };

  const resetForm = () => {
    setFormData({
      title: '',
      description: '',
      type: 'Highlight',
      videoUrl: '',
      duration: '00:05:00',
      platform: 'YouTube',
      thumbnailUrl: '',
      externalVideoId: '',
      tags: '',
      isFeatured: false
    });
    setSelectedVideo(null);
  };

  const handleInputChange = (field, value) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value
    }));
  };

  const getVideoTypeColor = (type) => {
    const colors = {
      Highlight: '#e10600',
      Interview: '#ff9800',
      Analysis: '#2196f3',
      OnBoard: '#4caf50',
      PressConference: '#9c27b0',
      Documentary: '#607d8b',
      LiveStream: '#f44336'
    };
    return colors[type] || '#757575';
  };

  const VideoForm = () => (
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <TextField fullWidth label="Title" value={formData.title} onChange={(e) => handleInputChange('title', e.target.value)} required />
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          label="Description"
          value={formData.description}
          onChange={(e) => handleInputChange('description', e.target.value)}
          multiline
          rows={3}
          required
        />
      </Grid>

      <Grid item xs={12} sm={6}>
        <FormControl fullWidth>
          <InputLabel>Type</InputLabel>
          <Select value={formData.type} onChange={(e) => handleInputChange('type', e.target.value)} label="Type">
            {Object.values(VideoTypes).map((type) => (
              <MenuItem key={type} value={type}>
                {type}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>

      <Grid item xs={12} sm={6}>
        <FormControl fullWidth>
          <InputLabel>Platform</InputLabel>
          <Select value={formData.platform} onChange={(e) => handleInputChange('platform', e.target.value)} label="Platform">
            {Object.values(VideoPlatforms).map((platform) => (
              <MenuItem key={platform} value={platform}>
                {platform}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          label="Video URL"
          value={formData.videoUrl}
          onChange={(e) => handleInputChange('videoUrl', e.target.value)}
          placeholder="https://www.youtube.com/embed/..."
          required
        />
      </Grid>

      <Grid item xs={12} sm={6}>
        <TextField
          fullWidth
          label="Duration (MM:SS)"
          value={formData.duration}
          onChange={(e) => handleInputChange('duration', e.target.value)}
          placeholder="05:30"
          required
        />
      </Grid>

      <Grid item xs={12} sm={6}>
        <TextField
          fullWidth
          label="External Video ID"
          value={formData.externalVideoId}
          onChange={(e) => handleInputChange('externalVideoId', e.target.value)}
          placeholder="YouTube video ID"
        />
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          label="Thumbnail URL"
          value={formData.thumbnailUrl}
          onChange={(e) => handleInputChange('thumbnailUrl', e.target.value)}
          placeholder="https://..."
        />
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          label="Tags (comma separated)"
          value={formData.tags}
          onChange={(e) => handleInputChange('tags', e.target.value)}
          placeholder="highlights, ducati, 2024"
        />
      </Grid>

      <Grid item xs={12}>
        <FormControlLabel
          control={<Switch checked={formData.isFeatured} onChange={(e) => handleInputChange('isFeatured', e.target.checked)} />}
          label="Featured Video"
        />
      </Grid>
    </Grid>
  );

  return (
    <Container maxWidth="xl" sx={{ py: 4 }}>
      {/* Header */}
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" sx={{ fontWeight: 700, mb: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
          <VideoLibraryIcon sx={{ fontSize: 40, color: '#e10600' }} />
          Video Management
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Manage MotoGP videos, create new content, and organize your video library
        </Typography>
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

      {/* Actions */}
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap', gap: 2 }}>
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center' }}>
          <FormControl sx={{ minWidth: 200 }}>
            <InputLabel>Filter by Type</InputLabel>
            <Select value={selectedFilter} onChange={(e) => setSelectedFilter(e.target.value)} label="Filter by Type">
              <MenuItem value="">All Types</MenuItem>
              {Object.values(VideoTypes).map((type) => (
                <MenuItem key={type} value={type}>
                  {type}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>

        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setIsCreateDialogOpen(true)}
          sx={{
            backgroundColor: '#e10600',
            '&:hover': { backgroundColor: '#c10500' }
          }}
        >
          Add New Video
        </Button>
      </Box>

      {/* Videos Table */}
      <Paper sx={{ borderRadius: 2, overflow: 'hidden' }}>
        <TableContainer>
          <Table>
            <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
              <TableRow>
                <TableCell sx={{ fontWeight: 700 }}>Video</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Type</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Status</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Views</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Duration</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Published</TableCell>
                <TableCell sx={{ fontWeight: 700 }}>Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {videos.map((video) => (
                <TableRow key={video.id} hover>
                  <TableCell>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Box
                        sx={{
                          width: 80,
                          height: 45,
                          borderRadius: 1,
                          backgroundColor: '#f0f0f0',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          backgroundImage: video.thumbnailUrl ? `url(${video.thumbnailUrl})` : 'linear-gradient(45deg, #e10600, #ff4444)',
                          backgroundSize: 'cover',
                          backgroundPosition: 'center'
                        }}
                      >
                        <PlayIcon sx={{ color: 'white', fontSize: 20 }} />
                      </Box>
                      <Box>
                        <Typography variant="subtitle2" sx={{ fontWeight: 600, lineHeight: 1.2 }}>
                          {video.title}
                        </Typography>
                        <Typography variant="caption" color="text.secondary">
                          {video.platform}
                        </Typography>
                      </Box>
                    </Box>
                  </TableCell>

                  <TableCell>
                    <Chip
                      label={video.type}
                      size="small"
                      sx={{
                        backgroundColor: getVideoTypeColor(video.type),
                        color: 'white',
                        fontWeight: 600
                      }}
                    />
                  </TableCell>

                  <TableCell>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <Chip
                        label={video.status}
                        size="small"
                        color={video.status === 'Published' ? 'success' : 'default'}
                        variant={video.status === 'Published' ? 'filled' : 'outlined'}
                      />
                      {video.isFeatured && (
                        <Chip
                          icon={<StarIcon sx={{ fontSize: 14 }} />}
                          label="Featured"
                          size="small"
                          sx={{ backgroundColor: '#e10600', color: 'white' }}
                        />
                      )}
                    </Box>
                  </TableCell>

                  <TableCell>
                    <Typography variant="body2">{video.viewCount?.toLocaleString() || 0}</Typography>
                  </TableCell>

                  <TableCell>
                    <Typography variant="body2" sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <TimeIcon sx={{ fontSize: 16 }} />
                      {video.formattedDuration}
                    </Typography>
                  </TableCell>

                  <TableCell>
                    <Typography variant="body2">{new Date(video.publishedDate).toLocaleDateString()}</Typography>
                  </TableCell>

                  <TableCell>
                    <Box sx={{ display: 'flex', gap: 1 }}>
                      <Tooltip title="Edit">
                        <IconButton size="small" onClick={() => openEditDialog(video)}>
                          <EditIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>

                      {video.status !== 'Published' && (
                        <Tooltip title="Publish">
                          <IconButton size="small" onClick={() => handlePublish(video.id)} sx={{ color: 'green' }}>
                            <PublishIcon fontSize="small" />
                          </IconButton>
                        </Tooltip>
                      )}

                      <Tooltip title={video.isFeatured ? 'Remove from Featured' : 'Set as Featured'}>
                        <IconButton
                          size="small"
                          onClick={() => handleToggleFeatured(video.id)}
                          sx={{ color: video.isFeatured ? '#e10600' : 'default' }}
                        >
                          <StarIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>

                      <Tooltip title="Delete">
                        <IconButton size="small" onClick={() => handleDelete(video.id)} sx={{ color: 'error.main' }}>
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Tooltip>
                    </Box>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      {/* Pagination */}
      {totalCount > pageSize && (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3 }}>
          <Button disabled={currentPage === 1} onClick={() => setCurrentPage(currentPage - 1)}>
            Previous
          </Button>
          <Typography sx={{ mx: 2, display: 'flex', alignItems: 'center' }}>
            Page {currentPage} of {Math.ceil(totalCount / pageSize)}
          </Typography>
          <Button disabled={currentPage >= Math.ceil(totalCount / pageSize)} onClick={() => setCurrentPage(currentPage + 1)}>
            Next
          </Button>
        </Box>
      )}

      {/* Create Video Dialog */}
      <Dialog open={isCreateDialogOpen} onClose={() => setIsCreateDialogOpen(false)} maxWidth="md" fullWidth>
        <DialogTitle>
          <Typography variant="h6" sx={{ fontWeight: 700 }}>
            Create New Video
          </Typography>
        </DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 2 }}>
            <VideoForm />
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: 3, pt: 1 }}>
          <Button onClick={() => setIsCreateDialogOpen(false)} startIcon={<CancelIcon />}>
            Cancel
          </Button>
          <Button
            variant="contained"
            onClick={handleCreate}
            disabled={loading}
            startIcon={<SaveIcon />}
            sx={{
              backgroundColor: '#e10600',
              '&:hover': { backgroundColor: '#c10500' }
            }}
          >
            Create Video
          </Button>
        </DialogActions>
      </Dialog>

      {/* Edit Video Dialog */}
      <Dialog open={isEditDialogOpen} onClose={() => setIsEditDialogOpen(false)} maxWidth="md" fullWidth>
        <DialogTitle>
          <Typography variant="h6" sx={{ fontWeight: 700 }}>
            Edit Video
          </Typography>
        </DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 2 }}>
            <VideoForm />
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: 3, pt: 1 }}>
          <Button onClick={() => setIsEditDialogOpen(false)} startIcon={<CancelIcon />}>
            Cancel
          </Button>
          <Button
            variant="contained"
            onClick={handleEdit}
            disabled={loading}
            startIcon={<SaveIcon />}
            sx={{
              backgroundColor: '#e10600',
              '&:hover': { backgroundColor: '#c10500' }
            }}
          >
            Update Video
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default VideoManagementPage;
