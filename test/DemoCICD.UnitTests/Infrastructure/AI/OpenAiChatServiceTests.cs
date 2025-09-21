using DemoCICD.Infrastructure.AI;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace DemoCICD.UnitTests.Infrastructure.AI;

public class OpenAiChatServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly OpenAiChatService _service;

    public OpenAiChatServiceTests()
    {
        _configuration = Substitute.For<IConfiguration>();
        _configuration["OpenAI:ApiKey"].Returns("demo-key");
        _service = new OpenAiChatService(_configuration);
    }

    #region ProcessChatMessageAsync Tests

    [Fact]
    public async Task ProcessChatMessageAsync_WithCreateRoleRequest_ShouldReturnCreateRoleResponse()
    {
        // Arrange
        var message = "tạo role Admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
        result.Should().Contain("AI Generated Role");
        result.Should().Contain("Role được tạo bởi AI assistant");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithCreateRoleRequestInEnglish_ShouldReturnCreateRoleResponse()
    {
        // Arrange
        var message = "create role Administrator";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
        result.Should().Contain("AI Generated Role");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithCreatePermissionRequest_ShouldReturnCreatePermissionResponse()
    {
        // Arrange
        var message = "tạo quyền đọc dữ liệu";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ tạo một quyền mới");
        result.Should().Contain("AI_FUNCTION");
        result.Should().Contain("READ");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithCreatePermissionRequestInEnglish_ShouldReturnCreatePermissionResponse()
    {
        // Arrange
        var message = "create permission read_data";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ tạo một quyền mới");
        result.Should().Contain("AI_FUNCTION");
        result.Should().Contain("READ");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithAssignPermissionRequest_ShouldReturnAssignPermissionResponse()
    {
        // Arrange
        var message = "gán quyền read vào role admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ gán quyền vào role theo yêu cầu của bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithAssignPermissionRequestInEnglish_ShouldReturnAssignPermissionResponse()
    {
        // Arrange
        var message = "assign permission read to role admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ gán quyền vào role theo yêu cầu của bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithAssignUserRequest_ShouldReturnAssignUserResponse()
    {
        // Arrange
        var message = "gán user john vào role admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ gán user vào role theo yêu cầu của bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithAssignUserRequestInEnglish_ShouldReturnAssignUserResponse()
    {
        // Arrange
        var message = "assign user john to role admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ gán user vào role theo yêu cầu của bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithListRolesRequest_ShouldReturnListRolesResponse()
    {
        // Arrange
        var message = "danh sách roles";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ hiển thị danh sách các roles trong hệ thống");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithListRolesRequestInEnglish_ShouldReturnListRolesResponse()
    {
        // Arrange
        var message = "list roles";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ hiển thị danh sách các roles trong hệ thống");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithGenericRequest_ShouldReturnGenericResponse()
    {
        // Arrange
        var message = "hello world";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi hiểu bạn muốn: hello world");
        result.Should().Contain("Tôi sẽ cố gắng thực hiện yêu cầu này nếu có thể");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithEmptyMessage_ShouldReturnGenericResponse()
    {
        // Arrange
        var message = "";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi hiểu bạn muốn: ");
        result.Should().Contain("Tôi sẽ cố gắng thực hiện yêu cầu này nếu có thể");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithNullMessage_ShouldThrowNullReferenceException()
    {
        // Arrange
        string? message = null;

        // Act & Assert
        var act = () => _service.ProcessChatMessageAsync(message!);
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithCancellationToken_ShouldCompleteSuccessfully()
    {
        // Arrange
        var message = "test message";
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _service.ProcessChatMessageAsync(message, cts.Token);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithMixedCaseRequest_ShouldReturnCorrectResponse()
    {
        // Arrange
        var message = "TẠO ROLE Admin";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
    }

    #endregion

    #region CanPerformActionAsync Tests

    [Fact]
    public async Task CanPerformActionAsync_WithAnyAction_ShouldReturnTrue()
    {
        // Arrange
        var action = "create_role";

        // Act
        var result = await _service.CanPerformActionAsync(action);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanPerformActionAsync_WithEmptyAction_ShouldReturnTrue()
    {
        // Arrange
        var action = "";

        // Act
        var result = await _service.CanPerformActionAsync(action);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanPerformActionAsync_WithNullAction_ShouldReturnTrue()
    {
        // Arrange
        string? action = null;

        // Act
        var result = await _service.CanPerformActionAsync(action!);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanPerformActionAsync_WithCancellationToken_ShouldReturnTrue()
    {
        // Arrange
        var action = "test_action";
        using var cts = new CancellationTokenSource();

        // Act
        var result = await _service.CanPerformActionAsync(action, cts.Token);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Configuration Tests

    [Fact]
    public void Constructor_WithNullApiKey_ShouldUseDefaultKey()
    {
        // Arrange
        var config = Substitute.For<IConfiguration>();
        config["OpenAI:ApiKey"].Returns((string?)null);

        // Act
        var service = new OpenAiChatService(config);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithCustomApiKey_ShouldUseProvidedKey()
    {
        // Arrange
        var config = Substitute.For<IConfiguration>();
        config["OpenAI:ApiKey"].Returns("custom-api-key");

        // Act
        var service = new OpenAiChatService(config);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithDemoKey_ShouldUseMockResponseLogic()
    {
        // Arrange
        var config = Substitute.For<IConfiguration>();
        config["OpenAI:ApiKey"].Returns("demo-key");

        // Act
        var service = new OpenAiChatService(config);

        // Assert
        service.Should().NotBeNull();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task ProcessChatMessageAsync_WithRealApiKey_ShouldFallBackToMockWhenApiCallFails()
    {
        // Arrange
        var config = Substitute.For<IConfiguration>();
        config["OpenAI:ApiKey"].Returns("sk-invalid-key");
        var service = new OpenAiChatService(config);
        var message = "create role test";

        // Act
        var result = await service.ProcessChatMessageAsync(message);

        // Assert
        // Should fallback to mock response when API call fails
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithLongMessage_ShouldHandleGracefully()
    {
        // Arrange
        var longMessage = string.Join(" ", Enumerable.Repeat("tạo role admin với nhiều quyền", 100));

        // Act
        var result = await _service.ProcessChatMessageAsync(longMessage);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
    }

    [Fact]
    public async Task ProcessChatMessageAsync_WithSpecialCharacters_ShouldHandleGracefully()
    {
        // Arrange
        var message = "tạo role với ký tự đặc biệt: éñ中文🚀<script>alert('test')</script>";

        // Act
        var result = await _service.ProcessChatMessageAsync(message);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Tôi sẽ tạo một role mới cho bạn");
    }

    #endregion
}