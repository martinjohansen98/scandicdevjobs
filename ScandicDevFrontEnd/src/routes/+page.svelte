  <script lang="ts">
    import Navbar from '$lib/components/Navbar.svelte';
    import Map from '$lib/components/Map.svelte';
    import JobSection from '$lib/components/JobSection.svelte';
  </script>
  
  <div class="flex flex-col h-screen">
    <Navbar />
    <div class="flex flex-grow">
      <!-- Left: Job Section (60% width) -->
      <div class="w-3/5">
        <JobSection />
      </div>
      <!-- Right: Map (40% width) -->
      <div class="w-2/5 h-full relative z-0">
        <Map />
      </div>
    </div>
  </div>
  