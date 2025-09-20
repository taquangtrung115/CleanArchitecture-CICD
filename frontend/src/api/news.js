import axios from './axios';
import { getNewsMock, getFeaturedNewsMock, getBreakingNewsMock, getNewsByIdMock } from './mockNews';

// Use mock data for demonstration (set to false when backend is available)
const USE_MOCK_DATA = false;

// Get all news with pagination and filters
export const getNews = async ({
  pageIndex = 1,
  pageSize = 10,
  category = null,
  searchTerm = null,
  isFeatured = null,
  isBreaking = null
} = {}) => {
  if (USE_MOCK_DATA) {
    return getNewsMock({ pageIndex, pageSize, category, searchTerm, isFeatured, isBreaking });
  }

  const params = new URLSearchParams();

  params.append('pageIndex', pageIndex.toString());
  params.append('pageSize', pageSize.toString());

  if (category) params.append('category', category);
  if (searchTerm) params.append('searchTerm', searchTerm);
  if (isFeatured !== null) params.append('isFeatured', isFeatured.toString());
  if (isBreaking !== null) params.append('isBreaking', isBreaking.toString());

  try {
    const response = await axios.get(`/api/v1/motogp/news?${params.toString()}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news:', error);
    throw error;
  }
};

// Get featured news
export const getFeaturedNews = async (limit = 5) => {
  if (USE_MOCK_DATA) {
    return getFeaturedNewsMock(limit);
  }

  try {
    const response = await axios.get(`/api/v1/motogp/news/featured?limit=${limit}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching featured news:', error);
    throw error;
  }
};

// Get breaking news
export const getBreakingNews = async () => {
  if (USE_MOCK_DATA) {
    return getBreakingNewsMock();
  }

  try {
    const response = await axios.get('/api/v1/motogp/news/breaking');
    return response.data;
  } catch (error) {
    console.error('Error fetching breaking news:', error);
    throw error;
  }
};

// Get news by category
export const getNewsByCategory = async (category, pageIndex = 1, pageSize = 10) => {
  if (USE_MOCK_DATA) {
    return getNewsMock({ pageIndex, pageSize, category });
  }

  try {
    const response = await axios.get(`/api/v1/motogp/news/category/${category}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news by category:', error);
    throw error;
  }
};

// Get news by ID
export const getNewsById = async (id) => {
  if (USE_MOCK_DATA) {
    return getNewsByIdMock(id);
  }

  try {
    const response = await axios.get(`/api/v1/motogp/news/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching news by ID:', error);
    throw error;
  }
};

// Get news by slug
export const getNewsBySlug = async (slug) => {
  if (USE_MOCK_DATA) {
    // For mock, find by slug
    const mockNews = await getNewsMock({});
    const news = mockNews.value.items.find(n => n.slug === slug);
    return {
      isSuccess: !!news,
      value: news,
      error: news ? null : 'News not found'
    };
  }

  try {
    const response = await axios.get(`/api/v1/motogp/news/slug/${slug}`);
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

// Admin Functions - Create news
export const createNews = async (newsData) => {
  if (USE_MOCK_DATA) {
    // For mock implementation, just return success with generated ID
    const newNews = {
      id: `news-${Date.now()}`,
      ...newsData,
      status: 'Draft',
      publishedDate: new Date().toISOString(),
      viewCount: 0,
      createdDate: new Date().toISOString(),
      lastModifiedDate: new Date().toISOString()
    };
    return {
      isSuccess: true,
      value: newNews
    };
  }

  try {
    const response = await axios.post('/api/v1/motogp/news', newsData);
    return response.data;
  } catch (error) {
    console.error('Error creating news:', error);
    throw error;
  }
};

// Update news
export const updateNews = async (id, newsData) => {
  if (USE_MOCK_DATA) {
    return {
      isSuccess: true,
      value: null
    };
  }

  try {
    const response = await axios.put(`/api/v1/motogp/news/${id}`, { ...newsData, id });
    return response.data;
  } catch (error) {
    console.error('Error updating news:', error);
    throw error;
  }
};

// Delete news
export const deleteNews = async (id) => {
  if (USE_MOCK_DATA) {
    return {
      isSuccess: true,
      value: null
    };
  }

  try {
    const response = await axios.delete(`/api/v1/motogp/news/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting news:', error);
    throw error;
  }
};

// Publish news
export const publishNews = async (id) => {
  if (USE_MOCK_DATA) {
    return {
      isSuccess: true,
      value: null
    };
  }

  try {
    const response = await axios.post(`/api/v1/motogp/news/${id}/publish`);
    return response.data;
  } catch (error) {
    console.error('Error publishing news:', error);
    throw error;
  }
};

// Archive news
export const archiveNews = async (id) => {
  if (USE_MOCK_DATA) {
    return {
      isSuccess: true,
      value: null
    };
  }

  try {
    const response = await axios.post(`/api/v1/motogp/news/${id}/archive`);
    return response.data;
  } catch (error) {
    console.error('Error archiving news:', error);
    throw error;
  }
};

// Update news slug
export const updateNewsSlug = async (id, slug) => {
  if (USE_MOCK_DATA) {
    return {
      isSuccess: true,
      value: null
    };
  }

  try {
    const response = await axios.put(`/api/v1/motogp/news/${id}/slug`, { slug });
    return response.data;
  } catch (error) {
    console.error('Error updating news slug:', error);
    throw error;
  }
};