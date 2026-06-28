<script setup lang="ts">
defineProps<{
  message?: string;
  type?: string;
}>();
</script>

<template>
  <p v-if="message" :class="['status', type]">{{ message }}</p>
</template>

<style scoped>
.status {
  margin: 0 0 1rem;
  padding: 0.9rem 1rem;
  border-radius: 14px;
  border: 1px solid transparent;
  font-weight: 600;
  line-height: 1.45;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.06);
}
.status.error {
  background: rgba(254, 226, 226, 0.82);
  color: #991b1b;
  border-color: rgba(239, 68, 68, 0.18);
}
.status.success {
  background: rgba(220, 252, 231, 0.82);
  color: #166534;
  border-color: rgba(34, 197, 94, 0.18);
}
</style>
