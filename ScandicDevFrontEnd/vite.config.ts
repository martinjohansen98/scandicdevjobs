import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
  plugins: [
    tailwindcss(),
    sveltekit()
  ],

  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7077',
        changeOrigin: true,
        secure: false,
        rewrite: path => path.replace(/^\/api/, '/api')
      }
    }
  },

  optimizeDeps: {
    // Include Leaflet and its plugins for pre-bundling
    include: [
      'leaflet',
      'leaflet/dist/leaflet-src.esm',
      'leaflet.markercluster'
    ]
  },

  ssr: {
    // Force these to be processed by Vite's SSR transform
    noExternal: [
      'leaflet',
      'leaflet.markercluster',
      /leaflet.*/
    ]
  }
});