<script lang="ts">
  import { goto } from '$app/navigation';
  import Navbar from '$lib/components/Navbar.svelte';
  import type { User } from '$lib/types/user';
  import { browser } from '$app/environment';
  import { programmingLanguages } from '$lib/constants/programmingLanguages';
  import { onMount, onDestroy, tick } from 'svelte';
  
  export let data: { user: User };

  const user = data.user;
  const ET = { FullTime: 0, PartTime: 1, Contract: 2 };
  const WM = { Onsite: 0, Remote: 1, Hybrid: 2 };
  const allTagNames = programmingLanguages.map(t => t.label);
  let tagQuery = '';
  let highlightedIndex = -1;
  let menuOpen = false;
  let wrapper: HTMLDivElement;

  $: filteredTags = allTagNames
    .filter(t =>
      t.toLowerCase().includes(tagQuery.trim().toLowerCase()) &&
      !form.tagsSelected.includes(t)
    );

  $: showDropdown = menuOpen && filteredTags.length > 0;

  const VISIBLE_ROWS = 7;
  const ROW_PX = 36;
  const LIST_MAX_PX = VISIBLE_ROWS * ROW_PX;

  function toggleMenu() {
    menuOpen = !menuOpen;
    if (menuOpen) highlightedIndex = -1;
  }

  function openMenu() { menuOpen = true; }
  function closeMenu() { menuOpen = false; highlightedIndex = -1; }

  function addTag(t: string) {
    if (!form.tagsSelected.includes(t)) {
      form.tagsSelected = [...form.tagsSelected, t];
      tagQuery = '';
      highlightedIndex = -1;
      // keep open so user can add multiple
      openMenu();
    }
  }
  function removeTag(t: string) {
    form.tagsSelected = form.tagsSelected.filter(x => x !== t);
  }

  async function handleKey(e: KeyboardEvent) {
    // Open on typing/arrow when closed
    if (!menuOpen && (e.key.length === 1 || e.key === 'ArrowDown')) {
      openMenu();
      await tick();
    }

    if (!filteredTags.length) return;

    if (e.key === 'ArrowDown') {
      e.preventDefault();
      highlightedIndex = (highlightedIndex + 1) % filteredTags.length;
      await tick();
      document.getElementById(`tag-opt-${highlightedIndex}`)?.scrollIntoView({ block: 'nearest' });
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      highlightedIndex = (highlightedIndex - 1 + filteredTags.length) % filteredTags.length;
      await tick();
      document.getElementById(`tag-opt-${highlightedIndex}`)?.scrollIntoView({ block: 'nearest' });
    } else if (e.key === 'Enter') {
      e.preventDefault();
      if (highlightedIndex >= 0) addTag(filteredTags[highlightedIndex]);
    } else if (e.key === 'Escape') {
      closeMenu();
    }
  }

  function handleInput() {
    highlightedIndex = -1;
    openMenu(); // typing should open
  }

  function handleDocClick(e: MouseEvent) {
    if (!wrapper?.contains(e.target as Node)) closeMenu();
  }
  onMount(() => document.addEventListener('mousedown', handleDocClick));
  onDestroy(() => document.removeEventListener('mousedown', handleDocClick));

  type UploadResponse =
  | { url?: string; blobUrl?: string; uri?: string; name?: string; [k: string]: any }
  | string;

  let defaults = {
    title: '',
    description: '',
    tagsSelected: [] as string[],
    category: '',
    employmentType: 'FullTime' as keyof typeof ET,
    workMode: 'Onsite' as keyof typeof WM,
    isPublished: true,
    contactEmail: '',
    applicationUrl: '',
    currency: 'SEK',
    salaryMin: null,
    salaryMax: null,
    address: '',
    city: '',
    country: '',
    expiry: '',
    deadline: '',
    logoFileId: '',
  };

  let form = { ...defaults };

  $: if (browser && user.id != null) {
    const key = `jobListingDraft-${user.id}`;
    const saved = localStorage.getItem(key);
    if (saved) {
      try { form = { ...defaults, ...JSON.parse(saved) }; }
      catch {}
    }
  }

  // 3) autosave on *any* change to form
  $: if (browser && user.id != null) {
    const key = `jobListingDraft-${user.id}`;
    localStorage.setItem(key, JSON.stringify(form));
  }

  // live preview object
  $: preview = {
    title: form.title,
    company: `${user.firstName} ${user.lastName}`,
    salary:
      form.salaryMin && form.salaryMax
        ? `${form.currency} ${form.salaryMin}–${form.salaryMax}`
        : 'Not specified',
    location: `${form.city}, ${form.country}`,
    description: form.description
  };

  let selectedFile: File | null = null;
  let uploadBusy = false;
  let uploadError = '';
  let uploadedName = '';

  function onPickFile(e: Event) {
    const input = e.target as HTMLInputElement;
    selectedFile = input.files?.[0] ?? null;
    uploadError = '';
  }

  async function uploadBlob() {
    if (!selectedFile) { uploadError = 'Please choose a file first.'; return; }
    uploadBusy = true; uploadError = '';
    try {
      const fd = new FormData();
      fd.append('file', selectedFile);

      const res = await fetch('/api/blob/upload', { method: 'POST', body: fd, credentials: 'include' });
      if (!res.ok) throw new Error(await res.text());

      const ct = res.headers.get('content-type') ?? '';
      const data: UploadResponse = ct.includes('application/json') ? await res.json() : await res.text();

      const fileId = typeof data === 'string' ? '' : (data.fileId || data.id || data.fileID || data.FileId || '');
      if (!fileId) throw new Error('Upload succeeded but no fileId returned from blobstorage');

      form.logoFileId = fileId;

      uploadedName = selectedFile.name;
      selectedFile = null;
    } catch (err: any) {
      uploadError = err?.message ?? 'Upload failed.';
    } finally {
      uploadBusy = false;
    }
  }

  async function submit() {
    const dto = {
      Title: form.title,
      Description: form.description,
      Tags: form.tagsSelected,
      Category: form.category,
      EmploymentType: ET[form.employmentType as keyof typeof ET],
      WorkMode: WM[form.workMode as keyof typeof WM],
      IsPublished: form.isPublished,
      ContactEmail: form.contactEmail,
      ApplicationUrl: form.applicationUrl,
      Currency: form.currency,
      SalaryRangeMin: form.salaryMin,
      SalaryRangeMax: form.salaryMax,
      Address: form.address,
      City: form.city,
      Country: form.country,
      JobListingExpiryDate: form.expiry || null,
      ApplicationDeadline: form.deadline || null,
      LogoFileId: form.logoFileId || null
    };

    const res = await fetch('/api/Joblistings/create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(dto)
    });

    if (!res.ok) {
      alert('Failed to create job: ' + await res.text());
      return;
    }

    localStorage.removeItem(`jobListingDraft-${user.id}`);

    // back home
    await goto('/', { replaceState: true, noScroll: true });
  }
</script>

<Navbar />

<div class="flex space-x-8 p-8">
  <!-- Joblisting form -->
  <form on:submit|preventDefault={submit} class="w-1/2 space-y-4">
    <h2 class="text-2xl font-bold">New Job Listing</h2>

    <!-- Title -->
    <div class="form-control">
      <label for="title" class="label p-0 mb-1">
        <span class="label-text font-semibold">Job title</span>
      </label>
      <input id="title" bind:value={form.title} required class="input input-bordered w-full" />
    </div>
    
    <!-- Description -->
    <div class="form-control">
      <label for="description" class="label p-0 mb-1">
        <span class="label-text font-semibold">Description</span>
      </label>
      <textarea id="description" bind:value={form.description} required class="textarea textarea-bordered w-full"></textarea>
    </div>

    <!-- Tags -->
    <div class="form-control">
      <label for="tags-input" class="label p-0 mb-1">
        <span class="label-text font-semibold">Tags</span>
      </label>

      <!-- Selected tags -->
      <div class="flex flex-wrap gap-2 mb-2">
        {#each form.tagsSelected as t}
          <span class="badge badge-outline">
            {t}
            <button type="button" class="ml-2" on:click={() => removeTag(t)} aria-label={`Remove ${t}`}>✕</button>
          </span>
        {/each}
      </div>

    <div class="relative" bind:this={wrapper}>
      <div class="flex items-stretch">
        <input
          id="tags-input"
          class="input input-bordered w-full rounded-r-none"
          placeholder="Search tags…"
          bind:value={tagQuery}
          on:keydown={handleKey}
          on:input={handleInput}
          on:click={toggleMenu}
          aria-expanded={showDropdown}
          aria-haspopup="listbox"
        />
        <button type="button" class="btn rounded-l-none" on:click={toggleMenu} aria-label="Toggle tags list">▾</button>
      </div>

      {#if showDropdown}
        <ul
          class="absolute z-20 mt-1 w-full bg-base-100 border rounded shadow overflow-y-auto"
          style={`max-height:${LIST_MAX_PX}px;`}
          role="listbox"
        >
          {#each filteredTags as t, i}
            <li
              id={`tag-opt-${i}`}
              role="option"
              aria-selected={i === highlightedIndex}
              class="px-3 py-2 cursor-pointer select-none {i === highlightedIndex ? 'bg-base-200' : ''}"
              on:mouseenter={() => (highlightedIndex = i)}
              on:mousedown|preventDefault={() => addTag(t)}
            >
            {t}
            </li>
          {/each}
        </ul>
      {/if}
    </div>

    <!-- Employment type -->
    <div class="form-control">
      <label for="employmentType" class="label p-0 mb-1 mt-6">
        <span class="label-text font-semibold">Employment type</span>
      </label>
      <select id="employmentType" bind:value={form.employmentType} class="select select-bordered w-full">
        <option>FullTime</option><option>PartTime</option><option>Contract</option>
      </select>
    </div>

    <!-- Work mode -->
    <div class="form-control">
      <label for="workMode" class="label p-0 mb-1 mt-6">
        <span class="label-text font-semibold">Work mode</span>
      </label>
      <select id="workMode" bind:value={form.workMode} class="select select-bordered w-full">
        <option>Onsite</option><option>Remote</option><option>Hybrid</option>
      </select>
    </div>

    <!-- Salary -->
    <div class="flex gap-3">
      <div class="form-control w-1/3">
        <label for="currency" class="label p-0 mb-1 mt-6">
          <span class="label-text font-semibold">Currency</span>
        </label>
        <input id="currency" bind:value={form.currency} placeholder="SEK" class="input input-bordered w-full" />
      </div>
      <div class="form-control w-1/3">
        <label for="salaryMin" class="label p-0 mb-1 mt-6">
          <span class="label-text font-semibold">Min salary</span>
        </label>
        <input id="salaryMin" type="number" bind:value={form.salaryMin} class="input input-bordered w-full" />
      </div>
      <div class="form-control w-1/3">
        <label for="salaryMax" class="label p-0 mb-1 mt-6">
          <span class="label-text font-semibold">Max salary</span>
        </label>
        <input id="salaryMax" type="number" bind:value={form.salaryMax} class="input input-bordered w-full" />
      </div>
    </div>

    <!-- Location -->
    <div class="space-y-3">
      <div class="form-control">
        <label for="address" class="label mt-6 p-0 mb-1">
          <span class="label-text font-semibold">Address</span>
        </label>
        <input
          id="address"
          bind:value={form.address}
          placeholder="Street and number (ex. Vasagatan 1)"
          class="input input-bordered w-full"
          required
        />
      </div>

      <div class="form-control">
        <label for="city" class="label">
          <span class="label-text font-semibold">City</span>
        </label>
        <input
          id="city"
          bind:value={form.city}
          placeholder="City"
          class="input input-bordered w-full"
        />
      </div>

      <div class="form-control">
        <label for="country" class="label">
          <span class="label-text font-semibold">Country</span>
        </label>
        <input
          id="country"
          bind:value={form.country}
          placeholder="Country"
          class="input input-bordered w-full"
        />
      </div>
    </div>

    <!-- Dates -->
    <div class="space-y-3">
      <div class="form-control">
        <label for="expiry" class="label mt-2 p-0 mb-1">
          <span class="label-text font-semibold">Expiry date</span>
        </label>
        <input
          id="expiry"
          type="date"
          bind:value={form.expiry}
          class="input input-bordered w-full"
        />
      </div>

      <div class="form-control">
        <label for="deadline" class="label p-0 mb-1">
          <span class="label-text font-semibold">Application deadline</span>
        </label>
        <input
          id="deadline"
          type="date"
          bind:value={form.deadline}
          class="input input-bordered w-full"
        />
      </div>
    </div>

    <!-- Logo -->
    <div class="mt-4 space-y-2">
      <label for="blob-file" class="font-semibold">
        Attachment / Logo
      </label>
      <div class="flex items-center gap-2">
        <input
          id="blob-file"
          name="blob-file"
          type="file"
          on:change={onPickFile}
          class="file-input file-input-bordered"
        />
        <button
          type="button"
          class="btn btn-outline"
          on:click={uploadBlob}
          disabled={uploadBusy}
          aria-label="Upload selected file to Azure Blob"
        >
          {uploadBusy ? 'Uploading…' : 'Upload'}
        </button>
      </div>
      {#if uploadError}
        <p class="text-error text-sm">{uploadError}</p>
      {/if}

      {#if form.logoFileId}
        <div class="alert shadow-sm">
          <div>
            <p class="font-medium">Uploaded:</p>
            <span>{uploadedName}</span>
          </div>
        </div>
        <img
          alt="uploaded preview"
          src={`/api/blob/logo/${form.logoFileId}`}
          class="max-h-40 rounded border"
          on:error={(e)=>{ const img = e.currentTarget as HTMLImageElement; img.onerror = null; img.src = '/placeholder-company.svg'; }}
        />
      {/if}

    </div>

    <button type="submit" class="btn btn-primary mt-6">Create Listing</button>
  </form>

  <!-- Live preview -->
  <div class="w-1/2 p-4 border rounded shadow">
    <h3 class="text-xl font-semibold">{preview.title || 'Your job title here'}</h3>
    <p class="italic">by {preview.company || 'Company name'}</p>
    <p><strong>Salary:</strong> {preview.salary}</p>
    <p><strong>Location:</strong> {preview.location}</p>
    <p class="mt-2">{preview.description || 'Job description preview…'}</p>
  </div>
</div>