<template>
  <div class="feature-card glass">
    <div class="feature-icon">
      <component :is="iconComponent" :size="32" :stroke-width="1.5" />
    </div>
    <h3 class="feature-title">{{ title }}</h3>
    <p class="feature-description">{{ description }}</p>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import {
  FolderCog,
  RefreshCw,
  Undo2,
  Palette,
  Save,
  Settings
} from 'lucide-vue-next';

const props = defineProps<{
  title: string;
  description: string;
  icon: string;
}>();

const iconMap = {
  folder: FolderCog,
  refresh: RefreshCw,
  undo: Undo2,
  palette: Palette,
  save: Save,
  settings: Settings
};

const iconComponent = computed(() => iconMap[props.icon] || FolderCog);
</script>

<style lang="scss" scoped>
.feature-card {
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.14), rgba(255, 255, 255, 0.04));
  backdrop-filter: blur(24px) saturate(200%);
  -webkit-backdrop-filter: blur(24px) saturate(200%);
  border: 1px solid rgba(255, 255, 255, 0.22);
  border-radius: 1.25rem;
  padding: 2rem 1.5rem;
  text-align: center;
  transition: all 0.4s cubic-bezier(0.22, 1, 0.36, 1);
  box-shadow: 
    0 8px 32px rgba(0, 0, 0, 0.1),
    inset 0 1px 0 rgba(255, 255, 255, 0.18),
    inset 0 -1px 0 rgba(0, 0, 0, 0.06);

  &:hover {
    border-color: rgba(255, 255, 255, 0.35);
    background: linear-gradient(135deg, rgba(255, 255, 255, 0.2), rgba(255, 255, 255, 0.07));
    transform: scale(1.03);
    box-shadow: 
      0 16px 48px rgba(0, 0, 0, 0.15),
      inset 0 1px 0 rgba(255, 255, 255, 0.25),
      inset 0 -1px 0 rgba(0, 0, 0, 0.05);
  }
}

.feature-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 72px;
  height: 72px;
  border-radius: 50%;
  background: linear-gradient(135deg, rgba(134, 160, 217, 0.25), rgba(134, 160, 217, 0.1));
  backdrop-filter: blur(16px) saturate(160%);
  -webkit-backdrop-filter: blur(16px) saturate(160%);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: $accent;
  margin-bottom: 1.25rem;
  box-shadow: 
    0 4px 16px rgba(134, 160, 217, 0.2),
    inset 0 1px 0 rgba(255, 255, 255, 0.25);
}

.feature-title {
  font-size: 1.25rem;
  font-weight: 600;
  letter-spacing: -0.01em;
  margin-bottom: 0.5rem;
}

.feature-description {
  color: $foreground-muted;
  line-height: 1.6;
}
</style>