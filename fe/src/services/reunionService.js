import { api } from '../api/client';

export const getReunions = () => api.get('/api/reunions');
export const getReunion = (id) => api.get(`/api/reunions/${id}`);
