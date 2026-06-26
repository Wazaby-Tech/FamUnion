import React from 'react';
import { render } from '@testing-library/react-native';
import Events from '../../components/Events/Events';

describe('Events component', () => {
  it('shows empty-state message when events array is empty', () => {
    const { getByText } = render(<Events events={[]} />);
    expect(getByText('No events scheduled.')).toBeTruthy();
  });

  it('shows empty-state message when events prop is null', () => {
    const { getByText } = render(<Events events={null} />);
    expect(getByText('No events scheduled.')).toBeTruthy();
  });

  it('renders each event name', () => {
    const events = [
      { id: '1', name: 'BBQ Night', startTime: null },
      { id: '2', name: 'Family Photo', startTime: null },
    ];
    const { getByText } = render(<Events events={events} />);
    expect(getByText('BBQ Night')).toBeTruthy();
    expect(getByText('Family Photo')).toBeTruthy();
  });
});
