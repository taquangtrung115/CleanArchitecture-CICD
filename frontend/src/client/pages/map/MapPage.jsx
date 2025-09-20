import React, { useState, useEffect, useCallback } from 'react';
import {
  Box,
  Typography,
  Container,
  Alert,
  CircularProgress,
  Fade,
  Card,
  CardContent,
  Button,
  IconButton,
  Chip
} from '@mui/material';
import { styled } from '@mui/material/styles';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import MyLocationIcon from '@mui/icons-material/MyLocation';
import RefreshIcon from '@mui/icons-material/Refresh';
import { Wrapper, Status } from '@googlemaps/react-wrapper';

// MotoGP styled components matching existing pages
const PageContainer = styled(Box)(({ theme }) => ({
  backgroundColor: '#101014',
  minHeight: '100vh',
  padding: theme.spacing(3, 0),
  fontFamily: 'Oswald, Arial Black, sans-serif'
}));

const HeaderCard = styled(Card)(({ theme }) => ({
  background: 'linear-gradient(135deg, #e10600 0%, #a50400 100%)',
  color: '#fff',
  marginBottom: theme.spacing(3),
  borderRadius: 12,
  position: 'relative',
  overflow: 'hidden',
  '&::before': {
    content: '""',
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    background: 'linear-gradient(45deg, rgba(255,255,255,0.1) 0%, rgba(255,255,255,0) 50%)',
    pointerEvents: 'none'
  }
}));

const MapContainer = styled(Box)({
  height: '500px',
  width: '100%',
  borderRadius: 12,
  overflow: 'hidden',
  border: '2px solid #e10600'
});

const LocationCard = styled(Card)(({ theme }) => ({
  backgroundColor: 'rgba(255,255,255,0.05)',
  backdropFilter: 'blur(10px)',
  border: '1px solid rgba(255,255,255,0.1)',
  borderRadius: 12,
  marginBottom: theme.spacing(2)
}));

// Google Maps component
function GoogleMap({ center, zoom, onLocationUpdate }) {
  const mapRef = React.useRef(null);
  const [map, setMap] = useState(null);
  const [marker, setMarker] = useState(null);

  const onMapLoad = useCallback((map) => {
    mapRef.current = map;
    setMap(map);
  }, []);

  const getCurrentLocation = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const newCenter = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
          };
          
          if (map) {
            map.setCenter(newCenter);
            map.setZoom(15);
            
            // Remove existing marker
            if (marker) {
              marker.setMap(null);
            }
            
            // Add new marker
            const newMarker = new window.google.maps.Marker({
              position: newCenter,
              map: map,
              title: 'Your Location',
              icon: {
                url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(`
                  <svg width="30" height="30" viewBox="0 0 30 30" xmlns="http://www.w3.org/2000/svg">
                    <circle cx="15" cy="15" r="12" fill="#e10600" stroke="#fff" stroke-width="3"/>
                    <circle cx="15" cy="15" r="6" fill="#fff"/>
                  </svg>
                `),
                scaledSize: new window.google.maps.Size(30, 30),
                anchor: new window.google.maps.Point(15, 15)
              }
            });
            
            setMarker(newMarker);
            
            // Update parent component with location info
            onLocationUpdate({
              lat: position.coords.latitude,
              lng: position.coords.longitude,
              accuracy: position.coords.accuracy
            });
          }
        },
        (error) => {
          console.error('Error getting location:', error);
        },
        { enableHighAccuracy: true, timeout: 10000, maximumAge: 600000 }
      );
    }
  };

  useEffect(() => {
    if (map && center) {
      // Initial marker setup
      const initialMarker = new window.google.maps.Marker({
        position: center,
        map: map,
        title: 'Your Location',
        icon: {
          url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(`
            <svg width="30" height="30" viewBox="0 0 30 30" xmlns="http://www.w3.org/2000/svg">
              <circle cx="15" cy="15" r="12" fill="#e10600" stroke="#fff" stroke-width="3"/>
              <circle cx="15" cy="15" r="6" fill="#fff"/>
            </svg>
          `),
          scaledSize: new window.google.maps.Size(30, 30),
          anchor: new window.google.maps.Point(15, 15)
        }
      });
      
      setMarker(initialMarker);
    }
  }, [map, center]);

  return (
    <Box sx={{ position: 'relative', height: '100%' }}>
      <div
        ref={mapRef}
        style={{ height: '100%', width: '100%' }}
        id="map"
      />
      <IconButton
        onClick={getCurrentLocation}
        sx={{
          position: 'absolute',
          bottom: 16,
          right: 16,
          backgroundColor: '#e10600',
          color: '#fff',
          '&:hover': {
            backgroundColor: '#a50400'
          }
        }}
      >
        <MyLocationIcon />
      </IconButton>
    </Box>
  );
}

// Map render component
function MapRender({ center, zoom, onLocationUpdate }) {
  const render = (status) => {
    switch (status) {
      case Status.LOADING:
        return (
          <Box display="flex" justifyContent="center" alignItems="center" height="100%">
            <CircularProgress sx={{ color: '#e10600' }} />
          </Box>
        );
      case Status.FAILURE:
        return (
          <Alert severity="error">
            Failed to load Google Maps. Please check your internet connection and API key.
          </Alert>
        );
      case Status.SUCCESS:
        return <GoogleMap center={center} zoom={zoom} onLocationUpdate={onLocationUpdate} />;
      default:
        return null;
    }
  };

  return (
    <Wrapper
      apiKey={import.meta.env.VITE_GOOGLE_MAPS_API_KEY || 'YOUR_API_KEY_HERE'}
      render={render}
      callback={(map, maps) => {
        if (map && maps) {
          map.setOptions({
            styles: [
              {
                featureType: 'all',
                stylers: [{ saturation: -20 }]
              },
              {
                featureType: 'road',
                elementType: 'geometry',
                stylers: [{ color: '#2c2c2c' }]
              },
              {
                featureType: 'water',
                stylers: [{ color: '#1a1a1a' }]
              }
            ]
          });
        }
      }}
    />
  );
}

export default function MapPage() {
  const [show, setShow] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [userLocation, setUserLocation] = useState(null);
  const [locationInfo, setLocationInfo] = useState(null);

  // Default center (Ho Chi Minh City, Vietnam)
  const [center, setCenter] = useState({
    lat: 10.8231,
    lng: 106.6297
  });

  useEffect(() => {
    setShow(true);
    getCurrentPosition();
  }, []);

  const getCurrentPosition = () => {
    setLoading(true);
    setError(null);

    if (!navigator.geolocation) {
      setError('Geolocation is not supported by this browser');
      setLoading(false);
      return;
    }

    navigator.geolocation.getCurrentPosition(
      (position) => {
        const location = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };
        setCenter(location);
        setUserLocation(location);
        setLocationInfo({
          lat: position.coords.latitude,
          lng: position.coords.longitude,
          accuracy: position.coords.accuracy,
          timestamp: new Date(position.timestamp)
        });
        setLoading(false);
      },
      (error) => {
        console.error('Error getting location:', error);
        setError('Unable to get your location. Using default location.');
        setLoading(false);
      },
      { enableHighAccuracy: true, timeout: 10000, maximumAge: 600000 }
    );
  };

  const handleLocationUpdate = (location) => {
    setLocationInfo({
      ...location,
      timestamp: new Date()
    });
    setUserLocation({ lat: location.lat, lng: location.lng });
    setCenter({ lat: location.lat, lng: location.lng });
  };

  const formatCoordinate = (coord) => {
    return coord ? coord.toFixed(6) : 'N/A';
  };

  return (
    <Fade in={show} timeout={900}>
      <PageContainer>
        <Container maxWidth="xl">
          {/* Header */}
          <HeaderCard>
            <CardContent sx={{ position: 'relative', zIndex: 1, py: 4 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
                <LocationOnIcon sx={{ fontSize: 40, color: '#fff' }} />
                <Box>
                  <Typography variant="h3" sx={{ color: '#fff', fontWeight: 900, mb: 1 }}>
                    MAP LOCATION
                  </Typography>
                  <Typography variant="h6" sx={{ color: 'rgba(255,255,255,0.9)', fontWeight: 600 }}>
                    Your current location on Google Maps
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </HeaderCard>

          {/* Error Alert */}
          {error && (
            <Alert severity="warning" sx={{ mb: 3 }}>
              {error}
            </Alert>
          )}

          {/* Location Information */}
          {locationInfo && (
            <LocationCard>
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                  <Typography variant="h6" sx={{ color: '#fff', fontWeight: 700 }}>
                    Location Information
                  </Typography>
                  <Button
                    onClick={getCurrentPosition}
                    startIcon={<RefreshIcon />}
                    sx={{
                      color: '#e10600',
                      borderColor: '#e10600',
                      '&:hover': {
                        backgroundColor: 'rgba(225, 6, 0, 0.1)',
                        borderColor: '#e10600'
                      }
                    }}
                    variant="outlined"
                    size="small"
                  >
                    Refresh
                  </Button>
                </Box>
                <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' }, gap: 2 }}>
                  <Box>
                    <Typography variant="body2" sx={{ color: '#aaa', mb: 0.5 }}>
                      Latitude
                    </Typography>
                    <Chip 
                      label={formatCoordinate(locationInfo.lat)} 
                      sx={{ backgroundColor: 'rgba(225, 6, 0, 0.1)', color: '#e10600' }} 
                    />
                  </Box>
                  <Box>
                    <Typography variant="body2" sx={{ color: '#aaa', mb: 0.5 }}>
                      Longitude
                    </Typography>
                    <Chip 
                      label={formatCoordinate(locationInfo.lng)} 
                      sx={{ backgroundColor: 'rgba(225, 6, 0, 0.1)', color: '#e10600' }} 
                    />
                  </Box>
                  <Box>
                    <Typography variant="body2" sx={{ color: '#aaa', mb: 0.5 }}>
                      Accuracy
                    </Typography>
                    <Chip 
                      label={`±${Math.round(locationInfo.accuracy)}m`} 
                      sx={{ backgroundColor: 'rgba(225, 6, 0, 0.1)', color: '#e10600' }} 
                    />
                  </Box>
                  <Box>
                    <Typography variant="body2" sx={{ color: '#aaa', mb: 0.5 }}>
                      Last Updated
                    </Typography>
                    <Chip 
                      label={locationInfo.timestamp.toLocaleTimeString()} 
                      sx={{ backgroundColor: 'rgba(225, 6, 0, 0.1)', color: '#e10600' }} 
                    />
                  </Box>
                </Box>
              </CardContent>
            </LocationCard>
          )}

          {/* Map */}
          <Card sx={{ backgroundColor: 'rgba(255,255,255,0.05)', backdropFilter: 'blur(10px)' }}>
            <CardContent sx={{ p: 0 }}>
              <MapContainer>
                {loading ? (
                  <Box display="flex" justifyContent="center" alignItems="center" height="100%">
                    <CircularProgress sx={{ color: '#e10600' }} />
                  </Box>
                ) : (
                  <MapRender 
                    center={center} 
                    zoom={userLocation ? 15 : 10} 
                    onLocationUpdate={handleLocationUpdate}
                  />
                )}
              </MapContainer>
            </CardContent>
          </Card>
        </Container>
      </PageContainer>
    </Fade>
  );
}