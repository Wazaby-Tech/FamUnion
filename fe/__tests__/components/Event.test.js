import React from 'react';
import { render } from '@testing-library/react-native';
import Event from '../../components/Events/Event/Event';

describe('Event component', () => {
  it('renders the event name', () => {
    const { getByText } = render(
      <Event event={{ id: '1', name: 'BBQ Night', startTime: null }} />,
    );
    expect(getByText('BBQ Night')).toBeTruthy();
  });

  it('does not render a time when startTime is absent', () => {
    const { queryByText } = render(
      <Event event={{ id: '1', name: 'BBQ Night', startTime: null }} />,
    );
    // No date/time string should appear beyond the event name
    expect(queryByText(/\d{1,2}\/\d{1,2}\/\d{4}/)).toBeNull();
  });

  it('renders a time string when startTime is present', () => {
    const { getByText } = render(
      <Event
        event={{ id: '1', name: 'BBQ Night', startTime: '2024-07-04T18:00:00' }}
      />,
    );
    // Formatted date text should be present (exact format depends on locale)
    expect(getByText(/2024|7\/4|Jul/i)).toBeTruthy();
  });
});
