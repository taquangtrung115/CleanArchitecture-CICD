// Mock news data for demonstration
const mockNewsData = [
  {
    id: '1',
    title: 'Bagnaia wins dramatic season finale at Valencia',
    summary: 'Francesco Bagnaia secured a thrilling victory in the final race of the MotoGP season at Valencia, finishing ahead of championship rival Jorge Martin in a spectacular showdown.',
    content: 'In an unforgettable season finale at the Circuit Ricardo Tormo, Francesco Bagnaia delivered a masterclass performance to claim victory in Valencia...',
    category: 'RaceResults',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-20T10:00:00Z',
    featuredImage: '/images/17-Japan.jpg',
    imageCaption: 'Bagnaia celebrates his victory',
    slug: 'bagnaia-wins-valencia-2024',
    viewCount: 1520,
    isFeatured: true,
    isBreaking: false,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Valencia', 'Bagnaia', 'Championship'],
    createdAt: '2024-09-20T08:00:00Z',
    updatedAt: '2024-09-20T10:00:00Z'
  },
  {
    id: '2',
    title: 'Marc Marquez announces switch to factory Ducati for 2024',
    summary: 'Eight-time world champion Marc Marquez will join the factory Ducati team next season, ending his long association with Honda.',
    content: 'In a move that has sent shockwaves through the MotoGP paddock, Marc Marquez has confirmed his switch to the factory Ducati team...',
    category: 'Transfers',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-19T14:30:00Z',
    featuredImage: '/images/Marc-Marquez-dragging-a-knee.jpg',
    imageCaption: 'Marc Marquez in action',
    slug: 'marquez-ducati-2024',
    viewCount: 2340,
    isFeatured: true,
    isBreaking: false,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Marquez', 'Ducati', 'Transfer'],
    createdAt: '2024-09-19T12:00:00Z',
    updatedAt: '2024-09-19T14:30:00Z'
  },
  {
    id: '3',
    title: 'Revolutionary aerodynamics package unveiled by Yamaha',
    summary: 'Yamaha has introduced a groundbreaking aerodynamics package that could reshape the competitive landscape in MotoGP.',
    content: 'Yamaha has pulled back the curtain on their latest technical innovation – a revolutionary aerodynamics package...',
    category: 'Technical',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-18T11:15:00Z',
    featuredImage: '/images/Maverick-Vin-ales-earplugs-help-with-his-laser-focus2.jpg',
    imageCaption: 'Yamaha aerodynamics testing',
    slug: 'yamaha-aerodynamics-2024',
    viewCount: 890,
    isFeatured: false,
    isBreaking: false,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Yamaha', 'Technical', 'Aerodynamics'],
    createdAt: '2024-09-18T09:00:00Z',
    updatedAt: '2024-09-18T11:15:00Z'
  },
  {
    id: '4',
    title: 'BREAKING: Unexpected weather forces race postponement',
    summary: 'Severe weather conditions at the circuit have forced officials to postpone today\'s MotoGP race to tomorrow morning.',
    content: 'In an unprecedented turn of events, race officials have been forced to postpone today\'s MotoGP race due to severe weather conditions...',
    category: 'Breaking',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-20T06:00:00Z',
    featuredImage: '/images/19-australia2.jpg',
    imageCaption: 'Weather conditions at the circuit',
    slug: 'race-postponed-weather',
    viewCount: 5670,
    isFeatured: false,
    isBreaking: true,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Breaking', 'Weather', 'Race'],
    createdAt: '2024-09-20T05:30:00Z',
    updatedAt: '2024-09-20T06:00:00Z'
  },
  {
    id: '5',
    title: 'Jorge Martin leads championship with two races remaining',
    summary: 'The Pramac Ducati rider has extended his championship lead to 14 points with just two races left in the season.',
    content: 'Jorge Martin\'s championship aspirations received a major boost with his commanding victory at the Malaysian Grand Prix...',
    category: 'Championship',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-17T16:45:00Z',
    featuredImage: '/images/Fabio-Di-Giannantonio-a-tear-off-close-up.jpg',
    imageCaption: 'Jorge Martin celebrates',
    slug: 'martin-leads-championship',
    viewCount: 3420,
    isFeatured: true,
    isBreaking: false,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Martin', 'Championship', 'Pramac'],
    createdAt: '2024-09-17T14:00:00Z',
    updatedAt: '2024-09-17T16:45:00Z'
  },
  {
    id: '6',
    title: 'Exclusive: Valentino Rossi on the new generation of riders',
    summary: 'The MotoGP legend shares his thoughts on today\'s young stars and the evolution of the sport.',
    content: 'In an exclusive interview, nine-time world champion Valentino Rossi reflected on the current state of MotoGP...',
    category: 'Interviews',
    status: 'Published',
    authorId: '123',
    publishedDate: '2024-09-16T13:20:00Z',
    featuredImage: '/images/DS_09650.jpg',
    imageCaption: 'Valentino Rossi interview',
    slug: 'rossi-interview-new-generation',
    viewCount: 4560,
    isFeatured: false,
    isBreaking: false,
    relatedSeasonId: null,
    relatedRaceId: null,
    relatedTeamId: null,
    relatedRiderId: null,
    tags: ['Rossi', 'Interview', 'Legend'],
    createdAt: '2024-09-16T11:00:00Z',
    updatedAt: '2024-09-16T13:20:00Z'
  }
];

// Mock API functions
export const getNewsMock = async ({ 
  pageIndex = 1, 
  pageSize = 10, 
  category = null, 
  searchTerm = null, 
  isFeatured = null, 
  isBreaking = null 
} = {}) => {
  // Simulate API delay
  await new Promise(resolve => setTimeout(resolve, 500));

  let filteredNews = [...mockNewsData];

  // Apply filters
  if (category) {
    filteredNews = filteredNews.filter(news => news.category === category);
  }

  if (searchTerm) {
    const term = searchTerm.toLowerCase();
    filteredNews = filteredNews.filter(news => 
      news.title.toLowerCase().includes(term) ||
      news.summary.toLowerCase().includes(term) ||
      news.content.toLowerCase().includes(term)
    );
  }

  if (isFeatured !== null) {
    filteredNews = filteredNews.filter(news => news.isFeatured === isFeatured);
  }

  if (isBreaking !== null) {
    filteredNews = filteredNews.filter(news => news.isBreaking === isBreaking);
  }

  // Sort by published date (newest first)
  filteredNews.sort((a, b) => new Date(b.publishedDate) - new Date(a.publishedDate));

  // Apply pagination
  const startIndex = (pageIndex - 1) * pageSize;
  const endIndex = startIndex + pageSize;
  const paginatedNews = filteredNews.slice(startIndex, endIndex);

  return {
    isSuccess: true,
    value: {
      items: paginatedNews,
      totalCount: filteredNews.length,
      pageIndex,
      pageSize
    }
  };
};

export const getFeaturedNewsMock = async (limit = 5) => {
  await new Promise(resolve => setTimeout(resolve, 300));
  
  const featuredNews = mockNewsData
    .filter(news => news.isFeatured)
    .slice(0, limit);

  return {
    isSuccess: true,
    value: featuredNews
  };
};

export const getBreakingNewsMock = async () => {
  await new Promise(resolve => setTimeout(resolve, 200));
  
  const breakingNews = mockNewsData.filter(news => news.isBreaking);

  return {
    isSuccess: true,
    value: breakingNews
  };
};

export const getNewsByIdMock = async (id) => {
  await new Promise(resolve => setTimeout(resolve, 300));
  
  const news = mockNewsData.find(n => n.id === id);
  
  if (news) {
    return {
      isSuccess: true,
      value: news
    };
  } else {
    return {
      isSuccess: false,
      error: 'News not found'
    };
  }
};