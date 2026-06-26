import { APIURL } from '@env';

if (!APIURL) {
  throw new Error('APIURL is not set. Add APIURL=<your-api-url> to fe/.env');
}

// Normalize trailing slash so paths like /api/foo always join cleanly.
const base = APIURL.replace(/\/+$/, '');

async function request(path, options = {}) {
  const url = `${base}${path}`;
  // Destructure headers out so ...rest does not overwrite the merged headers object.
  const { headers: extraHeaders, ...rest } = options;

  const response = await fetch(url, {
    ...rest,
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      ...extraHeaders,
    },
  });

  if (!response.ok) {
    throw new Error(`API error ${response.status}: ${response.statusText}`);
  }

  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

export const api = {
  get: (path, options) => request(path, options),
  post: (path, body, options) =>
    request(path, { ...options, method: 'POST', body: JSON.stringify(body) }),
  put: (path, body, options) =>
    request(path, { ...options, method: 'PUT', body: JSON.stringify(body) }),
};
