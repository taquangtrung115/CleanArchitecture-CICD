import React, { useState, useEffect, useRef } from 'react';
import {
  Box,
  Paper,
  Typography,
  TextField,
  Button,
  Avatar,
  Chip,
  Alert,
  CircularProgress,
  Stack,
  Grid,
  Card,
  CardContent,
  Divider
} from '@mui/material';
import {
  Send as SendIcon,
  SmartToy as BotIcon,
  Person as PersonIcon,
  CheckCircle as SuccessIcon,
  Error as ErrorIcon
} from '@mui/icons-material';
import { sendChatMessage, getChatHistory } from '../../api/chat';
import { getErrorMessage } from '../../utils/errorHandler';

const ChatPage = () => {
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const messagesEndRef = useRef(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  useEffect(() => {
    loadChatHistory();
  }, []);

  const loadChatHistory = async () => {
    try {
      const result = await getChatHistory();
      if (result.data && result.data.value && result.data.value.messages) {
        const historyMessages = result.data.value.messages.map((msg) => ({
          id: msg.id,
          type: 'history',
          userMessage: msg.message,
          botResponse: msg.response,
          actionPerformed: msg.actionPerformed,
          timestamp: new Date(msg.timestamp)
        }));
        setMessages(historyMessages);
      } else if (result.error) {
        console.error('Error loading chat history:', getErrorMessage(result));
      }
    } catch (err) {
      console.error('Error loading chat history:', err);
    }
  };

  const handleSendMessage = async () => {
    if (!inputMessage.trim()) return;

    const userMessage = inputMessage.trim();
    setInputMessage('');
    setLoading(true);
    setError(null);

    // Add user message to the UI immediately
    const tempMessage = {
      id: Date.now().toString(),
      type: 'user',
      userMessage,
      timestamp: new Date()
    };
    setMessages((prev) => [...prev, tempMessage]);

    try {
      const result = await sendChatMessage(userMessage);

      if (result.data && result.data.value) {
        const response = result.data.value;
        const botMessage = {
          id: response.id,
          type: 'bot',
          userMessage: response.message,
          botResponse: response.response,
          actionPerformed: response.actionPerformed,
          actionResult: response.actionResult,
          success: response.success,
          timestamp: new Date(response.timestamp)
        };

        // Replace temp message with complete conversation
        setMessages((prev) => {
          const filtered = prev.filter((msg) => msg.id !== tempMessage.id);
          return [...filtered, botMessage];
        });
      } else if (result.error) {
        // Use getErrorMessage to extract user-friendly message from error object
        const errorMessage = getErrorMessage(result);
        setError(errorMessage || 'Có lỗi xảy ra khi gửi tin nhắn. Vui lòng thử lại.');
        console.error('Error sending message:', result.error);

        // Remove temp message on error
        setMessages((prev) => prev.filter((msg) => msg.id !== tempMessage.id));
      } else {
        throw new Error('Invalid response format');
      }
    } catch (err) {
      setError('Có lỗi xảy ra khi gửi tin nhắn. Vui lòng thử lại.');
      console.error('Error sending message:', err);

      // Remove temp message on error
      setMessages((prev) => prev.filter((msg) => msg.id !== tempMessage.id));
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (event) => {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      handleSendMessage();
    }
  };

  const renderMessage = (message) => {
    const isBot = message.type === 'bot' || message.type === 'history';

    return (
      <Box key={message.id} sx={{ mb: 2 }}>
        <Stack direction="row" spacing={2} alignItems="flex-start">
          <Avatar sx={{ bgcolor: isBot ? 'primary.main' : 'secondary.main' }}>{isBot ? <BotIcon /> : <PersonIcon />}</Avatar>

          <Box sx={{ flex: 1 }}>
            <Typography variant="caption" color="text.secondary">
              {isBot ? 'AI Assistant' : 'Bạn'} • {message.timestamp?.toLocaleTimeString()}
            </Typography>

            <Card variant="outlined" sx={{ mt: 0.5 }}>
              <CardContent sx={{ p: 2, '&:last-child': { pb: 2 } }}>
                <Typography variant="body2" sx={{ mb: 1 }}>
                  <strong>Tin nhắn:</strong> {message.userMessage}
                </Typography>

                {message.botResponse && (
                  <>
                    <Divider sx={{ my: 1 }} />
                    <Typography variant="body2" sx={{ mb: 1 }}>
                      <strong>Phản hồi:</strong> {message.botResponse}
                    </Typography>
                  </>
                )}

                {message.actionPerformed && (
                  <Box sx={{ mt: 2 }}>
                    <Chip
                      icon={message.success ? <SuccessIcon /> : <ErrorIcon />}
                      label={`Thao tác: ${message.actionPerformed}`}
                      color={message.success ? 'success' : 'error'}
                      size="small"
                    />
                  </Box>
                )}
              </CardContent>
            </Card>
          </Box>
        </Stack>
      </Box>
    );
  };

  return (
    <Box sx={{ height: '80vh', display: 'flex', flexDirection: 'column' }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        AI Assistant - Quản lý Quyền và Vai trò
      </Typography>

      <Grid container spacing={3} sx={{ flex: 1 }}>
        <Grid item xs={12} md={8}>
          <Paper sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            {/* Chat Header */}
            <Box sx={{ p: 2, borderBottom: 1, borderColor: 'divider' }}>
              <Typography variant="h6">Trò chuyện với AI</Typography>
              <Typography variant="body2" color="text.secondary">
                Bạn có thể yêu cầu tạo role, tạo quyền, gán quyền, gán user vào role...
              </Typography>
            </Box>

            {/* Messages Area */}
            <Box sx={{ flex: 1, overflow: 'auto', p: 2 }}>
              {messages.length === 0 && (
                <Box sx={{ textAlign: 'center', mt: 4 }}>
                  <Typography variant="body2" color="text.secondary">
                    Chưa có tin nhắn nào. Hãy bắt đầu trò chuyện!
                  </Typography>
                </Box>
              )}

              {messages.map(renderMessage)}

              {loading && (
                <Box sx={{ display: 'flex', justifyContent: 'center', my: 2 }}>
                  <CircularProgress size={24} />
                  <Typography variant="body2" sx={{ ml: 1 }}>
                    AI đang xử lý...
                  </Typography>
                </Box>
              )}

              <div ref={messagesEndRef} />
            </Box>

            {/* Input Area */}
            <Box sx={{ p: 2, borderTop: 1, borderColor: 'divider' }}>
              {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                  {error}
                </Alert>
              )}

              <Stack direction="row" spacing={1}>
                <TextField
                  fullWidth
                  multiline
                  maxRows={4}
                  value={inputMessage}
                  onChange={(e) => setInputMessage(e.target.value)}
                  onKeyPress={handleKeyPress}
                  placeholder="Nhập tin nhắn... (ví dụ: 'Tạo role Admin' hoặc 'Danh sách roles')"
                  disabled={loading}
                  variant="outlined"
                  size="small"
                />
                <Button
                  variant="contained"
                  endIcon={<SendIcon />}
                  onClick={handleSendMessage}
                  disabled={loading || !inputMessage.trim()}
                  sx={{ minWidth: 80 }}
                >
                  Gửi
                </Button>
              </Stack>
            </Box>
          </Paper>
        </Grid>

        <Grid item xs={12} md={4}>
          <Paper sx={{ p: 2, height: '100%' }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
              Hướng dẫn sử dụng
            </Typography>

            <Typography variant="body2" sx={{ mb: 2 }}>
              Bạn có thể sử dụng các lệnh sau:
            </Typography>

            <Stack spacing={1}>
              <Chip label="Tạo role [tên role]" variant="outlined" size="small" />
              <Chip label="Tạo quyền [tên quyền]" variant="outlined" size="small" />
              <Chip label="Gán quyền [quyền] vào role [role]" variant="outlined" size="small" />
              <Chip label="Gán user [user] vào role [role]" variant="outlined" size="small" />
              <Chip label="Danh sách roles" variant="outlined" size="small" />
              <Chip label="Danh sách users" variant="outlined" size="small" />
            </Stack>

            <Typography variant="body2" sx={{ mt: 3, color: 'text.secondary' }}>
              AI sẽ hiểu và thực hiện các thao tác quản lý quyền tự động.
            </Typography>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};

export default ChatPage;
