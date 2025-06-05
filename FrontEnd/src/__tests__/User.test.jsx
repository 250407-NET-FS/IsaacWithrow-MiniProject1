import React from 'react';
import { render, screen } from '@testing-library/react';
import User from '../Pages/User/User';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../Pages/Context/AuthContext';
import { api } from '../Pages/Services/ApiService';

jest.mock('../Pages/Services/ApiService');

beforeEach(() => {
  api.get.mockResolvedValue({ data: [] });
});
test('renders NavBar in User page', () => {
  render(
    <AuthProvider>
        <BrowserRouter>
            <User />
        </BrowserRouter>
    </AuthProvider>
  );
  expect(screen.getByRole('banner')).toBeInTheDocument();
});