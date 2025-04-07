<!-- Map.svelte -->
<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import 'leaflet/dist/leaflet.css';
    import type { Joblisting } from '$lib/types/Joblisting';
    import { jobList, jobListLoading, jobListError, fetchJobs } from '$lib/stores/jobListingStore';
  
    // Default dummy job data for testing.
    // In a real application, this would be passed in as a prop.
    export let jobs: Joblisting[] = [];
  
    // We'll load Leaflet dynamically on the client.
    let map: any;
    // Explicitly type markers as an array using the return type of L.marker.
    let markers: ReturnType<typeof L.marker>[] = [];
    let L: any; // This will hold the Leaflet module once loaded.
  
    // Helper to create a blue bubble icon (for zoomed out view).
    const createClusterIcon = () =>
      L.divIcon({
        className: 'cluster-marker',
        html: `<div class="w-6 h-6 bg-blue-500 rounded-full border-2 border-white"></div>`,
        iconSize: [24, 24]
      });
  
    // Helper to create a company logo icon (for zoomed in view).
    const createLogoIcon = (url: string) =>
      L.divIcon({
        className: 'logo-marker',
        html: `<img src="${url}" class="w-8 h-8 rounded-full border-2 border-white object-cover" />`,
        iconSize: [32, 32]
      });
  
    // Function to update markers on the map.
    const updateMarkers = () => {
      // Remove existing markers.
      markers.forEach(marker => marker.removeFrom(map));
      markers = [];
  
      // Create and add a new marker for each job.
      jobs.forEach(job => {
        const marker = L.marker([job.latitude, job.longitude], {
          icon: map.getZoom() >= 12 ? createLogoIcon(job.company?.companyLogoUrl ?? "") : createClusterIcon()
        });
        marker.bindPopup(`
          <div class="p-2">
            <h3 class="font-bold">${job.title}</h3>
            <p>${job.company}</p>
            <p class="text-sm">${job.address}</p>
          </div>
        `);
        marker.addTo(map);
        markers.push(marker);
      });
    };
  
    onMount(async () => {
      fetchJobs();
      jobList.subscribe(value => {
            jobs = value; // ✅ Sync store data with local variable
        });
      // Dynamically import Leaflet (runs only on the client).
      const LModule = await import('leaflet');
      L = LModule.default;
  
      // Initialize the map (centered on Sweden).
      map = L.map('map').setView([59.3293, 18.0686], 6);
  
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
      }).addTo(map);
  
      // Update markers when the zoom level changes.
      map.on('zoomend', updateMarkers);
      updateMarkers();
    });
  
    onDestroy(() => {
      if (map) map.remove();
    });
  </script>
  
  <style>
    /* Ensure the #map div fills its container */
    #map {
      height: 100%;
      width: 100%;
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
  