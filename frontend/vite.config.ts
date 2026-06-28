import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/trades': {
        target: 'http://127.0.0.1:5048',
        changeOrigin: true,
      },
      '/positions': {
        target: 'http://127.0.0.1:5048',
        changeOrigin: true,
      },
      '/health': {
        target: 'http://127.0.0.1:5048',
        changeOrigin: true,
      },
    },
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
});