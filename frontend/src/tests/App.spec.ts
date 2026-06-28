import { describe, it, expect, beforeEach } from 'vitest';
import { mount } from '@vue/test-utils';
import { createPinia, setActivePinia } from 'pinia';
import router from '../router';
import App from '../App.vue';

describe('App', () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it('renders the trade blotter interface', async () => {
    const wrapper = mount(App, {
      global: {
        plugins: [createPinia(), router],
      },
    });

    await router.isReady();
    await wrapper.vm.$nextTick();

    expect(wrapper.text()).toContain('Trade Blotter');
    expect(wrapper.text()).toContain('New trade');
    expect(wrapper.text()).toContain('Positions');
    expect(wrapper.text()).toContain('Blotter');
  });
});