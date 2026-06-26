import { api } from '../api/client';

export const getEventsByReunion = (reunionId) =>
  api.get(`/api/events/reunion/${reunionId}`);

export const getEvent = (id) => api.get(`/api/events/${id}`);
