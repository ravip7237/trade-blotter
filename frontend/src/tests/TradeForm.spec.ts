import { beforeEach, describe, expect, it, vi } from 'vitest';
import { mount } from '@vue/test-utils';
import { createPinia, setActivePinia } from 'pinia';

import TradeForm from '../components/TradeForm.vue';
import { useTradeStore } from '../stores/tradeStore';

describe('TradeForm', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it('shows validation errors and blocks invalid submission', async () => {
    const tradeStore = useTradeStore();
    const submitTradeSpy = vi.spyOn(tradeStore, 'submitTrade');

    const wrapper = mount(TradeForm, {
      global: {
        plugins: [createPinia()],
      },
    });

    await wrapper.find('form').trigger('submit.prevent');

    expect(wrapper.text()).toContain('Symbol is required.');
    expect(wrapper.text()).toContain('Quantity must be greater than zero.');
    expect(wrapper.text()).toContain('Price must be greater than zero.');

    expect(submitTradeSpy).not.toHaveBeenCalled();
  });

  it('submits a valid trade and resets the form on success', async () => {
    const wrapper = mount(TradeForm, {
      global: {
        plugins: [createPinia()],
      },
    });

    const tradeStore = useTradeStore();
    const submitTradeSpy = vi.spyOn(tradeStore, 'submitTrade').mockResolvedValue(true);

    await wrapper.get('input[placeholder="AAPL"]').setValue('msft');
    await wrapper.get('select').setValue('Sell');
    await wrapper.get('input[type="number"][min="1"][step="1"]').setValue('15');
    await wrapper.get('input[type="number"][min="0.01"][step="0.01"]').setValue('250.5');

    await wrapper.find('form').trigger('submit.prevent');

    expect(submitTradeSpy).toHaveBeenCalledTimes(1);
    expect(submitTradeSpy).toHaveBeenCalledWith(
      expect.objectContaining({
        symbol: 'MSFT',
        side: 'Sell',
        quantity: 15,
        price: 250.5,
      }),
    );
    expect((wrapper.get('input[placeholder="AAPL"]').element as HTMLInputElement).value).toBe('');
    expect((wrapper.get('select').element as HTMLSelectElement).value).toBe('Buy');
    expect((wrapper.get('input[type="number"][min="1"][step="1"]').element as HTMLInputElement).value).toBe('');
    expect((wrapper.get('input[type="number"][min="0.01"][step="0.01"]').element as HTMLInputElement).value).toBe('');
  });
});