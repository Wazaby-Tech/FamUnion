jest.mock('../../src/api/client');

const { api } = require('../../src/api/client');
const {
  getEventsByReunion,
  getEvent,
} = require('../../src/services/eventService');

describe('eventService', () => {
  beforeEach(() => jest.clearAllMocks());

  it('getEventsByReunion calls GET /api/events/reunion/:id', async () => {
    const data = [{ id: 'e1', name: 'BBQ' }];
    api.get.mockResolvedValue(data);

    const result = await getEventsByReunion('r1');

    expect(api.get).toHaveBeenCalledWith('/api/events/reunion/r1');
    expect(result).toBe(data);
  });

  it('getEvent calls GET /api/events/:id', async () => {
    const data = { id: 'e1', name: 'BBQ' };
    api.get.mockResolvedValue(data);

    const result = await getEvent('e1');

    expect(api.get).toHaveBeenCalledWith('/api/events/e1');
    expect(result).toBe(data);
  });

  it('getEventsByReunion propagates API errors', async () => {
    api.get.mockRejectedValue(new Error('API error 500'));
    await expect(getEventsByReunion('r1')).rejects.toThrow('API error 500');
  });
});
