<script setup lang="ts">
import type { Position } from '../types/tradeTypes';

const props = withDefaults(defineProps<{ positions?: Position[]; loading?: boolean }>(), {
  positions: () => [],
  loading: false,
});
</script>

<template>
  <section class="panel">
    <div class="panel-header">
      <h2>Positions</h2>
    </div>
    <p v-if="props.loading">Loading positions…</p>
    <p v-else-if="!props.positions.length" class="empty">No open positions.</p>
    <div v-else class="table-shell">
      <table>
        <thead>
          <tr>
            <th>Symbol</th>
            <th class="numeric">Net qty</th>
            <th class="numeric">Avg cost</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="position in props.positions" :key="position.symbol">
            <td><strong>{{ position.symbol }}</strong></td>
            <td class="numeric">{{ position.netQuantity }}</td>
            <td class="numeric">{{ position.averageCost.toFixed(2) }}</td>
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
  height: 100%;
  display: flex;
  flex-direction: column;
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
.panel-header p,
.empty {
  margin: 0;
  color: #64748b;
}
.table-shell {
  overflow: hidden;
  border: 1px solid rgba(148, 163, 184, 0.15);
  border-radius: 14px;
  flex: 1;
  min-height: 0;
}
table {
  width: 100%;
  border-collapse: collapse;
  background: rgba(255, 255, 255, 0.96);
}
th,
td {
  text-align: left;
  padding: 0.8rem 0.7rem;
  border-bottom: 1px solid #e2e8f0;
}
th {
  color: #2563eb;
  font-size: 0.82rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
}
tbody tr:nth-child(even) {
  background: #f8fafc;
}
tbody tr:hover {
  background: #eef2ff;
}
tbody tr:nth-child(even) {
  background: #f8fafc;
}
tbody tr:hover {
  background: #eef2ff;
}
td strong {
  font-size: 1rem;
}
.numeric {
  text-align: right;
}
@media (max-width: 800px) {
  .panel-header {
    flex-direction: column;
    align-items: flex-start;
  }

  th,
  td {
    padding: 0.7rem 0.55rem;
  }
}
</style>
