# Destonize Frontend

This directory contains the frontend web application for Destonize, built with Nuxt 4 and deployed to Cloudflare Pages.

## Requirements

- Node.js 20 or later
- npm 10 or later
- Cloudflare account (for deployment)
- Wrangler CLI (installed as dev dependency)

## Setup

1. Install dependencies:
   npm install

2. Start development server:
   npm run dev

3. Build for production:
   npm run build

4. Preview production build:
   npm run preview

5. Deploy to Cloudflare Pages:
   npm run deploy

## Configuration

The Nitro preset is set to `cloudflare-pages` in `nuxt.config.ts`.

## Assets

Place your custom icons and images in the `public` folder.
The favicon should be named `app-icon.ico` and placed in `public/`.

## Useful Links

- [Nuxt Documentation](https://nuxt.com/docs)
- [Cloudflare Pages Documentation](https://developers.cloudflare.com/pages/)
- [Vue.js Documentation](https://vuejs.org/guide/introduction.html)
- [Sass Documentation](https://sass-lang.com/documentation/)