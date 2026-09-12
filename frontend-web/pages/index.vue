<template>
  <section class="section download-section">
    <div class="container">
      <h1 class="section-title">Download Destonize</h1>
      <p class="download-intro">
        Choose your platform. Windows is fully supported; macOS and Linux versions are coming soon.
      </p>

      <div class="download-grid">
        <div class="download-card disabled">
          <div class="os-icon-wrapper">
            <svg class="os-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" v-html="appleIcon.svg" />
          </div>
          <h2>macOS</h2>
          <p>Coming soon</p>
          <button class="btn btn-disabled" disabled>Unavailable</button>
        </div>

        <div class="download-card active featured">
          <div class="os-icon-wrapper featured-icon">
            <svg class="os-icon" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" v-html="windowsIcon.svg" />
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
          <p>Coming soon</p>
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
import { siApple, siWindows, siLinux } from 'simple-icons';

const appleIcon = siApple;
const windowsIcon = siWindows;
const linuxIcon = siLinux;
</script>