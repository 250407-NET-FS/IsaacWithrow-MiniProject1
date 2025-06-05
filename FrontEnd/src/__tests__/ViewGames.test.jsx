import React from 'react';
import { render, screen } from '@testing-library/react';
import ViewGames from '../Pages/Games/ViewGames';
import { BrowserRouter } from 'react-router-dom';
import { AuthProvider } from '../Pages/Context/AuthContext';
import { api } from '../Pages/Services/ApiService';

jest.mock('../Pages/Services/ApiService');

beforeEach(() => {
  api.get.mockResolvedValue({ data: [] });
});
test('renders Browse Games heading', () => {
  render(
    <AuthProvider>
        <BrowserRouter>
            <ViewGames />
        </BrowserRouter>
    </AuthProvider>
  );
  expect(screen.getByText(/Browse Games/i)).toBeInTheDocument();
});