import axios from './axios';

// Get all news with pagination and filters
export const getNews = async ({ 
  pageIndex = 1, 
  pageSize = 10, 
  category = null, 
  searchTerm = null, 
  isFeatured = null, 
  isBreaking = null 
} = {}) => {
  const params = new URLSearchParams();
  
  params.append('pageIndex', pageIndex.toString());
  params.append('pageSize', pageSize.toString());
  
  if (category) params.append('category', category);
  if (searchTerm) params.append('searchTerm', searchTerm);
  if (isFeatured !== null) params.append('isFeatured', isFeatured.toString());
  if (isBreaking !== null) params.append('isBreaking', isBreaking.toString());

  try {
    const response = await axios.get(`/api/v1/news?${params.toString()}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news:', error);
    throw error;
  }
};

// Get featured news
export const getFeaturedNews = async (limit = 5) => {
  try {
    const response = await axios.get(`/api/v1/news/featured?limit=${limit}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching featured news:', error);
    throw error;
  }
};

// Get breaking news
export const getBreakingNews = async () => {
  try {
    const response = await axios.get('/api/v1/news/breaking');
    return response.data;
  } catch (error) {
    console.error('Error fetching breaking news:', error);
    throw error;
  }
};

// Get news by category
export const getNewsByCategory = async (category, pageIndex = 1, pageSize = 10) => {
  try {
    const response = await axios.get(`/api/v1/news/category/${category}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news by category:', error);
    throw error;
  }
};

// Get news by ID
export const getNewsById = async (id) => {
  try {
    const response = await axios.get(`/api/v1/news/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news by ID:', error);
    throw error;
  }
};

// Get news by slug
export const getNewsBySlug = async (slug) => {
  try {
    const response = await axios.get(`/api/v1/news/slug/${slug}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news by slug:', error);
    throw error;
  }
};

// News categories
export const NewsCategories = {
  GENERAL: 'General',
  RACE_RESULTS: 'RaceResults',
  TRANSFERS: 'Transfers',
  TECHNICAL: 'Technical',
  INTERVIEWS: 'Interviews',
  CHAMPIONSHIP: 'Championship',
  BREAKING: 'Breaking'
};