# Hướng dẫn AI Agent cho Quản lý Role và Permission

## Tổng quan

AI Agent được triển khai theo mô hình Clean Architecture để hỗ trợ quản lý role và permission thông qua ngôn ngữ tự nhiên. Hệ thống hỗ trợ cả tiếng Việt và tiếng Anh.

## Kiến trúc Hệ thống

### Backend Architecture (C# .NET Core)

```
src/
├── Domain/                           # Domain Layer
│   └── Entities/Chat/               # Chat entities (ChatMessage, ChatRoom)
├── Contract/Services/V1/AI/         # Contracts Layer
│   ├── Command.cs                   # AI Chat commands
│   ├── Query.cs                     # AI Chat queries  
│   └── Response.cs                  # AI Chat responses
├── Application/UserCases/V1/        # Application Layer
│   ├── Commands/AI/                 # Command handlers
│   │   └── ProcessChatMessageCommandHandler.cs
│   └── Queries/AI/                  # Query handlers
│       └── GetChatHistoryQueryHandler.cs
├── Infrastructure/AI/               # Infrastructure Layer
│   ├── OpenAiChatService.cs        # OpenAI integration
│   ├── OllamaChatService.cs         # Ollama integration (alternative)
│   └── IdentityManagementService.cs # Identity operations bridge
└── Presentation/APIs/AI/            # Presentation Layer
    └── ChatAgentApi.cs              # API endpoints
```

### Frontend Architecture (React/Material-UI)

```
frontend/src/
├── api/chat.js                      # Chat API integration
├── pages/chat/                      # Chat interface components
│   ├── ChatPage.jsx                 # AI Assistant chat page
│   └── UserChatPage.jsx             # User-to-user chat page
├── routes/MainRoutes.jsx            # Route configuration
└── menu-items/management.jsx        # Navigation menu
```

## Tính năng Chính

### 1. Lệnh Chat Hỗ trợ

#### Tiếng Việt:
- `"Tạo role Quản trị viên"` - Tạo role mới
- `"Tạo quyền USER_MANAGE"` - Tạo permission mới
- `"Gán quyền USER_READ vào role Admin"` - Gán permission vào role
- `"Gán user nguyen@email.com vào role Manager"` - Gán user vào role
- `"Danh sách tất cả roles"` - Liệt kê roles

#### Tiếng Anh:
- `"Create role Administrator"` - Tạo role mới
- `"Create permission USER_DELETE"` - Tạo permission mới
- `"Assign permission USER_WRITE to role Editor"` - Gán permission vào role
- `"Assign user jane@email.com to role Admin"` - Gán user vào role
- `"List all roles"` - Liệt kê roles

### 2. API Endpoints

```
POST /api/v1/chat/message    # Xử lý tin nhắn chat
GET  /api/v1/chat/history    # Lấy lịch sử chat
```

### 3. Frontend Routes

```
/admin/chat                  # AI Assistant chat interface
/admin/user-chat            # User-to-user chat interface
```

## Luồng Xử lý AI

1. **Nhận Input**: Người dùng gửi tin nhắn bằng ngôn ngữ tự nhiên
2. **AI Processing**: OpenAI xử lý tin nhắn để hiểu ý định
3. **Action Mapping**: Hệ thống map AI response thành operation cụ thể
4. **Execution**: Thực hiện CQRS command tương ứng qua MediatR
5. **Response**: Trả kết quả về cho người dùng

## Cấu hình

### Backend Configuration (appsettings.json)

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

### Dependency Injection

```csharp
// Infrastructure/DependencyInjection/Extensions/ServiceCollectionExtensions.cs
services.AddScoped<IAiChatService, OpenAiChatService>();
services.AddTransient<IIdentityManagementService, IdentityManagementService>();
```

## Clean Architecture Compliance

### 1. Separation of Concerns
- **Domain**: Chứa entities và business logic
- **Application**: Chứa use cases và business rules
- **Infrastructure**: Chứa external concerns (AI services, databases)
- **Presentation**: Chứa API controllers và UI logic

### 2. Dependency Inversion
- Application layer define interfaces
- Infrastructure layer implement interfaces
- Dependency injection thông qua DI container

### 3. CQRS Pattern
- Commands cho operations thay đổi state
- Queries cho operations đọc data
- Sử dụng MediatR cho message handling

## Conventions Tuân thủ

### 1. Naming Convention
- **Classes**: PascalCase (`ProcessChatMessageCommandHandler`)
- **Methods**: PascalCase (`ProcessChatMessageAsync`)
- **Variables**: camelCase (`chatId`, `aiResponse`)
- **Constants**: PascalCase (`BaseUrl`)

### 2. Error Handling
- Sử dụng Result pattern cho error handling
- Logging với Serilog
- Consistent error responses

### 3. Documentation
- XML documentation cho public APIs
- Comments bằng tiếng Việt cho business logic
- README files cho hướng dẫn sử dụng

## Security & Authorization

- Tất cả endpoints yêu cầu authentication
- Sử dụng JWT token system hiện có
- Role-based access control integration
- Chat history được filter theo user

## Testing

### Unit Tests
- Command/Query handlers có unit tests
- Mock dependencies để test isolation
- Test coverage cho happy path và error scenarios

### Integration Tests
- API endpoints testing
- Database integration testing
- AI service integration testing

## Performance Considerations

### 1. Caching
- Cache AI responses cho frequent queries
- Redis cache cho session management
- Memory cache cho lookup data

### 2. Pagination
- Chat history support pagination
- Configurable page sizes
- Efficient database queries

### 3. Async Operations
- Tất cả I/O operations đều async
- CancellationToken support
- Proper async/await usage

## Monitoring & Logging

### 1. Structured Logging
- Serilog với structured logging
- Log levels appropriate cho environment
- Correlation IDs cho request tracking

### 2. Metrics
- Response time tracking
- Error rate monitoring
- AI service usage metrics

## Extensibility

### 1. AI Provider Abstraction
- `IAiChatService` interface cho multiple providers
- Easy to add new AI providers (Ollama, Azure OpenAI, etc.)
- Configuration-based provider selection

### 2. Action Extension
- Easy to add new chat actions
- Plugin-based action processing
- Custom business logic injection

### 3. Localization
- Support multiple languages
- Resource files cho messages
- Cultural-specific formatting

## Best Practices Implemented

### 1. Clean Code
- Single Responsibility Principle
- Don't Repeat Yourself (DRY)
- Meaningful names và comments
- Small, focused methods

### 2. Error Handling
- Never throw exceptions từ API controllers
- Consistent error response format
- Proper logging cho debugging

### 3. Security
- Input validation và sanitization
- SQL injection prevention
- XSS protection
- Rate limiting considerations

## Troubleshooting

### Common Issues

1. **OpenAI API Key không hoạt động**
   - Kiểm tra API key trong appsettings.json
   - Verify OpenAI account có credit
   - Fallback về demo mode

2. **Chat history rỗng**
   - Kiểm tra database connection
   - Verify user permissions
   - Check pagination parameters

3. **AI không hiểu lệnh**
   - Cải thiện prompt engineering
   - Thêm more training examples
   - Enhance parsing logic

### Debug Mode

```json
{
  "OpenAI": {
    "ApiKey": "demo-key"  // Sử dụng mock responses
  }
}
```

## Future Enhancements

### 1. Enhanced NLP
- More sophisticated intent recognition
- Multi-step conversation support
- Context awareness

### 2. Real-time Features
- SignalR integration cho real-time updates
- Live notifications
- Collaborative chat sessions

### 3. Advanced Features
- Voice-to-text integration
- File attachment support
- Advanced role management workflows

## Kết luận

AI Agent implementation tuân thủ đầy đủ Clean Architecture principles và conventions của dự án. Code được viết theo best practices với documentation đầy đủ và ready for production use.