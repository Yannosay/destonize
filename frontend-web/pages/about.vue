<template>
  <section class="section">
    <div class="container about-content glass">
      <h1 class="section-title">About Destonize</h1>
      <p>Destonize is developed by Yannosay Productions, a software company dedicated to creating professional, user-friendly tools for Windows.</p>
      <p>The project is open source and licensed under the GNU General Public License v3.0.</p>
      <p>
        Visit our GitHub repository at <a href="https://github.com/YannosayProductions/Destonize" target="_blank" rel="noopener">github.com/YannosayProductions/Destonize</a>
      </p>
    </div>
  </section>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
.about-content {
  max-width: 800px;
  margin: 0 auto;
  text-align: center;
  background: $glass-bg;
  backdrop-filter: blur($glass-blur);
  -webkit-backdrop-filter: blur($glass-blur);
  border: 1px solid $glass-border;
  border-radius: 0.75rem;
  padding: 2rem;
  box-shadow: $glass-shadow;
}

.about-content p {
  margin-bottom: 1rem;
  font-size: 1.2rem;
}
</style>