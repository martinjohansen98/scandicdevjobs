<script lang="ts">
    //import { onMount } from 'svelte';
    import type { Joblisting } from '$lib/types/JobListing';
    const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
    const API_BASE_URL_BLOB = import.meta.env.VITE_API_BASE_URL_BLOB;

    export let jobs: Joblisting[] = [];
    // Removed duplicate declaration of jobs
    /* --- Top Level Filters --- */

    // TODO get tags fropm DB
    // Topfilters should be replaced,
    // we want to group the tags by Tag Category and have one dropdown for each
    // TODO extend Tag in backend with imageId / image url / unicode caracter

    // programing languages
    
    // technologies, DevOps, security, Testautomation, docker, kubernetes etc
    // Cloud?
    // Design?
    // Business?

    let topFilters = [
      { id: 0,        label: 'Java',      icon: '☕' },
      { id: 1,        label: 'C#/.NET',   icon: '🔷' },
      { id: 0,        label: 'C/C++',     icon: '💻' },
      { id: 0,        label: 'PHP',       icon: '🐘' },
      { id: 0,        label: 'Ruby',      icon: '💎' },
      { id: 5,        label: 'Python',    icon: '🐍' },
      { id: 0,        label: 'IT',        icon: '🖥️' },
      { id: 0,        label: 'Rust',      icon: '🦀' },
      { id: 0,        label: 'Manager',   icon: '👔' },
      { id: 0,        label: 'Network',   icon: '🌐' },
      { id: 0,        label: 'Security',  icon: '🔒' },
      { id: 0,        label: 'Blockchain',icon: '⛓️' },
      { id: 0,        label: 'UX/UI',     icon: '🎨' },
      { id: 0,        label: 'Business',  icon: '💼' },
      { id: 0,        label: 'Support',   icon: '🛠️' },
      { id: 0,        label: 'Architect', icon: '📐' },
      { id: 0,        label: 'Data',      icon: '📊' },
      { id: 0,        label: 'AI/ML',     icon: '🤖' },
      { id: 0,        label: 'DevOps',    icon: '⚙️' },
      { id: 0,        label: 'System',    icon: '🔧' },
      { id: 0,        label: 'GameDev',   icon: '🎮' },
      { id: 0,        label: 'QA/Test',   icon: '🧪' }
    ];
    let activeTopFilters: number[] = [];
    function toggleTopFilter(id: number) {
      if (activeTopFilters.includes(id)) {
        activeTopFilters = activeTopFilters.filter(f => f !== id);
      } else {
        activeTopFilters = [...activeTopFilters, id];
      }
      currentPage = 1;
    }
  
    /* --- Lower Level Filter (Cities) --- */
    let cities = ['Stockholm', 'Gothenburg', 'Malmo', 'Uppsala'];
    let activeCity = 'all';
    function selectCity(city: string) {
      activeCity = city;
      currentPage = 1;
    }
  
    /* --- Dynamic Pagination Setup --- */
    let currentPage = 1;
    const jobsPerPageFallback = 5; // fallback value if container height isn't measured yet
    let jobContainer: HTMLElement;
    const cardHeight = 112; // in pixels; adjust as needed
    let maxJobs = 0;
  
    function recalcMaxJobs() {
      if (jobContainer) {
        maxJobs = Math.floor(jobContainer.clientHeight / cardHeight);
        if (currentPage > totalPages) {
          currentPage = 1;
        }
      }
    }
  
    $: filteredJobs = jobs.filter((job) => {
      const matchesCity = activeCity === 'all' || job.city === activeCity;
      const matchesTopFilter =
        activeTopFilters.length === 0 ||
        activeTopFilters.some(filter => job.tags?.map(x => x.id).includes(filter));
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
  </script>
  
  <!--TODO förslag vi sparar en siffra som motsvarar % av alla filter och försöker matcha så minst % av alla valda filter är inkluderade,
  denna n% kan följa en algoritm så att ju fler filter man klickar i ju färre bör vara inkluderade, 
  med detta kan vi hävda vi har en nytänkande algoritm för att matcha kandidater med jobblistings XD
  exempel:
  1 -> 1
  2 => 1
  3 => 1
  4 => 2
  5 => 2
  6 => 3
  7 => 3
  8 => 3
  9 => 4
  10 => 4
  10+ => 5
  om för få jobb hittas >25 så sänker vi siffran ett steg tills antal jobb är minst 25 / 2 sidor av jobblistings
  -->
  <div class="job-section flex flex-col h-full">
    <!-- Header & Filters -->
    <header class="p-4 bg-base-200">
      <h1 class="text-xl mb-4">Current job listings</h1>
      <!-- Top Level Filters -->
      <nav class="flex flex-wrap gap-2 mb-2">
        {#each topFilters as filter}
          <button
            on:click={() => toggleTopFilter(filter.id)}
            class="flex items-center p-2 rounded border transition-colors duration-200 {activeTopFilters.includes(filter.id) ? 'bg-blue-500 text-white' : 'bg-white text-gray-800'}">
            <span class="mr-1">{filter.icon}</span>
            <span>{filter.label}</span>
          </button>
        {/each}
      </nav>
      <!-- City Filters -->
      <nav class="flex space-x-4">
        <button
          on:click={() => selectCity('all')}
          class="p-2 rounded border transition-colors duration-200 {activeCity === 'all' ? 'bg-blue-500 text-white' : 'bg-white text-gray-800'}">
          All Cities
        </button>
        {#each cities as city}
          <button
            on:click={() => selectCity(city)}
            class="p-2 rounded border transition-colors duration-200 {activeCity === city ? 'bg-blue-500 text-white' : 'bg-white text-gray-800'}">
            {city}
          </button>
        {/each}
      </nav>
    </header>
  
    <!-- Main content: Job Listings & Pagination -->
    <main class="flex flex-col flex-grow p-4">
      <!-- Job Listings Container -->
      <div class="job-listings flex-1 overflow-hidden" bind:this={jobContainer}>
        {#if displayedJobs.length > 0}
          {#each displayedJobs as job}
 
            <div class="job-card bg-base-300 flex items-center rounded-lg shadow p-4 h-[96px] mb-2">
              <!-- Company Logo -->
              <div class="mr-4">
                <!-- TODO DEFAULT LOGO -->

                <img src= "{API_BASE_URL_BLOB}{job.company?.companyLogoGuid}" alt="{job.title}" class="w-16 h-16 object-contain" /> 
              </div>
              <!-- Job Information -->
              <div class="flex-grow">
                <h2 class="text-xl font-semibold">{job.title}</h2>
                <p class="text-gray-600">{job.description}</p>
              </div>
              <!-- Salary Range and Tags -->
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
    /* Additional styling can be added here */
  </style>
  