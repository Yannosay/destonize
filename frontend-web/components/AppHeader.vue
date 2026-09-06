<template>
  <header class="header glass">
    <nav class="nav container">
      <NuxtLink to="/" class="brand">
        <img src="/app-icon.png" alt="Destonize Logo" class="logo" />
        <span>Destonize</span>
      </NuxtLink>
      <div class="links">
        <NuxtLink to="/" class="link">Home</NuxtLink>
        <NuxtLink to="/features" class="link">Features</NuxtLink>
        <NuxtLink to="/download" class="link">Download</NuxtLink>
        <NuxtLink to="/docs" class="link">Docs</NuxtLink>
        <NuxtLink to="/about" class="link">About</NuxtLink>
      </div>
    </nav>
  </header>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.header {
  position: sticky;
  top: 0;
  z-index: 100;
  background: rgba(10, 15, 26, 0.85);
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border-bottom: 1px solid $glass-border;
}

.nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 2rem;
}

.brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.5rem;
  font-weight: bold;
  color: $foreground;

  .logo {
    height: 40px;
    width: auto;
  }
}

.links {
  display: flex;
  gap: 1.5rem;
}

.link {
  color: $foreground;
  font-size: 1rem;
  transition: color 0.2s;

  &:hover {
    color: $accent;
  }
}
</style>