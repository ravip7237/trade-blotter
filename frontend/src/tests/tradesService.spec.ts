import { beforeEach, describe, expect, it, vi } from 'vitest';

import { createTrade, getTrades } from '../services/tradesService';

describe('tradesService', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it('fetches trades on success', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: vi.fn().mockResolvedValue([
        {
          id: 1,
          symbol: 'AAPL',
          side: 'Buy',
          quantity: 10,
          price: 100,
          timestamp: '2026-01-01T10:00:00.000Z',
        },
      ]),
    }));

    const trades = await getTrades();

    expect(trades).toHaveLength(1);
    expect(trades[0].symbol).toBe('AAPL');
  });

  it('returns the server error when trade creation fails', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: false,
      headers: {
        get: () => 'application/problem+json',
      },
      json: vi.fn().mockResolvedValue({
        title: 'Validation failed',
        detail: 'Quantity must be greater than zero.',
      }),
      text: vi.fn().mockResolvedValue(''),
    }));

    await expect(
      createTrade({
        symbol: 'AAPL',
        side: 'Buy',
        quantity: 0,
        price: 100,
        timestamp: '2026-01-01T10:00:00.000Z',
      }),
    ).rejects.toThrow('Quantity must be greater than zero.');
  });
});