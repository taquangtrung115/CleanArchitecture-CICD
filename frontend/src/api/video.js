import axios from './axios';

// Use mock data for demonstration (set to false when backend is available)
const USE_MOCK_DATA = false;

// Mock video data
const mockVideos = [
  {
    id: '1',
    title: 'MotoGP 2024 Race Highlights - Best Overtakes',
    description:
      'Experience the most thrilling overtakes from the 2024 MotoGP season with heart-stopping moments from Ducati, Yamaha, and KTM riders.',
    type: 'Highlight',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/1QhQF6l1bJw',
    thumbnailUrl: '/images/video-thumb-1.jpg',
    formattedDuration: '05:30',
    publishedDate: '2024-01-15T10:00:00Z',
    viewCount: 125000,
    isFeatured: true,
    platform: 'YouTube',
    externalVideoId: '1QhQF6l1bJw',
    tags: ['highlights', 'overtakes', '2024', 'ducati'],
    createdDate: '2024-01-15T10:00:00Z'
  },
  {
    id: '2',
    title: 'Marc Marquez Exclusive Interview - Championship Ambitions',
    description: 'Marc Marquez opens up about his championship goals and his move to Ducati in this exclusive one-on-one interview.',
    type: 'Interview',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/dQw4w9WgXcQ',
    thumbnailUrl: '/images/video-thumb-2.jpg',
    formattedDuration: '12:45',
    publishedDate: '2024-01-12T14:30:00Z',
    viewCount: 89000,
    isFeatured: true,
    platform: 'YouTube',
    externalVideoId: 'dQw4w9WgXcQ',
    tags: ['interview', 'marc-marquez', 'ducati', 'championship'],
    createdDate: '2024-01-12T14:30:00Z'
  },
  {
    id: '3',
    title: 'OnBoard Camera - Pecco Bagnaia Lap Record',
    description: "Experience the track from Pecco Bagnaia's perspective as he sets a new lap record at Misano.",
    type: 'OnBoard',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/ScMzIvxBSi4',
    thumbnailUrl: '/images/video-thumb-3.jpg',
    formattedDuration: '01:52',
    publishedDate: '2024-01-10T16:20:00Z',
    viewCount: 76000,
    isFeatured: false,
    platform: 'YouTube',
    externalVideoId: 'ScMzIvxBSi4',
    tags: ['onboard', 'bagnaia', 'misano', 'lap-record'],
    createdDate: '2024-01-10T16:20:00Z'
  },
  {
    id: '4',
    title: 'Technical Analysis: Aerodynamics in MotoGP 2024',
    description: 'Deep dive into the aerodynamic innovations that are shaping the 2024 MotoGP season.',
    type: 'Analysis',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/jNQXAC9IVRw',
    thumbnailUrl: '/images/video-thumb-4.jpg',
    formattedDuration: '15:22',
    publishedDate: '2024-01-08T12:00:00Z',
    viewCount: 45000,
    isFeatured: false,
    platform: 'YouTube',
    externalVideoId: 'jNQXAC9IVRw',
    tags: ['analysis', 'technical', 'aerodynamics', '2024'],
    createdDate: '2024-01-08T12:00:00Z'
  },
  {
    id: '5',
    title: 'Pre-Season Press Conference 2024',
    description: 'Key moments from the official 2024 MotoGP pre-season press conference with all team managers.',
    type: 'PressConference',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/L_jWHffIx5E',
    thumbnailUrl: '/images/video-thumb-5.jpg',
    formattedDuration: '45:15',
    publishedDate: '2024-01-05T09:00:00Z',
    viewCount: 32000,
    isFeatured: false,
    platform: 'YouTube',
    externalVideoId: 'L_jWHffIx5E',
    tags: ['press-conference', 'pre-season', '2024', 'managers'],
    createdDate: '2024-01-05T09:00:00Z'
  },
  {
    id: '6',
    title: 'Behind the Scenes: Yamaha Factory Tour',
    description: 'Exclusive behind-the-scenes access to the Yamaha factory and their MotoGP bike development process.',
    type: 'Documentary',
    status: 'Published',
    videoUrl: 'https://www.youtube.com/embed/Me-VhC9ieh0',
    thumbnailUrl: '/images/video-thumb-6.jpg',
    formattedDuration: '28:33',
    publishedDate: '2024-01-03T11:45:00Z',
    viewCount: 67000,
    isFeatured: true,
    platform: 'YouTube',
    externalVideoId: 'Me-VhC9ieh0',
    tags: ['documentary', 'yamaha', 'factory', 'behind-scenes'],
    createdDate: '2024-01-03T11:45:00Z'
  }
];

// Mock API functions
const getVideosMock = ({ pageIndex = 1, pageSize = 10, type = null, searchTerm = null, platform = null, isFeatured = null } = {}) => {
  let filteredVideos = [...mockVideos];

  if (type) {
    filteredVideos = filteredVideos.filter((video) => video.type.toLowerCase() === type.toLowerCase());
  }

  if (searchTerm) {
    const term = searchTerm.toLowerCase();
    filteredVideos = filteredVideos.filter(
      (video) =>
        video.title.toLowerCase().includes(term) ||
        video.description.toLowerCase().includes(term) ||
        video.tags.some((tag) => tag.toLowerCase().includes(term))
    );
  }

  if (platform) {
    filteredVideos = filteredVideos.filter((video) => video.platform && video.platform.toLowerCase() === platform.toLowerCase());
  }

  if (isFeatured !== null) {
    filteredVideos = filteredVideos.filter((video) => video.isFeatured === isFeatured);
  }

  const startIndex = (pageIndex - 1) * pageSize;
  const endIndex = startIndex + pageSize;
  const paginatedVideos = filteredVideos.slice(startIndex, endIndex);

  return {
    isSuccess: true,
    value: {
      items: paginatedVideos,
      pageIndex,
      pageSize,
      totalCount: filteredVideos.length,
      hasNextPage: endIndex < filteredVideos.length,
      hasPreviousPage: pageIndex > 1
    }
  };
};

const getFeaturedVideosMock = () => {
  const featuredVideos = mockVideos.filter((video) => video.isFeatured);
  return {
    isSuccess: true,
    value: featuredVideos
  };
};

const getVideoByIdMock = (id) => {
  const video = mockVideos.find((v) => v.id === id);
  return {
    isSuccess: !!video,
    value: video,
    error: video ? null : 'Video not found'
  };
};

// API functions
export const getVideos = async ({
  pageIndex = 1,
  pageSize = 10,
  type = null,
  searchTerm = null,
  platform = null,
  isFeatured = null
} = {}) => {
  if (USE_MOCK_DATA) {
    return getVideosMock({ pageIndex, pageSize, type, searchTerm, platform, isFeatured });
  }

  const params = new URLSearchParams();

  params.append('pageIndex', pageIndex.toString());
  params.append('pageSize', pageSize.toString());

  if (type) params.append('type', type);
  if (searchTerm) params.append('searchTerm', searchTerm);
  if (platform) params.append('platform', platform);
  if (isFeatured !== null) params.append('isFeatured', isFeatured.toString());

  try {
    const response = await axios.get(`/api/v1/motogp/videos?${params.toString()}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching videos:', error);
    throw error;
  }
};

// Get featured videos
export const getFeaturedVideos = async () => {
  if (USE_MOCK_DATA) {
    return getFeaturedVideosMock();
  }

  try {
    const response = await axios.get('/api/v1/motogp/videos/featured');
    return response.data;
  } catch (error) {
    console.error('Error fetching featured videos:', error);
    throw error;
  }
};

// Get videos by type
export const getVideosByType = async (type, pageIndex = 1, pageSize = 10) => {
  if (USE_MOCK_DATA) {
    return getVideosMock({ pageIndex, pageSize, type });
  }

  try {
    const response = await axios.get(`/api/v1/motogp/videos/type/${type}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching videos by type:', error);
    throw error;
  }
};

// Get videos by platform
export const getVideosByPlatform = async (platform, pageIndex = 1, pageSize = 10) => {
  if (USE_MOCK_DATA) {
    return getVideosMock({ pageIndex, pageSize, platform });
  }

  try {
    const response = await axios.get(`/api/v1/motogp/videos/platform/${platform}?pageIndex=${pageIndex}&pageSize=${pageSize}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching videos by platform:', error);
    throw error;
  }
};

// Get video by ID
export const getVideoById = async (id) => {
  if (USE_MOCK_DATA) {
    return getVideoByIdMock(id);
  }

  try {
    const response = await axios.get(`/api/v1/motogp/videos/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching video by ID:', error);
    throw error;
  }
};

// Search videos
export const searchVideos = async (searchTerm, pageIndex = 1, pageSize = 10) => {
  if (USE_MOCK_DATA) {
    return getVideosMock({ pageIndex, pageSize, searchTerm });
  }

  try {
    const response = await axios.get(
      `/api/v1/motogp/video/search?searchTerm=${encodeURIComponent(searchTerm)}&pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
    return response.data;
  } catch (error) {
    console.error('Error searching videos:', error);
    throw error;
  }
};

// Increment view count
export const incrementVideoViewCount = async (id) => {
  if (USE_MOCK_DATA) {
    // In mock mode, just return success
    return { isSuccess: true };
  }

  try {
    const response = await axios.patch(`/api/v1/motogp/videos/${id}/view`);
    return response.data;
  } catch (error) {
    console.error('Error incrementing video view count:', error);
    throw error;
  }
};

// Admin functions for video management
export const createVideo = async (videoData) => {
  if (USE_MOCK_DATA) {
    return { isSuccess: true, value: 'Video created successfully' };
  }

  try {
    const response = await axios.post('/api/v1/motogp/videos', videoData);
    return response.data;
  } catch (error) {
    console.error('Error creating video:', error);
    throw error;
  }
};

export const updateVideo = async (id, videoData) => {
  if (USE_MOCK_DATA) {
    return { isSuccess: true, value: 'Video updated successfully' };
  }

  try {
    const response = await axios.put(`/api/v1/motogp/videos/${id}`, videoData);
    return response.data;
  } catch (error) {
    console.error('Error updating video:', error);
    throw error;
  }
};

export const deleteVideo = async (id) => {
  if (USE_MOCK_DATA) {
    return { isSuccess: true, value: 'Video deleted successfully' };
  }

  try {
    const response = await axios.delete(`/api/v1/motogp/videos/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting video:', error);
    throw error;
  }
};

export const publishVideo = async (id) => {
  if (USE_MOCK_DATA) {
    return { isSuccess: true, value: 'Video published successfully' };
  }

  try {
    const response = await axios.patch(`/api/v1/motogp/videos/${id}/publish`);
    return response.data;
  } catch (error) {
    console.error('Error publishing video:', error);
    throw error;
  }
};

export const setVideoAsFeatured = async (id) => {
  if (USE_MOCK_DATA) {
    return { isSuccess: true, value: 'Video set as featured successfully' };
  }

  try {
    const response = await axios.patch(`/api/v1/motogp/videos/${id}/feature`);
    return response.data;
  } catch (error) {
    console.error('Error setting video as featured:', error);
    throw error;
  }
};

// Video types
export const VideoTypes = {
  HIGHLIGHT: 'Highlight',
  INTERVIEW: 'Interview',
  ANALYSIS: 'Analysis',
  ONBOARD: 'OnBoard',
  PRESS_CONFERENCE: 'PressConference',
  DOCUMENTARY: 'Documentary',
  LIVE_STREAM: 'LiveStream'
};

// Video platforms
export const VideoPlatforms = {
  YOUTUBE: 'YouTube',
  VIMEO: 'Vimeo',
  INTERNAL: 'Internal'
};
