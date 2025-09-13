import React from 'react';
import ProductManager from './components/ProductManager';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';

const theme = createTheme();

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <div style={{ padding: 32 }}>
        <h1>Welcome to React + TypeScript Frontend!</h1>
        <ProductManager />
      </div>
    </ThemeProvider>
  );
}

export default App;
