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
  background: $glass-bg;
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border: 1px solid $glass-border;
  border-radius: 0.75rem;
  padding: 1.5rem;
  text-align: center;
  transition: transform 0.2s, border-color 0.2s, background 0.2s;
  box-shadow: $glass-shadow;

  &:hover {
    border-color: $accent;
    background: $glass-bg-hover;
    transform: translateY(-5px);
  }
}

.feature-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background: rgba(134, 160, 217, 0.15);
  color: $accent;
  margin-bottom: 1rem;
}

.feature-title {
  font-size: 1.25rem;
  margin-bottom: 0.5rem;
}

.feature-description {
  color: $foreground-muted;
}
</style>