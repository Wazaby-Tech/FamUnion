import React from 'react';
import { View, Text } from 'react-native';
import Event from './Event/Event';

const Events = ({ events }) => {
  if (!events || events.length === 0) {
    return <Text>No events scheduled.</Text>;
  }
  return (
    <View>
      {events.map((e) => (
        <Event key={e.id} event={e} />
      ))}
    </View>
  );
};

export default Events;
