jest.mock('../../src/api/client');

const { api } = require('../../src/api/client');
const {
  getReunions,
  getReunion,
} = require('../../src/services/reunionService');

describe('reunionService', () => {
  beforeEach(() => jest.clearAllMocks());

  it('getReunions calls GET /api/reunions and returns the result', async () => {
    const data = [{ id: 'r1', name: 'Smith Reunion' }];
    api.get.mockResolvedValue(data);

    const result = await getReunions();

    expect(api.get).toHaveBeenCalledWith('/api/reunions');
    expect(result).toBe(data);
  });

  it('getReunion calls GET /api/reunions/:id and returns the result', async () => {
    const data = { id: 'r1', name: 'Smith Reunion' };
    api.get.mockResolvedValue(data);

    const result = await getReunion('r1');

    expect(api.get).toHaveBeenCalledWith('/api/reunions/r1');
    expect(result).toBe(data);
  });

  it('getReunion propagates API errors', async () => {
    api.get.mockRejectedValue(new Error('API error 404'));
    await expect(getReunion('missing')).rejects.toThrow('API error 404');
  });
});
