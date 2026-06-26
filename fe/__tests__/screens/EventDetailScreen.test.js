import React from 'react';
import { render, waitFor } from '@testing-library/react-native';
import EventDetailScreen from '../../src/screens/EventDetailScreen';

const navigation = { setOptions: jest.fn() };

const EVENT = {
  id: 'e1',
  name: 'Welcome BBQ',
  details: 'Cookout at the park near the lake',
  startTime: '2024-07-04T18:00:00',
  endTime: '2024-07-04T21:00:00',
  attireType: 0,
  location: {
    description: 'Piedmont Park',
    line1: '400 Park Dr NE',
    line2: null,
    city: 'Atlanta',
    state: 'GA',
    zipCode: '30306',
  },
};

describe('EventDetailScreen', () => {
  beforeEach(() => jest.clearAllMocks());

  const renderScreen = (event = EVENT) =>
    render(
      <EventDetailScreen
        route={{ params: { event } }}
        navigation={navigation}
      />,
    );

  it('renders the event name', () => {
    const { getByText } = renderScreen();
    expect(getByText('Welcome BBQ')).toBeTruthy();
  });

  it('renders the event details text', () => {
    const { getByText } = renderScreen();
    expect(getByText('Cookout at the park near the lake')).toBeTruthy();
  });

  it('renders the attire label for attireType 0 (Casual)', () => {
    const { getByText } = renderScreen();
    expect(getByText('Casual')).toBeTruthy();
  });

  it('renders "Black Tie" for attireType 4', () => {
    const { getByText } = renderScreen({ ...EVENT, attireType: 4 });
    expect(getByText('Black Tie')).toBeTruthy();
  });

  it('renders location description and street address', () => {
    const { getByText } = renderScreen();
    expect(getByText('Piedmont Park')).toBeTruthy();
    expect(getByText('400 Park Dr NE')).toBeTruthy();
  });

  it('renders city, state, and zip joined', () => {
    const { getByText } = renderScreen();
    expect(getByText('Atlanta, GA, 30306')).toBeTruthy();
  });

  it('shows TBD when startTime and endTime are absent', () => {
    const { getAllByText } = renderScreen({
      ...EVENT,
      startTime: null,
      endTime: null,
    });
    const tbds = getAllByText('TBD');
    expect(tbds.length).toBeGreaterThanOrEqual(2);
  });

  it('omits location section entirely when location is null', () => {
    const { queryByText } = renderScreen({ ...EVENT, location: null });
    expect(queryByText('Location')).toBeNull();
    expect(queryByText('Piedmont Park')).toBeNull();
  });

  it('omits details section when details is absent', () => {
    const { queryByText } = renderScreen({ ...EVENT, details: null });
    expect(queryByText('Cookout at the park near the lake')).toBeNull();
  });

  it('omits attire row when attireType is null', () => {
    const { queryByText } = renderScreen({ ...EVENT, attireType: null });
    expect(queryByText('Attire')).toBeNull();
  });

  it('sets the navigation title to the event name', async () => {
    renderScreen();
    await waitFor(() =>
      expect(navigation.setOptions).toHaveBeenCalledWith({
        title: 'Welcome BBQ',
      }),
    );
  });
});
