# AI Agent for Role and Permission Management

## Overview

This implementation adds an AI-powered chat agent to the CleanArchitecture-CICD project that helps automate role and permission management through natural language interactions.

## Features Implemented

### Backend (API)
1. **AI Chat API Endpoints** (`/api/v1/chat/`)
   - `POST /api/v1/chat/message` - Process chat messages
   - `GET /api/v1/chat/history` - Get chat history

2. **AI Services**
   - `OpenAiChatService` - Integrates with OpenAI API for natural language processing
   - `IdentityManagementService` - Bridges chat requests to existing identity operations

3. **Supported Operations**
   - Create roles: "Tạo role Admin" or "Create role Manager"
   - Create permissions: "Tạo quyền USER_READ" or "Create permission USER_WRITE"
   - Assign permissions to roles: "Gán quyền USER_READ vào role Admin"
   - Assign users to roles: "Gán user john@email.com vào role Admin"
   - List roles: "Danh sách roles" or "List roles"

### Frontend (React)
1. **Chat Interface** (`/admin/chat`)
   - Real-time chat interface with AI assistant
   - Message history display
   - Action result visualization
   - Usage guide panel

2. **Navigation Integration**
   - Added "AI Assistant" menu item in Management section
   - Accessible via `/admin/chat` route

## Technical Architecture

### Backend Structure
```
src/
├── DemoCICD.Contract/Services/V1/AI/
│   ├── Command.cs         # Chat command contracts
│   ├── Query.cs          # Chat query contracts
│   └── Response.cs       # Chat response contracts
├── DemoCICD.Presentation/APIs/AI/
│   └── ChatAgentApi.cs   # API endpoints
├── DemoCID.Application/UserCases/V1/
│   ├── Commands/AI/ProcessChatMessageCommandHandler.cs
│   └── Queries/AI/GetChatHistoryQueryHandler.cs
└── DemoCICD.Infrastructure/AI/
    ├── OpenAiChatService.cs          # OpenAI integration
    └── IdentityManagementService.cs  # Identity operations bridge
```

### Frontend Structure
```
frontend/src/
├── api/chat.js           # Chat API integration
├── pages/chat/ChatPage.jsx  # Main chat interface
└── menu-items/management.jsx  # Updated navigation
```

## Configuration

### Backend Configuration
Add to `appsettings.json`:
```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

For demo purposes, the system uses "demo-key" and provides mock responses when no real OpenAI key is configured.

## Usage Examples

### Vietnamese Commands
- "Tạo role Quản trị viên"
- "Tạo quyền USER_MANAGE"
- "Gán quyền USER_READ vào role Admin"
- "Gán user nguyenvana@email.com vào role Manager"
- "Danh sách tất cả roles"

### English Commands
- "Create role Administrator"
- "Create permission USER_DELETE"
- "Assign permission USER_WRITE to role Editor"
- "Assign user jane@email.com to role Admin"
- "List all roles"

## AI Processing Flow

1. **User Input**: User sends natural language message
2. **AI Processing**: OpenAI processes the message to understand intent
3. **Action Mapping**: System maps AI response to specific identity operations
4. **Execution**: Relevant CQRS command is executed via MediatR
5. **Response**: Result is returned to user with action confirmation

## Security & Authorization

- All chat endpoints require authorization
- Uses existing JWT authentication system
- Integrates with existing role-based access control
- Chat history is user-specific (when userId is provided)

## Future Enhancements

The system is designed for extensibility:

1. **Enhanced NLP**: More sophisticated intent recognition
2. **Complex Operations**: Multi-step operations (create role + assign permissions)
3. **User Context**: Better user context understanding
4. **Audit Trail**: Detailed logging of AI-initiated operations
5. **Real-time Updates**: WebSocket integration for real-time updates
6. **Voice Integration**: Voice-to-text and text-to-voice capabilities

## Testing

The system includes mock responses for testing without OpenAI API key:
- Demo mode activated when ApiKey is "demo-key"
- Provides realistic responses for all supported operations
- Suitable for development and demonstration purposes

## Dependencies Added

### Backend
- `OpenAI` (v2.1.0) - Official OpenAI .NET SDK

### Frontend
- No new dependencies (uses existing Material-UI components)

## Files Modified/Added

### New Files
- 13 new files for AI chat functionality
- Complete chat interface implementation
- API integration and navigation updates

### Modified Files
- `ServiceCollectionExtensions.cs` - DI registration
- `MainRoutes.jsx` - Route configuration
- `management.jsx` - Navigation menu
- `appsettings.json` - Configuration

All changes follow the existing project architecture and patterns, ensuring maintainability and consistency.