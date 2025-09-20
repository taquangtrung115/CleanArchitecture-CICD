import { useEffect, useState } from 'react';
import { 
  Typography, 
  Box, 
  Card, 
  CardContent, 
  Avatar, 
  Grid, 
  Divider, 
  Chip,
  Stack,
  CircularProgress,
  Alert
} from '@mui/material';
import MainCard from 'components/MainCard';
import { getCurrentUserProfile } from 'api/user';
import UserOutlined from '@ant-design/icons/UserOutlined';
import MailOutlined from '@ant-design/icons/MailOutlined';
import CalendarOutlined from '@ant-design/icons/CalendarOutlined';
import TeamOutlined from '@ant-design/icons/TeamOutlined';
import avatar1 from 'assets/images/users/avatar-1.png';

export default function ProfileViewPage() {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchProfile = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const res = await getCurrentUserProfile();
      if (res.data && res.data.value) {
        setProfile(res.data.value);
      } else {
        setError('Không thể tải thông tin profile');
      }
    } catch (err) {
      setError('Đã xảy ra lỗi khi tải profile');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfile();
  }, []);

  if (loading) {
    return (
      <MainCard title="Profile">
        <Box display="flex" justifyContent="center" p={3}>
          <CircularProgress />
        </Box>
      </MainCard>
    );
  }

  if (error) {
    return (
      <MainCard title="Profile">
        <Alert severity="error">{error}</Alert>
      </MainCard>
    );
  }

  if (!profile) {
    return (
      <MainCard title="Profile">
        <Alert severity="warning">Không tìm thấy thông tin profile</Alert>
      </MainCard>
    );
  }

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('vi-VN');
  };

  return (
    <MainCard title="Thông tin cá nhân">
      <Grid container spacing={3}>
        {/* Profile Header */}
        <Grid item xs={12}>
          <Card sx={{ mb: 2 }}>
            <CardContent>
              <Box display="flex" alignItems="center" gap={3}>
                <Avatar
                  src={avatar1}
                  sx={{ width: 120, height: 120 }}
                  alt={profile.fullName}
                />
                <Box>
                  <Typography variant="h4" gutterBottom>
                    {profile.fullName || 'N/A'}
                  </Typography>
                  <Typography variant="h6" color="text.secondary" gutterBottom>
                    @{profile.userName}
                  </Typography>
                  <Stack direction="row" spacing={1} alignItems="center" sx={{ mt: 1 }}>
                    <MailOutlined style={{ fontSize: '16px' }} />
                    <Typography variant="body2" color="text.secondary">
                      {profile.email}
                    </Typography>
                  </Stack>
                  {profile.isLocked && (
                    <Chip 
                      label="Tài khoản bị khóa" 
                      color="error" 
                      size="small" 
                      sx={{ mt: 1 }}
                    />
                  )}
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        {/* Basic Information */}
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <UserOutlined />
                Thông tin cơ bản
              </Typography>
              <Divider sx={{ mb: 2 }} />
              
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Họ
                  </Typography>
                  <Typography variant="body1">
                    {profile.firstName || 'N/A'}
                  </Typography>
                </Box>
                
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Tên
                  </Typography>
                  <Typography variant="body1">
                    {profile.lastName || 'N/A'}
                  </Typography>
                </Box>
                
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Ngày sinh
                  </Typography>
                  <Typography variant="body1" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <CalendarOutlined style={{ fontSize: '16px' }} />
                    {formatDate(profile.dayOfBirth)}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        {/* Professional Information */}
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <TeamOutlined />
                Thông tin công việc
              </Typography>
              <Divider sx={{ mb: 2 }} />
              
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Vị trí
                  </Typography>
                  <Typography variant="body1">
                    {profile.positionId ? `Position ID: ${profile.positionId}` : 'N/A'}
                  </Typography>
                </Box>
                
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Quản lý
                  </Typography>
                  <Typography variant="body1">
                    {profile.managerId ? `Manager ID: ${profile.managerId}` : 'N/A'}
                  </Typography>
                </Box>
                
                <Box>
                  <Typography variant="body2" color="text.secondary">
                    Vai trò đặc biệt
                  </Typography>
                  <Stack direction="column" spacing={1}>
                    {profile.isDirector && (
                      <Chip label="Giám đốc" color="primary" size="small" variant="outlined" />
                    )}
                    {profile.isHeadOfDepartment && (
                      <Chip label="Trưởng phòng" color="secondary" size="small" variant="outlined" />
                    )}
                    {!profile.isDirector && !profile.isHeadOfDepartment && (
                      <Typography variant="body1" color="text.secondary">
                        Nhân viên
                      </Typography>
                    )}
                  </Stack>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>

        {/* Account Information */}
        <Grid item xs={12}>
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Thông tin tài khoản
              </Typography>
              <Divider sx={{ mb: 2 }} />
              
              <Grid container spacing={2}>
                <Grid item xs={12} sm={6}>
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      ID tài khoản
                    </Typography>
                    <Typography variant="body1" sx={{ fontFamily: 'monospace' }}>
                      {profile.userId}
                    </Typography>
                  </Box>
                </Grid>
                
                <Grid item xs={12} sm={6}>
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Ngày tạo
                    </Typography>
                    <Typography variant="body1">
                      {formatDate(profile.createdAt)}
                    </Typography>
                  </Box>
                </Grid>
                
                <Grid item xs={12}>
                  <Box>
                    <Typography variant="body2" color="text.secondary">
                      Trạng thái tài khoản
                    </Typography>
                    <Chip 
                      label={profile.isLocked ? "Bị khóa" : "Hoạt động"} 
                      color={profile.isLocked ? "error" : "success"} 
                      size="small"
                    />
                  </Box>
                </Grid>
              </Grid>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </MainCard>
  );
}