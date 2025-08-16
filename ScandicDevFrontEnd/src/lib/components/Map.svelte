<script lang="ts">
  import { browser } from '$app/environment';
  import { onMount, onDestroy, tick } from 'svelte';
  import 'leaflet/dist/leaflet.css';
  import type { Joblisting } from '$lib/types/JobListing';

  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
  const API_BASE_URL_BLOB = import.meta.env.VITE_API_BASE_URL_BLOB;

  export let jobs: Joblisting[] = [];


  let map: any;
  let L: any;
  let markers: any[] = [];

  function createClusterIcon() {
    return L.divIcon({
      className: 'cluster-marker',
      html: `<div class="w-6 h-6 bg-blue-500 rounded-full border-2 border-white"></div>`,
      iconSize: [24, 24]
    });
  }

  function createLogoIcon(url: string) {
    return L.divIcon({
      className: 'logo-marker',
      html: `
        <div class="w-10 h-10 rounded-full border-2 border-white overflow-hidden flex items-center justify-center bg-white">

      <img src="${url}" class="w-20 h-20 object-cover object-center"/>
      </div>
      `,
      iconSize: [32, 32]
    });
  }
  // w-12 h-12 rounded-full border-2 border-white object-cover
  function updateMarkers() {
    if (!map) return;
    markers.forEach(m => m.remove());
    markers = [];

    for (const job of jobs) {
      console.log(map.getZoom())
      const icon = map.getZoom() >= 6
        ? createLogoIcon(API_BASE_URL_BLOB + job.company?.companyLogoGuid)
        : createClusterIcon();

      const marker = L.marker([job.latitude, job.longitude], { icon });
      marker.bindPopup(
        `<div class="p-2">  
           <h3 class="font-bold">${job.company?.name}</h3>
           <p>${job.title}</p>
           <p class="text-sm">${job.address}</p>
         </div>`
      );
      marker.addTo(map);
      markers.push(marker);
    }
  }

  onMount(async () => {
    if (!browser) return;

    // Dynamically import Leaflet
    const imported = await import('leaflet');
    L = imported;

    // Initialize the map
    map = L.map('map').setView([59.3293, 18.0686], 6);

    // Add tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    // Force Leaflet to recalc size once CSS is applied
    await tick();
    map.invalidateSize();

    // Redraw on zoom end
    map.on('zoomend', updateMarkers);

    // Initial draw
    updateMarkers();
  });

  onDestroy(() => {
    map?.remove();
  });

  // Redraw when jobs change
  $: if (browser && map) updateMarkers();
</script>

<style>
  #map {
    width: 100%;
    height: 100%;
    min-height: 300px;
  }
  .cluster-marker {
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .logo-marker img {
    box-shadow: 0 2px 4px rgba(0,0,0,0.2);
  }
</style>

<div id="map"></div>