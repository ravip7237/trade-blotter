import { beforeEach, describe, expect, it, vi } from 'vitest';
import { createPinia, setActivePinia } from 'pinia';

import { useTradeStore } from '../stores/tradeStore';
import * as tradesService from '../services/tradesService';

describe('tradeStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it('loads trades and keeps the newest trade first', async () => {
    vi.spyOn(tradesService, 'getTrades').mockResolvedValue([
      {
        id: 1,
        symbol: 'AAPL',
        side: 'Buy',
        quantity: 10,
        price: 100,
        timestamp: '2026-01-01T10:00:00.000Z',
      },
      {
        id: 2,
        symbol: 'MSFT',
        side: 'Sell',
        quantity: 5,
        price: 200,
        timestamp: '2026-01-02T10:00:00.000Z',
      },
    ]);

    const store = useTradeStore();

    await store.fetchTrades();

    expect(store.loading).toBe(false);
    expect(store.error).toBe('');
    expect(store.sortedTrades[0].symbol).toBe('MSFT');
    expect(store.sortedTrades[1].symbol).toBe('AAPL');
  });

  it('surfaces an error when trade submission fails', async () => {
    vi.spyOn(tradesService, 'createTrade').mockRejectedValue(new Error('Unable to submit trade'));

    const store = useTradeStore();

    const result = await store.submitTrade({
      symbol: 'AAPL',
      side: 'Buy',
      quantity: 10,
      price: 100,
      timestamp: '2026-01-01T10:00:00.000Z',
    });

    expect(result).toBe(false);
    expect(store.error).toBe('Unable to submit trade');
    expect(store.submitting).toBe(false);
    expect(store.trades).toHaveLength(0);
  });
});