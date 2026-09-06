export default defineNuxtConfig({
  compatibilityDate: '2025-07-01',
  devtools: { enabled: true },
  css: ['~/assets/scss/main.scss'],
  vite: {
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: '@use "~/assets/scss/_variables.scss" as *;'
        }
      }
    }
  },
  nitro: {
    preset: 'cloudflare-pages'
  },
  app: {
    head: {
      title: 'Destonize – Professional Desktop Organization',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'description', content: 'Destonize is a professional desktop cleaning and organizing tool for Windows. Organize files with custom rules, schedules, and more.' },
        { name: 'keywords', content: 'desktop organizer, file organizer, Windows, automation, productivity' },
        { name: 'author', content: 'Yannosay Productions' },
        { property: 'og:title', content: 'Destonize' },
        { property: 'og:description', content: 'Professional desktop cleaning and organizing tool.' },
        { property: 'og:type', content: 'website' },
        { property: 'og:url', content: 'https://destonize.pages.dev' }
      ],
      link: [
        { rel: 'icon', type: 'image/png', href: '/app-icon.png' }
      ]
    }
  }
})