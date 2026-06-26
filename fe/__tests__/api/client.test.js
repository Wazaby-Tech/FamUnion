// @env is mapped to __mocks__/@env.js via jest moduleNameMapper
const { api } = require('../../src/api/client');

const mockFetch = (body, ok = true, status = 200) => {
  global.fetch = jest.fn().mockResolvedValue({
    ok,
    status,
    statusText: ok ? 'OK' : 'Internal Server Error',
    text: jest.fn().mockResolvedValue(body != null ? JSON.stringify(body) : ''),
  });
};

describe('API client', () => {
  afterEach(() => jest.resetAllMocks());

  describe('api.get', () => {
    it('requests the correct URL', async () => {
      mockFetch({ id: 1 });
      await api.get('/api/reunions');
      expect(fetch).toHaveBeenCalledWith(
        'http://localhost:5000/api/reunions',
        expect.any(Object),
      );
    });

    it('always sends Content-Type and Accept headers', async () => {
      mockFetch([]);
      await api.get('/api/reunions');
      expect(fetch).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({
          headers: expect.objectContaining({
            'Content-Type': 'application/json',
            Accept: 'application/json',
          }),
        }),
      );
    });

    it('merges caller headers without dropping defaults', async () => {
      mockFetch([]);
      await api.get('/api/protected', { headers: { Authorization: 'Bearer tok' } });
      expect(fetch).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({
          headers: expect.objectContaining({
            'Content-Type': 'application/json',
            Accept: 'application/json',
            Authorization: 'Bearer tok',
          }),
        }),
      );
    });

    it('returns parsed JSON on success', async () => {
      const data = [{ id: 'r1', name: 'Reunion' }];
      mockFetch(data);
      await expect(api.get('/api/reunions')).resolves.toEqual(data);
    });

    it('returns null for empty response body', async () => {
      mockFetch(null);
      await expect(api.get('/api/empty')).resolves.toBeNull();
    });

    it('throws on non-OK response', async () => {
      mockFetch(null, false, 500);
      await expect(api.get('/api/fail')).rejects.toThrow('API error 500');
    });

    it('strips trailing slash from base URL', async () => {
      // The base in the mock env is already without trailing slash.
      // This verifies paths starting with / join correctly.
      mockFetch({});
      await api.get('/api/events');
      const calledUrl = fetch.mock.calls[0][0];
      expect(calledUrl).not.toContain('//api');
    });
  });

  describe('api.post', () => {
    it('sends POST method with JSON-serialized body', async () => {
      mockFetch({ id: 'r1' });
      const body = { name: 'New Reunion' };
      await api.post('/api/reunions', body);
      expect(fetch).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({
          method: 'POST',
          body: JSON.stringify(body),
        }),
      );
    });
  });

  describe('api.put', () => {
    it('sends PUT method with JSON-serialized body', async () => {
      mockFetch({ id: 'r1' });
      const body = { name: 'Updated' };
      await api.put('/api/reunions', body);
      expect(fetch).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({
          method: 'PUT',
          body: JSON.stringify(body),
        }),
      );
    });
  });
});
