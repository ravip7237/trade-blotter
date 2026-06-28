<script setup lang="ts">
import { computed, reactive } from 'vue';

import { useTradeStore } from '../stores/tradeStore';
import type { TradePayload } from '../services/tradesService';

type FormField = 'symbol' | 'quantity' | 'price';

type FieldErrors = Partial<Record<FormField, string>>;

interface TradeFormState {
  symbol: string;
  side: 'Buy' | 'Sell';
  quantity: string;
  price: string;
}

const tradeStore = useTradeStore();
const form = reactive<TradeFormState>({
  symbol: '',
  side: 'Buy',
  quantity: '',
  price: ''
});

const touched = reactive<Record<FormField, boolean>>({
  symbol: false,
  quantity: false,
  price: false,
});

const fieldErrors = computed(() => {
  const errors: FieldErrors = {};

  if (!form.symbol.trim()) {
    errors.symbol = 'Symbol is required.';
  }

  if (!form.quantity || Number(form.quantity) <= 0) {
    errors.quantity = 'Quantity must be greater than zero.';
  }

  if (!form.price || Number(form.price) <= 0) {
    errors.price = 'Price must be greater than zero.';
  }

  return errors;
});

const isSubmitDisabled = computed(() => Object.keys(fieldErrors.value).length > 0 || tradeStore.submitting);

const showSymbolError = computed(() => touched.symbol && !!fieldErrors.value.symbol);
const showQuantityError = computed(() => touched.quantity && !!fieldErrors.value.quantity);
const showPriceError = computed(() => touched.price && !!fieldErrors.value.price);

function markTouched(fieldName: FormField) {
  touched[fieldName] = true;
}

function resetForm() {
  form.symbol = '';
  form.side = 'Buy';
  form.quantity = '';
  form.price = '';
  touched.symbol = false;
  touched.quantity = false;
  touched.price = false;
}

async function submitTrade() {
  touched.symbol = true;
  touched.quantity = true;
  touched.price = true;

  if (isSubmitDisabled.value) {
    return;
  }

  const payload: TradePayload = {
    symbol: form.symbol.trim().toUpperCase(),
    side: form.side,
    quantity: Number(form.quantity),
    price: Number(form.price),
    timestamp: new Date().toISOString(),
  };

  const submitted = await tradeStore.submitTrade(payload);

  if (submitted) {
    resetForm();
  }
}
</script>

<template>
  <section class="panel">
    <h2>New trade</h2>
    <p class="helper">Use a symbol, choose buy or sell, then enter quantity and price. The blotter updates as soon as the trade is accepted.</p>
    <form class="form" @submit.prevent="submitTrade">
      <label>
        Symbol
        <input v-model="form.symbol" placeholder="AAPL" @blur="markTouched('symbol')" />
        <small v-if="showSymbolError" class="error-text">{{ fieldErrors.symbol }}</small>
      </label>
      <label>
        Side
        <select v-model="form.side">
          <option value="Buy">Buy</option>
          <option value="Sell">Sell</option>
        </select>
      </label>
      <label>
        Quantity
        <input v-model="form.quantity" type="number" min="1" step="1" @blur="markTouched('quantity')" />
        <small v-if="showQuantityError" class="error-text">{{ fieldErrors.quantity }}</small>
      </label>
      <label>
        Price
        <input v-model="form.price" type="number" min="0.01" step="0.01" @blur="markTouched('price')" />
        <small v-if="showPriceError" class="error-text">{{ fieldErrors.price }}</small>
      </label>
      <button type="submit" :disabled="isSubmitDisabled">
        {{ tradeStore.submitting ? 'Submitting…' : 'Submit trade' }}
      </button>
    </form>
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
h2 {
  margin: 0;
  font-size: 1.25rem;
  letter-spacing: -0.02em;
}
.helper {
  margin: 0.5rem 0 1rem;
  color: #475569;
  line-height: 1.6;
}
.form {
  display: grid;
  gap: 0.9rem;
}
label {
  display: grid;
  gap: 0.35rem;
  font-size: 0.92rem;
  font-weight: 700;
  color: #2563eb;
}
.error-text {
  color: #b91c1c;
  font-size: 0.85rem;
  font-weight: 500;
}
input,
select,
button {
  border-radius: 12px;
  border: 1px solid #cbd5e1;
  padding: 0.72rem 0.85rem;
  font: inherit;
  transition: border-color 0.15s ease, box-shadow 0.15s ease, transform 0.15s ease;
}
input,
select {
  background: rgba(248, 250, 252, 0.96);
  color: #0f172a;
}
input:focus,
select:focus,
button:focus {
  outline: none;
  border-color: #60a5fa;
  box-shadow: 0 0 0 4px rgba(37, 99, 235, 0.14);
}
button {
  margin-top: 0.25rem;
  background: linear-gradient(135deg, #2563eb, #1d4ed8);
  color: white;
  border: none;
  cursor: pointer;
  font-weight: 700;
  letter-spacing: 0.01em;
  box-shadow: 0 10px 24px rgba(37, 99, 235, 0.22);
}
button:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 14px 28px rgba(37, 99, 235, 0.26);
}
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}
</style>
