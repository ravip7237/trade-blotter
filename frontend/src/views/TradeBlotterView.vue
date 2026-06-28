<script setup>
import { computed, onMounted } from 'vue';

import TradeForm from '../components/TradeForm.vue';
import BlotterTable from '../components/BlotterTable.vue';
import PositionsPanel from '../components/PositionsPanel.vue';
import StatusMessage from '../components/StatusMessage.vue';
import { useTradeStore } from '../stores/tradeStore';

const tradeStore = useTradeStore();

const tradeCount = computed(() => tradeStore.sortedTrades.length);
const positionCount = computed(() => tradeStore.positions.length);
const lastLoadedAt = computed(() => {
  if (!tradeStore.sortedTrades.length) {
    return 'No trades loaded yet';
  }

  return `Latest trade ${new Date(tradeStore.sortedTrades[0].timestamp).toLocaleString()}`;
});

onMounted(async () => {
  await tradeStore.fetchTrades();
  await tradeStore.fetchPositions();
});
</script>

<template>
  <main class="page">
    <header class="hero">
      <div class="hero-copy">
        <p class="eyebrow">Trade monitoring</p>
        <h1>Trade Blotter</h1>
        <p class="intro">Enter trades, review positions, and keep the blotter current in one place.</p>
      </div>
      <div class="summary" aria-label="Trade summary">
        <article>
          <span class="summary-label">Trades</span>
          <strong>{{ tradeCount }}</strong>
        </article>
        <article>
          <span class="summary-label">Open positions</span>
          <strong>{{ positionCount }}</strong>
        </article>
        <article>
          <span class="summary-label">Activity</span>
          <strong>{{ lastLoadedAt }}</strong>
        </article>
      </div>
    </header>

    <StatusMessage :message="tradeStore.error" type="error" />

    <section class="grid">
      <TradeForm />
      <PositionsPanel :positions="tradeStore.positions" :loading="tradeStore.loading" />
    </section>

    <BlotterTable :trades="tradeStore.sortedTrades" :loading="tradeStore.loading" />
  </main>
</template>

<style scoped>
.page {
  max-width: 1240px;
  margin: 0 auto;
  padding: 2.5rem 1.25rem 3rem;
  color: #0f172a;
}
.hero {
  display: grid;
  gap: 1rem;
  margin-bottom: 1.5rem;
}
.hero-copy {
  max-width: 720px;
}
.eyebrow {
  text-transform: uppercase;
  letter-spacing: 0.2em;
  font-size: 0.8rem;
  color: #2563eb;
  font-weight: 700;
  margin: 0 0 0.25rem;
}
h1 {
  margin: 0 0 0.5rem;
  font-size: clamp(2.2rem, 3.5vw, 3.5rem);
  line-height: 1.02;
  letter-spacing: -0.04em;
}
.intro {
  margin: 0;
  color: #2563eb;
  font-size: 1.05rem;
  line-height: 1.7;
  font-weight: 600;
}
.summary {
  display: grid;
  gap: 0.75rem;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  margin-top: 1rem;
}
.summary article {
  background: rgba(255, 255, 255, 0.86);
  border: 1px solid rgba(148, 163, 184, 0.18);
  border-radius: 18px;
  padding: 0.9rem 1rem;
  box-shadow: 0 12px 30px rgba(15, 23, 42, 0.05);
  backdrop-filter: blur(12px);
}
.summary-label {
  display: block;
  font-size: 0.78rem;
  text-transform: uppercase;
  letter-spacing: 0.12em;
  color: #2563eb;
  font-weight: 700;
  margin-bottom: 0.35rem;
}
.summary strong {
  display: block;
  font-size: 1.05rem;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.35;
  word-break: break-word;
}
.grid {
  display: grid;
  gap: 1rem;
  grid-template-columns: minmax(300px, 380px) 1fr;
  align-items: stretch;
  margin-bottom: 1rem;
}
@media (max-width: 800px) {
  .summary {
    grid-template-columns: 1fr;
  }
  .grid {
    grid-template-columns: 1fr;
  }
  .page {
    padding-top: 1.25rem;
  }
}
</style>