<template>
  <footer class="footer glass">
    <div class="container footer-content">
      <div class="footer-links">
        <NuxtLink to="/" class="link" :class="{ active: isActive('/') }">Home</NuxtLink>
        <NuxtLink to="/features" class="link" :class="{ active: isActive('/features') }">Features</NuxtLink>
        <NuxtLink to="/download" class="link" :class="{ active: isActive('/download') }">Download</NuxtLink>
        <NuxtLink to="/docs" class="link" :class="{ active: isActive('/docs') }">Docs</NuxtLink>
        <NuxtLink to="/about" class="link" :class="{ active: isActive('/about') }">About</NuxtLink>
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
const route = useRoute();

const isActive = (path: string) => {
  return route.path === path;
};
</script>

<style lang="scss" scoped>
.footer {
  background: linear-gradient(135deg, rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.03));
  backdrop-filter: blur(24px) saturate(200%);
  -webkit-backdrop-filter: blur(24px) saturate(200%);
  border-top: 1px solid rgba(255, 255, 255, 0.22);
  box-shadow: 
    0 -8px 32px rgba(0, 0, 0, 0.1),
    inset 0 -1px 0 rgba(255, 255, 255, 0.18);
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
  gap: 0.5rem;
  align-items: center;
}

.link {
  position: relative;
  color: $foreground;
  font-size: 0.95rem;
  font-weight: 500;
  padding: 0.5rem 1rem;
  border-radius: 9999px;
  border: 1px solid transparent;
  transition: 
    color 0.2s ease,
    border-color 0.2s ease,
    background 0.2s ease,
    box-shadow 0.2s ease,
    transform 0.2s ease;

  &::before {
    content: '';
    position: absolute;
    inset: 0;
    border-radius: 9999px;
    background: linear-gradient(135deg, rgba(255, 255, 255, 0.14), rgba(255, 255, 255, 0.05));
    border: 1px solid rgba(255, 255, 255, 0.22);
    backdrop-filter: blur(16px) saturate(180%);
    -webkit-backdrop-filter: blur(16px) saturate(180%);
    opacity: 0;
    transition: opacity 0.2s ease;
    pointer-events: none;
  }

  &:hover {
    color: #fff;
    transform: scale(1.05);

    &::before {
      opacity: 1;
    }
  }

  &.active {
    color: #fff;

    &::before {
      opacity: 1;
      background: linear-gradient(135deg, rgba(134, 160, 217, 0.3), rgba(134, 160, 217, 0.12));
      border-color: rgba(134, 160, 217, 0.4);
      box-shadow: 
        0 4px 16px rgba(134, 160, 217, 0.25),
        inset 0 1px 0 rgba(255, 255, 255, 0.2);
    }
  }
}

.footer-copyright {
  color: $foreground-muted;
  font-size: 0.9rem;
  opacity: 0.7;
}
</style>