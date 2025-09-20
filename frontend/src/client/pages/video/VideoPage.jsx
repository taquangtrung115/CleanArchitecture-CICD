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
  Dialog,
  DialogContent,
  DialogTitle,
  IconButton,
  Tooltip,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  List,
  ListItem,
  ListItemText,
  Avatar,
  Divider
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
  FilterList as FilterIcon,
  Close as CloseIcon,
  ExpandMore as ExpandMoreIcon,
  Tag as TagIcon,
  Fullscreen as FullscreenIcon
} from '@mui/icons-material';
import { getVideos, getFeaturedVideos, VideoTypes, getVideosByType, getVideoById, incrementVideoViewCount } from '../../../api/video';
import Pagination from '../../../components/Shared/Pagination';

export default function VideoPage() {
  const [videos, setVideos] = useState([]);
  const [featuredVideos, setFeaturedVideos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedType, setSelectedType] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [selectedVideo, setSelectedVideo] = useState(null);
  const [isVideoDialogOpen, setIsVideoDialogOpen] = useState(false);
  const [relatedVideos, setRelatedVideos] = useState([]);
  const pageSize = 16;

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
    fetchVideos(1, selectedType, searchTerm);
    setCurrentPage(1);
  }, [selectedType, searchTerm]);

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

  const handleVideoClick = async (video) => {
    setSelectedVideo(video);
    setIsVideoDialogOpen(true);

    // Increment view count
    try {
      await incrementVideoViewCount(video.id);
    } catch (err) {
      console.error('Error incrementing view count:', err);
    }

    // Fetch related videos (same type, excluding current video)
    try {
      const response = await getVideosByType(video.type, 1, 6);
      if (response.isSuccess) {
        const related = response.value.items.filter((v) => v.id !== video.id);
        setRelatedVideos(related.slice(0, 4));
      }
    } catch (err) {
      console.error('Error fetching related videos:', err);
    }
  };

  const handleCloseVideoDialog = () => {
    setIsVideoDialogOpen(false);
    setSelectedVideo(null);
    setRelatedVideos([]);
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
      month: 'long',
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

  const VideoCard = ({ video, size = 'normal' }) => (
    <Card
      sx={{
        height: '100%',
        borderRadius: 2,
        boxShadow: video.isFeatured ? 4 : 2,
        border: video.isFeatured ? '2px solid #e10600' : 'none',
        transition: 'all 0.3s ease',
        cursor: 'pointer',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 6
        }
      }}
      onClick={() => handleVideoClick(video)}
    >
      <CardActionArea>
        <Box sx={{ position: 'relative' }}>
          <CardMedia
            component="div"
            sx={{
              height: size === 'large' ? 280 : size === 'small' ? 140 : 200,
              backgroundImage: video.thumbnailUrl ? `url(${video.thumbnailUrl})` : 'linear-gradient(45deg, #e10600, #ff4444)',
              backgroundSize: 'cover',
              backgroundPosition: 'center',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }}
          >
            <PlayIcon
              sx={{
                fontSize: size === 'large' ? 80 : size === 'small' ? 40 : 60,
                color: 'rgba(255,255,255,0.9)',
                backgroundColor: 'rgba(0,0,0,0.5)',
                borderRadius: '50%',
                p: 1
              }}
            />
          </CardMedia>
          <Box
            sx={{
              position: 'absolute',
              bottom: 8,
              right: 8,
              backgroundColor: 'rgba(0,0,0,0.8)',
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
        <CardContent sx={{ p: size === 'small' ? 1.5 : 2 }}>
          <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 1, mb: 1 }}>
            <Chip
              label={video.type}
              size="small"
              sx={{
                backgroundColor: getVideoTypeColor(video.type),
                color: 'white',
                fontWeight: 600,
                fontSize: size === 'small' ? '0.7rem' : '0.75rem'
              }}
            />
            {video.platform && (
              <Chip label={video.platform} size="small" variant="outlined" sx={{ fontSize: size === 'small' ? '0.7rem' : '0.75rem' }} />
            )}
          </Box>

          <Typography
            variant={size === 'large' ? 'h6' : size === 'small' ? 'body2' : 'subtitle1'}
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

          {size !== 'small' && (
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
          )}

          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 'auto' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <ViewIcon sx={{ fontSize: 16, color: 'text.secondary' }} />
              <Typography variant="caption" color="text.secondary">
                {formatViewCount(video.viewCount)}
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
              MotoGP Video Library
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ maxWidth: 600, mx: 'auto' }}>
              Discover the complete collection of MotoGP videos - from race highlights to exclusive interviews
            </Typography>
          </Box>

          {/* Featured Videos Section */}
          {featuredVideos.length > 0 && (
            <Box sx={{ mb: 6 }}>
              <Typography
                variant="h4"
                sx={{
                  fontWeight: 700,
                  mb: 3,
                  color: '#e10600',
                  display: 'flex',
                  alignItems: 'center',
                  gap: 2
                }}
              >
                <StarIcon sx={{ fontSize: 32 }} />
                Featured Videos
              </Typography>
              <Grid container spacing={3}>
                {featuredVideos.slice(0, 3).map((video) => (
                  <Grid item xs={12} md={4} key={video.id}>
                    <VideoCard video={video} size="large" />
                  </Grid>
                ))}
              </Grid>
            </Box>
          )}

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

          {/* All Videos Section */}
          <Box sx={{ mb: 4 }}>
            <Typography
              variant="h4"
              sx={{
                fontWeight: 700,
                mb: 3,
                color: 'white',
                display: 'flex',
                alignItems: 'center',
                gap: 2
              }}
            >
              <OndemandVideoIcon sx={{ fontSize: 32 }} />
              All Videos
              <Typography variant="h6" color="text.secondary" sx={{ ml: 1 }}>
                ({totalCount} videos)
              </Typography>
            </Typography>

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
          </Box>
        </Container>

        {/* Video Player Dialog */}
        <Dialog
          open={isVideoDialogOpen}
          onClose={handleCloseVideoDialog}
          maxWidth="lg"
          fullWidth
          sx={{
            '& .MuiDialog-paper': {
              backgroundColor: '#1a1a1a',
              color: 'white'
            }
          }}
        >
          <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', pb: 1 }}>
            <Typography variant="h6" sx={{ fontWeight: 700 }}>
              {selectedVideo?.title}
            </Typography>
            <IconButton onClick={handleCloseVideoDialog} sx={{ color: 'white' }}>
              <CloseIcon />
            </IconButton>
          </DialogTitle>
          <DialogContent sx={{ p: 0 }}>
            {selectedVideo && (
              <Box>
                {/* Video Player */}
                <Box sx={{ position: 'relative', paddingBottom: '56.25%', height: 0 }}>
                  <iframe
                    src={selectedVideo.videoUrl}
                    style={{
                      position: 'absolute',
                      top: 0,
                      left: 0,
                      width: '100%',
                      height: '100%'
                    }}
                    frameBorder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                    allowFullScreen
                    title={selectedVideo.title}
                  />
                </Box>

                {/* Video Info */}
                <Box sx={{ p: 3 }}>
                  <Box sx={{ display: 'flex', gap: 1, mb: 2, flexWrap: 'wrap' }}>
                    <Chip
                      label={selectedVideo.type}
                      sx={{
                        backgroundColor: getVideoTypeColor(selectedVideo.type),
                        color: 'white',
                        fontWeight: 600
                      }}
                    />
                    {selectedVideo.platform && <Chip label={selectedVideo.platform} variant="outlined" />}
                    {selectedVideo.isFeatured && (
                      <Chip icon={<StarIcon />} label="Featured" sx={{ backgroundColor: '#e10600', color: 'white' }} />
                    )}
                  </Box>

                  <Typography variant="body1" sx={{ mb: 2, lineHeight: 1.6 }}>
                    {selectedVideo.description}
                  </Typography>

                  <Box sx={{ display: 'flex', gap: 3, mb: 2, color: 'text.secondary' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <ViewIcon sx={{ fontSize: 18 }} />
                      <Typography variant="body2">{formatViewCount(selectedVideo.viewCount)} views</Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <CalendarIcon sx={{ fontSize: 18 }} />
                      <Typography variant="body2">{formatDate(selectedVideo.publishedDate)}</Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                      <TimeIcon sx={{ fontSize: 18 }} />
                      <Typography variant="body2">{selectedVideo.formattedDuration}</Typography>
                    </Box>
                  </Box>

                  {selectedVideo.tags && selectedVideo.tags.length > 0 && (
                    <Box sx={{ mb: 3 }}>
                      <Typography variant="subtitle2" sx={{ mb: 1, display: 'flex', alignItems: 'center', gap: 1 }}>
                        <TagIcon sx={{ fontSize: 18 }} />
                        Tags
                      </Typography>
                      <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                        {selectedVideo.tags.map((tag) => (
                          <Chip key={tag} label={tag} size="small" variant="outlined" sx={{ borderColor: 'rgba(255,255,255,0.3)' }} />
                        ))}
                      </Box>
                    </Box>
                  )}

                  {/* Related Videos */}
                  {relatedVideos.length > 0 && (
                    <Box>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 700 }}>
                        Related Videos
                      </Typography>
                      <Grid container spacing={2}>
                        {relatedVideos.map((video) => (
                          <Grid item xs={12} sm={6} key={video.id}>
                            <VideoCard video={video} size="small" />
                          </Grid>
                        ))}
                      </Grid>
                    </Box>
                  )}
                </Box>
              </Box>
            )}
          </DialogContent>
        </Dialog>
      </Box>
    </Fade>
  );
}
