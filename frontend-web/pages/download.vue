<template>
  <section class="section download-section">
    <div class="container">
      <h1 class="section-title">Download Destonize</h1>
      <p class="download-intro">
        Choose your platform. Windows is fully supported; macOS and Linux versions are coming soon.
      </p>

      <div class="download-grid">
        <div class="download-card disabled">
          <div class="os-icon">
            <Apple :size="48" :stroke-width="1.5" />
          </div>
          <h2>macOS</h2>
          <p>Coming soon</p>
          <button class="btn btn-disabled" disabled>Unavailable</button>
        </div>

        <div class="download-card active featured">
          <div class="os-icon">
            <Monitor :size="48" :stroke-width="1.5" />
          </div>
          <h2>Windows</h2>
          <p>Installer (EXE) – Run the setup wizard.</p>
          <a href="https://github.com/Yannosay/Destonize/releases/latest/download/Destonize.exe" class="btn btn-primary">Download .exe</a>
        </div>

        <div class="download-card disabled">
          <div class="os-icon">
            <Terminal :size="48" :stroke-width="1.5" />
          </div>
          <h2>Linux</h2>
          <p>Coming soon</p>
          <button class="btn btn-disabled" disabled>Unavailable</button>
        </div>
      </div>

      <p class="release-note">
        View all releases on <a href="https://github.com/Yannosay/Destonize/releases" target="_blank" rel="noopener">GitHub Releases</a>.
      </p>
    </div>
  </section>
</template>

<script setup lang="ts">
import { Monitor, Apple, Terminal } from 'lucide-vue-next';
</script>

<style lang="scss" scoped>
.download-section {
  padding: 6rem 0;
}

.download-intro {
  text-align: center;
  max-width: 600px;
  margin: 0 auto 3rem;
  font-size: 1.2rem;
  color: $foreground-muted;
}

.download-grid {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-items: stretch;
  gap: 2rem;
  flex-wrap: wrap;
}

.download-card {
  background: $glass-bg;
  backdrop-filter: blur($glass-blur) saturate(150%);
  -webkit-backdrop-filter: blur($glass-blur) saturate(150%);
  border: 1px solid $glass-border;
  border-radius: 1rem;
  padding: 2.5rem 2rem;
  text-align: center;
  width: 280px;
  box-shadow: $glass-shadow;
  transition: transform 0.2s, border-color 0.2s, background 0.2s;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
}

.download-card.active {
  border-color: $accent;
  background: rgba(134, 160, 217, 0.12);
  backdrop-filter: blur($glass-blur) saturate(180%);
  -webkit-backdrop-filter: blur($glass-blur) saturate(180%);
  &:hover {
    transform: translateY(-8px);
    border-color: $accent-hover;
    background: rgba(134, 160, 217, 0.18);
  }
}

.download-card.disabled {
  opacity: 0.5;
  pointer-events: none;
  filter: grayscale(60%);
  background: rgba(255, 255, 255, 0.02);
}

.download-card.featured {
  width: 320px;
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.3);
}

.os-icon {
  color: $accent;
  margin-bottom: 1.5rem;
}

.download-card h2 {
  margin-bottom: 0.75rem;
  font-size: 1.5rem;
}

.download-card p {
  margin-bottom: 1.5rem;
  color: $foreground-muted;
}

.btn-disabled {
  background: transparent;
  border: 1px solid $glass-border;
  color: rgba(230, 235, 245, 0.4);
  cursor: not-allowed;
}

.release-note {
  text-align: center;
  margin-top: 3rem;
  color: $foreground-muted;
}
</style>