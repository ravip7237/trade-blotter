import { createRouter, createWebHistory } from 'vue-router';

import TradeBlotterView from '../views/TradeBlotterView.vue';

const routes = [
  {
    path: '/',
    name: 'trade-blotter',
    component: TradeBlotterView,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;