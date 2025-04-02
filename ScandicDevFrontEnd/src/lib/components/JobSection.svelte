<script lang="ts">
    import { onMount } from 'svelte';
    import type { JobListing } from '$lib/types/JobListing';
    import { jobList, jobListLoading, jobListError, fetchJobs } from '$lib/stores/jobListingStore';


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
      { id: 'Java',       label: 'Java',      icon: '☕' },
      { id: 'C/C++',      label: 'C/C++',     icon: '💻' },
      { id: 'C#/.NET',    label: 'C#/.NET',   icon: '🔷' },
      { id: 'PHP',        label: 'PHP',       icon: '🐘' },
      { id: 'Ruby',       label: 'Ruby',      icon: '💎' },
      { id: 'Python',     label: 'Python',    icon: '🐍' },
      { id: 'IT',         label: 'IT',        icon: '🖥️' },
      { id: 'Rust',       label: 'Rust',      icon: '🦀' },
      { id: 'Manager',    label: 'Manager',   icon: '👔' },
      { id: 'Network',    label: 'Network',   icon: '🌐' },
      { id: 'Security',   label: 'Security',  icon: '🔒' },
      { id: 'Blockchain', label: 'Blockchain',icon: '⛓️' },
      { id: 'UX/UI',      label: 'UX/UI',     icon: '🎨' },
      { id: 'Business',   label: 'Business',  icon: '💼' },
      { id: 'Support',    label: 'Support',   icon: '🛠️' },
      { id: 'Architect',  label: 'Architect', icon: '📐' },
      { id: 'Data',       label: 'Data',      icon: '📊' },
      { id: 'AI/ML',      label: 'AI/ML',     icon: '🤖' },
      { id: 'DevOps',     label: 'DevOps',    icon: '⚙️' },
      { id: 'System',     label: 'System',    icon: '🔧' },
      { id: 'GameDev',    label: 'GameDev',   icon: '🎮' },
      { id: 'QA/Test',    label: 'QA/Test',   icon: '🧪' }
    ];
    let activeTopFilters: string[] = [];
    function toggleTopFilter(id: string) {
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


    

    let jobs: JobListing[] = [];

    /* --- Dummy Job Listings Data --- */
    
  
    // let jobs: Job[] = [
    //   {
    //     id: 1,
    //     title: 'Frontend Developer',
    //     city: 'Stockholm',
    //     category: 'tech',
    //     description: 'Develop engaging user interfaces with modern frameworks.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/2/2f/Google_2015_logo.svg',
    //     salaryRange: '$50k - $70k',
    //     tags: ['JavaScript', 'React'],
    //     filterJob: ['Java', 'Python'],
    //     address: 'Sveavägen 1, Stockholm, Sweden',
    //     lat: 59.33258,
    //     lng: 18.0649
    //   },
    //   {
    //     id: 2,
    //     title: 'UI/UX Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Design delightful digital experiences that users love.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$40k - $60k',
    //     tags: ['Sketch', 'Figma'],
    //     filterJob: ['UX/UI', 'Business'],
    //     address: 'Östra Hamngatan 10, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 3,
    //     title: 'Digital Marketer',
    //     city: 'Malmo',
    //     category: 'marketing',
    //     description: 'Promote our brand and reach new audiences across channels.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['SEO', 'Content'],
    //     filterJob: ['Business', 'Manager'],
    //     address: 'Stortorget, Malmo, Sweden',
    //     lat: 55.6050,
    //     lng: 13.0038
    //   },
    //   {
    //     id: 4,
    //     title: 'Backend Developer',
    //     city: 'Uppsala',
    //     category: 'tech',
    //     description: 'Build robust server-side applications and APIs.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/0/05/Facebook_Logo_%282019%29.png',
    //     salaryRange: '$55k - $75k',
    //     tags: ['API', '.NET Framework'],
    //     filterJob: ['C#/.NET', 'C/C++'],
    //     address: 'Universitetsvägen 1, Uppsala, Sweden',
    //     lat: 59.8586,
    //     lng: 17.6389
    //   },
    //   {
    //     id: 5,
    //     title: 'Full Stack Engineer',
    //     city: 'Stockholm',
    //     category: 'tech',
    //     description: 'Work on both front and backend tasks in a dynamic environment.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/2/2f/Google_2015_logo.svg',
    //     salaryRange: '$60k - $80k',
    //     tags: ['Node.js', 'GraphQL'],
    //     filterJob: ['Java', 'C/C++', 'Python'],
    //     address: 'Sergels Torg, Stockholm, Sweden',
    //     lat: 59.3293,
    //     lng: 18.0686
    //   },
    //   {
    //     id: 7,
    //     title: 'Product Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Create intuitive product designs that drive engagement.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['Adobe XD', 'Illustrator'],
    //     filterJob: ['UX/UI', 'Business'],
    //     address: 'Kungstorget, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 8,
    //     title: 'Product Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Create intuitive product designs that drive engagement.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['Adobe XD', 'Illustrator'],
    //     filterJob: ['UX/UI', 'QA/Test'],
    //     address: 'Kungstorget, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 9,
    //     title: 'Product Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Create intuitive product designs that drive engagement.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['Adobe XD', 'Illustrator'],
    //     filterJob: ['UX/UI', 'QA/Test'],
    //     address: 'Kungstorget, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 10,
    //     title: 'Product Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Create intuitive product designs that drive engagement.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['Adobe XD', 'Illustrator'],
    //     filterJob: ['UX/UI', 'QA/Test'],
    //     address: 'Kungstorget, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 11,
    //     title: 'Product Designer',
    //     city: 'Gothenburg',
    //     category: 'design',
    //     description: 'Create intuitive product designs that drive engagement.',
    //     companyLogo: 'https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg',
    //     salaryRange: '$45k - $65k',
    //     tags: ['Adobe XD', 'Illustrator'],
    //     filterJob: ['UX/UI', 'QA/Test'],
    //     address: 'Kungstorget, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   // Additional jobs demonstrating new filters:
    //   {
    //     id: 12,
    //     title: 'Network Engineer',
    //     city: 'Stockholm',
    //     category: 'tech',
    //     description: 'Design and manage complex network infrastructures.',
    //     companyLogo: 'https://via.placeholder.com/64?text=Network',
    //     salaryRange: '$55k - $75k',
    //     tags: ['Networking'],
    //     filterJob: ['Network', 'Security'],
    //     address: 'Drottninggatan 2, Stockholm, Sweden',
    //     lat: 59.3326,
    //     lng: 18.0649
    //   },
    //   {
    //     id: 13,
    //     title: 'Blockchain Developer',
    //     city: 'Gothenburg',
    //     category: 'tech',
    //     description: 'Develop decentralized applications and smart contracts.',
    //     companyLogo: 'https://via.placeholder.com/64?text=Blockchain',
    //     salaryRange: '$65k - $85k',
    //     tags: ['Blockchain', 'Solidity'],
    //     filterJob: ['Blockchain', 'Python'],
    //     address: 'Västra Hamngatan, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   },
    //   {
    //     id: 14,
    //     title: 'DevOps Engineer',
    //     city: 'Malmo',
    //     category: 'tech',
    //     description: 'Implement continuous integration and deployment pipelines.',
    //     companyLogo: 'https://via.placeholder.com/64?text=DevOps',
    //     salaryRange: '$60k - $80k',
    //     tags: ['AWS', 'CI/CD'],
    //     filterJob: ['DevOps', 'System', 'C/C++'],
    //     address: 'Lilla Varvsgatan, Malmo, Sweden',
    //     lat: 55.6050,
    //     lng: 13.0038
    //   },
    //   {
    //     id: 15,
    //     title: 'Data Scientist',
    //     city: 'Uppsala',
    //     category: 'tech',
    //     description: 'Analyze large datasets to derive actionable insights.',
    //     companyLogo: 'https://via.placeholder.com/64?text=Data',
    //     salaryRange: '$70k - $90k',
    //     tags: ['Python', 'R'],
    //     filterJob: ['Data', 'AI/ML', 'Python'],
    //     address: 'Carolina Rediviva, Uppsala, Sweden',
    //     lat: 59.8586,
    //     lng: 17.6389
    //   },
    //   {
    //     id: 16,
    //     title: 'Game Developer',
    //     city: 'Stockholm',
    //     category: 'tech',
    //     description: 'Develop engaging games for various platforms.',
    //     companyLogo: 'https://via.placeholder.com/64?text=GameDev',
    //     salaryRange: '$60k - $80k',
    //     tags: ['Unity', 'C#'],
    //     filterJob: ['GameDev', 'C/C++'],
    //     address: 'Birger Jarlsgatan, Stockholm, Sweden',
    //     lat: 59.33258,
    //     lng: 18.0649
    //   },
    //   {
    //     id: 17,
    //     title: 'QA Tester',
    //     city: 'Gothenburg',
    //     category: 'tech',
    //     description: 'Ensure quality through rigorous testing and automation.',
    //     companyLogo: 'https://via.placeholder.com/64?text=QA',
    //     salaryRange: '$40k - $60k',
    //     tags: ['Selenium', 'JMeter'],
    //     filterJob: ['QA/Test', 'Support'],
    //     address: 'Kungsportsavenyen, Gothenburg, Sweden',
    //     lat: 57.7089,
    //     lng: 11.9746
    //   }
    // ];
  
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
  
    onMount(() => {
      fetchJobs();
      jobList.subscribe(value => {
            jobs = value; // ✅ Sync store data with local variable
        });
      recalcMaxJobs();
      window.addEventListener('resize', recalcMaxJobs);
      return () => window.removeEventListener('resize', recalcMaxJobs);
    });
  
    $: filteredJobs = jobs.filter((job) => {
      const matchesCity = activeCity === 'all' || job.city === activeCity;
      const matchesTopFilter =
        activeTopFilters.length === 0 ||
        activeTopFilters.some(filter => job.filterJob.includes(filter));
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
            <div class="job-card flex items-center bg-white rounded-lg shadow p-4 h-[96px] mb-2">
              <!-- Company Logo -->
              <div class="mr-4">
                <img src={job.companyLogo} alt="{job.title} Logo" class="w-16 h-16 object-contain" />
              </div>
              <!-- Job Information -->
              <div class="flex-grow">
                <h2 class="text-xl font-semibold">{job.title}</h2>
                <p class="text-gray-600">{job.description}</p>
              </div>
              <!-- Salary Range and Tags -->
              <div class="flex flex-col items-end">
                <span class="text-lg font-bold">{job.salaryRange}</span>
                <div class="mt-2 flex flex-wrap gap-1">
                  {#each job.tags as tag}
                    <span class="px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded">{tag}</span>
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
  