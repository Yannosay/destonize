<template>
  <div>
    <section class="hero section">
      <div class="container hero-content">
        <img src="/destonize.png" alt="Destonize" class="hero-logo" />
        <p class="hero-subtitle">
          Destonize is a professional desktop cleaning and organizing tool that brings order to chaos with customizable rules, automation, and an intuitive dark interface.
        </p>
        <div class="hero-actions">
          <NuxtLink to="/download" class="btn btn-primary">Download Now</NuxtLink>
          <NuxtLink to="/docs" class="btn btn-secondary">Read Documentation</NuxtLink>
        </div>
      </div>
    </section>

    <section class="features-overview section">
      <div class="container">
        <h2 class="section-title">Why Destonize?</h2>
        <div class="feature-grid">
          <FeatureCard
            v-for="feature in features"
            :key="feature.title"
            :title="feature.title"
            :description="feature.description"
            :icon="feature.icon"
          />
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import FeatureCard from '~/components/FeatureCard.vue';

const features = [
  {
    title: 'Rule‑Based Organization',
    description: 'Define precise rules using file extensions, names, sizes, dates, and categories.',
    icon: 'folder'
  },
  {
    title: 'Automatic Updates',
    description: 'Stay up to date with the latest improvements automatically.',
    icon: 'refresh'
  },
  {
    title: 'Undo & Safety',
    description: 'Built‑in undo log ensures you can always revert the last move.',
    icon: 'undo'
  },
  {
    title: 'Schema Import/Export',
    description: 'Save and share your organization rules as .ize schema files.',
    icon: 'save'
  }
];
</script>

<style lang="scss" scoped>
.hero {
  background: transparent;
  border-bottom: 1px solid $glass-border;
}

.hero-content {
  text-align: center;
  padding: 2rem 0; /* reduced from 4rem */
}

.hero-logo {
  max-width: 800px; /* much bigger */
  width: 100%;
  margin-bottom: 0.5rem; /* less bottom margin */
}

.hero-subtitle {
  font-size: 1.25rem;
  max-width: 700px;
  margin: 0 auto 1.5rem;
  color: $foreground-muted;
}

.hero-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

.features-overview {
  background: transparent;
}

.feature-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}
</style>