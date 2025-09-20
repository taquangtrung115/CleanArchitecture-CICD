# Error Handling Implementation Guide

## Overview

This implementation provides comprehensive error handling for validation errors and other API errors across both backend and frontend of the CleanArchitecture-CICD application.

## Backend Changes

### 1. ExceptionHandlingMiddleware
- **File**: `src/DemoCICD.API/Middleware/ExceptionHandlingMiddleware.cs`
- **Changes**: 
  - Fixed the middleware to properly include validation errors in the response
  - Added support for both `Application.Exceptions.ValidationException` and `FluentValidation.ValidationException`
  - Standardized error response format to match ProblemDetails specification

### 2. API Controllers Standardization
- **Files**: 
  - `src/DemoCICD.Presentation/APIs/MotoGP/RiderApi.cs`
  - `src/DemoCICD.Presentation/APIs/MotoGP/TeamApi.cs`
- **Changes**:
  - Updated to inherit from `ApiEndpoint` base class
  - Removed duplicated error handling code
  - Fixed typos ("Bab Request" → "Bad Request")

### Error Response Format
All API errors now return the following standardized format:
```json
{
  "type": "ValidationError",
  "title": "Validation Error", 
  "status": 400,
  "detail": "A validation problem occurred.",
  "errors": [
    {
      "code": "Password",
      "message": "Password must be at least 6 characters"
    }
  ]
}
```

## Frontend Changes

### 1. Centralized Error Handler
- **File**: `frontend/src/utils/errorHandler.js`
- **Features**:
  - `handleApiError(error)`: Processes axios errors and returns standardized format
  - `formatValidationErrors(errors)`: Formats validation errors for display
  - `getErrorMessage(errorResponse)`: Gets user-friendly error messages
  - `isValidationError(errorResponse)`: Detects validation errors
  - `showErrorNotification(errorResponse, notificationFunction)`: Displays errors in UI

### 2. Updated API Files
- **Files Updated**:
  - `frontend/src/api/auth.js`
  - `frontend/src/api/permission.js`
  - `frontend/src/api/user.js`
  - `frontend/src/api/role.js`
- **Changes**: All API functions now use the centralized `handleApiError` utility

### 3. UI Components
- **Files Updated**:
  - `frontend/src/pages/LoginPage.jsx`
  - `frontend/src/pages/RegisterPage.jsx`
- **Features**:
  - Separate display areas for general errors vs validation errors
  - Proper styling to distinguish error types
  - Clear error messages when user starts typing

## Usage Examples

### In API Functions
```javascript
import { handleApiError } from '../utils/errorHandler';

export const createUser = async (userData) => {
  try {
    const response = await axiosInstance.post('/api/v1/users', userData);
    return { data: response.data, status: response.status, error: null };
  } catch (error) {
    return handleApiError(error);
  }
};
```

### In React Components
```javascript
import { getErrorMessage, formatValidationErrors, isValidationError } from '../utils/errorHandler';

const MyComponent = () => {
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState([]);

  const handleSubmit = async (formData) => {
    const result = await createUser(formData);
    
    if (result.error) {
      if (isValidationError(result)) {
        setValidationErrors(formatValidationErrors(result.error.errors));
        setError(result.error.detail);
      } else {
        setError(getErrorMessage(result));
      }
    } else {
      // Success handling
    }
  };

  return (
    <div>
      {/* General error display */}
      {error && <div className="error-display">{error}</div>}
      
      {/* Validation errors display */}
      {validationErrors.length > 0 && (
        <div className="validation-errors">
          <strong>Validation Errors:</strong>
          <ul>
            {validationErrors.map((err, index) => (
              <li key={index}>{err}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};
```

## Testing

A demo HTML file (`error-handling-demo.html`) is included to demonstrate the error handling functionality with different types of API responses:

1. **Validation Errors**: Shows how password validation errors are displayed
2. **General Errors**: Shows how 404 Not Found errors are displayed  
3. **Network Errors**: Shows how connection issues are handled
4. **Success Responses**: Shows proper success handling

## Benefits

1. **Consistency**: All APIs now use the same error format and handling logic
2. **User Experience**: Clear, actionable error messages for users
3. **Developer Experience**: Centralized error handling reduces code duplication
4. **Maintainability**: Easy to update error handling behavior across the entire application
5. **Type Safety**: Proper error detection and handling for different error types

## Error Types Handled

- **Validation Errors**: Field-specific validation failures (required fields, format validation, etc.)
- **Business Logic Errors**: Domain-specific errors from business rules
- **Authentication/Authorization Errors**: Login failures, permission issues
- **Network Errors**: Connection timeouts, server unavailable
- **Server Errors**: Internal server errors, unexpected exceptions