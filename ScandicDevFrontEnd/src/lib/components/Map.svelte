<script lang="ts">
  import { browser } from '$app/environment';
  import { onMount, onDestroy, tick } from 'svelte';
  import 'leaflet/dist/leaflet.css';
  import type { Joblisting } from '$lib/types/JobListing';

  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
  export let jobs: Joblisting[] = [];
  export let onSelect: (arg: { job: Joblisting }) => void = () => {};

  let map: any, L: any, layer: any;
  const markersById = new Map<number, any>();

  const logoUrl = (j: Joblisting) =>
    j.logoFileId
      ? (API_BASE_URL ? `${API_BASE_URL}/api/blob/logo/${j.logoFileId}` : `/api/blob/logo/${j.logoFileId}`)
      : null;

  const DEFAULT_BOUNDS_SCAN = [
    [54.2, 3.5],
    [71.2, 27.5]
  ];

  function setDefaultView(fly = false) {
    if (!map) return;
    const options = { maxZoom: 6, duration: 0.6 };
    const bounds = L.latLngBounds(DEFAULT_BOUNDS_SCAN as any);
    fly ? map.flyToBounds(bounds, options) : map.fitBounds(bounds, options);
  }

  function popupHtml(j: Joblisting) {
    const company = j.company?.name ?? '';
    const place = [j.city, j.country].filter(Boolean).join(', ');
    return `
      <div class="p-2">
        <h3 class="font-bold">${j.title ?? ''}</h3>
        <p>${company}</p>
        <p class="text-sm opacity-70">${place}</p>
      </div>
    `;
  }

  function createClusterIcon() {
    return L.divIcon({
      className: 'cluster-marker',
      html: `<div class="w-3.5 h-3.5 bg-blue-500 rounded-full border-2 border-white shadow"></div>`,
      iconSize: [20, 20],
      iconAnchor: [10, 10],
      popupAnchor: [0, -12]
    });
  }

  function createLogoIcon(url: string | null) {
    if (!url) return createClusterIcon();
    return L.divIcon({
      className: 'logo-marker',
      html: `<div class="w-10 h-10 rounded-full border-2 border-white overflow-hidden flex items-center justify-center bg-white shadow">
              <img src="${url}" class="w-20 h-20 object-cover object-center"/>
            </div>`,
      iconSize: [40, 40],
      iconAnchor: [20, 20],
      popupAnchor: [0, -16]
    });
  }

  const validCoords = (j: Joblisting) =>
    j.latitude != null && j.longitude != null && !Number.isNaN(j.latitude) && !Number.isNaN(j.longitude);

  function markerIconFor(j: Joblisting) {
    const z = map?.getZoom?.() ?? 0;
    return createLogoIcon(z >= 6 ? logoUrl(j) : null);
  }

  let hasUserMoved = false;
  let initialFitDone = false;
  let isFocused = false;

  function syncMarkers({ fit = false } = {}) {
    if (!map || !layer) return;

    for (const [id, m] of markersById) {
      const j = jobs.find(x => x.id === id);
      if (!j || !validCoords(j)) {
        layer.removeLayer(m);
        markersById.delete(id);
      }
    }

    // add/update
    const bounds = L.latLngBounds([]);
    
    for (const j of jobs) {
      if (!validCoords(j)) continue;
      const ll = L.latLng(j.latitude, j.longitude);
      bounds.extend(ll);

      let marker = markersById.get(j.id);
      const icon = markerIconFor(j);
      const html = popupHtml(j);

      if (!marker) {
        marker = L.marker(ll, { icon, title: j.title ?? '' })
        .on('click',  () => {
          onSelect({ job: j });
          this.openPopup();
        })
        .bindPopup(html, {
          closeButton: true,
          autoPan: true,
          offset: L.point(0, -8)
        });

        marker.addTo(layer);
        markersById.set(j.id, marker);
      } else {
        marker.setLatLng(ll);
        marker.setIcon(icon);
      }
    }

      if (fit && bounds.isValid()) {
        map.fitBounds(bounds.pad(0.2), { maxZoom: 11 });
        initialFitDone = true;
      }
  }

  function refreshIcons() {
    for (const [id, m] of markersById) {
      const j = jobs.find(x => x.id === id);
      if (!j) continue;
      m.setIcon(markerIconFor(j));
    }
  }

  function ensureMarker(j: Joblisting) {
    if (!validCoords(j)) return null;

    let marker = markersById.get(j.id);
    const ll = L.latLng(j.latitude, j.longitude);

    if (!marker) {
      const icon = markerIconFor(j);
      const html = popupHtml(j);

      marker = L.marker(ll, { icon, title: j.title ?? '' })
        .on('click', () => {
          onSelect({ job: j });
          marker!.openPopup();
        })
        .bindPopup(html, {
          closeButton: true,
          autoPan: true,
          offset: L.point(0, -8)
        });

      marker.addTo(layer);
      markersById.set(j.id, marker);
    } else {
      marker.setLatLng(ll);
      marker.setIcon(markerIconFor(j));
      marker.setPopupContent(popupHtml(j));
    }
    return marker;
  }

  export function focusOn(job: Joblisting, zoom = 13) {
    if (!map || !validCoords(job)) return;
    isFocused = true;

    const m = ensureMarker(job);
    map.flyTo([job.latitude!, job.longitude!], zoom, { duration: 0.6 });
    if (m) m.openPopup();
  }

  export function resetView() {
    if (!map) return;
    isFocused = false;
    const b = L.latLngBounds([]);
    for (const j of jobs) if (validCoords(j)) b.extend([j.latitude, j.longitude]);
    if (b.isValid()) map.flyToBounds(b.pad(0.2), { duration: 0.6, maxZoom: 11 });
    setDefaultView(true);
  }

  onMount(async () => {
    if (!browser) return;
    const imported = await import('leaflet');
    L = imported;

    map = L.map('map').setView([59.3293, 18.0686], 6);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    layer = L.layerGroup().addTo(map);

    await tick();
    map.invalidateSize();
    map.on('zoomstart moveStart movestart', () => (hasUserMoved = true));
    map.on('zoomend moveend', () => refreshIcons());

    setDefaultView(false);
    initialFitDone = true;
    syncMarkers({ fit: false });
  });

  onDestroy(() => {
    map?.remove();
    markersById.clear();
  });

  $: if (browser && map) {
    syncMarkers({ fit: false });
  }
</script>

<style>
  #map { width: 100%; height: 100%; min-height: 300px; }
</style>

<div id="map"></div>
