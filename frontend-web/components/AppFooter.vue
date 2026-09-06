<template>
  <footer class="footer glass">
    <div class="container footer-content">
      <div class="footer-links">
        <NuxtLink to="/" class="link">Home</NuxtLink>
        <NuxtLink to="/features" class="link">Features</NuxtLink>
        <NuxtLink to="/download" class="link">Download</NuxtLink>
        <NuxtLink to="/docs" class="link">Docs</NuxtLink>
        <NuxtLink to="/about" class="link">About</NuxtLink>
      </div>
      <div class="footer-copyright">
        © 2026 Yannosay Productions
  
      </div>
      <div class="footer-copyright">
CURRENT LICENSE: GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007, not affiliated with Microsoft in any way.
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.footer {
  background: rgba(10, 15, 26, 0.85);
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border-top: 1px solid $glass-border;
  padding: 2rem 0;
}

.footer-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.footer-links {
  display: flex;
  gap: 1.5rem;
}

.link {
  color: $foreground;
  &:hover {
    color: $accent;
  }
}

.footer-copyright {
  color: $foreground-soft;
  font-size: 0.9rem;
}
</style>