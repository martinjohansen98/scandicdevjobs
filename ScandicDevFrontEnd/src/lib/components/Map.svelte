<script lang="ts">
  import { browser } from '$app/environment';
  import { onMount, onDestroy, tick } from 'svelte';
  import 'leaflet/dist/leaflet.css';
  import type { Joblisting } from '$lib/stores/JobListing';
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
      html: `<img src="${url}" class="w-8 h-8 rounded-full border-2 border-white object-cover"/>`,
      iconSize: [32, 32]
    });
  }

  function updateMarkers() {
    if (!map) return;
    markers.forEach(m => m.remove());
    markers = [];

    for (const job of jobs) {
      const icon = map.getZoom() >= 12
        ? createLogoIcon(job.company?.companyLogoUrl ?? '')
        : createClusterIcon();

      const marker = L.marker([job.latitude, job.longitude], { icon });
      marker.bindPopup(
        `<div class="p-2">  
           <h3 class="font-bold">${job.title}</h3>
           <p>${job.company?.name}</p>
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