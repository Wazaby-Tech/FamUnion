import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const Event = ({ event }) => (
  <View style={styles.container}>
    <Text style={styles.name}>{event.name}</Text>
    {event.startTime ? (
      <Text style={styles.time}>
        {new Date(event.startTime).toLocaleString()}
      </Text>
    ) : null}
  </View>
);

const styles = StyleSheet.create({
  container: {
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#eee',
  },
  name: {
    fontSize: 15,
    fontWeight: '600',
    color: '#1A1A2E',
  },
  time: {
    fontSize: 13,
    color: '#4A90E2',
    marginTop: 2,
  },
});

export default Event;
