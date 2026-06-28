import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

import { createTrade, getPositions, getTrades, type PositionResponse, type TradePayload, type TradeResponse } from '../services/tradesService';

export const useTradeStore = defineStore('trade', () => {
  const trades = ref<TradeResponse[]>([]);
  const positions = ref<PositionResponse[]>([]);
  const loading = ref(false);
  const submitting = ref(false);
  const error = ref('');

  const sortedTrades = computed(() => [...trades.value].sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()));

  async function fetchTrades() {
    loading.value = true;
    error.value = '';

    try {
      trades.value = await getTrades();
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to load trades';
    } finally {
      loading.value = false;
    }
  }

  async function fetchPositions() {
    try {
      positions.value = await getPositions();
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to load positions';
    }
  }

  async function submitTrade(payload: TradePayload) {
    submitting.value = true;
    error.value = '';

    try {
      const createdTrade = await createTrade(payload);
      trades.value = [createdTrade, ...trades.value];
      await fetchPositions();
      return true;
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Unable to submit trade';
      return false;
    } finally {
      submitting.value = false;
    }
  }

  return { trades, positions, sortedTrades, loading, submitting, error, fetchTrades, fetchPositions, submitTrade };
});