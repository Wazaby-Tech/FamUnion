import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import ReunionDetailScreen from '../../src/screens/ReunionDetailScreen';
import * as reunionService from '../../src/services/reunionService';
import * as eventService from '../../src/services/eventService';

jest.mock('../../src/services/reunionService');
jest.mock('../../src/services/eventService');

const navigation = { navigate: jest.fn(), setOptions: jest.fn() };

const REUNION = {
  id: 'r1',
  name: 'Smith Reunion',
  description: 'Annual gathering for the Smiths',
  startDate: '2024-07-04T00:00:00',
  endDate: '2024-07-06T00:00:00',
  location: { city: 'Atlanta', state: 'GA' },
};

const EVENTS = [
  { id: 'e1', name: 'Welcome BBQ', details: 'Cookout at the park', startTime: '2024-07-04T18:00:00' },
  { id: 'e2', name: 'Family Photo', details: null, startTime: null },
];

const route = { params: { reunion: REUNION } };

describe('ReunionDetailScreen', () => {
  beforeEach(() => jest.clearAllMocks());

  it('renders the reunion name and description', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue(EVENTS);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText('Smith Reunion')).toBeTruthy();
    expect(await findByText('Annual gathering for the Smiths')).toBeTruthy();
  });

  it('renders the events list', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue(EVENTS);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText('Welcome BBQ')).toBeTruthy();
    expect(await findByText('Family Photo')).toBeTruthy();
  });

  it('shows event count in the section header', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue(EVENTS);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText('Events (2)')).toBeTruthy();
  });

  it('shows "No events scheduled." when the events list is empty', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue([]);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText('No events scheduled.')).toBeTruthy();
  });

  it('sets the navigation title to the reunion name', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue([]);
    render(<ReunionDetailScreen route={route} navigation={navigation} />);
    await waitFor(() =>
      expect(navigation.setOptions).toHaveBeenCalledWith({ title: 'Smith Reunion' }),
    );
  });

  it('navigates to EventDetail when an event card is pressed', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue(EVENTS);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    fireEvent.press(await findByText('Welcome BBQ'));
    expect(navigation.navigate).toHaveBeenCalledWith('EventDetail', {
      event: EVENTS[0],
    });
  });

  it('shows location city and state', async () => {
    reunionService.getReunion.mockResolvedValue(REUNION);
    eventService.getEventsByReunion.mockResolvedValue([]);
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText('Atlanta, GA')).toBeTruthy();
  });

  it('shows an error banner when API calls fail', async () => {
    reunionService.getReunion.mockRejectedValue(new Error('Network error'));
    eventService.getEventsByReunion.mockRejectedValue(new Error('Network error'));
    const { findByText } = render(
      <ReunionDetailScreen route={route} navigation={navigation} />,
    );
    expect(await findByText(/Failed to load reunion details/)).toBeTruthy();
  });
});
