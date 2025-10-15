<script lang="ts">
  import type { Joblisting } from '$lib/types/JobListing';
  import { programmingLanguages } from '$lib/constants/programmingLanguages';

  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

  export let jobs: Joblisting[] = [];
  export let onOpen: (arg: { job: Joblisting }) => void = () => {};
  export let onClose: () => void = () => {};

  export function open(job: Joblisting) { openDetail(job); }
  export function close() { closeDetail(); }

  let selected: Joblisting | null = null;
  let detailOpen = false;
  let domainTags = {}; // embedded, cloud, iot, web, gameDev,
  let activeTopFilters: string[] = [];

  function toggleTopFilter(label: string) {
    activeTopFilters = activeTopFilters.includes(label)
      ? activeTopFilters.filter(l => l !== label)
      : [...activeTopFilters, label];
    currentPage = 1;
  }

  // City Filters
  let cities = ['Stockholm', 'Gothenburg', 'Malmo', 'Uppsala'];
  let activeCity = 'all';
  function selectCity(city: string) {
    activeCity = city;
    currentPage = 1;
  }

  // Dynamic Pagination Setup
  let currentPage = 1;
  const jobsPerPageFallback = 6;
  let jobContainer: HTMLElement;
  let maxJobs = 0;

  $: filteredJobs = jobs.filter((job) => {
    const matchesCity = activeCity === 'all' || job.city === activeCity;

    const matchesTopFilter =
      activeTopFilters.length === 0 ||
      activeTopFilters.some(lbl =>
        job.tags?.some(t => t.name?.toLowerCase() === lbl.toLowerCase())
      );

    return matchesCity && matchesTopFilter;
  });

  $: effectiveMaxJobs = maxJobs > 0 ? maxJobs : jobsPerPageFallback;
  $: totalPages = Math.ceil(filteredJobs.length / effectiveMaxJobs);
  $: displayedJobs = filteredJobs.slice(
    (currentPage - 1) * effectiveMaxJobs,
    (currentPage - 1) * effectiveMaxJobs + effectiveMaxJobs
  );

  function goToPage(page: number) {
    currentPage = page;
  }

  function openDetail(job: Joblisting) {
    selected = job;
    detailOpen = true;
    onOpen({ job });
  }

  function closeDetail() {
    detailOpen = false;
    selected = null;
    onClose();
  }

  function onCardKeydown(e: KeyboardEvent, job: Joblisting) {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      openDetail(job);
    }
  }

  function jobImageSrc(job: Joblisting) {
    if (job.logoFileId) {
      return API_BASE_URL
        ? `${API_BASE_URL}/api/blob/logo/${job.logoFileId}`
        : `/api/blob/logo/${job.logoFileId}`;
    }
    return '/placeholder-company.svg';
  }
</script>

<!-- Tag filters -->
<div class="job-section flex flex-col h-full relative">
  <header class="p-4 bg-base-200">
    <nav class="flex flex-wrap gap-2 mb-2">
      <div class="w-[calc(100%-0.5rem)] ">
        <div class="flex flex-wrap gap-2">
          {#each programmingLanguages as filter}
            <button
              on:click={() => toggleTopFilter(filter.label)}
              class="flex items-center p-2 rounded  transition-colors duration-200 w-15 justify-center {activeTopFilters.includes(filter.label) ? 'bg-blue-500 text-white' : ''}">
              
              <div class="flex flex-col">
                <img src={filter.icon} alt={filter.label} class="m-auto w-8 h-8 mb-1" />
                <span class="text-xs text-blue-200">{filter.label}</span>
              </div>
            </button>
          {/each}
        </div>
      </div>
    </nav>

    <!-- City Filters -->
    <nav class="flex space-x-4">
      <button
        on:click={() => selectCity('all')}
        class="p-2 rounded border transition-colors duration-200 {activeCity === 'all' ? 'bg-blue-500 text-white' : 'bg-blue text-blue-200'}">
        All Cities
      </button>
      {#each cities as city}
        <button
          on:click={() => selectCity(city)}
          class="p-2 rounded border transition-colors duration-200 {activeCity === city ? 'bg-blue-500 text-white' : 'bg-blue text-blue-200'}">
          {city}
        </button>
      {/each}
    </nav>
  </header>

  <h1 class="text-xl m-4"> {activeTopFilters.length >= 1 ? 'Filtered Joblistings:' : 'All Joblistings:'}</h1>

  <!-- Job listing popup -->
  <main class="flex flex-col flex-grow p-4">
    {#if detailOpen && selected}
      <button
        type="button"
        class="absolute inset-0 z-20 bg-base-300/40 backdrop-blur-sm"
        aria-label="Close details"
        on:click={closeDetail}
        tabindex="-1"
      ></button>

    <section class="absolute inset-0 z-30 p-4">
      <div class="h-full w-full bg-base-100 rounded-xl shadow-xl overflow-auto">
        <header class="flex items-center gap-4 p-4 border-b">
          {#if selected.logoFileId}
            <img
              src={API_BASE_URL ? `${API_BASE_URL}/api/blob/logo/${selected.logoFileId}` : `/api/blob/logo/${selected.logoFileId}`}
              alt="logo"
              class="w-16 h-16 rounded object-cover"
              on:error={(e)=>{ const img = e.currentTarget as HTMLImageElement; img.onerror=null; img.style.display='none'; }}
            />
          {/if}

          <div class="flex-1">
            <h2 class="text-2xl font-semibold leading-tight">{selected.title}</h2>
            <p class="text-sm opacity-70">
              {selected.city}{selected.city && selected.country ? ', ' : ''}{selected.country}
            </p>
          </div>

          <button class="btn btn-sm btn-ghost" on:click={closeDetail} aria-label="Close">✕</button>
        </header>

        <div class="p-4 space-y-4">
          {#if selected.description}
            <p class="whitespace-pre-line">{selected.description}</p>
          {/if}

          <div class="flex flex-wrap gap-2">
            {#each selected.tags ?? [] as tag}
              <span class="badge badge-outline">{tag.name}</span>
            {/each}
          </div>

          {#if selected.salaryRangeMin || selected.salaryRangeMax}
            <div class="text-lg font-semibold">
              {selected.salaryRangeMin} – {selected.salaryRangeMax} {selected.currency}
            </div>
          {/if}

          <div class="flex gap-2">
            {#if selected.applicationUrl}
              <a class="btn btn-primary" href={selected.applicationUrl} target="_blank" rel="noreferrer">Apply</a>
            {/if}
            {#if selected.contactEmail}
              <a class="btn" href={`mailto:${selected.contactEmail}`}>Email</a>
            {/if}
          </div>
        </div>
      </div>
    </section>
  {/if}


    <!-- Job Listings Container -->
    <div class="job-listings flex-1 overflow-hidden" bind:this={jobContainer}>
      {#if displayedJobs.length > 0}
        {#each displayedJobs as job}

          <div
            class="job-card bg-base-300 flex items-center rounded-lg shadow p-4 h-[96px] mb-2 cursor-pointer focus:outline-none focus:ring-2 focus:ring-primary"
            role="button"
            tabindex="0"
            on:click={() => openDetail(job)}
            on:keydown={(e) => onCardKeydown(e, job)}
          >

            <div class="mr-4">
              <div class="w-16 h-16 rounded-lg overflow-hidden flex items-center justify-center bg-base-200">
                {#if job.logoFileId}
                  <img
                    src={jobImageSrc(job)}
                    alt={`${job.title ?? 'Job'} image`}
                    class="w-20 h-20 object-cover object-center"
                    loading="lazy"
                    on:error={(e)=>{ const img = e.currentTarget as HTMLImageElement; img.onerror=null; img.style.display='none'; }}
                  />
                {/if}
              </div>
            </div>
            
            <div class="flex-grow min-w-0">
              <h2 class="text-xl font-semibold truncate">{job.title}</h2>

              <div class="mt-1 flex items-center gap-4 text-sm opacity-80">
                {#if job.company?.name}
                  <span class="inline-flex items-center gap-1.5 min-w-0">
                    <svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="1.5"
                        stroke-linecap="round" stroke-linejoin="round"
                        class="w-4 h-4 text-base-content/70 shrink-0" aria-hidden="true">
                      <path d="M9 7V5a3 3 0 0 1 3-3h0a3 3 0 0 1 3 3v2" />
                      <rect x="3" y="7" width="18" height="12" rx="2" />
                      <path d="M3 12h18" />
                    </svg>
                    <span class="truncate">{job.company.name}</span>
                  </span>
                {/if}

                {#if job.country || job.city}
                  <span class="inline-flex items-center gap-1.5 min-w-0">
                    <svg xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24" fill="none"
                        stroke="currentColor" stroke-width="1.5"
                        stroke-linecap="round" stroke-linejoin="round"
                        class="w-4 h-4 text-base-content/70 shrink-0" aria-hidden="true">
                      <path d="M15 10.5a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
                      <path d="M19.5 10.5c0 7.142-7.5 11.25-7.5 11.25S4.5 17.642 4.5 10.5a7.5 7.5 0 1 1 15 0Z" />
                    </svg>

                    <span class="truncate">
                      {job.country}{job.country && job.city ? ', ' : ''}{job.city}
                    </span>
                  </span>
                {/if}
              </div>
            </div>

            <div class="flex flex-col items-end">
              <span class="text-lg font-bold">{job.salaryRangeMin} - {job.salaryRangeMax} {job.currency}</span>
              <div class="mt-2 flex flex-wrap gap-1">
                {#each job.tags ?? [] as tag}
                  <span class="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">{tag.name}</span>
                {/each}
              </div>
            </div>
          </div>
        {/each}
      {:else}
        <div class="text-center py-8">No job listings found.</div>
      {/if}
    </div>

    <!-- Pagination Controls -->
    {#if totalPages > 1}
      <div class="pagination mt-4 flex justify-center space-x-2">
        {#each Array(totalPages) as _, index}
          <button
            on:click={() => goToPage(index + 1)}
            class="px-3 py-1 border rounded transition-colors duration-200 {currentPage === index + 1 ? 'bg-blue-500 text-white' : 'bg-white text-gray-800'}">
            {index + 1}
          </button>
        {/each}
      </div>
    {/if}
  </main>
</div>

<style>
  .job-section {
    height: 100%;
  }
  .job-listings {
    flex: 1;
  }
</style>
