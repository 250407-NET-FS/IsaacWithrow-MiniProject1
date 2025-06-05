import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import CreateGame from '../Pages/Games/CreateGame';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../Pages/Context/AuthContext';
import { api } from '../Pages/Services/ApiService';

jest.mock('../Pages/Services/ApiService');

beforeEach(() => {
  api.get.mockResolvedValue({ data: [] });
});
test('renders CreateGame form', () => {
  render(
    <AuthProvider>
        <BrowserRouter>
            <CreateGame />
        </BrowserRouter>
    </AuthProvider>
  );
  expect(screen.getByText(/Title/i)).toBeInTheDocument();
  expect(screen.getByText(/Publisher/i)).toBeInTheDocument();
  expect(screen.getByText(/Price/i)).toBeInTheDocument();
  expect(screen.getByText(/Image/i)).toBeInTheDocument();
});