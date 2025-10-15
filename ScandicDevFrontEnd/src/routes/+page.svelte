<script lang="ts">
  import { onMount } from 'svelte';
  import type { Joblisting } from '$lib/types/JobListing';
  import Navbar from '$lib/components/Navbar.svelte';
  import JobSection from '$lib/components/JobSection.svelte';
  import { jobList, fetchJobs } from '$lib/stores/jobListingStore';

  export let data: { jobs: Joblisting[] };

  let MapComp: typeof import('$lib/components/Map.svelte').default | null = null;
  let jobs: Joblisting[] = data.jobs;

  let jobSectionRef: any = null;
  let mapRef: any = null;

onMount(() => {
    fetchJobs();
    const unsub = jobList.subscribe(v => (jobs = v));

    (async () => {
      const module = await import('$lib/components/Map.svelte');
      MapComp = module.default;
    })();

    return () => unsub();
  });
  
  function handleJobOpen({ job }: { job: Joblisting }) {
    mapRef?.focusOn?.(job, 13);
  }
  function handleJobClose() {
    mapRef?.resetView?.();
  }
  function handleMarkerSelect({ job }: { job: Joblisting }) {
    jobSectionRef?.open?.(job);
  }
</script>

<div class="flex flex-col h-screen">
  <Navbar />
  <div class="flex flex-grow">
    <div class="w-3/5">
      <JobSection
        bind:this={jobSectionRef}
        {jobs}
        onOpen={handleJobOpen}
        onClose={handleJobClose}
      />
    </div>
    <div class="w-2/5 h-full relative z-0">
      {#if MapComp}
        <svelte:component
          this={MapComp}
          bind:this={mapRef}
          {jobs}
          onSelect={handleMarkerSelect}
        />
      {:else}
        <div class="flex items-center justify-center h-full">
          <p>Loading map...</p>
        </div>
      {/if}
    </div>
  </div>
</div>
