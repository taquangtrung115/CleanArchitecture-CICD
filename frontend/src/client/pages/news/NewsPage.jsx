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
  Divider,
  Alert,
  Tabs,
  Tab,
  IconButton
} from '@mui/material';
import {
  Search as SearchIcon,
  CalendarToday as CalendarIcon,
  Visibility as ViewIcon,
  TrendingUp as TrendingIcon,
  Whatshot as HotIcon,
  FilterList as FilterIcon,
  Clear as ClearIcon
} from '@mui/icons-material';
import { getNews, getFeaturedNews, getBreakingNews, NewsCategories } from '../../../api/news';
import Pagination from '../../../components/Shared/Pagination';

// Placeholder images for news items
const placeholderImages = [
  '/images/Fabio-Di-Giannantonio-a-tear-off-close-up.jpg',
  '/images/Marc-Marquez-dragging-a-knee.jpg',
  '/images/17-Japan.jpg',
  '/images/Maverick-Vin-ales-earplugs-help-with-his-laser-focus2.jpg',
  '/images/19-australia2.jpg'
];

export default function NewsPage() {
  const [news, setNews] = useState([]);
  const [featuredNews, setFeaturedNews] = useState([]);
  const [breakingNews, setBreakingNews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [tabValue, setTabValue] = useState(0);
  const pageSize = 9;

  // Fetch news data
  const fetchNews = async (page = 1, category = '', search = '') => {
    try {
      setLoading(true);
      const params = {
        pageIndex: page,
        pageSize: pageSize,
        category: category || null,
        searchTerm: search || null
      };

      const response = await getNews(params);
      if (response.isSuccess) {
        setNews(response.value.items || []);
        setTotalCount(response.value.totalCount || 0);
      } else {
        setError('Failed to fetch news');
      }
    } catch (err) {
      setError('Error loading news');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Fetch featured and breaking news
  const fetchSpecialNews = async () => {
    try {
      const [featuredResponse, breakingResponse] = await Promise.all([
        getFeaturedNews(3),
        getBreakingNews()
      ]);

      if (featuredResponse.isSuccess) {
        setFeaturedNews(featuredResponse.value || []);
      }
      if (breakingResponse.isSuccess) {
        setBreakingNews(breakingResponse.value || []);
      }
    } catch (err) {
      console.error('Error fetching special news:', err);
    }
  };

  useEffect(() => {
    fetchNews(currentPage, selectedCategory, searchTerm);
  }, [currentPage, selectedCategory, searchTerm]);

  useEffect(() => {
    fetchSpecialNews();
  }, []);

  // Handle search
  const handleSearch = (e) => {
    if (e.key === 'Enter') {
      setCurrentPage(1);
      fetchNews(1, selectedCategory, searchTerm);
    }
  };

  // Handle category change
  const handleCategoryChange = (event, newValue) => {
    const categories = ['', ...Object.values(NewsCategories)];
    const category = categories[newValue];
    setSelectedCategory(category);
    setCurrentPage(1);
    setTabValue(newValue);
  };

  // Handle page change
  const handlePageChange = (page) => {
    setCurrentPage(page);
    // Scroll to top
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // Clear filters
  const clearFilters = () => {
    setSearchTerm('');
    setSelectedCategory('');
    setTabValue(0);
    setCurrentPage(1);
  };

  // Format date
  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  // Get placeholder image
  const getPlaceholderImage = (index) => {
    return placeholderImages[index % placeholderImages.length];
  };

  // News card component
  const NewsCard = ({ newsItem, index, featured = false }) => (
    <Card
      sx={{
        height: '100%',
        borderRadius: featured ? 4 : 2,
        boxShadow: featured ? 6 : 2,
        transition: 'all 0.3s ease',
        background: 'linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%)',
        border: featured ? '2px solid #e10600' : 'none',
        '&:hover': {
          transform: 'translateY(-8px)',
          boxShadow: featured ? 12 : 8,
          '& .news-image': {
            transform: 'scale(1.05)'
          }
        }
      }}
    >
      <CardActionArea sx={{ height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'stretch' }}>
        <Box sx={{ position: 'relative', overflow: 'hidden' }}>
          <CardMedia
            component="img"
            height={featured ? 200 : 160}
            image={newsItem.featuredImage || getPlaceholderImage(index)}
            alt={newsItem.title}
            className="news-image"
            sx={{
              transition: 'transform 0.3s ease',
              objectFit: 'cover'
            }}
          />
          <Box
            sx={{
              position: 'absolute',
              top: 8,
              left: 8,
              display: 'flex',
              gap: 1
            }}
          >
            <Chip
              label={newsItem.category}
              size="small"
              sx={{
                bgcolor: '#e10600',
                color: 'white',
                fontWeight: 600,
                fontSize: '0.7rem'
              }}
            />
            {newsItem.isFeatured && (
              <Chip
                icon={<TrendingIcon sx={{ fontSize: 14 }} />}
                label="Featured"
                size="small"
                sx={{
                  bgcolor: '#ff9800',
                  color: 'white',
                  fontWeight: 600,
                  fontSize: '0.7rem'
                }}
              />
            )}
            {newsItem.isBreaking && (
              <Chip
                icon={<HotIcon sx={{ fontSize: 14 }} />}
                label="Breaking"
                size="small"
                sx={{
                  bgcolor: '#f44336',
                  color: 'white',
                  fontWeight: 600,
                  fontSize: '0.7rem',
                  animation: 'pulse 2s infinite'
                }}
              />
            )}
          </Box>
        </Box>
        <CardContent sx={{ flexGrow: 1, p: featured ? 3 : 2 }}>
          <Typography
            variant={featured ? 'h5' : 'h6'}
            component="h2"
            sx={{
              fontWeight: 700,
              mb: 1,
              color: 'white',
              fontSize: featured ? '1.5rem' : '1.1rem',
              lineHeight: 1.3,
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              display: '-webkit-box',
              WebkitLineClamp: featured ? 2 : 2,
              WebkitBoxOrient: 'vertical'
            }}
          >
            {newsItem.title}
          </Typography>
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{
              mb: 2,
              lineHeight: 1.4,
              overflow: 'hidden',
              textOverflow: 'ellipsis',
              display: '-webkit-box',
              WebkitLineClamp: featured ? 3 : 2,
              WebkitBoxOrient: 'vertical'
            }}
          >
            {newsItem.summary}
          </Typography>
          <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mt: 'auto' }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <CalendarIcon sx={{ fontSize: 14, color: '#888' }} />
              <Typography variant="caption" color="text.secondary">
                {formatDate(newsItem.publishedDate)}
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              <ViewIcon sx={{ fontSize: 14, color: '#888' }} />
              <Typography variant="caption" color="text.secondary">
                {newsItem.viewCount || 0}
              </Typography>
            </Box>
          </Box>
        </CardContent>
      </CardActionArea>
    </Card>
  );

  // Loading skeleton
  const LoadingSkeleton = () => (
    <Grid container spacing={3}>
      {[...Array(6)].map((_, index) => (
        <Grid item xs={12} sm={6} md={4} key={index}>
          <Card sx={{ borderRadius: 2 }}>
            <Skeleton variant="rectangular" height={160} />
            <CardContent>
              <Skeleton variant="text" sx={{ fontSize: '1.2rem', mb: 1 }} />
              <Skeleton variant="text" sx={{ mb: 1 }} />
              <Skeleton variant="text" width="60%" />
            </CardContent>
          </Card>
        </Grid>
      ))}
    </Grid>
  );

  return (
    <Box sx={{ bgcolor: '#101014', minHeight: '100vh', pb: 8 }}>
      <Container maxWidth="xl" sx={{ pt: 4 }}>
        {/* Page Header */}
        <Fade in timeout={600}>
          <Box sx={{ mb: 6, textAlign: 'center' }}>
            <Typography
              variant="h2"
              sx={{
                fontWeight: 900,
                mb: 2,
                color: '#e10600',
                fontFamily: 'Oswald, Arial Black, sans-serif',
                letterSpacing: 2,
                textShadow: '2px 2px 4px rgba(0,0,0,0.5)'
              }}
            >
              MotoGP NEWS
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ maxWidth: 600, mx: 'auto' }}>
              Stay updated with the latest MotoGP news, race results, rider transfers, and championship updates
            </Typography>
          </Box>
        </Fade>

        {/* Breaking News Section */}
        {breakingNews.length > 0 && (
          <Fade in timeout={800}>
            <Box sx={{ mb: 4 }}>
              <Typography variant="h4" sx={{ fontWeight: 700, mb: 3, color: '#e10600', display: 'flex', alignItems: 'center', gap: 1 }}>
                <HotIcon /> Breaking News
              </Typography>
              <Grid container spacing={3}>
                {breakingNews.slice(0, 2).map((item, index) => (
                  <Grid item xs={12} md={6} key={item.id}>
                    <NewsCard newsItem={item} index={index} featured />
                  </Grid>
                ))}
              </Grid>
            </Box>
          </Fade>
        )}

        {/* Search and Filter Section */}
        <Fade in timeout={1000}>
          <Box sx={{ mb: 4 }}>
            <Grid container spacing={3} alignItems="center">
              <Grid item xs={12} md={6}>
                <TextField
                  fullWidth
                  placeholder="Search news..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  onKeyPress={handleSearch}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <SearchIcon sx={{ color: '#888' }} />
                      </InputAdornment>
                    ),
                    endAdornment: searchTerm && (
                      <InputAdornment position="end">
                        <IconButton onClick={() => setSearchTerm('')} size="small">
                          <ClearIcon />
                        </IconButton>
                      </InputAdornment>
                    )
                  }}
                  sx={{
                    '& .MuiOutlinedInput-root': {
                      bgcolor: 'rgba(255,255,255,0.05)',
                      '& fieldset': { borderColor: 'rgba(255,255,255,0.2)' },
                      '&:hover fieldset': { borderColor: '#e10600' },
                      '&.Mui-focused fieldset': { borderColor: '#e10600' }
                    }
                  }}
                />
              </Grid>
              <Grid item xs={12} md={6}>
                <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
                  <Button
                    startIcon={<FilterIcon />}
                    onClick={clearFilters}
                    variant="outlined"
                    sx={{
                      borderColor: '#e10600',
                      color: '#e10600',
                      '&:hover': { borderColor: '#ff0000', bgcolor: 'rgba(225,6,0,0.1)' }
                    }}
                  >
                    Clear Filters
                  </Button>
                </Box>
              </Grid>
            </Grid>
          </Box>
        </Fade>

        {/* Category Tabs */}
        <Fade in timeout={1200}>
          <Box sx={{ mb: 4 }}>
            <Tabs
              value={tabValue}
              onChange={handleCategoryChange}
              variant="scrollable"
              scrollButtons="auto"
              sx={{
                '& .MuiTab-root': {
                  color: '#888',
                  fontWeight: 600,
                  fontSize: '0.9rem',
                  '&.Mui-selected': { color: '#e10600' }
                },
                '& .MuiTabs-indicator': { backgroundColor: '#e10600' }
              }}
            >
              <Tab label="All News" />
              {Object.values(NewsCategories).map((category) => (
                <Tab key={category} label={category.replace(/([A-Z])/g, ' $1').trim()} />
              ))}
            </Tabs>
            <Divider sx={{ mt: 2, borderColor: 'rgba(255,255,255,0.1)' }} />
          </Box>
        </Fade>

        {/* Error Alert */}
        {error && (
          <Alert severity="error" sx={{ mb: 4 }}>
            {error}
          </Alert>
        )}

        {/* Featured News Section */}
        {featuredNews.length > 0 && !searchTerm && !selectedCategory && (
          <Fade in timeout={1400}>
            <Box sx={{ mb: 6 }}>
              <Typography variant="h4" sx={{ fontWeight: 700, mb: 3, color: '#e10600', display: 'flex', alignItems: 'center', gap: 1 }}>
                <TrendingIcon /> Featured Stories
              </Typography>
              <Grid container spacing={3}>
                {featuredNews.map((item, index) => (
                  <Grid item xs={12} md={4} key={item.id}>
                    <NewsCard newsItem={item} index={index} featured />
                  </Grid>
                ))}
              </Grid>
            </Box>
          </Fade>
        )}

        {/* News Grid */}
        <Fade in timeout={1600}>
          <Box sx={{ mb: 4 }}>
            <Typography variant="h4" sx={{ fontWeight: 700, mb: 3, color: 'white' }}>
              {selectedCategory ? `${selectedCategory} News` : searchTerm ? `Search Results for "${searchTerm}"` : 'Latest News'}
            </Typography>
            
            {loading ? (
              <LoadingSkeleton />
            ) : news.length > 0 ? (
              <Grid container spacing={3}>
                {news.map((item, index) => (
                  <Grid item xs={12} sm={6} md={4} key={item.id}>
                    <NewsCard newsItem={item} index={index} />
                  </Grid>
                ))}
              </Grid>
            ) : (
              <Box sx={{ textAlign: 'center', py: 8 }}>
                <Typography variant="h6" color="text.secondary">
                  No news found. Try adjusting your search or filters.
                </Typography>
              </Box>
            )}
          </Box>
        </Fade>

        {/* Pagination */}
        {!loading && news.length > 0 && (
          <Fade in timeout={1800}>
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <Pagination
                page={currentPage}
                pageSize={pageSize}
                total={totalCount}
                onChange={handlePageChange}
              />
            </Box>
          </Fade>
        )}
      </Container>

      {/* Add pulse animation for breaking news */}
      <style>
        {`
          @keyframes pulse {
            0% { opacity: 1; }
            50% { opacity: 0.7; }
            100% { opacity: 1; }
          }
        `}
      </style>
    </Box>
  );
}
