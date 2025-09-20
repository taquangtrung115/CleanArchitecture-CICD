import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Grid,
  Card,
  CardMedia,
  CardContent,
  CardActionArea,
  Chip,
  TextField,
  InputAdornment,
  Button,
  Fade,
  Skeleton,
  Container,
  Tabs,
  Tab,
  IconButton,
  Avatar,
  Tooltip
} from '@mui/material';
import {
  Search as SearchIcon,
  PlayArrow as PlayIcon,
  Visibility as ViewIcon,
  Star as StarIcon,
  CalendarToday as CalendarIcon,
  AccessTime as TimeIcon,
  VideoLibrary as VideoLibraryIcon,
  OndemandVideo as OndemandVideoIcon,
  FilterList as FilterIcon
} from '@mui/icons-material';
import { getVideos, getFeaturedVideos, VideoTypes, getVideosByType } from '../../../api/video';
import Pagination from '../../../components/Shared/Pagination';

export default function MediaPage() {
  const [videos, setVideos] = useState([]);
  const [featuredVideos, setFeaturedVideos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedType, setSelectedType] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [tabValue, setTabValue] = useState(0);
  const pageSize = 12;

  // Fetch videos data
  const fetchVideos = async (page = 1, type = '', search = '') => {
    try {
      setLoading(true);
      const params = {
        pageIndex: page,
        pageSize: pageSize,
        type: type || null,
        searchTerm: search || null
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
      console.error('Error fetching videos:', err);
    } finally {
      setLoading(false);
    }
  };

  // Fetch featured videos
  const fetchFeaturedVideos = async () => {
    try {
      const response = await getFeaturedVideos();
      if (response.isSuccess) {
        setFeaturedVideos(response.value);
      }
    } catch (err) {
      console.error('Error fetching featured videos:', err);
    }
  };

  useEffect(() => {
    fetchFeaturedVideos();
    fetchVideos();
  }, []);

  useEffect(() => {
    if (tabValue === 0) {
      fetchVideos(1, selectedType, searchTerm);
    }
    setCurrentPage(1);
  }, [selectedType, searchTerm, tabValue]);

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  const handleTypeFilter = (type) => {
    setSelectedType(selectedType === type ? '' : type);
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
    fetchVideos(page, selectedType, searchTerm);
  };

  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
    if (newValue === 0) {
      fetchVideos(1, selectedType, searchTerm);
    }
  };

  const formatViewCount = (count) => {
    if (count >= 1000000) {
      return `${(count / 1000000).toFixed(1)}M`;
    } else if (count >= 1000) {
      return `${(count / 1000).toFixed(1)}K`;
    }
    return count.toString();
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
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

  const VideoCard = ({ video, isFeatured = false }) => (
    <Card
      sx={{
        height: '100%',
        borderRadius: 2,
        boxShadow: isFeatured ? 4 : 2,
        border: isFeatured ? '2px solid #e10600' : 'none',
        transition: 'all 0.3s ease',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 6
        }
      }}
    >
      <CardActionArea>
        <Box sx={{ position: 'relative' }}>
          <CardMedia
            component="iframe"
            height={isFeatured ? 300 : 200}
            src={video.videoUrl}
            title={video.title}
            sx={{ borderRadius: '8px 8px 0 0' }}
            frameBorder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            allowFullScreen
          />
          <Box
            sx={{
              position: 'absolute',
              top: 8,
              right: 8,
              backgroundColor: 'rgba(0,0,0,0.7)',
              color: 'white',
              padding: '4px 8px',
              borderRadius: 1,
              fontSize: '0.875rem',
              display: 'flex',
              alignItems: 'center',
              gap: 0.5
            }}
          >
            <TimeIcon sx={{ fontSize: 16 }} />
            {video.formattedDuration}
          </Box>
          {video.isFeatured && (
            <Box
              sx={{
                position: 'absolute',
                top: 8,
                left: 8,
                backgroundColor: '#e10600',
                color: 'white',
                padding: '4px 8px',
                borderRadius: 1,
                fontSize: '0.75rem',
                display: 'flex',
                alignItems: 'center',
                gap: 0.5
              }}
            >
              <StarIcon sx={{ fontSize: 14 }} />
              FEATURED
            </Box>
          )}
        </Box>
        <CardContent sx={{ p: 2 }}>
          <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 1, mb: 1 }}>
            <Chip
              label={video.type}
              size="small"
              sx={{
                backgroundColor: getVideoTypeColor(video.type),
                color: 'white',
                fontWeight: 600
              }}
            />
            {video.platform && <Chip label={video.platform} size="small" variant="outlined" sx={{ fontSize: '0.75rem' }} />}
          </Box>

          <Typography
            variant={isFeatured ? 'h6' : 'subtitle1'}
            sx={{
              fontWeight: 700,
              mb: 1,
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden',
              lineHeight: 1.3
            }}
          >
            {video.title}
          </Typography>

          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              mb: 2,
              display: '-webkit-box',
              WebkitLineClamp: 2,
              WebkitBoxOrient: 'vertical',
              overflow: 'hidden'
            }}
          >
            {video.description}
          </Typography>

          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 'auto' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <ViewIcon sx={{ fontSize: 16, color: 'text.secondary' }} />
              <Typography variant="caption" color="text.secondary">
                {formatViewCount(video.viewCount)} views
              </Typography>
            </Box>
            <Typography variant="caption" color="text.secondary">
              {formatDate(video.publishedDate)}
            </Typography>
          </Box>
        </CardContent>
      </CardActionArea>
    </Card>
  );

  if (error) {
    return (
      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Typography color="error" variant="h6" align="center">
          {error}
        </Typography>
      </Container>
    );
  }

  return (
    <Fade in timeout={900}>
      <Box sx={{ bgcolor: '#101014', minHeight: '100vh', pb: 8 }}>
        <Container maxWidth="xl" sx={{ py: 4 }}>
          {/* Header */}
          <Box sx={{ mb: 4, textAlign: 'center' }}>
            <Typography
              variant="h3"
              sx={{
                fontWeight: 900,
                color: '#e10600',
                fontFamily: 'Oswald, Arial Black, sans-serif',
                letterSpacing: 1,
                mb: 2
              }}
            >
              <VideoLibraryIcon sx={{ fontSize: 40, mr: 2, verticalAlign: 'middle' }} />
              MotoGP Videos
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ maxWidth: 600, mx: 'auto' }}>
              Experience the thrill of MotoGP with exclusive videos, highlights, interviews, and behind-the-scenes content
            </Typography>
          </Box>

          {/* Search and Filter Controls */}
          <Box sx={{ mb: 4 }}>
            <Grid container spacing={2} alignItems="center">
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  variant="outlined"
                  placeholder="Search videos..."
                  value={searchTerm}
                  onChange={handleSearch}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <SearchIcon />
                      </InputAdornment>
                    )
                  }}
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      backgroundColor: 'rgba(255,255,255,0.1)',
                      '& fieldset': { borderColor: 'rgba(255,255,255,0.3)' },
                      '&:hover fieldset': { borderColor: '#e10600' },
                      '&.Mui-focused fieldset': { borderColor: '#e10600' }
                    }
                  }}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                  {Object.values(VideoTypes).map((type) => (
                    <Button
                      key={type}
                      variant={selectedType === type ? 'contained' : 'outlined'}
                      size="small"
                      onClick={() => handleTypeFilter(type)}
                      sx={{
                        borderColor: selectedType === type ? '#e10600' : 'rgba(255,255,255,0.3)',
                        backgroundColor: selectedType === type ? '#e10600' : 'transparent',
                        color: selectedType === type ? 'white' : 'rgba(255,255,255,0.8)',
                        '&:hover': {
                          borderColor: '#e10600',
                          backgroundColor: selectedType === type ? '#c10500' : 'rgba(225,6,0,0.1)'
                        }
                      }}
                    >
                      {type}
                    </Button>
                  ))}
                </Box>
              </Grid>
            </Grid>
          </Box>

          {/* Tabs */}
          <Box sx={{ mb: 3 }}>
            <Tabs
              value={tabValue}
              onChange={handleTabChange}
              sx={{
                '& .MuiTabs-indicator': { backgroundColor: '#e10600' },
                '& .MuiTab-root': {
                  color: 'rgba(255,255,255,0.7)',
                  '&.Mui-selected': { color: '#e10600' }
                }
              }}
            >
              <Tab label="All Videos" />
              <Tab label="Featured Videos" />
            </Tabs>
          </Box>

          {/* Content */}
          {tabValue === 0 ? (
            // All Videos Tab
            <>
              {loading ? (
                <Grid container spacing={3}>
                  {Array.from({ length: 8 }).map((_, index) => (
                    <Grid item xs={12} sm={6} md={4} lg={3} key={index}>
                      <Card sx={{ height: 320 }}>
                        <Skeleton variant="rectangular" height={200} />
                        <CardContent>
                          <Skeleton variant="text" height={24} />
                          <Skeleton variant="text" height={20} />
                          <Skeleton variant="text" height={16} width="60%" />
                        </CardContent>
                      </Card>
                    </Grid>
                  ))}
                </Grid>
              ) : (
                <>
                  <Grid container spacing={3}>
                    {videos.map((video) => (
                      <Grid item xs={12} sm={6} md={4} lg={3} key={video.id}>
                        <VideoCard video={video} />
                      </Grid>
                    ))}
                  </Grid>

                  {/* Pagination */}
                  {totalCount > pageSize && (
                    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                      <Pagination currentPage={currentPage} totalPages={Math.ceil(totalCount / pageSize)} onPageChange={handlePageChange} />
                    </Box>
                  )}
                </>
              )}
            </>
          ) : (
            // Featured Videos Tab
            <Grid container spacing={3}>
              {featuredVideos.map((video) => (
                <Grid item xs={12} md={6} lg={4} key={video.id}>
                  <VideoCard video={video} isFeatured />
                </Grid>
              ))}
            </Grid>
          )}
        </Container>
      </Box>
    </Fade>
  );
}
