import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

class ChatService {
  constructor() {
    this.connection = null;
    this.isConnected = false;
    this.connectionPromise = null;
    this.eventHandlers = new Map();
  }

  async startConnection(token) {
    if (this.connection) {
      await this.stopConnection();
    }

    const baseURL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

    this.connection = new HubConnectionBuilder()
      .withUrl(`${baseURL}/chathub`, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    // Set up reconnection handlers
    this.connection.onreconnecting(() => {
      console.log('SignalR connection lost, attempting to reconnect...');
      this.isConnected = false;
    });

    this.connection.onreconnected(() => {
      console.log('SignalR connection restored');
      this.isConnected = true;
    });

    this.connection.onclose(() => {
      console.log('SignalR connection closed');
      this.isConnected = false;
    });

    // Set up event handlers
    this.setupEventHandlers();

    try {
      await this.connection.start();
      this.isConnected = true;
      console.log('SignalR connected successfully');

      // Get online users after connecting
      await this.getOnlineUsers();
    } catch (error) {
      console.error('Error starting SignalR connection:', error);
      this.isConnected = false;
      throw error;
    }
  }

  async stopConnection() {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      this.isConnected = false;
    }
  }

  setupEventHandlers() {
    if (!this.connection) return;

    // Message events
    this.connection.on('ReceiveMessage', (messageData) => {
      this.emit('messageReceived', messageData);
    });

    this.connection.on('MessageSent', (messageData) => {
      this.emit('messageSent', messageData);
    });

    this.connection.on('ReceiveRoomMessage', (messageData) => {
      this.emit('roomMessageReceived', messageData);
    });

    // User presence events
    this.connection.on('UserOnline', (userId) => {
      this.emit('userOnline', userId);
    });

    this.connection.on('UserOffline', (userId) => {
      this.emit('userOffline', userId);
    });

    this.connection.on('OnlineUsers', (users) => {
      this.emit('onlineUsers', users);
    });

    // Room events
    this.connection.on('UserJoinedRoom', (userId, roomId) => {
      this.emit('userJoinedRoom', { userId, roomId });
    });

    this.connection.on('UserLeftRoom', (userId, roomId) => {
      this.emit('userLeftRoom', { userId, roomId });
    });
  }

  // Event handling methods
  on(event, handler) {
    if (!this.eventHandlers.has(event)) {
      this.eventHandlers.set(event, []);
    }
    this.eventHandlers.get(event).push(handler);
  }

  off(event, handler) {
    if (this.eventHandlers.has(event)) {
      const handlers = this.eventHandlers.get(event);
      const index = handlers.indexOf(handler);
      if (index > -1) {
        handlers.splice(index, 1);
      }
    }
  }

  emit(event, data) {
    if (this.eventHandlers.has(event)) {
      this.eventHandlers.get(event).forEach((handler) => {
        try {
          handler(data);
        } catch (error) {
          console.error(`Error in event handler for ${event}:`, error);
        }
      });
    }
  }

  // Chat methods
  async sendMessageToUser(receiverId, message) {
    if (!this.isConnected || !this.connection) {
      throw new Error('Not connected to chat server');
    }

    try {
      await this.connection.invoke('SendMessageToUser', receiverId, message);
    } catch (error) {
      console.error('Error sending message:', error);
      throw error;
    }
  }

  async sendMessageToRoom(roomId, message) {
    if (!this.isConnected || !this.connection) {
      throw new Error('Not connected to chat server');
    }

    try {
      await this.connection.invoke('SendMessageToRoom', roomId, message);
    } catch (error) {
      console.error('Error sending room message:', error);
      throw error;
    }
  }

  async joinChatRoom(roomId) {
    if (!this.isConnected || !this.connection) {
      throw new Error('Not connected to chat server');
    }

    try {
      await this.connection.invoke('JoinChatRoom', roomId);
    } catch (error) {
      console.error('Error joining room:', error);
      throw error;
    }
  }

  async leaveChatRoom(roomId) {
    if (!this.isConnected || !this.connection) {
      throw new Error('Not connected to chat server');
    }

    try {
      await this.connection.invoke('LeaveChatRoom', roomId);
    } catch (error) {
      console.error('Error leaving room:', error);
      throw error;
    }
  }

  async getOnlineUsers() {
    if (!this.isConnected || !this.connection) {
      throw new Error('Not connected to chat server');
    }

    try {
      await this.connection.invoke('GetOnlineUsers');
    } catch (error) {
      console.error('Error getting online users:', error);
      throw error;
    }
  }

  // Connection status
  getConnectionState() {
    return {
      isConnected: this.isConnected,
      connectionId: this.connection?.connectionId
    };
  }
}

// Export singleton instance
export default new ChatService();
