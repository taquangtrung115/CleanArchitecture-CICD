// Simple notification utility
// You can integrate this with a notification library like react-toastify, notistack, etc.

export const showNotification = (type, message) => {
  // For now, we'll use console and alert - you can replace this with your preferred notification library
  console.log(`${type.toUpperCase()}: ${message}`);
  
  if (type === 'error') {
    alert(`Lỗi: ${message}`);
  } else if (type === 'success') {
    alert(`Thành công: ${message}`);
  } else {
    alert(message);
  }
};

// Alternative implementation with console styling
export const logNotification = (type, message) => {
  const styles = {
    error: 'color: red; font-weight: bold;',
    success: 'color: green; font-weight: bold;',
    warning: 'color: orange; font-weight: bold;',
    info: 'color: blue; font-weight: bold;'
  };
  
  console.log(`%c${type.toUpperCase()}: ${message}`, styles[type] || '');
};