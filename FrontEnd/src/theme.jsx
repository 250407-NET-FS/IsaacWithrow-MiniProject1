// theme.js
import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: 'rgba(50, 50, 50, 0.77)',
    },
    secondary: {
      main: 'rgba(0, 0, 255, 0.77)',
    },
    background: {
      default: 'rgba(0, 0, 255, 0.77)',
    },
  },
  typography: {
    fontFamily: 'Roboto, sans-serif',
    button: {
      textTransform: 'none', // disable uppercase
    },
  },
  Button: {
    fontFamily: 'Roboto, sans-serif',
    button: {
      textTransform: 'none', // disable uppercase
    },
    sx: {
      color: 'rgba(255, 255, 255, 0.77)',
      '&:hover': {
      color: 'rgb(255, 255, 255)',
      },
    }
  },
});

export default theme;