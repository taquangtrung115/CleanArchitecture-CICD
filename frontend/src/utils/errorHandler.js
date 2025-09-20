// src/utils/errorHandler.js
// Centralized error handling utility for API responses

/**
 * Processes error responses from API calls and extracts user-friendly error messages
 * @param {Object} error - The error object from axios or API response
 * @returns {Object} - Standardized error response with data, status, and error information
 */
export const handleApiError = (error) => {
  // Check if this is an axios error with response
  if (error.response) {
    const { status, data } = error.response;
    
    // Extract error information from the response
    const errorInfo = extractErrorInfo(data);
    
    return {
      data: null,
      status: status,
      error: errorInfo
    };
  }
  
  // Network error or request setup error
  return {
    data: null,
    status: 500,
    error: {
      title: 'Network Error',
      detail: error.message || 'Unable to connect to server',
      errors: null
    }
  };
};

/**
 * Extracts error information from API response data
 * @param {Object} data - The response data from API
 * @returns {Object} - Extracted error information
 */
const extractErrorInfo = (data) => {
  // Check if data follows the ProblemDetails format from backend
  if (data && typeof data === 'object') {
    // Standard ProblemDetails format
    if (data.title && data.detail) {
      return {
        title: data.title,
        type: data.type || 'Error',
        detail: data.detail,
        errors: data.errors || null
      };
    }
    
    // Legacy format or different error structure
    if (data.error) {
      if (typeof data.error === 'string') {
        return {
          title: 'Error',
          type: 'Error',
          detail: data.error,
          errors: null
        };
      }
      
      if (typeof data.error === 'object') {
        return {
          title: data.error.title || 'Error',
          type: data.error.type || 'Error',
          detail: data.error.message || data.error.detail || 'An error occurred',
          errors: data.error.errors || null
        };
      }
    }
    
    // If data is the error message itself
    if (typeof data === 'string') {
      return {
        title: 'Error',
        type: 'Error',
        detail: data,
        errors: null
      };
    }
  }
  
  // Fallback
  return {
    title: 'Error',
    type: 'Error',
    detail: 'An unexpected error occurred',
    errors: null
  };
};

/**
 * Formats validation errors for display in UI components
 * @param {Array} errors - Array of validation errors
 * @returns {Array} - Formatted error messages
 */
export const formatValidationErrors = (errors) => {
  if (!errors || !Array.isArray(errors)) {
    return [];
  }
  
  return errors.map(error => {
    if (typeof error === 'object' && error.code && error.message) {
      return `${error.code}: ${error.message}`;
    }
    
    if (typeof error === 'string') {
      return error;
    }
    
    return 'Invalid input';
  });
};

/**
 * Gets a user-friendly error message from error response
 * @param {Object} errorResponse - The error response object
 * @returns {string} - User-friendly error message
 */
export const getErrorMessage = (errorResponse) => {
  if (!errorResponse || !errorResponse.error) {
    return 'An unexpected error occurred';
  }
  
  const { error } = errorResponse;
  
  // If there are validation errors, format them
  if (error.errors && Array.isArray(error.errors) && error.errors.length > 0) {
    const validationMessages = formatValidationErrors(error.errors);
    return `${error.detail}\n${validationMessages.join('\n')}`;
  }
  
  // Return the main error detail
  return error.detail || error.title || 'An unexpected error occurred';
};

/**
 * Checks if the error is a validation error
 * @param {Object} errorResponse - The error response object
 * @returns {boolean} - True if it's a validation error
 */
export const isValidationError = (errorResponse) => {
  return errorResponse && 
         errorResponse.error && 
         (errorResponse.error.type === 'ValidationError' || 
          errorResponse.error.title === 'Validation Error' ||
          (errorResponse.error.errors && Array.isArray(errorResponse.error.errors)));
};

/**
 * Displays error notification in the UI (can be customized based on UI framework)
 * @param {Object} errorResponse - The error response object
 * @param {Function} notificationFunction - Function to show notifications (e.g., toast, alert)
 */
export const showErrorNotification = (errorResponse, notificationFunction) => {
  const message = getErrorMessage(errorResponse);
  const isValidation = isValidationError(errorResponse);
  
  if (notificationFunction) {
    notificationFunction({
      type: isValidation ? 'warning' : 'error',
      message: message,
      duration: isValidation ? 6000 : 4000 // Show validation errors longer
    });
  } else {
    // Fallback to console error
    console.error('API Error:', message);
  }
};