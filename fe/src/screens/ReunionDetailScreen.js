import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
} from 'react-native';
import { getReunion } from '../services/reunionService';
import { getEventsByReunion } from '../services/eventService';

export default function ReunionDetailScreen({ route, navigation }) {
  const { reunion: initialReunion } = route.params;
  const [reunion, setReunion] = useState(initialReunion);
  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    navigation.setOptions({ title: initialReunion.name });

    Promise.all([
      getReunion(initialReunion.id),
      getEventsByReunion(initialReunion.id),
    ])
      .then(([reunionData, eventsData]) => {
        if (reunionData) setReunion(reunionData);
        setEvents(eventsData || []);
      })
      .catch(() => setError('Failed to load reunion details.'))
      .finally(() => setLoading(false));
  }, [initialReunion.id, initialReunion.name, navigation]);

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#4A90E2" />
      </View>
    );
  }

  return (
    <ScrollView contentContainerStyle={styles.container}>
      {error ? <Text style={styles.errorText}>{error}</Text> : null}

      <View style={styles.headerCard}>
        <Text style={styles.name}>{reunion.name}</Text>
        {reunion.description ? (
          <Text style={styles.description}>{reunion.description}</Text>
        ) : null}
        <Text style={styles.dates}>
          {formatDateRange(reunion.startDate, reunion.endDate)}
        </Text>
        {reunion.location ? (
          <Text style={styles.location}>
            {formatLocation(reunion.location)}
          </Text>
        ) : null}
      </View>

      <Text style={styles.sectionHeader}>
        Events{events.length > 0 ? ` (${events.length})` : ''}
      </Text>

      {events.length === 0 ? (
        <Text style={styles.emptyText}>No events scheduled.</Text>
      ) : (
        events.map((event) => (
          <TouchableOpacity
            key={event.id}
            style={styles.eventCard}
            onPress={() => navigation.navigate('EventDetail', { event })}
            activeOpacity={0.7}>
            <Text style={styles.eventName}>{event.name}</Text>
            <Text style={styles.eventTime}>
              {formatDateTime(event.startTime)}
            </Text>
            {event.details ? (
              <Text style={styles.eventDetails} numberOfLines={1}>
                {event.details}
              </Text>
            ) : null}
          </TouchableOpacity>
        ))
      )}
    </ScrollView>
  );
}

function formatDateRange(start, end) {
  if (!start) return 'Date TBD';
  const s = new Date(start).toLocaleDateString();
  if (!end) return s;
  return `${s} – ${new Date(end).toLocaleDateString()}`;
}

function formatDateTime(dt) {
  if (!dt) return 'Time TBD';
  return new Date(dt).toLocaleString(undefined, {
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

function formatLocation(loc) {
  const parts = [loc.city, loc.state].filter(Boolean);
  return parts.join(', ') || loc.description;
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
  centered: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  headerCard: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 16,
    marginBottom: 20,
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
  description: {
    fontSize: 15,
    color: '#444',
    marginBottom: 10,
    lineHeight: 22,
  },
  dates: {
    fontSize: 14,
    color: '#4A90E2',
    fontWeight: '500',
    marginBottom: 4,
  },
  location: {
    fontSize: 14,
    color: '#666',
  },
  sectionHeader: {
    fontSize: 17,
    fontWeight: '700',
    color: '#1A1A2E',
    marginBottom: 12,
  },
  eventCard: {
    backgroundColor: '#fff',
    borderRadius: 10,
    padding: 14,
    marginBottom: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.08,
    shadowRadius: 3,
    elevation: 1,
  },
  eventName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1A1A2E',
    marginBottom: 4,
  },
  eventTime: {
    fontSize: 13,
    color: '#4A90E2',
    marginBottom: 2,
  },
  eventDetails: {
    fontSize: 13,
    color: '#777',
  },
  emptyText: {
    fontSize: 15,
    color: '#888',
  },
  errorText: {
    fontSize: 14,
    color: '#C0392B',
    marginBottom: 12,
  },
});
