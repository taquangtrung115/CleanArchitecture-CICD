import React, { useState, useEffect, useRef, useCallback } from 'react';
import {
  Box,
  Paper,
  Typography,
  TextField,
  Button,
  Avatar,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  IconButton,
  Badge,
  Divider,
  InputAdornment,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
  Alert,
  CircularProgress,
  Stack,
  Grid,
  Card,
  CardContent,
  AppBar,
  Toolbar,
  Menu,
  MenuList,
  MenuItem as MenuItemComponent,
  ListItemIcon
} from '@mui/material';
import {
  Send as SendIcon,
  Person as PersonIcon,
  Group as GroupIcon,
  Add as AddIcon,
  Search as SearchIcon,
  MoreVert as MoreVertIcon,
  Chat as ChatIcon,
  Circle as CircleIcon,
  AttachFile as AttachFileIcon,
  EmojiEmotions as EmojiIcon,
  VideoCall as VideoCallIcon,
  Phone as PhoneIcon,
  Info as InfoIcon
} from '@mui/icons-material';
import { useTheme } from '@mui/material/styles';
import chatService from '../../services/chatService';
import * as userChatApi from '../../api/userChat';

const UserChatPage = () => {
  const theme = useTheme();
  const [conversations, setConversations] = useState([]);
  const [selectedConversation, setSelectedConversation] = useState(null);
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
  const [onlineUsers, setOnlineUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [connectionStatus, setConnectionStatus] = useState('disconnected');
  const [error, setError] = useState(null);
  const [newChatDialog, setNewChatDialog] = useState(false);
  const [searchUsers, setSearchUsers] = useState('');
  const [menuAnchor, setMenuAnchor] = useState(null);

  const messagesEndRef = useRef(null);
  const currentUser = {
    id: 'current-user-id', // This should come from auth context
    name: 'Current User',
    email: 'user@example.com'
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // Initialize chat service
  useEffect(() => {
    const initializeChat = async () => {
      try {
        setLoading(true);
        const token = localStorage.getItem('token'); // Get from auth context

        if (token) {
          await chatService.startConnection(token);
          setConnectionStatus('connected');

          // Set up event listeners
          chatService.on('messageReceived', handleMessageReceived);
          chatService.on('messageSent', handleMessageSent);
          chatService.on('userOnline', handleUserOnline);
          chatService.on('userOffline', handleUserOffline);
          chatService.on('onlineUsers', handleOnlineUsers);

          // Load initial data
          await loadConversations();
          await loadOnlineUsers();
        }
      } catch (error) {
        console.error('Failed to initialize chat:', error);
        setError('Failed to connect to chat server');
        setConnectionStatus('error');
      } finally {
        setLoading(false);
      }
    };

    initializeChat();

    return () => {
      // Cleanup event listeners
      if (chatService) {
        chatService.off('messageReceived', handleMessageReceived);
        chatService.off('messageSent', handleMessageSent);
        chatService.off('userOnline', handleUserOnline);
        chatService.off('userOffline', handleUserOffline);
        chatService.off('onlineUsers', handleOnlineUsers);
      }
    };
  }, []);

  // Event handlers
  const handleMessageReceived = useCallback((messageData) => {
    setMessages((prev) => [
      ...prev,
      {
        id: Date.now(),
        content: messageData.Message,
        senderId: messageData.SenderId,
        receiverId: messageData.ReceiverId,
        timestamp: new Date(messageData.Timestamp),
        type: 'received'
      }
    ]);
  }, []);

  const handleMessageSent = useCallback((messageData) => {
    setMessages((prev) => [
      ...prev,
      {
        id: Date.now(),
        content: messageData.Message,
        senderId: messageData.SenderId,
        receiverId: messageData.ReceiverId,
        timestamp: new Date(messageData.Timestamp),
        type: 'sent'
      }
    ]);
  }, []);

  const handleUserOnline = useCallback((userId) => {
    setOnlineUsers((prev) => [...new Set([...prev, userId])]);
  }, []);

  const handleUserOffline = useCallback((userId) => {
    setOnlineUsers((prev) => prev.filter((id) => id !== userId));
  }, []);

  const handleOnlineUsers = useCallback((users) => {
    setOnlineUsers(users);
  }, []);

  // Data loading functions
  const loadConversations = async () => {
    try {
      const result = await userChatApi.getChatRooms(currentUser.id);
      if (result.data && result.data.isSuccess) {
        setConversations(result.data.value.rooms || []);
      }
    } catch (error) {
      console.error('Failed to load conversations:', error);
    }
  };

  const loadOnlineUsers = async () => {
    try {
      const result = await userChatApi.getOnlineUsers(currentUser.id);
      if (result.data && result.data.isSuccess) {
        setOnlineUsers(result.data.value.users.map((u) => u.id) || []);
      }
    } catch (error) {
      console.error('Failed to load online users:', error);
    }
  };

  const loadChatHistory = async (conversationId, isRoom = false) => {
    try {
      setLoading(true);
      const result = await userChatApi.getChatHistory(currentUser.id, isRoom ? null : conversationId, isRoom ? conversationId : null);

      if (result.data && result.data.isSuccess) {
        const historyMessages = result.data.value.messages.map((msg) => ({
          id: msg.id,
          content: msg.content,
          senderId: msg.senderId,
          receiverId: msg.receiverId,
          timestamp: new Date(msg.createdDate),
          type: msg.senderId === currentUser.id ? 'sent' : 'received',
          senderName: msg.senderName
        }));
        setMessages(historyMessages);
      }
    } catch (error) {
      console.error('Failed to load chat history:', error);
      setError('Failed to load chat history');
    } finally {
      setLoading(false);
    }
  };

  // Message sending
  const handleSendMessage = async () => {
    if (!newMessage.trim() || !selectedConversation) return;

    try {
      const messageContent = newMessage.trim();
      setNewMessage('');

      if (selectedConversation.type === 'direct') {
        await chatService.sendMessageToUser(selectedConversation.id, messageContent);
      } else {
        await chatService.sendMessageToRoom(selectedConversation.id, messageContent);
      }
    } catch (error) {
      console.error('Failed to send message:', error);
      setError('Failed to send message');
    }
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      handleSendMessage();
    }
  };

  // Conversation selection
  const handleConversationSelect = async (conversation) => {
    setSelectedConversation(conversation);
    await loadChatHistory(conversation.id, conversation.type === 'room');
  };

  // New chat dialog
  const handleNewChatDialog = () => {
    setNewChatDialog(true);
  };

  const handleCloseNewChatDialog = () => {
    setNewChatDialog(false);
    setSearchUsers('');
  };

  // Get connection status color
  const getConnectionStatusColor = () => {
    switch (connectionStatus) {
      case 'connected':
        return 'success';
      case 'connecting':
        return 'warning';
      case 'error':
        return 'error';
      default:
        return 'default';
    }
  };

  const filteredOnlineUsers = onlineUsers.filter(
    (userId) => userId !== currentUser.id && userId.toLowerCase().includes(searchUsers.toLowerCase())
  );

  return (
    <Box sx={{ height: '100vh', display: 'flex', flexDirection: 'column' }}>
      {/* Header */}
      <AppBar position="static" color="default" elevation={1}>
        <Toolbar>
          <ChatIcon sx={{ mr: 2 }} />
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            User Chat
          </Typography>
          <Chip icon={<CircleIcon />} label={connectionStatus} color={getConnectionStatusColor()} size="small" sx={{ mr: 2 }} />
          <IconButton onClick={(e) => setMenuAnchor(e.currentTarget)} size="small">
            <MoreVertIcon />
          </IconButton>
        </Toolbar>
      </AppBar>

      {error && (
        <Alert severity="error" onClose={() => setError(null)} sx={{ m: 1 }}>
          {error}
        </Alert>
      )}

      <Grid container sx={{ flex: 1, overflow: 'hidden' }}>
        {/* Conversations List */}
        <Grid item xs={12} md={4} lg={3}>
          <Paper sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            {/* Conversations Header */}
            <Box sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                <Typography variant="h6">Conversations</Typography>
                <IconButton onClick={handleNewChatDialog} size="small">
                  <AddIcon />
                </IconButton>
              </Box>
              <TextField
                fullWidth
                size="small"
                placeholder="Search conversations..."
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <SearchIcon />
                    </InputAdornment>
                  )
                }}
              />
            </Box>

            {/* Conversations List */}
            <Box sx={{ flex: 1, overflow: 'auto' }}>
              <List disablePadding>
                {conversations.map((conversation) => (
                  <ListItem
                    key={conversation.id}
                    button
                    selected={selectedConversation?.id === conversation.id}
                    onClick={() => handleConversationSelect(conversation)}
                    sx={{
                      borderBottom: 1,
                      borderColor: 'divider',
                      '&.Mui-selected': {
                        backgroundColor: theme.palette.action.selected
                      }
                    }}
                  >
                    <ListItemAvatar>
                      <Badge color="success" variant="dot" invisible={!onlineUsers.includes(conversation.id)}>
                        <Avatar>{conversation.type === 'room' ? <GroupIcon /> : <PersonIcon />}</Avatar>
                      </Badge>
                    </ListItemAvatar>
                    <ListItemText
                      primary={conversation.name}
                      secondary={conversation.lastMessage || 'No messages yet'}
                      secondaryTypographyProps={{
                        noWrap: true,
                        sx: { maxWidth: 150 }
                      }}
                    />
                    {conversation.unreadCount > 0 && <Chip label={conversation.unreadCount} color="primary" size="small" />}
                  </ListItem>
                ))}
              </List>
            </Box>
          </Paper>
        </Grid>

        {/* Chat Area */}
        <Grid item xs={12} md={8} lg={9}>
          {selectedConversation ? (
            <Paper sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
              {/* Chat Header */}
              <Box sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                  <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Avatar sx={{ mr: 2 }}>{selectedConversation.type === 'room' ? <GroupIcon /> : <PersonIcon />}</Avatar>
                    <Box>
                      <Typography variant="h6">{selectedConversation.name}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        {selectedConversation.type === 'room'
                          ? `${selectedConversation.memberCount || 0} members`
                          : onlineUsers.includes(selectedConversation.id)
                            ? 'Online'
                            : 'Offline'}
                      </Typography>
                    </Box>
                  </Box>
                  <Box>
                    <IconButton size="small">
                      <PhoneIcon />
                    </IconButton>
                    <IconButton size="small">
                      <VideoCallIcon />
                    </IconButton>
                    <IconButton size="small">
                      <InfoIcon />
                    </IconButton>
                  </Box>
                </Box>
              </Box>

              {/* Messages Area */}
              <Box sx={{ flex: 1, overflow: 'auto', p: 1 }}>
                {loading ? (
                  <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}>
                    <CircularProgress />
                  </Box>
                ) : (
                  <Stack spacing={1}>
                    {messages.map((message) => (
                      <Box
                        key={message.id}
                        sx={{
                          display: 'flex',
                          justifyContent: message.type === 'sent' ? 'flex-end' : 'flex-start',
                          mb: 1
                        }}
                      >
                        <Card
                          sx={{
                            maxWidth: '70%',
                            backgroundColor: message.type === 'sent' ? theme.palette.primary.main : theme.palette.grey[100],
                            color: message.type === 'sent' ? theme.palette.primary.contrastText : theme.palette.text.primary
                          }}
                        >
                          <CardContent sx={{ p: 1.5, '&:last-child': { pb: 1.5 } }}>
                            {message.type === 'received' && selectedConversation.type === 'room' && (
                              <Typography variant="caption" color="primary">
                                {message.senderName}
                              </Typography>
                            )}
                            <Typography variant="body1">{message.content}</Typography>
                            <Typography variant="caption" sx={{ opacity: 0.7 }}>
                              {message.timestamp.toLocaleTimeString()}
                            </Typography>
                          </CardContent>
                        </Card>
                      </Box>
                    ))}
                    <div ref={messagesEndRef} />
                  </Stack>
                )}
              </Box>

              {/* Message Input */}
              <Box sx={{ p: 2, borderTop: 1, borderColor: 'divider' }}>
                <TextField
                  fullWidth
                  multiline
                  maxRows={4}
                  value={newMessage}
                  onChange={(e) => setNewMessage(e.target.value)}
                  onKeyPress={handleKeyPress}
                  placeholder="Type a message..."
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <IconButton size="small">
                          <AttachFileIcon />
                        </IconButton>
                        <IconButton size="small">
                          <EmojiIcon />
                        </IconButton>
                      </InputAdornment>
                    ),
                    endAdornment: (
                      <InputAdornment position="end">
                        <IconButton onClick={handleSendMessage} disabled={!newMessage.trim()} color="primary">
                          <SendIcon />
                        </IconButton>
                      </InputAdornment>
                    )
                  }}
                />
              </Box>
            </Paper>
          ) : (
            <Box
              sx={{
                height: '100%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                flexDirection: 'column'
              }}
            >
              <ChatIcon sx={{ fontSize: 64, color: 'text.secondary', mb: 2 }} />
              <Typography variant="h5" color="text.secondary" gutterBottom>
                Select a conversation to start chatting
              </Typography>
              <Typography variant="body1" color="text.secondary">
                Choose from existing conversations or start a new one
              </Typography>
            </Box>
          )}
        </Grid>
      </Grid>

      {/* New Chat Dialog */}
      <Dialog open={newChatDialog} onClose={handleCloseNewChatDialog} maxWidth="sm" fullWidth>
        <DialogTitle>Start New Conversation</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Search users"
            fullWidth
            variant="outlined"
            value={searchUsers}
            onChange={(e) => setSearchUsers(e.target.value)}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              )
            }}
          />
          <List>
            {filteredOnlineUsers.map((userId) => (
              <ListItem
                key={userId}
                button
                onClick={() => {
                  // Handle user selection for new chat
                  console.log('Start chat with:', userId);
                  handleCloseNewChatDialog();
                }}
              >
                <ListItemAvatar>
                  <Badge color="success" variant="dot">
                    <Avatar>
                      <PersonIcon />
                    </Avatar>
                  </Badge>
                </ListItemAvatar>
                <ListItemText primary={userId} secondary="Online" />
              </ListItem>
            ))}
          </List>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseNewChatDialog}>Cancel</Button>
        </DialogActions>
      </Dialog>

      {/* Menu */}
      <Menu anchorEl={menuAnchor} open={Boolean(menuAnchor)} onClose={() => setMenuAnchor(null)}>
        <MenuItemComponent onClick={() => setMenuAnchor(null)}>
          <ListItemIcon>
            <PersonIcon />
          </ListItemIcon>
          Profile
        </MenuItemComponent>
        <MenuItemComponent onClick={() => setMenuAnchor(null)}>
          <ListItemIcon>
            <GroupIcon />
          </ListItemIcon>
          Create Group
        </MenuItemComponent>
      </Menu>
    </Box>
  );
};

export default UserChatPage;
