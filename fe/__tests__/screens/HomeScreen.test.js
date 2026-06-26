import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import HomeScreen from '../../src/screens/HomeScreen';
import * as reunionService from '../../src/services/reunionService';

jest.mock('../../src/services/reunionService');

const navigation = { navigate: jest.fn() };

const REUNIONS = [
  {
    id: 'r1',
    name: 'Smith Reunion',
    description: 'Annual gathering',
    startDate: '2024-07-04T00:00:00',
    endDate: '2024-07-06T00:00:00',
  },
  {
    id: 'r2',
    name: 'Johnson Family',
    description: null,
    startDate: null,
    endDate: null,
  },
];

describe('HomeScreen', () => {
  beforeEach(() => jest.clearAllMocks());

  it('shows reunion names after data loads', async () => {
    reunionService.getReunions.mockResolvedValue(REUNIONS);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText('Smith Reunion')).toBeTruthy();
    expect(await findByText('Johnson Family')).toBeTruthy();
  });

  it('shows "Date TBD" when startDate is absent', async () => {
    reunionService.getReunions.mockResolvedValue([REUNIONS[1]]);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText('Date TBD')).toBeTruthy();
  });

  it('shows date range when both dates are present', async () => {
    reunionService.getReunions.mockResolvedValue([REUNIONS[0]]);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    // Date text contains a dash separator
    const dateEl = await findByText(/–/);
    expect(dateEl).toBeTruthy();
  });

  it('shows description text', async () => {
    reunionService.getReunions.mockResolvedValue([REUNIONS[0]]);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText('Annual gathering')).toBeTruthy();
  });

  it('shows empty state when no reunions are returned', async () => {
    reunionService.getReunions.mockResolvedValue([]);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText('No reunions found.')).toBeTruthy();
  });

  it('shows an error message when fetch fails', async () => {
    reunionService.getReunions.mockRejectedValue(new Error('Network error'));
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText(/Failed to load reunions/)).toBeTruthy();
  });

  it('shows a Retry button on error', async () => {
    reunionService.getReunions.mockRejectedValue(new Error('Network error'));
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    expect(await findByText('Retry')).toBeTruthy();
  });

  it('retries the fetch when Retry is pressed', async () => {
    reunionService.getReunions
      .mockRejectedValueOnce(new Error('fail'))
      .mockResolvedValueOnce(REUNIONS);

    const { findByText } = render(<HomeScreen navigation={navigation} />);
    const retryBtn = await findByText('Retry');
    fireEvent.press(retryBtn);
    expect(await findByText('Smith Reunion')).toBeTruthy();
  });

  it('navigates to ReunionDetail when a reunion card is pressed', async () => {
    reunionService.getReunions.mockResolvedValue(REUNIONS);
    const { findByText } = render(<HomeScreen navigation={navigation} />);
    fireEvent.press(await findByText('Smith Reunion'));
    expect(navigation.navigate).toHaveBeenCalledWith('ReunionDetail', {
      reunion: REUNIONS[0],
    });
  });
});
