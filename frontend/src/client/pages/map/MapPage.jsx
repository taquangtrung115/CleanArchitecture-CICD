import { useEffect, useState, useRef } from 'react';
import { Box, Paper, Typography, Alert, CircularProgress, Fab, Tooltip } from '@mui/material';
import { styled } from '@mui/material/styles';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import MyLocationIcon from '@mui/icons-material/MyLocation';
import { Wrapper, Status } from '@googlemaps/react-wrapper';

// Styled components
const MapContainer = styled(Box)(({ theme }) => ({
  height: 'calc(100vh - 120px)',
  width: '100%',
  position: 'relative',
  backgroundColor: theme.palette.grey[100]
}));

const MapPaper = styled(Paper)(({ theme }) => ({
  height: '100%',
  width: '100%',
  borderRadius: theme.spacing(2),
  overflow: 'hidden',
  position: 'relative'
}));

const LoadingContainer = styled(Box)(({ theme }) => ({
  height: '100%',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  flexDirection: 'column',
  gap: theme.spacing(2)
}));

const LocationButton = styled(Fab)(({ theme }) => ({
  position: 'absolute',
  bottom: theme.spacing(3),
  right: theme.spacing(3),
  zIndex: 1000,
  backgroundColor: theme.palette.primary.main,
  color: 'white',
  '&:hover': {
    backgroundColor: theme.palette.primary.dark
  }
}));

// Google Maps component
function Map({ center, zoom, onLocationUpdate }) {
  const ref = useRef(null);
  const [map, setMap] = useState(null);
  const [marker, setMarker] = useState(null);

  useEffect(() => {
    if (ref.current && !map) {
      const mapInstance = new window.google.maps.Map(ref.current, {
        center,
        zoom,
        mapTypeControl: true,
        streetViewControl: true,
        fullscreenControl: true,
        zoomControl: true,
        styles: [
          {
            featureType: 'poi',
            elementType: 'labels',
            stylers: [{ visibility: 'on' }]
          }
        ]
      });
      setMap(mapInstance);
    }
  }, [ref, map, center, zoom]);

  useEffect(() => {
    if (map && center) {
      // Remove existing marker
      if (marker) {
        marker.setMap(null);
      }

      // Create new marker
      const newMarker = new window.google.maps.Marker({
        position: center,
        map: map,
        title: 'Your Location',
        animation: window.google.maps.Animation.DROP,
        icon: {
          url:
            'data:image/svg+xml;charset=UTF-8,' +
            encodeURIComponent(`
            <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" viewBox="0 0 24 24" fill="#1976d2">
              <path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z"/>
            </svg>
          `),
          scaledSize: new window.google.maps.Size(30, 30),
          anchor: new window.google.maps.Point(15, 30)
        }
      });

      setMarker(newMarker);

      // Center map on location
      map.setCenter(center);
      map.setZoom(zoom);

      // Add click listener for new location
      map.addListener('click', (event) => {
        const newLocation = {
          lat: event.latLng.lat(),
          lng: event.latLng.lng()
        };
        onLocationUpdate(newLocation);
      });
    }
  }, [map, center, marker, zoom, onLocationUpdate]);

  return <div ref={ref} style={{ width: '100%', height: '100%' }} />;
}

// Loading component for Google Maps API
function LoadingStatus({ status }) {
  switch (status) {
    case Status.LOADING:
      return (
        <LoadingContainer>
          <CircularProgress size={60} />
          <Typography variant="h6" color="textSecondary">
            Loading Google Maps...
          </Typography>
        </LoadingContainer>
      );
    case Status.FAILURE:
      return (
        <LoadingContainer>
          <Alert severity="error" sx={{ maxWidth: 400 }}>
            <Typography variant="h6" gutterBottom>
              Failed to load Google Maps
            </Typography>
            <Typography variant="body2">Please check your internet connection and API key configuration.</Typography>
          </Alert>
        </LoadingContainer>
      );
    default:
      return null;
  }
}

export default function MapPage() {
  const [location, setLocation] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [zoom, setZoom] = useState(15);

  // Get user's current location
  const getCurrentLocation = () => {
    setLoading(true);
    setError(null);

    if (!navigator.geolocation) {
      setError('Geolocation is not supported by this browser.');
      setLocation({ lat: 10.8231, lng: 106.6297 }); // Default to Ho Chi Minh City
      setLoading(false);
      return;
    }

    // Set a backup timeout in case the native timeout doesn't work
    const backupTimeout = setTimeout(() => {
      setError('Location request timed out. Using default location.');
      setLocation({ lat: 10.8231, lng: 106.6297 }); // Default to Ho Chi Minh City
      setLoading(false);
    }, 6000); // 6 seconds backup timeout

    navigator.geolocation.getCurrentPosition(
      (position) => {
        clearTimeout(backupTimeout);
        const newLocation = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };
        setLocation(newLocation);
        setLoading(false);
      },
      (error) => {
        clearTimeout(backupTimeout);
        console.error('Error getting location:', error);
        let errorMessage = 'Unable to retrieve your location.';

        switch (error.code) {
          case error.PERMISSION_DENIED:
            errorMessage = 'Location access denied. Please enable location services.';
            break;
          case error.POSITION_UNAVAILABLE:
            errorMessage = 'Location information is unavailable.';
            break;
          case error.TIMEOUT:
            errorMessage = 'Location request timed out.';
            break;
          default:
            errorMessage = 'An unknown error occurred.';
            break;
        }

        setError(errorMessage);
        // Set default location to Ho Chi Minh City, Vietnam
        setLocation({ lat: 10.8231, lng: 106.6297 });
        setLoading(false);
      },
      {
        enableHighAccuracy: true,
        timeout: 5000, // 5 seconds timeout
        maximumAge: 300000 // 5 minutes
      }
    );
  };

  // Handle location update from map click
  const handleLocationUpdate = (newLocation) => {
    setLocation(newLocation);
  };

  // Get location on component mount
  useEffect(() => {
    getCurrentLocation();
  }, []);

  // Google Maps API key - In production, this should come from environment variables
  // For demo purposes, we'll show a placeholder when no API key is available
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || 'DEMO_MODE';

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom sx={{ mb: 3, fontWeight: 'bold' }}>
        <LocationOnIcon sx={{ mr: 1, verticalAlign: 'middle' }} />
        Interactive Map
      </Typography>

      {error && (
        <Alert severity="warning" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <MapContainer>
        <MapPaper elevation={3}>
          {loading && !location ? (
            <LoadingContainer>
              <CircularProgress size={60} />
              <Typography variant="h6" color="textSecondary">
                Getting your location...
              </Typography>
            </LoadingContainer>
          ) : apiKey === 'DEMO_MODE' ? (
            <LoadingContainer>
              <Alert severity="info" sx={{ maxWidth: 500 }}>
                <Typography variant="h6" gutterBottom>
                  Demo Mode - Google Maps Integration
                </Typography>
                <Typography variant="body2" gutterBottom>
                  This is a demonstration of the map page functionality. To see the actual Google Maps:
                </Typography>
                <ul style={{ margin: 0, paddingLeft: 20 }}>
                  <li>Get a Google Maps API key from Google Cloud Console</li>
                  <li>Add it to your environment variables as REACT_APP_GOOGLE_MAPS_API_KEY</li>
                  <li>Enable Maps JavaScript API and Places API</li>
                </ul>
                <Typography variant="body2" sx={{ mt: 2 }}>
                  Current coordinates: {location ? `${location.lat.toFixed(6)}, ${location.lng.toFixed(6)}` : 'Getting location...'}
                </Typography>
              </Alert>
            </LoadingContainer>
          ) : (
            <Wrapper apiKey={apiKey} render={LoadingStatus} libraries={['places']}>
              {location && <Map center={location} zoom={zoom} onLocationUpdate={handleLocationUpdate} />}
            </Wrapper>
          )}

          <Tooltip title="Get Current Location" placement="left">
            <LocationButton onClick={getCurrentLocation} disabled={loading}>
              <MyLocationIcon />
            </LocationButton>
          </Tooltip>
        </MapPaper>
      </MapContainer>

      {location && (
        <Paper sx={{ mt: 2, p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Current Location
          </Typography>
          <Typography variant="body2" color="textSecondary">
            Latitude: {location.lat.toFixed(6)}
          </Typography>
          <Typography variant="body2" color="textSecondary">
            Longitude: {location.lng.toFixed(6)}
          </Typography>
        </Paper>
      )}
    </Box>
  );
}
