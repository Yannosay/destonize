<template>
  <section class="section">
    <div class="container">
      <h1 class="section-title">Features</h1>
      <div class="features-list">
        <div v-for="feature in features" :key="feature.title" class="feature-item glass">
          <div class="feature-icon">
            <component :is="feature.iconComponent" :size="32" :stroke-width="1.5" />
          </div>
          <div class="feature-content">
            <h2 class="feature-title">{{ feature.title }}</h2>
            <p class="feature-description">{{ feature.description }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import {
  FolderCog,
  RefreshCw,
  Undo2,
  Palette,
  Save,
  Settings,
  Rocket,
  ImageIcon
} from 'lucide-vue-next';

const features = [
  {
    iconComponent: FolderCog,
    title: 'Rule‑Based Organization',
    description: 'Create powerful rules with file extensions, name patterns, categories, size ranges, age limits, and custom inclusions/exclusions.'
  },
  {
    iconComponent: RefreshCw,
    title: 'Automatic Updates',
    description: 'Destonize checks for updates in the background and notifies you when a new version is available.'
  },
  {
    iconComponent: Undo2,
    title: 'Undo & Safety',
    description: 'Every file move is logged. The Undo Last Move button lets you revert the most recent action instantly.'
  },
  {
    iconComponent: Palette,
    title: 'Professional Dark UI',
    description: 'A carefully designed dark theme reduces eye strain and looks great on any Windows desktop.'
  },
  {
    iconComponent: Save,
    title: 'Schema Import/Export',
    description: 'Save your organization rules as .ize schema files to share, backup, or reuse across machines.'
  },
  {
    iconComponent: Settings,
    title: 'System Integration',
    description: 'Start with Windows automatically and minimize to the system tray for continuous background operation.'
  },
  {
    iconComponent: Rocket,
    title: 'Fast and Lightweight',
    description: 'Built with .NET 8 and WPF for high performance and minimal resource usage.'
  },
  {
    iconComponent: ImageIcon,
    title: 'Category Recognition',
    description: 'Automatically recognize file categories like Images, Documents, Audio, Video, and more.'
  }
];
</script>

<style lang="scss" scoped>
.features-list {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.feature-item {
  display: flex;
  gap: 1.5rem;
  background: $glass-bg;
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border: 1px solid $glass-border;
  border-radius: 0.75rem;
  padding: 1.5rem;
  box-shadow: $glass-shadow;
}

.feature-icon {
  flex-shrink: 0;
  color: $accent;
}

.feature-title {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.feature-description {
  color: $foreground-muted;
}
</style>