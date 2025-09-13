# Identity Management API Documentation

This document demonstrates the comprehensive identity management system implemented for the Clean Architecture CICD project.

## System Management Features

### 1. User Registration (Sign-Up)
**Endpoint:** `POST /api/carter/v1/auth/register`

**Request Body:**
```json
{
  "userName": "john.doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123",
  "firstName": "John",
  "lastName": "Doe",
  "dayOfBirth": "1990-01-15T00:00:00Z"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "userId": "12345678-1234-1234-1234-123456789abc",
    "userName": "john.doe",
    "email": "john.doe@example.com"
  }
}
```

### 2. User Login (Sign-In)
**Endpoint:** `POST /api/carter/v1/auth/login`

**Request Body:**
```json
{
  "userName": "john.doe",
  "password": "SecurePassword123"
}
```

### 3. User Logout (Sign-Out)
**Endpoint:** `POST /api/carter/v1/auth/logout`

**Request Body:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

## User Management Features

### 1. Create User (Admin)
**Endpoint:** `POST /api/carter/v1/users`

**Request Body:**
```json
{
  "userName": "jane.smith",
  "email": "jane.smith@example.com",
  "password": "SecurePassword123",
  "firstName": "Jane",
  "lastName": "Smith",
  "dayOfBirth": "1985-03-20T00:00:00Z",
  "isDirector": false,
  "isHeadOfDepartment": true,
  "managerId": "87654321-4321-4321-4321-cba987654321",
  "positionId": "11111111-1111-1111-1111-111111111111"
}
```

### 2. Get Users (with pagination and search)
**Endpoint:** `GET /api/carter/v1/users?page=1&pageSize=10&searchTerm=john`

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "users": [
      {
        "userId": "12345678-1234-1234-1234-123456789abc",
        "userName": "john.doe",
        "email": "john.doe@example.com",
        "fullName": "John Doe",
        "isLocked": false
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10
  }
}
```

### 3. Get User by ID
**Endpoint:** `GET /api/carter/v1/users/{userId}`

### 4. Update User
**Endpoint:** `PUT /api/carter/v1/users/{userId}`

### 5. Delete User
**Endpoint:** `DELETE /api/carter/v1/users/{userId}`

### 6. Change Password
**Endpoint:** `POST /api/carter/v1/users/{userId}/change-password`

**Request Body:**
```json
{
  "userId": "12345678-1234-1234-1234-123456789abc",
  "currentPassword": "OldPassword123",
  "newPassword": "NewPassword456"
}
```

### 7. Reset Password (Admin only)
**Endpoint:** `POST /api/carter/v1/users/{userId}/reset-password`

**Request Body:**
```json
{
  "userId": "12345678-1234-1234-1234-123456789abc",
  "newPassword": "ResetPassword789"
}
```

### 8. Lock User
**Endpoint:** `POST /api/carter/v1/users/{userId}/lock`

### 9. Unlock User
**Endpoint:** `POST /api/carter/v1/users/{userId}/unlock`

### 10. Assign User to Role
**Endpoint:** `POST /api/carter/v1/users/{userId}/roles/{roleId}`

### 11. Remove User from Role
**Endpoint:** `DELETE /api/carter/v1/users/{userId}/roles/{roleId}`

### 12. Get User Roles
**Endpoint:** `GET /api/carter/v1/users/{userId}/roles`

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "userId": "12345678-1234-1234-1234-123456789abc",
    "roles": [
      {
        "roleId": "22222222-2222-2222-2222-222222222222",
        "name": "Admin",
        "roleCode": "ADMIN",
        "description": "Administrator role with full permissions"
      }
    ]
  }
}
```

## Role Management Features

### 1. Create Role
**Endpoint:** `POST /api/carter/v1/roles`

**Request Body:**
```json
{
  "name": "Manager",
  "description": "Department Manager role",
  "roleCode": "MANAGER"
}
```

### 2. Get Roles (with pagination and search)
**Endpoint:** `GET /api/carter/v1/roles?page=1&pageSize=10&searchTerm=admin`

### 3. Get Role by ID
**Endpoint:** `GET /api/carter/v1/roles/{roleId}`

### 4. Update Role
**Endpoint:** `PUT /api/carter/v1/roles/{roleId}`

### 5. Delete Role
**Endpoint:** `DELETE /api/carter/v1/roles/{roleId}`

### 6. Get Users in Role
**Endpoint:** `GET /api/carter/v1/roles/{roleId}/users`

### 7. Get Role Permissions
**Endpoint:** `GET /api/carter/v1/roles/{roleId}/permissions`

### 8. Grant Permission to Role
**Endpoint:** `POST /api/carter/v1/roles/{roleId}/permissions`

**Request Body:**
```json
{
  "roleId": "22222222-2222-2222-2222-222222222222",
  "functionId": "USER_MANAGEMENT",
  "actionId": "CREATE"
}
```

### 9. Revoke Permission from Role
**Endpoint:** `DELETE /api/carter/v1/roles/{roleId}/permissions`

**Request Body:**
```json
{
  "roleId": "22222222-2222-2222-2222-222222222222",
  "functionId": "USER_MANAGEMENT",
  "actionId": "DELETE"
}
```

## Permission Management Features

### 1. Create Permission
**Endpoint:** `POST /api/carter/v1/permissions`

**Request Body:**
```json
{
  "roleId": "22222222-2222-2222-2222-222222222222",
  "functionId": "PRODUCT_MANAGEMENT",
  "actionId": "READ"
}
```

### 2. Get Permissions (with pagination)
**Endpoint:** `GET /api/carter/v1/permissions?page=1&pageSize=10`

### 3. Get Permission by ID
**Endpoint:** `GET /api/carter/v1/permissions/{roleId}/{functionId}/{actionId}`

### 4. Delete Permission
**Endpoint:** `DELETE /api/carter/v1/permissions/{roleId}/{functionId}/{actionId}`

## Key Features Implemented

### ✅ Clean Architecture
- **Domain Layer**: Identity entities (AppUser, AppRole, Permission, Action, Function)
- **Application Layer**: CQRS with MediatR for commands and queries
- **Infrastructure Layer**: ASP.NET Core Identity services
- **Presentation Layer**: Carter minimal APIs with proper routing

### ✅ Validation
- FluentValidation for input validation
- Comprehensive error handling with Result pattern

### ✅ Security
- ASP.NET Core Identity for user management
- JWT tokens for authentication
- Authorization requirements on all management endpoints

### ✅ Database Integration
- Entity Framework Core with Identity
- Support for role-based permissions
- Proper navigation properties and relationships

### ✅ API Design
- RESTful endpoints following consistent patterns
- Proper HTTP status codes
- Structured JSON responses
- Pagination support for list operations

## Entity Relationships

The system uses the following entity structure:

```
AppUser (ASP.NET Core Identity)
├── UserRoles (Many-to-Many with AppRole)
├── Personal Information (FirstName, LastName, etc.)
└── Management Structure (ManagerId, IsDirector, etc.)

AppRole (ASP.NET Core Identity)
├── UserRoles (Many-to-Many with AppUser)
├── Permissions (One-to-Many)
└── Role Information (RoleCode, Description)

Permission
├── RoleId (Foreign Key to AppRole)
├── FunctionId (Foreign Key to Function)
└── ActionId (Foreign Key to Action)

Function
├── Permissions (One-to-Many)
└── Function Details (Name, Url, etc.)

Action
├── Permissions (One-to-Many)
└── Action Details (Name, etc.)
```

This implementation provides a complete identity management solution that meets all the requirements specified in the problem statement.