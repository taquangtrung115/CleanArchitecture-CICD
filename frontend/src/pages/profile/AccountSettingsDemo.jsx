import { useState } from 'react';
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
  Badge,
  Switch,
  FormControlLabel,
  TextField,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  ListItemButton
} from '@mui/material';

// Icons
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
import BellOutlined from '@ant-design/icons/BellOutlined';
import LockOutlined from '@ant-design/icons/LockOutlined';
import EyeOutlined from '@ant-design/icons/EyeOutlined';
import DeleteOutlined from '@ant-design/icons/DeleteOutlined';
import MobileOutlined from '@ant-design/icons/MobileOutlined';
import DesktopOutlined from '@ant-design/icons/DesktopOutlined';
import GlobalOutlined from '@ant-design/icons/GlobalOutlined';

import avatar1 from 'assets/images/users/avatar-1.png';

// Mock profile data
const mockProfile = {
  userId: '123e4567-e89b-12d3-a456-426614174000',
  userName: 'john.doe',
  email: 'john.doe@example.com',
  firstName: 'John',
  lastName: 'Doe',
  fullName: 'John Doe',
  dayOfBirth: '1990-05-15',
  isDirector: false,
  isHeadOfDepartment: true,
  managerId: null,
  positionId: 'pos-123',
  isLocked: false,
  createdAt: '2023-01-15'
};

export default function AccountSettingsDemo() {
  const [profile] = useState(mockProfile);
  const [activeTab, setActiveTab] = useState(2); // Start with Account Settings tab
  const [changePasswordOpen, setChangePasswordOpen] = useState(false);
  const [notificationSettings, setNotificationSettings] = useState({
    emailNotifications: true,
    pushNotifications: true,
    smsNotifications: false,
    newsUpdates: true,
    securityAlerts: true,
    marketingEmails: false
  });
  const [privacySettings, setPrivacySettings] = useState({
    profileVisibility: 'public',
    showEmail: false,
    showPhone: false,
    allowSearchByEmail: true,
    allowSearchByPhone: false
  });

  const handleTabChange = (event, newValue) => {
    setActiveTab(newValue);
  };

  const handleNotificationChange = (setting) => (event) => {
    setNotificationSettings((prev) => ({
      ...prev,
      [setting]: event.target.checked
    }));
  };

  const handlePrivacyChange = (setting) => (event) => {
    setPrivacySettings((prev) => ({
      ...prev,
      [setting]: event.target.checked || event.target.value
    }));
  };

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

  // Mock active sessions data
  const activeSessions = [
    {
      id: 1,
      device: 'Windows PC',
      browser: 'Chrome 120',
      location: 'Ho Chi Minh City, Vietnam',
      lastActive: '2 minutes ago',
      current: true,
      icon: <DesktopOutlined />
    },
    {
      id: 2,
      device: 'iPhone 15',
      browser: 'Safari Mobile',
      location: 'Ho Chi Minh City, Vietnam',
      lastActive: '1 hour ago',
      current: false,
      icon: <MobileOutlined />
    },
    {
      id: 3,
      device: 'MacBook Pro',
      browser: 'Safari 17',
      location: 'Hanoi, Vietnam',
      lastActive: '3 days ago',
      current: false,
      icon: <DesktopOutlined />
    }
  ];

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

          {/* Account Settings Tab - Enhanced */}
          <TabPanel value={activeTab} index={2}>
            <Box px={3} pb={3}>
              <Grid container spacing={3}>
                {/* Account Information */}
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2, height: 'fit-content' }}>
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

                {/* Security Settings */}
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2, height: 'fit-content' }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <SecurityScanOutlined />
                        Bảo mật
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Stack spacing={3}>
                        <Box>
                          <Typography variant="body1" fontWeight="medium" gutterBottom>
                            Mật khẩu
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Được cập nhật lần cuối: 15/01/2024
                          </Typography>
                          <Button
                            variant="outlined"
                            size="small"
                            startIcon={<EditOutlined />}
                            onClick={() => setChangePasswordOpen(true)}
                            sx={{ mt: 1 }}
                          >
                            Đổi mật khẩu
                          </Button>
                        </Box>

                        <Box>
                          <Typography variant="body1" fontWeight="medium" gutterBottom>
                            Xác thực 2 bước (2FA)
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Bảo vệ tài khoản với lớp bảo mật bổ sung
                          </Typography>
                          <Stack direction="row" spacing={1} alignItems="center" sx={{ mt: 1 }}>
                            <Chip label="Chưa kích hoạt" color="warning" size="small" />
                            <Button variant="outlined" size="small" startIcon={<SecurityScanOutlined />}>
                              Kích hoạt 2FA
                            </Button>
                          </Stack>
                        </Box>

                        <Box>
                          <Typography variant="body1" fontWeight="medium" gutterBottom>
                            Khôi phục tài khoản
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Email khôi phục: {profile.email}
                          </Typography>
                          <Button variant="outlined" size="small" startIcon={<MailOutlined />} sx={{ mt: 1 }}>
                            Cập nhật email khôi phục
                          </Button>
                        </Box>
                      </Stack>
                    </CardContent>
                  </Card>
                </Grid>

                {/* Notification Preferences */}
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2, height: 'fit-content' }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <BellOutlined />
                        Thông báo
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Stack spacing={2}>
                        <FormControlLabel
                          control={
                            <Switch
                              checked={notificationSettings.emailNotifications}
                              onChange={handleNotificationChange('emailNotifications')}
                            />
                          }
                          label={
                            <Box>
                              <Typography variant="body2" fontWeight="medium">
                                Email thông báo
                              </Typography>
                              <Typography variant="caption" color="text.secondary">
                                Nhận thông báo qua email
                              </Typography>
                            </Box>
                          }
                        />

                        <FormControlLabel
                          control={
                            <Switch
                              checked={notificationSettings.pushNotifications}
                              onChange={handleNotificationChange('pushNotifications')}
                            />
                          }
                          label={
                            <Box>
                              <Typography variant="body2" fontWeight="medium">
                                Thông báo đẩy
                              </Typography>
                              <Typography variant="caption" color="text.secondary">
                                Nhận thông báo trên trình duyệt
                              </Typography>
                            </Box>
                          }
                        />

                        <FormControlLabel
                          control={
                            <Switch
                              checked={notificationSettings.smsNotifications}
                              onChange={handleNotificationChange('smsNotifications')}
                            />
                          }
                          label={
                            <Box>
                              <Typography variant="body2" fontWeight="medium">
                                SMS thông báo
                              </Typography>
                              <Typography variant="caption" color="text.secondary">
                                Nhận thông báo qua tin nhắn
                              </Typography>
                            </Box>
                          }
                        />

                        <Divider />

                        <Typography variant="subtitle2" fontWeight="medium">
                          Loại thông báo
                        </Typography>

                        <FormControlLabel
                          control={
                            <Switch checked={notificationSettings.securityAlerts} onChange={handleNotificationChange('securityAlerts')} />
                          }
                          label="Cảnh báo bảo mật"
                        />

                        <FormControlLabel
                          control={<Switch checked={notificationSettings.newsUpdates} onChange={handleNotificationChange('newsUpdates')} />}
                          label="Cập nhật tin tức"
                        />

                        <FormControlLabel
                          control={
                            <Switch checked={notificationSettings.marketingEmails} onChange={handleNotificationChange('marketingEmails')} />
                          }
                          label="Email marketing"
                        />
                      </Stack>
                    </CardContent>
                  </Card>
                </Grid>

                {/* Privacy Settings */}
                <Grid item xs={12} md={6}>
                  <Card variant="outlined" sx={{ borderRadius: 2, height: 'fit-content' }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <EyeOutlined />
                        Quyền riêng tư
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Stack spacing={3}>
                        <Box>
                          <FormControl fullWidth size="small">
                            <InputLabel>Hiển thị profile</InputLabel>
                            <Select
                              value={privacySettings.profileVisibility}
                              onChange={handlePrivacyChange('profileVisibility')}
                              label="Hiển thị profile"
                            >
                              <MenuItem value="public">Công khai</MenuItem>
                              <MenuItem value="friends">Bạn bè</MenuItem>
                              <MenuItem value="private">Riêng tư</MenuItem>
                            </Select>
                          </FormControl>
                        </Box>

                        <FormControlLabel
                          control={<Switch checked={privacySettings.showEmail} onChange={handlePrivacyChange('showEmail')} />}
                          label="Hiển thị email công khai"
                        />

                        <FormControlLabel
                          control={<Switch checked={privacySettings.showPhone} onChange={handlePrivacyChange('showPhone')} />}
                          label="Hiển thị số điện thoại"
                        />

                        <FormControlLabel
                          control={
                            <Switch checked={privacySettings.allowSearchByEmail} onChange={handlePrivacyChange('allowSearchByEmail')} />
                          }
                          label="Cho phép tìm kiếm bằng email"
                        />

                        <FormControlLabel
                          control={
                            <Switch checked={privacySettings.allowSearchByPhone} onChange={handlePrivacyChange('allowSearchByPhone')} />
                          }
                          label="Cho phép tìm kiếm bằng SĐT"
                        />

                        <Alert severity="info" sx={{ mt: 2 }}>
                          Thay đổi cài đặt quyền riêng tư có thể mất vài phút để có hiệu lực.
                        </Alert>
                      </Stack>
                    </CardContent>
                  </Card>
                </Grid>

                {/* Active Sessions */}
                <Grid item xs={12}>
                  <Card variant="outlined" sx={{ borderRadius: 2 }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <GlobalOutlined />
                        Phiên đăng nhập hoạt động
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <List disablePadding>
                        {activeSessions.map((session, index) => (
                          <ListItem key={session.id} disablePadding sx={{ mb: 2 }}>
                            <ListItemButton
                              sx={{
                                borderRadius: 1,
                                border: session.current ? '2px solid' : '1px solid',
                                borderColor: session.current ? 'primary.main' : 'divider'
                              }}
                            >
                              <ListItemIcon sx={{ minWidth: 40 }}>{session.icon}</ListItemIcon>
                              <ListItemText
                                primary={
                                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                    <Typography variant="body1" fontWeight="medium">
                                      {session.device}
                                    </Typography>
                                    {session.current && <Chip label="Hiện tại" color="primary" size="small" />}
                                  </Box>
                                }
                                secondary={
                                  <Box>
                                    <Typography variant="body2" color="text.secondary">
                                      {session.browser} • {session.location}
                                    </Typography>
                                    <Typography variant="caption" color="text.secondary">
                                      Hoạt động lần cuối: {session.lastActive}
                                    </Typography>
                                  </Box>
                                }
                              />
                              {!session.current && (
                                <IconButton color="error" size="small">
                                  <DeleteOutlined />
                                </IconButton>
                              )}
                            </ListItemButton>
                          </ListItem>
                        ))}
                      </List>

                      <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
                        <Button variant="outlined" color="error" startIcon={<DeleteOutlined />}>
                          Đăng xuất tất cả thiết bị khác
                        </Button>
                        <Button variant="outlined" startIcon={<SecurityScanOutlined />}>
                          Xem lịch sử đăng nhập
                        </Button>
                      </Box>
                    </CardContent>
                  </Card>
                </Grid>

                {/* Account Actions */}
                <Grid item xs={12}>
                  <Card variant="outlined" sx={{ borderRadius: 2, borderColor: 'error.main' }}>
                    <CardContent>
                      <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1, color: 'error.main' }}>
                        <DeleteOutlined />
                        Vùng nguy hiểm
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      <Stack spacing={2}>
                        <Box>
                          <Typography variant="body1" fontWeight="medium" gutterBottom>
                            Tạm khóa tài khoản
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Tạm thời vô hiệu hóa tài khoản của bạn. Bạn có thể khôi phục bất cứ lúc nào.
                          </Typography>
                          <Button variant="outlined" color="warning" size="small">
                            Tạm khóa tài khoản
                          </Button>
                        </Box>

                        <Divider />

                        <Box>
                          <Typography variant="body1" fontWeight="medium" gutterBottom color="error.main">
                            Xóa tài khoản vĩnh viễn
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            Xóa hoàn toàn tài khoản và tất cả dữ liệu của bạn. Hành động này không thể hoàn tác.
                          </Typography>
                          <Button variant="outlined" color="error" size="small">
                            Xóa tài khoản
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

        {/* Change Password Dialog */}
        <Dialog open={changePasswordOpen} onClose={() => setChangePasswordOpen(false)} maxWidth="sm" fullWidth>
          <DialogTitle>Đổi mật khẩu</DialogTitle>
          <DialogContent>
            <Stack spacing={3} sx={{ mt: 1 }}>
              <TextField label="Mật khẩu hiện tại" type="password" fullWidth size="small" />
              <TextField label="Mật khẩu mới" type="password" fullWidth size="small" />
              <TextField label="Xác nhận mật khẩu mới" type="password" fullWidth size="small" />
              <Alert severity="info">Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.</Alert>
            </Stack>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setChangePasswordOpen(false)}>Hủy</Button>
            <Button variant="contained" onClick={() => setChangePasswordOpen(false)}>
              Cập nhật mật khẩu
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    </Container>
  );
}
