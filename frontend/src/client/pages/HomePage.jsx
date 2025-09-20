import Fade from '@mui/material/Fade';
import { keyframes } from '@mui/system';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
import CardContent from '@mui/material/CardContent';
import { useState, useEffect } from 'react';
import bannerImg from '../assets/images/banner.jpg';
import trackIcon from '../assets/images/track-jpn-promo.png';
import tissotIcon from '../assets/images/DS_09650.jpg';
import MotoGPHeader from '../components/MotoGPHeader';
import race1Img from '../assets/images/19-australia2.jpg';
import rider1Img from '../assets/images/Maverick-Vin-ales-earplugs-help-with-his-laser-focus2.jpg';
import team1Img from '../assets/images/Fabio-Di-Giannantonio-a-tear-off-close-up.jpg';
import news1Img from '../assets/images/Fabio-Di-Giannantonio-a-tear-off-close-up.jpg';
import news2Img from '../assets/images/Marc-Marquez-dragging-a-knee.jpg';
import news3Img from '../assets/images/17-Japan.jpg';
import motegiImg from '../assets/images/track-jpn-promo2.png';
import phillipIslandImg from '../assets/images/track-aus-promo2.png';

export default function HomePage() {
  const [show, setShow] = useState(false);
  useEffect(() => {
    setShow(true);
  }, []);

  // Animation for card hover
  const pulse = keyframes`
      0% { box-shadow: 0 0 0 0 #e10600; }
      70% { box-shadow: 0 0 0 10px rgba(225,6,0,0); }
      100% { box-shadow: 0 0 0 0 rgba(225,6,0,0); }
    `;

  return (
    <Fade in={show} timeout={900}>
      <Box sx={{ bgcolor: '#101014', minHeight: '100vh', pb: 8, fontFamily: 'Oswald, Arial Black, sans-serif' }}>
        <MotoGPHeader />
        {/* Banner motogp.com style */}
        <Box
          sx={{
            position: 'relative',
            mb: 6,
            borderRadius: 0,
            overflow: 'hidden',
            minHeight: 420,
            width: '100vw',
            left: '50%',
            right: '50%',
            ml: '-50vw',
            mr: '-50vw',
            maxWidth: '100vw'
          }}
        >
          <img
            src={bannerImg}
            alt="MotoGP Banner"
            style={{ width: '100vw', minHeight: 420, objectFit: 'cover', filter: 'brightness(0.7) saturate(1.2)' }}
          />
          {/* Overlay đỏ sự kiện */}
          <Box
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: 80,
              bgcolor: 'rgba(225,6,0,0.95)',
              display: 'flex',
              alignItems: 'center',
              px: { xs: 2, md: 8 },
              zIndex: 2
            }}
          >
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
              <img src={trackIcon} alt="track" style={{ height: 32, marginRight: 8 }} />
              <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 20, letterSpacing: 1, textTransform: 'uppercase', mr: 2 }}>
                12 SEP - 14 SEP
              </Typography>
              <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 28, letterSpacing: 1, textTransform: 'uppercase', mr: 4 }}>
                San Marino
              </Typography>
            </Box>
            <Box sx={{ flexGrow: 1 }} />
            <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 22, letterSpacing: 1, textTransform: 'uppercase', mr: 3 }}>
              MotoGP™ Warm Up
            </Typography>
            <Box sx={{ bgcolor: '#222', borderRadius: 2, px: 2, py: 0.5, display: 'flex', alignItems: 'center', gap: 1 }}>
              <img src={tissotIcon} alt="tissot" style={{ height: 22, marginRight: 6 }} />
              <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 18, letterSpacing: 1 }}>01</Typography>
              <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 14, mx: 0.5 }}>HRS</Typography>
              <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 18, letterSpacing: 1 }}>34</Typography>
              <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 14, mx: 0.5 }}>MIN</Typography>
              <Typography sx={{ color: '#fff', fontWeight: 900, fontSize: 18, letterSpacing: 1 }}>36</Typography>
              <Typography sx={{ color: '#fff', fontWeight: 700, fontSize: 14, mx: 0.5 }}>SEC</Typography>
            </Box>
          </Box>
          {/* Headline lớn */}
          <Box sx={{ position: 'absolute', bottom: 32, left: { xs: 16, md: 64 }, zIndex: 2, maxWidth: { xs: '90vw', md: '60vw' } }}>
            <Typography
              sx={{
                color: '#fff',
                fontWeight: 900,
                fontSize: { xs: 32, md: 54, lg: 64 },
                letterSpacing: 2,
                textTransform: 'uppercase',
                textShadow: '0 6px 32px #000, 0 2px 12px #e10600',
                lineHeight: 1.1
              }}
            >
              BEZZECCHI COMPLETES PERFECT SATURDAY AS MARC MARQUEZ
            </Typography>
          </Box>
        </Box>

        {/* Featured Sections */}
        <Grid container spacing={5} sx={{ px: { xs: 1, md: 6 } }}>
          <Grid item xs={12} md={4}>
            <Fade in={show} timeout={1200}>
              <Card
                sx={{
                  borderRadius: 8,
                  boxShadow: 12,
                  bgcolor: '#181A1B',
                  color: '#fff',
                  p: 2,
                  minHeight: 370,
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between',
                  transition: 'transform 0.3s, box-shadow 0.3s',
                  '&:hover': { transform: 'scale(1.06)', boxShadow: '0 0 0 4px #e10600, 0 12px 48px #000', border: '2px solid #e10600' }
                }}
              >
                <CardMedia
                  component="img"
                  height="180"
                  image={race1Img}
                  alt="Race"
                  sx={{ filter: 'saturate(1.2) contrast(1.1)', borderRadius: 4, mb: 1 }}
                />
                <CardContent>
                  <Typography
                    gutterBottom
                    variant="h5"
                    component="div"
                    sx={{
                      fontWeight: 900,
                      color: '#e10600',
                      fontFamily: 'Oswald, Arial Black, sans-serif',
                      fontSize: 28,
                      letterSpacing: 1
                    }}
                  >
                    Latest Races
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#eee', fontWeight: 600, fontSize: 18, mb: 2 }}>
                    Catch up with the latest Grand Prix, results, and race highlights from the MotoGP™ calendar.
                  </Typography>
                  <Button
                    variant="outlined"
                    color="error"
                    sx={{
                      mt: 2,
                      fontWeight: 800,
                      borderRadius: 8,
                      borderWidth: 2,
                      textTransform: 'uppercase',
                      letterSpacing: 1.5,
                      fontSize: 16,
                      px: 3,
                      py: 1.5,
                      borderColor: '#e10600',
                      color: '#e10600',
                      '&:hover': { bgcolor: '#e10600', color: '#fff', borderColor: '#e10600' }
                    }}
                    href="/races"
                  >
                    View Races
                  </Button>
                </CardContent>
              </Card>
            </Fade>
          </Grid>
          <Grid item xs={12} md={4}>
            <Fade in={show} timeout={1400}>
              <Card
                sx={{
                  borderRadius: 8,
                  boxShadow: 12,
                  bgcolor: '#181A1B',
                  color: '#fff',
                  p: 2,
                  minHeight: 370,
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between',
                  transition: 'transform 0.3s, box-shadow 0.3s',
                  '&:hover': { transform: 'scale(1.06)', boxShadow: '0 0 0 4px #e10600, 0 12px 48px #000', border: '2px solid #e10600' }
                }}
              >
                <CardMedia
                  component="img"
                  height="180"
                  image={rider1Img}
                  alt="Rider"
                  sx={{ filter: 'saturate(1.2) contrast(1.1)', borderRadius: 4, mb: 1 }}
                />
                <CardContent>
                  <Typography
                    gutterBottom
                    variant="h5"
                    component="div"
                    sx={{
                      fontWeight: 900,
                      color: '#e10600',
                      fontFamily: 'Oswald, Arial Black, sans-serif',
                      fontSize: 28,
                      letterSpacing: 1
                    }}
                  >
                    Star Riders
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#eee', fontWeight: 600, fontSize: 18, mb: 2 }}>
                    Meet the world’s fastest riders, their stats, and career highlights in MotoGP™ history.
                  </Typography>
                  <Button
                    variant="outlined"
                    color="error"
                    sx={{
                      mt: 2,
                      fontWeight: 800,
                      borderRadius: 8,
                      borderWidth: 2,
                      textTransform: 'uppercase',
                      letterSpacing: 1.5,
                      fontSize: 16,
                      px: 3,
                      py: 1.5,
                      borderColor: '#e10600',
                      color: '#e10600',
                      '&:hover': { bgcolor: '#e10600', color: '#fff', borderColor: '#e10600' }
                    }}
                    href="/riders"
                  >
                    View Riders
                  </Button>
                </CardContent>
              </Card>
            </Fade>
          </Grid>
          <Grid item xs={12} md={4}>
            <Fade in={show} timeout={1600}>
              <Card
                sx={{
                  borderRadius: 8,
                  boxShadow: 12,
                  bgcolor: '#181A1B',
                  color: '#fff',
                  p: 2,
                  minHeight: 370,
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between',
                  transition: 'transform 0.3s, box-shadow 0.3s',
                  '&:hover': { transform: 'scale(1.06)', boxShadow: '0 0 0 4px #e10600, 0 12px 48px #000', border: '2px solid #e10600' }
                }}
              >
                <CardMedia
                  component="img"
                  height="180"
                  image={team1Img}
                  alt="Team"
                  sx={{ filter: 'saturate(1.2) contrast(1.1)', borderRadius: 4, mb: 1 }}
                />
                <CardContent>
                  <Typography
                    gutterBottom
                    variant="h5"
                    component="div"
                    sx={{
                      fontWeight: 900,
                      color: '#e10600',
                      fontFamily: 'Oswald, Arial Black, sans-serif',
                      fontSize: 28,
                      letterSpacing: 1
                    }}
                  >
                    Top Teams
                  </Typography>
                  <Typography variant="body2" sx={{ color: '#eee', fontWeight: 600, fontSize: 18, mb: 2 }}>
                    Discover legendary MotoGP™ teams, their history, and their quest for championship glory.
                  </Typography>
                  <Button
                    variant="outlined"
                    color="error"
                    sx={{
                      mt: 2,
                      fontWeight: 800,
                      borderRadius: 8,
                      borderWidth: 2,
                      textTransform: 'uppercase',
                      letterSpacing: 1.5,
                      fontSize: 16,
                      px: 3,
                      py: 1.5,
                      borderColor: '#e10600',
                      color: '#e10600',
                      '&:hover': { bgcolor: '#e10600', color: '#fff', borderColor: '#e10600' }
                    }}
                    href="/teams"
                  >
                    View Teams
                  </Button>
                </CardContent>
              </Card>
            </Fade>
          </Grid>
        </Grid>

        {/* Latest News Section */}
        <Box sx={{ mt: 6, mb: 4 }}>
          <Typography variant="h4" sx={{ fontWeight: 800, mb: 2, color: '#e10600' }}>
            Latest News
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={4}>
              <Card
                sx={{ borderRadius: 3, boxShadow: 2, transition: 'transform 0.3s', '&:hover': { transform: 'scale(1.03)', boxShadow: 6 } }}
              >
                <CardMedia component="img" height="140" image={news1Img} alt="News1" />
                <CardContent>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                    Bagnaia wins dramatic race at Misano
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    The Italian rider claims victory in a thrilling last-lap battle.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} md={4}>
              <Card
                sx={{ borderRadius: 3, boxShadow: 2, transition: 'transform 0.3s', '&:hover': { transform: 'scale(1.03)', boxShadow: 6 } }}
              >
                <CardMedia component="img" height="140" image={news2Img} alt="News2" />
                <CardContent>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                    Marquez returns to podium
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Marc Marquez makes a stunning comeback with a P2 finish.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} md={4}>
              <Card
                sx={{ borderRadius: 3, boxShadow: 2, transition: 'transform 0.3s', '&:hover': { transform: 'scale(1.03)', boxShadow: 6 } }}
              >
                <CardMedia component="img" height="140" image={news3Img} alt="News3" />
                <CardContent>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                    Martin extends championship lead
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Jorge Martin increases his points gap after a solid weekend.
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        </Box>

        {/* Upcoming Races Section */}
        <Fade in={show} timeout={1200}>
          <Box sx={{ mt: 6, mb: 4 }}>
            <Typography
              variant="h4"
              sx={{ fontWeight: 900, mb: 2, color: '#e10600', fontFamily: 'Oswald, Arial Black, sans-serif', letterSpacing: 1 }}
            >
              Upcoming Races
            </Typography>
            <Grid container spacing={3}>
              <Grid item xs={12} md={6}>
                <Card
                  sx={{
                    borderRadius: 6,
                    boxShadow: 8,
                    p: 2.5,
                    display: 'flex',
                    alignItems: 'center',
                    gap: 3,
                    bgcolor: '#232323',
                    color: '#fff',
                    transition: 'transform 0.3s',
                    '&:hover': { transform: 'scale(1.025)', boxShadow: 16, border: '2px solid #e10600' }
                  }}
                >
                  <img src={motegiImg} alt="Motegi" style={{ width: 120, borderRadius: 12, boxShadow: '0 2px 12px #000' }} />
                  <Box>
                    <Typography
                      variant="subtitle1"
                      sx={{ fontWeight: 800, color: '#e10600', fontFamily: 'Oswald, Arial Black, sans-serif' }}
                    >
                      Motegi Grand Prix
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#eee', fontWeight: 500 }}>
                      Date: 22 Sep 2024
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#eee', fontWeight: 500 }}>
                      Location: Motegi, Japan
                    </Typography>
                  </Box>
                </Card>
              </Grid>
              <Grid item xs={12} md={6}>
                <Card
                  sx={{
                    borderRadius: 6,
                    boxShadow: 8,
                    p: 2.5,
                    display: 'flex',
                    alignItems: 'center',
                    gap: 3,
                    bgcolor: '#232323',
                    color: '#fff',
                    transition: 'transform 0.3s',
                    '&:hover': { transform: 'scale(1.025)', boxShadow: 16, border: '2px solid #e10600' }
                  }}
                >
                  <img src={phillipIslandImg} alt="Phillip Island" style={{ width: 120, borderRadius: 12, boxShadow: '0 2px 12px #000' }} />
                  <Box>
                    <Typography
                      variant="subtitle1"
                      sx={{ fontWeight: 800, color: '#e10600', fontFamily: 'Oswald, Arial Black, sans-serif' }}
                    >
                      Phillip Island Grand Prix
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#eee', fontWeight: 500 }}>
                      Date: 6 Oct 2024
                    </Typography>
                    <Typography variant="body2" sx={{ color: '#eee', fontWeight: 500 }}>
                      Location: Phillip Island, Australia
                    </Typography>
                  </Box>
                </Card>
              </Grid>
            </Grid>
          </Box>
        </Fade>

        {/* Featured Video Section */}
        <Fade in={show} timeout={1500}>
          <Box sx={{ mt: 6, mb: 4 }}>
            <Typography
              variant="h4"
              sx={{ fontWeight: 900, mb: 2, color: '#e10600', fontFamily: 'Oswald, Arial Black, sans-serif', letterSpacing: 1 }}
            >
              Featured Video
            </Typography>
            <Box sx={{ display: 'flex', justifyContent: 'center' }}>
              <Box sx={{ borderRadius: 6, overflow: 'hidden', boxShadow: 8, maxWidth: 720, width: '100%', border: '2px solid #e10600' }}>
                <iframe
                  width="100%"
                  height="400"
                  src="https://www.youtube.com/embed/1QhQF6l1bJw"
                  title="MotoGP 2024 Highlights"
                  frameBorder="0"
                  allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                  allowFullScreen
                ></iframe>
              </Box>
            </Box>
          </Box>
        </Fade>
      </Box>
    </Fade>
  );
}
