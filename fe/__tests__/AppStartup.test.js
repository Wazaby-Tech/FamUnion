/**
 * Startup integration test — verifies the first screen users see when
 * the app launches, exercising App → NavigationContainer → HomeScreen
 * as a single unit (navigation internals are mocked; service layer is mocked).
 */
import React from 'react';
import { ActivityIndicator } from 'react-native';
import { render } from '@testing-library/react-native';
import App from '../App';
import * as reunionService from '../src/services/reunionService';

// Replace the native navigation stack with a thin shim that renders only
// the initial (Home) screen, keeping the rest of the test focused on startup.
jest.mock('@react-navigation/native', () => ({
  NavigationContainer: ({ children }) => children,
}));

jest.mock('@react-navigation/native-stack', () => ({
  createNativeStackNavigator: () => ({
    Navigator: ({ children }) => children,
    Screen: ({ name, component: Component }) =>
      name === 'Home' ? (
        <Component
          navigation={{ navigate: jest.fn(), setOptions: jest.fn() }}
          route={{ params: {} }}
        />
      ) : null,
  }),
}));

jest.mock('react-native-gesture-handler', () => ({}));
jest.mock('../src/services/reunionService');

describe('App startup', () => {
  beforeEach(() => jest.clearAllMocks());

  it('shows a loading indicator while the initial data fetch is in flight', () => {
    // Promise that never resolves — simulates a slow network on first paint.
    reunionService.getReunions.mockReturnValue(new Promise(() => {}));
    const { UNSAFE_getByType } = render(<App />);
    expect(UNSAFE_getByType(ActivityIndicator)).toBeTruthy();
  });

  it('displays the reunion list once data loads', async () => {
    reunionService.getReunions.mockResolvedValue([
      { id: 'r1', name: 'Smith Family Reunion', description: 'Annual gathering', startDate: '2024-07-04T00:00:00', endDate: null },
      { id: 'r2', name: 'Johnson Family',        description: null,               startDate: null,                  endDate: null },
    ]);
    const { findByText } = render(<App />);
    expect(await findByText('Smith Family Reunion')).toBeTruthy();
    expect(await findByText('Johnson Family')).toBeTruthy();
  });

  it('shows the empty-state message when there are no reunions', async () => {
    reunionService.getReunions.mockResolvedValue([]);
    const { findByText } = render(<App />);
    expect(await findByText('No reunions found.')).toBeTruthy();
  });

  it('shows an error message when the API is unreachable at startup', async () => {
    reunionService.getReunions.mockRejectedValue(new Error('Network error'));
    const { findByText } = render(<App />);
    expect(await findByText(/Failed to load reunions/)).toBeTruthy();
  });

  it('offers a Retry button when startup fetch fails', async () => {
    reunionService.getReunions.mockRejectedValue(new Error('Network error'));
    const { findByText } = render(<App />);
    expect(await findByText('Retry')).toBeTruthy();
  });
});
