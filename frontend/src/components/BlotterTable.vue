<script setup lang="ts">
import { computed, ref } from 'vue';

import type { Trade } from '../types/tradeTypes';

const props = withDefaults(defineProps<{ trades?: Trade[]; loading?: boolean }>(), {
  trades: () => [],
  loading: false,
});

type SortKey = 'timestamp' | 'symbol' | 'side' | 'quantity' | 'price' | 'notionalValue';

const sortKey = ref<SortKey>('timestamp');
const sortDirection = ref<'asc' | 'desc'>('desc');

const sortedTrades = computed(() => {
  const rows = [...props.trades];
  rows.sort((a, b) => {
    const modifier = sortDirection.value === 'asc' ? 1 : -1;
    const aValue = sortKey.value === 'notionalValue' ? a.quantity * a.price : a[sortKey.value];
    const bValue = sortKey.value === 'notionalValue' ? b.quantity * b.price : b[sortKey.value];

    if (typeof aValue === 'number' && typeof bValue === 'number') {
      return (aValue - bValue) * modifier;
    }

    return String(aValue).localeCompare(String(bValue)) * modifier;
  });

  return rows;
});

const sortIndicators: Record<SortKey, string> = {
  timestamp: 'Timestamp',
  symbol: 'Symbol',
  side: 'Side',
  quantity: 'Quantity',
  price: 'Price',
  notionalValue: 'Notional',
};

function setSort(key: SortKey) {
  if (sortKey.value === key) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
    return;
  }

  sortKey.value = key;
  sortDirection.value = 'asc';
}
</script>

<template>
  <section class="panel">
    <div class="panel-header">
      <h2>Blotter</h2>
      <p>Newest trades stay at the top; click a column to sort the blotter.</p>
    </div>
    <p v-if="loading">Loading trades…</p>
    <p v-else-if="!sortedTrades.length" class="empty">No trades yet. Submit the first trade to populate the blotter.</p>
    <div v-else class="table-shell">
      <table>
        <thead>
          <tr>
            <th><button type="button" @click="setSort('timestamp')">{{ sortIndicators.timestamp }} <span>{{ sortKey === 'timestamp' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
            <th><button type="button" @click="setSort('symbol')">{{ sortIndicators.symbol }} <span>{{ sortKey === 'symbol' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
            <th><button type="button" @click="setSort('side')">{{ sortIndicators.side }} <span>{{ sortKey === 'side' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
            <th class="numeric"><button type="button" @click="setSort('quantity')">{{ sortIndicators.quantity }} <span>{{ sortKey === 'quantity' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
            <th class="numeric"><button type="button" @click="setSort('price')">{{ sortIndicators.price }} <span>{{ sortKey === 'price' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
            <th class="numeric"><button type="button" @click="setSort('notionalValue')">{{ sortIndicators.notionalValue }} <span>{{ sortKey === 'notionalValue' ? (sortDirection === 'asc' ? '↑' : '↓') : '' }}</span></button></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="trade in sortedTrades" :key="trade.id">
            <td>{{ new Date(trade.timestamp).toLocaleString() }}</td>
            <td>{{ trade.symbol }}</td>
            <td>
              <span :class="['side', trade.side.toLowerCase()]">{{ trade.side }}</span>
            </td>
            <td class="numeric">{{ trade.quantity }}</td>
            <td class="numeric">{{ trade.price.toFixed(2) }}</td>
            <td class="numeric">{{ (trade.quantity * trade.price).toFixed(2) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>

<style scoped>
.panel {
  background: rgba(255, 255, 255, 0.9);
  border: 1px solid rgba(148, 163, 184, 0.18);
  border-radius: 18px;
  padding: 1.5rem;
  box-shadow: 0 12px 30px rgba(15, 23, 42, 0.06);
  backdrop-filter: blur(12px);
}
.panel-header {
  display: flex;
  align-items: baseline;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 0.75rem;
}
.panel-header h2 {
  margin: 0;
  font-size: 1.25rem;
  letter-spacing: -0.02em;
}
.panel-header p {
  margin: 0;
  color: #64748b;
  text-align: right;
}
.table-shell {
  overflow: hidden;
  border: 1px solid rgba(148, 163, 184, 0.15);
  border-radius: 14px;
}
table {
  width: 100%;
  border-collapse: collapse;
  background: rgba(255, 255, 255, 0.96);
}
th, td {
  text-align: left;
  padding: 0.8rem 0.7rem;
  border-bottom: 1px solid #e2e8f0;
}
thead th {
  background: #f8fafc;
  color: #475569;
  font-size: 0.78rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}
tbody tr:nth-child(even) {
  background: #f8fafc;
}
tbody tr:hover {
  background: #eef2ff;
}
button {
  background: none;
  border: none;
  padding: 0;
  font: inherit;
  cursor: pointer;
  color: #2563eb;
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
}
button span {
  display: inline-block;
  width: 1ch;
}
.numeric {
  text-align: right;
}
.numeric button {
  margin-left: auto;
}
.empty {
  margin: 0;
  color: #64748b;
}
.side {
  display: inline-block;
  font-weight: 700;
  padding: 0.2rem 0.45rem;
  border-radius: 999px;
}
.side.buy {
  background: #dcfce7;
  color: #166534;
}
.side.sell {
  background: #fee2e2;
  color: #b91c1c;
}
@media (max-width: 800px) {
  .panel-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .panel-header p {
    text-align: left;
  }

  th,
  td {
    padding: 0.7rem 0.55rem;
  }
}
</style>
