import React from 'react';
import { render, screen } from '@testing-library/react';
import NavBar from '../Pages/Shared/NavBar';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../Pages/Context/AuthContext';
import { api } from '../Pages/Services/ApiService';

jest.mock('../Pages/Services/ApiService');

beforeEach(() => {
  api.get.mockResolvedValue({ data: [] });
});
test('renders Home button', () => {
  render(
  <AuthProvider>
    <BrowserRouter>
        <NavBar />
    </BrowserRouter>
  </AuthProvider>
    );
  expect(screen.getByText(/Home/i)).toBeInTheDocument();
});