import 'react-native';
import React from 'react';
import renderer from 'react-test-renderer';
import App from '../App';

jest.mock('@react-navigation/native', () => ({
  NavigationContainer: ({ children }) => children,
}));

jest.mock('@react-navigation/native-stack', () => ({
  createNativeStackNavigator: () => ({
    Navigator: ({ children }) => children,
    // Render null for each screen to avoid pulling in screen deps here
    Screen: () => null,
  }),
}));

jest.mock('react-native-gesture-handler', () => ({}));

it('renders without crashing', () => {
  renderer.create(<App />);
});
