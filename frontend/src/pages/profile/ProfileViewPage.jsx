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
  Alert,
  Button,
  IconButton,
  Tabs,
  Tab,
  Container,
  Paper,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Badge
} from '@mui/material';
import { getCurrentUserProfile } from 'api/user';
import UserOutlined from '@ant-design/icons/UserOutlined';
import MailOutlined from '@ant-design/icons/MailOutlined';
import CalendarOutlined from '@ant-design/icons/CalendarOutlined';
import TeamOutlined from '@ant-design/icons/TeamOutlined';
import EditOutlined from '@ant-design/icons/EditOutlined';
import SecurityScanOutlined from '@ant-design/icons/SecurityScanOutlined';
import SettingOutlined from '@ant-design/icons/SettingOutlined';
import CameraOutlined from '@ant-design/icons/CameraOutlined';
import PhoneOutlined from '@ant-design/icons/PhoneOutlined';
import EnvironmentOutlined from '@ant-design/icons/EnvironmentOutlined';
import IdcardOutlined from '@ant-design/icons/IdcardOutlined';
import ClockCircleOutlined from '@ant-design/icons/ClockCircleOutlined';
import CheckCircleOutlined from '@ant-design/icons/CheckCircleOutlined';
import avatar1 from 'assets/images/users/avatar-1.png';

export default function ProfileViewPage() {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [activeTab, setActiveTab] = useState(0);

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
    } catch {
      setError('Đã xảy ra lỗi khi tải profile');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfile();
  }, []);

  const handleTabChange = (event, newValue) => {
    setActiveTab(newValue);
  };

  if (loading) {
    return (
      <Container maxWidth="lg">
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
          <CircularProgress size={60} />
        </Box>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg">
        <Box mt={3}>
          <Alert
            severity="error"
            action={
              <Button color="inherit" size="small" onClick={fetchProfile}>
                Thử lại
              </Button>
            }
          >
            {error}
          </Alert>
        </Box>
      </Container>
    );
  }

  if (!profile) {
    return (
      <Container maxWidth="lg">
        <Box mt={3}>
          <Alert severity="warning">Không tìm thấy thông tin profile</Alert>
        </Box>
      </Container>
    );
  }

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('vi-VN');
  };

  const TabPanel = ({ children, value, index, ...other }) => {
    return (
      <div role="tabpanel" hidden={value !== index} id={`profile-tabpanel-${index}`} aria-labelledby={`profile-tab-${index}`} {...other}>
        {value === index && <Box pt={3}>{children}</Box>}
      </div>
    );
  };

  return (
    <Container maxWidth="lg">
      <Box py={3}>
        {/* Profile Header */}
        <Paper elevation={0} sx={{ mb: 3, borderRadius: 2, overflow: 'hidden' }}>
          <Box
            sx={{
              background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
              height: 200,
              position: 'relative'
            }}
          >
            <Box
              sx={{
                position: 'absolute',
                bottom: -60,
                left: 40,
                display: 'flex',
                alignItems: 'end',
                gap: 3
              }}
            >
              <Badge
                overlap="circular"
                anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                badgeContent={
                  <IconButton
                    size="small"
                    sx={{
                      backgroundColor: 'primary.main',
                      color: 'white',
                      '&:hover': { backgroundColor: 'primary.dark' },
                      width: 32,
                      height: 32
                    }}
                  >
                    <CameraOutlined style={{ fontSize: 16 }} />
                  </IconButton>
                }
              >
                <Avatar
                  src={avatar1}
                  sx={{
                    width: 120,
                    height: 120,
                    border: 4,
                    borderColor: 'white',
                    boxShadow: 3
                  }}
                  alt={profile.fullName}
                />
              </Badge>
              <Box pb={1}>
                <Typography variant="h4" fontWeight="bold" color="white" sx={{ mb: 0.5 }}>
                  {profile.fullName || 'N/A'}
                </Typography>
                <Typography variant="body1" color="rgba(255,255,255,0.8)" sx={{ mb: 1 }}>
                  @{profile.userName}
                </Typography>
                <Stack direction="row" spacing={1}>
                  {profile.isDirector && (
                    <Chip label="Giám đốc" size="small" sx={{ backgroundColor: 'rgba(255,255,255,0.2)', color: 'white' }} />
                  )}
                  {profile.isHeadOfDepartment && (
                    <Chip label="Trưởng phòng" size="small" sx={{ backgroundColor: 'rgba(255,255,255,0.2)', color: 'white' }} />
                  )}
                </Stack>
              </Box>
            </Box>
            <Box
              sx={{
                position: 'absolute',
                top: 20,
                right: 20
              }}
            >
              <Button
                variant="contained"
                startIcon={<EditOutlined />}
                sx={{
                  backgroundColor: 'rgba(255,255,255,0.2)',
                  backdropFilter: 'blur(10px)',
                  color: 'white',
                  '&:hover': { backgroundColor: 'rgba(255,255,255,0.3)' }
                }}
              >
                Chỉnh sửa Profile
              </Button>
            </Box>
          </Box>
          <Box pt={8} pb={2} px={4}>
            <Grid container spacing={3}>
              <Grid item xs={12} md={4}>
                <Stack direction="row" alignItems="center" spacing={1}>
                  <MailOutlined style={{ color: '#666' }} />
                  <Typography variant="body2" color="text.secondary">
                    {profile.email}
                  </Typography>
                </Stack>
              </Grid>
              <Grid item xs={12} md={4}>
                <Stack direction="row" alignItems="center" spacing={1}>
                  <CalendarOutlined style={{ color: '#666' }} />
                  <Typography variant="body2" color="text.secondary">
                    Tham gia {formatDate(profile.createdAt)}
                  </Typography>
                </Stack>
              </Grid>
              <Grid item xs={12} md={4}>
                <Stack direction="row" alignItems="center" spacing={1}>
                  {profile.isLocked ? (
                    <SecurityScanOutlined style={{ color: '#f44336' }} />
                  ) : (
                    <CheckCircleOutlined style={{ color: '#4caf50' }} />
                  )}
                  <Typography variant="body2" color="text.secondary">
                    {profile.isLocked ? 'Tài khoản bị khóa' : 'Tài khoản hoạt động'}
                  </Typography>
                </Stack>
              </Grid>
            </Grid>
          </Box>
        </Paper>

        {/* Profile Content Tabs */}
        <Paper elevation={0} sx={{ borderRadius: 2 }}>
          <Box px={3}>
            <Tabs
              value={activeTab}
              onChange={handleTabChange}
              aria-label="profile tabs"
              sx={{
                borderBottom: 1,
                borderColor: 'divider',
                '& .MuiTab-root': {
                  textTransform: 'none',
                  fontWeight: 500,
                  minHeight: 64
                }
              }}
            >
              <Tab icon={<UserOutlined />} label="Thông tin cá nhân" iconPosition="start" />
              <Tab icon={<TeamOutlined />} label="Thông tin công việc" iconPosition="start" />
              <Tab icon={<SettingOutlined />} label="Cài đặt tài khoản" iconPosition="start" />
            </Tabs>
          </Box>

          {/* Personal Information Tab */}
          <TabPanel value={activeTab} index={0}>
            <Box px={3} pb={3}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <UserOutlined />
                        Thông tin cơ bản
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <List disablePadding>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <IdcardOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Họ và tên" secondary={profile.fullName || 'N/A'} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <UserOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Họ" secondary={profile.firstName || 'N/A'} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <UserOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Tên" secondary={profile.lastName || 'N/A'} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <CalendarOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Ngày sinh" secondary={formatDate(profile.dayOfBirth)} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <MailOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Email" secondary={profile.email} />
                        </ListItem>
                      </List>
                    </CardContent>
                  </Card>
                </Grid>

                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <EnvironmentOutlined />
                        Thông tin liên hệ
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <List disablePadding>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <PhoneOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Số điện thoại" secondary="Chưa cập nhật" />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <EnvironmentOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Địa chỉ" secondary="Chưa cập nhật" />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <EnvironmentOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Thành phố" secondary="Chưa cập nhật" />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <EnvironmentOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Quốc gia" secondary="Việt Nam" />
                        </ListItem>
                      </List>

                      <Box mt={2}>
                        <Button variant="outlined" size="small" startIcon={<EditOutlined />}>
                          Cập nhật thông tin liên hệ
                        </Button>
                      </Box>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Box>
          </TabPanel>

          {/* Professional Information Tab */}
          <TabPanel value={activeTab} index={1}>
            <Box px={3} pb={3}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <TeamOutlined />
                        Thông tin công việc
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <List disablePadding>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <IdcardOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Mã vị trí" secondary={profile.positionId || 'N/A'} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <UserOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Mã quản lý" secondary={profile.managerId || 'N/A'} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <TeamOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Phòng ban" secondary="Chưa cập nhật" />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <CalendarOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Ngày bắt đầu làm việc" secondary="Chưa cập nhật" />
                        </ListItem>
                      </List>
                    </CardContent>
                  </Card>
                </Grid>

                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <SecurityScanOutlined />
                        Vai trò và quyền
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Box sx={{ mb: 2 }}>
                        <Typography variant="body2" color="text.secondary" gutterBottom>
                          Vai trò đặc biệt
                        </Typography>
                        <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                          {profile.isDirector && <Chip label="Giám đốc" color="primary" size="small" />}
                          {profile.isHeadOfDepartment && <Chip label="Trưởng phòng" color="secondary" size="small" />}
                          {!profile.isDirector && !profile.isHeadOfDepartment && <Chip label="Nhân viên" color="default" size="small" />}
                        </Stack>
                      </Box>

                      <Box sx={{ mb: 2 }}>
                        <Typography variant="body2" color="text.secondary" gutterBottom>
                          Quyền hạn
                        </Typography>
                        <Stack spacing={1}>
                          <Chip label="Xem thông tin cá nhân" color="success" size="small" variant="outlined" />
                          <Chip label="Chỉnh sửa profile" color="success" size="small" variant="outlined" />
                          {(profile.isDirector || profile.isHeadOfDepartment) && (
                            <Chip label="Quản lý nhân viên" color="primary" size="small" variant="outlined" />
                          )}
                        </Stack>
                      </Box>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Box>
          </TabPanel>

          {/* Account Settings Tab */}
          <TabPanel value={activeTab} index={2}>
            <Box px={3} pb={3}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <IdcardOutlined />
                        Thông tin tài khoản
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <List disablePadding>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <IdcardOutlined />
                          </ListItemIcon>
                          <ListItemText
                            primary="ID tài khoản"
                            secondary={
                              <Typography variant="body2" sx={{ fontFamily: 'monospace', fontSize: '0.875rem' }}>
                                {profile.userId}
                              </Typography>
                            }
                          />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <UserOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Tên đăng nhập" secondary={profile.userName} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            <ClockCircleOutlined />
                          </ListItemIcon>
                          <ListItemText primary="Ngày tạo tài khoản" secondary={formatDate(profile.createdAt)} />
                        </ListItem>
                        <ListItem disablePadding sx={{ mb: 1 }}>
                          <ListItemIcon sx={{ minWidth: 40 }}>
                            {profile.isLocked ? (
                              <SecurityScanOutlined style={{ color: '#f44336' }} />
                            ) : (
                              <CheckCircleOutlined style={{ color: '#4caf50' }} />
                            )}
                          </ListItemIcon>
                          <ListItemText
                            primary="Trạng thái tài khoản"
                            secondary={
                              <Chip
                                label={profile.isLocked ? 'Bị khóa' : 'Hoạt động'}
                                color={profile.isLocked ? 'error' : 'success'}
                                size="small"
                              />
                            }
                          />
                        </ListItem>
                      </List>
                    </CardContent>
                  </Card>
                </Grid>

                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <SecurityScanOutlined />
                        Bảo mật
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Stack spacing={2}>
                        <Box>
                          <Typography variant="body2" fontWeight="medium" gutterBottom>
                            Mật khẩu
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Được cập nhật lần cuối: Chưa rõ
                          </Typography>
                          <Button variant="outlined" size="small" startIcon={<EditOutlined />}>
                            Đổi mật khẩu
                          </Button>
                        </Box>

                        <Box>
                          <Typography variant="body2" fontWeight="medium" gutterBottom>
                            Xác thực 2 bước
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Chưa kích hoạt
                          </Typography>
                          <Button variant="outlined" size="small" startIcon={<SecurityScanOutlined />}>
                            Kích hoạt 2FA
                          </Button>
                        </Box>

                        <Box>
                          <Typography variant="body2" fontWeight="medium" gutterBottom>
                            Phiên đăng nhập
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            1 thiết bị đang hoạt động
                          </Typography>
                          <Button variant="outlined" size="small" color="error">
                            Đăng xuất tất cả thiết bị
                          </Button>
                        </Box>
                      </Stack>
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Box>
          </TabPanel>
        </Paper>
      </Box>
    </Container>
  );
}
