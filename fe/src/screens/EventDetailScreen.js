import React, { useEffect } from 'react';
import { View, Text, ScrollView, StyleSheet } from 'react-native';

const ATTIRE_LABELS = {
  0: 'Casual',
  1: 'Informal',
  2: 'Semi-Formal',
  3: 'Formal',
  4: 'Black Tie',
};

export default function EventDetailScreen({ route, navigation }) {
  const { event } = route.params;

  useEffect(() => {
    navigation.setOptions({ title: event.name });
  }, [event.name, navigation]);

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <View style={styles.card}>
        <Text style={styles.name}>{event.name}</Text>
        {event.details ? (
          <Text style={styles.details}>{event.details}</Text>
        ) : null}
      </View>

      <View style={styles.card}>
        <Text style={styles.sectionTitle}>Schedule</Text>
        <InfoRow label="Start" value={formatDateTime(event.startTime)} />
        <InfoRow label="End" value={formatDateTime(event.endTime)} />
        {event.attireType !== undefined && event.attireType !== null ? (
          <InfoRow
            label="Attire"
            value={ATTIRE_LABELS[event.attireType] ?? 'Casual'}
          />
        ) : null}
      </View>

      {event.location ? (
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Location</Text>
          {event.location.description ? (
            <Text style={styles.locationName}>{event.location.description}</Text>
          ) : null}
          {event.location.line1 ? (
            <Text style={styles.locationLine}>{event.location.line1}</Text>
          ) : null}
          {event.location.line2 ? (
            <Text style={styles.locationLine}>{event.location.line2}</Text>
          ) : null}
          <Text style={styles.locationLine}>
            {[event.location.city, event.location.state, event.location.zipCode]
              .filter(Boolean)
              .join(', ')}
          </Text>
        </View>
      ) : null}
    </ScrollView>
  );
}

function InfoRow({ label, value }) {
  return (
    <View style={styles.row}>
      <Text style={styles.label}>{label}</Text>
      <Text style={styles.value}>{value}</Text>
    </View>
  );
}

function formatDateTime(dt) {
  if (!dt) return 'TBD';
  return new Date(dt).toLocaleString(undefined, {
    weekday: 'short',
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
  card: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 16,
    marginBottom: 14,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  name: {
    fontSize: 22,
    fontWeight: '700',
    color: '#1A1A2E',
    marginBottom: 8,
  },
  details: {
    fontSize: 15,
    color: '#444',
    lineHeight: 22,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: '700',
    color: '#1A1A2E',
    marginBottom: 10,
  },
  row: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  label: {
    width: 60,
    fontSize: 14,
    fontWeight: '600',
    color: '#555',
  },
  value: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  locationName: {
    fontSize: 15,
    fontWeight: '600',
    color: '#333',
    marginBottom: 4,
  },
  locationLine: {
    fontSize: 14,
    color: '#555',
    marginBottom: 2,
  },
});
