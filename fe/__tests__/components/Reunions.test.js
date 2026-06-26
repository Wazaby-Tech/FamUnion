import React from 'react';
import { render } from '@testing-library/react-native';
import Reunions from '../../components/Reunions/Reunions';

describe('Reunions component', () => {
  it('shows "No reunions found" when the list is empty', () => {
    const { getByText } = render(<Reunions reunions={[]} />);
    expect(getByText('No reunions found')).toBeTruthy();
  });

  it('renders each reunion name with its event count', () => {
    const reunions = [
      { id: '1', name: 'Smith Reunion', events: [] },
      { id: '2', name: 'Johnson Family', events: [{ id: 'e1' }] },
    ];
    const { getByText } = render(<Reunions reunions={reunions} />);
    expect(getByText('Smith Reunion (0)')).toBeTruthy();
    expect(getByText('Johnson Family (1)')).toBeTruthy();
  });
});
