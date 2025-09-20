import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import Divider from '@mui/material/Divider';
import Stack from '@mui/material/Stack';

// Icons
import FacebookIcon from '@mui/icons-material/Facebook';
import TwitterIcon from '@mui/icons-material/Twitter';
import InstagramIcon from '@mui/icons-material/Instagram';
import YouTubeIcon from '@mui/icons-material/YouTube';
import LinkedInIcon from '@mui/icons-material/LinkedIn';

export default function ClientFooter() {
  const currentYear = new Date().getFullYear();

  const footerSections = [
    {
      title: 'Championship',
      links: [
        { text: 'Calendar', href: '/seasons' },
        { text: 'Results & Standings', href: '/races' },
        { text: 'Riders', href: '/riders' },
        { text: 'Teams', href: '/teams' }
      ]
    },
    {
      title: 'Content',
      links: [
        { text: 'News', href: '/news' },
        { text: 'Videos', href: '/media' },
        { text: 'Photos', href: '/media' },
        { text: 'Live Timing', href: '#' }
      ]
    },
    {
      title: 'About',
      links: [
        { text: 'About MotoGP', href: '#' },
        { text: 'Regulations', href: '#' },
        { text: 'Safety', href: '#' },
        { text: 'Sustainability', href: '#' }
      ]
    },
    {
      title: 'Support',
      links: [
        { text: 'Help Center', href: '#' },
        { text: 'Contact Us', href: '#' },
        { text: 'Privacy Policy', href: '#' },
        { text: 'Terms & Conditions', href: '#' }
      ]
    }
  ];

  return (
    <Box
      component="footer"
      sx={{
        bgcolor: '#111',
        color: '#fff',
        py: 6,
        mt: 'auto',
        borderTop: '1px solid #333'
      }}
    >
      <Container maxWidth="xl">
        {/* Main Footer Content */}
        <Grid container spacing={4} sx={{ mb: 4 }}>
          {/* Logo and Description */}
          <Grid item xs={12} md={3}>
            <Box sx={{ mb: 3 }}>
              <Typography
                variant="h5"
                sx={{
                  fontWeight: 900,
                  color: '#e10600',
                  letterSpacing: 2,
                  mb: 2
                }}
              >
                MotoGP
              </Typography>
              <Typography variant="body2" sx={{ color: '#ccc', lineHeight: 1.6 }}>
                The premier class of motorcycle road racing events held on road circuits sanctioned by the FIM.
              </Typography>
            </Box>

            {/* Social Media */}
            <Stack direction="row" spacing={1} sx={{ mb: 2 }}>
              <IconButton
                sx={{
                  color: '#ccc',
                  '&:hover': { color: '#e10600', bgcolor: 'rgba(225, 6, 0, 0.1)' }
                }}
              >
                <FacebookIcon />
              </IconButton>
              <IconButton
                sx={{
                  color: '#ccc',
                  '&:hover': { color: '#e10600', bgcolor: 'rgba(225, 6, 0, 0.1)' }
                }}
              >
                <TwitterIcon />
              </IconButton>
              <IconButton
                sx={{
                  color: '#ccc',
                  '&:hover': { color: '#e10600', bgcolor: 'rgba(225, 6, 0, 0.1)' }
                }}
              >
                <InstagramIcon />
              </IconButton>
              <IconButton
                sx={{
                  color: '#ccc',
                  '&:hover': { color: '#e10600', bgcolor: 'rgba(225, 6, 0, 0.1)' }
                }}
              >
                <YouTubeIcon />
              </IconButton>
              <IconButton
                sx={{
                  color: '#ccc',
                  '&:hover': { color: '#e10600', bgcolor: 'rgba(225, 6, 0, 0.1)' }
                }}
              >
                <LinkedInIcon />
              </IconButton>
            </Stack>
          </Grid>

          {/* Footer Links Sections */}
          {footerSections.map((section, index) => (
            <Grid item xs={6} md={2} key={index}>
              <Typography
                variant="h6"
                sx={{
                  fontWeight: 700,
                  mb: 2,
                  color: '#fff',
                  fontSize: '1rem'
                }}
              >
                {section.title}
              </Typography>
              <Stack spacing={1}>
                {section.links.map((link, linkIndex) => (
                  <Link
                    key={linkIndex}
                    href={link.href}
                    sx={{
                      color: '#ccc',
                      textDecoration: 'none',
                      fontSize: '0.875rem',
                      '&:hover': {
                        color: '#e10600',
                        textDecoration: 'underline'
                      }
                    }}
                  >
                    {link.text}
                  </Link>
                ))}
              </Stack>
            </Grid>
          ))}
        </Grid>

        <Divider sx={{ bgcolor: '#333', mb: 3 }} />

        {/* Bottom Footer */}
        <Box
          sx={{
            display: 'flex',
            flexDirection: { xs: 'column', md: 'row' },
            justifyContent: 'space-between',
            alignItems: { xs: 'flex-start', md: 'center' },
            gap: 2
          }}
        >
          <Typography variant="body2" sx={{ color: '#999' }}>
            © {currentYear} MotoGP. All rights reserved. |
            <Link
              href="#"
              sx={{
                color: '#999',
                ml: 1,
                '&:hover': { color: '#e10600' }
              }}
            >
              Privacy Policy
            </Link>
            {' | '}
            <Link
              href="#"
              sx={{
                color: '#999',
                ml: 1,
                '&:hover': { color: '#e10600' }
              }}
            >
              Terms of Use
            </Link>
          </Typography>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <Typography variant="body2" sx={{ color: '#999' }}>
              Official Championship
            </Typography>
            <Box
              sx={{
                px: 2,
                py: 0.5,
                border: '1px solid #e10600',
                borderRadius: 1,
                bgcolor: 'rgba(225, 6, 0, 0.1)'
              }}
            >
              <Typography
                variant="caption"
                sx={{
                  color: '#e10600',
                  fontWeight: 700,
                  fontSize: '0.75rem'
                }}
              >
                FIM WORLD CHAMPIONSHIP
              </Typography>
            </Box>
          </Box>
        </Box>
      </Container>
    </Box>
  );
}
