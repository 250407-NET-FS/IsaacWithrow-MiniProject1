import React from 'react';
import { render, screen } from '@testing-library/react';
import Home from '../Pages/Home';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../Pages/Context/AuthContext';
import { api } from '../Pages/Services/ApiService';

jest.mock('../Pages/Services/ApiService');

beforeEach(() => {
  api.get.mockResolvedValue({ data: [] });
});
test('renders NavBar and sale banner', () => {
  render(
    <AuthProvider>
        <BrowserRouter>
            <Home />
        </BrowserRouter>
    </AuthProvider>
  );
  expect(screen.getByAltText(/Rainbow Six Siege/i)).toBeInTheDocument();
  expect(screen.getByText(/Sale!/i)).toBeInTheDocument();
});