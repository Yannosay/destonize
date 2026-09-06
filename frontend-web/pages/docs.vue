<template>
  <section class="section">
    <div class="container">
      <h1 class="section-title">Documentation</h1>
      <div class="docs-content glass">
        <h2>Getting Started</h2>
        <p>After installing Destonize, launch the application. You will be greeted by a welcome screen that asks you to accept access to your desktop.</p>
        <h2>Creating Rules</h2>
        <p>Click on <strong>Organize...</strong> to open the rule editor. Define a rule by specifying a name, destination folder, and one or more conditions such as file extension, name filter, category, size, or age.</p>
        <h2>Undo</h2>
        <p>If you make a mistake, use the <strong>Undo Last Move</strong> button to revert the most recent file move.</p>
        <h2>Settings</h2>
        <p>Access settings via the <strong>Settings</strong> menu or button. Here you can enable automatic updates, start with Windows, minimize to tray, and more.</p>
        <h2>Advanced</h2>
        <p>You can save your rules as .ize schema files for backup or sharing.</p>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.docs-content {
  max-width: 800px;
  margin: 0 auto;
  background: $glass-bg;
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border: 1px solid $glass-border;
  border-radius: 0.75rem;
  padding: 2rem;
  box-shadow: $glass-shadow;
}

.docs-content h2 {
  margin-top: 2rem;
  margin-bottom: 0.5rem;
  color: $accent;
}

.docs-content p {
  margin-bottom: 1rem;
}
</style>