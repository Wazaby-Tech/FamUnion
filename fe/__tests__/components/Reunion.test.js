import React from 'react';
import { render } from '@testing-library/react-native';
import Reunion from '../../components/Reunions/Reunion/Reunion';

describe('Reunion component', () => {
  it('renders the reunion name with event count', () => {
    const { getByText } = render(
      <Reunion reunion={{ id: '1', name: 'Smith Reunion', events: [] }} />,
    );
    expect(getByText('Smith Reunion (0)')).toBeTruthy();
  });

  it('shows the correct event count', () => {
    const events = [{ id: 'e1' }, { id: 'e2' }, { id: 'e3' }];
    const { getByText } = render(
      <Reunion reunion={{ id: '1', name: 'Test', events }} />,
    );
    expect(getByText('Test (3)')).toBeTruthy();
  });

  it('handles a null events array gracefully', () => {
    const { getByText } = render(
      <Reunion reunion={{ id: '1', name: 'Test', events: null }} />,
    );
    expect(getByText('Test (0)')).toBeTruthy();
  });
});
