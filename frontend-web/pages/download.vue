<template>
  <section class="section download-section">
    <div class="container">
      <h1 class="section-title">Download Destonize</h1>
      <p class="download-intro">
        Choose your platform and download the installer EXE.
      </p>

      <div class="download-grid">
        <div class="download-card disabled">
          <div class="os-icon-wrapper">
            <svg class="os-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" v-html="appleIcon.svg" />
          </div>
          <h2>macOS</h2>
          <p>Coming soon (HOPEFULLY?)</p>
          <button class="btn btn-disabled" disabled>Unavailable</button>
        </div>

        <div class="download-card active featured">
          <div class="os-icon-wrapper featured-icon">
            <svg class="os-icon" viewBox="0 0 24 24" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
              <path d="M0 3.449L9.75 2.1v9.451H0m10.949-9.602L24 0v11.4H10.949M0 12.6h9.75v9.451L0 20.699M10.949 12.6H24V24l-12.9-1.801"/>
            </svg>
          </div>
          <h2>Windows</h2>
          <p>Installer (EXE) – Run the setup wizard.</p>
          <a href="https://github.com/Yannosay/destonize/releases/latest/download/DestonizeSetup.exe" class="btn btn-primary">
            <svg class="btn-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/>
              <polyline points="7 10 12 15 17 10"/>
              <line x1="12" y1="15" x2="12" y2="3"/>
            </svg>
            Download .exe
          </a>
        </div>

        <div class="download-card disabled">
          <div class="os-icon-wrapper">
            <svg class="os-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" v-html="linuxIcon.svg" />
          </div>
          <h2>Linux</h2>
          <p>Coming soon (someday...)</p>
          <button class="btn btn-disabled" disabled>Unavailable</button>
        </div>
      </div>

      <p class="release-note">
        View all releases on <a href="https://github.com/Yannosay/Destonize/releases" target="_blank" rel="noopener" class="release-link">
          GitHub Releases
        </a>
      </p>
    </div>
  </section>
</template>

<script setup lang="ts">
import { siApple, siLinux } from 'simple-icons';

const appleIcon = siApple;
const linuxIcon = siLinux;
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
  position: relative;
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.14), rgba(255, 255, 255, 0.04));
  backdrop-filter: blur(24px) saturate(200%);
  -webkit-backdrop-filter: blur(24px) saturate(200%);
  border: 1px solid rgba(255, 255, 255, 0.22);
  border-radius: 1.5rem;
  padding: 2.5rem 2rem;
  text-align: center;
  width: 280px;
  box-shadow: 
    0 8px 32px rgba(0, 0, 0, 0.1),
    inset 0 1px 0 rgba(255, 255, 255, 0.18),
    inset 0 -1px 0 rgba(0, 0, 0, 0.06);
  transition: all 0.4s cubic-bezier(0.22, 1, 0.36, 1);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-between;
}

.download-card.active {
  border-color: rgba(134, 160, 217, 0.4);
  background: linear-gradient(135deg, rgba(134, 160, 217, 0.18), rgba(134, 160, 217, 0.06));
  
  &:hover {
    transform: scale(1.03);
    border-color: rgba(134, 160, 217, 0.55);
    background: linear-gradient(135deg, rgba(134, 160, 217, 0.24), rgba(134, 160, 217, 0.08));
    box-shadow: 
      0 16px 48px rgba(134, 160, 217, 0.2),
      inset 0 1px 0 rgba(255, 255, 255, 0.25),
      inset 0 -1px 0 rgba(0, 0, 0, 0.05);
  }
}

.download-card.disabled {
  opacity: 0.45;
  pointer-events: none;
  filter: grayscale(60%);
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.06), rgba(255, 255, 255, 0.02));
}

.download-card.featured {
  width: 320px;
  box-shadow: 
    0 12px 48px rgba(134, 160, 217, 0.25),
    inset 0 1px 0 rgba(255, 255, 255, 0.25),
    inset 0 -1px 0 rgba(0, 0, 0, 0.08);
}

.os-icon-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 96px;
  height: 96px;
  border-radius: 28px;
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.12), rgba(255, 255, 255, 0.04));
  backdrop-filter: blur(16px) saturate(180%);
  -webkit-backdrop-filter: blur(16px) saturate(180%);
  border: 1px solid rgba(255, 255, 255, 0.2);
  margin-bottom: 1.5rem;
  box-shadow: 
    0 4px 16px rgba(0, 0, 0, 0.1),
    inset 0 1px 0 rgba(255, 255, 255, 0.2);
}

.featured-icon {
  background: linear-gradient(135deg, rgba(134, 160, 217, 0.3), rgba(134, 160, 217, 0.12));
  border-color: rgba(134, 160, 217, 0.4);
  box-shadow: 
    0 8px 24px rgba(134, 160, 217, 0.3),
    inset 0 1px 0 rgba(255, 255, 255, 0.3);
}

.os-icon {
  width: 48px;
  height: 48px;
  display: block;
  color: $foreground;
}

.download-card h2 {
  margin-bottom: 0.75rem;
  font-size: 1.5rem;
  font-weight: 700;
  letter-spacing: -0.02em;
}

.download-card p {
  margin-bottom: 1.5rem;
  color: $foreground-muted;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-icon {
  width: 18px;
  height: 18px;
  flex-shrink: 0;
}

.btn-disabled {
  background: transparent;
  border: 1px solid rgba(255, 255, 255, 0.15);
  color: rgba(230, 235, 245, 0.4);
  cursor: not-allowed;
  backdrop-filter: none;
  -webkit-backdrop-filter: none;
}

.release-note {
  text-align: center;
  margin-top: 3rem;
  color: $foreground-muted;
}

.release-link {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  color: $accent;
  transition: color 0.3s ease;

  &:hover {
    color: $accent-hover;
    text-decoration: underline;
  }
}
</style>