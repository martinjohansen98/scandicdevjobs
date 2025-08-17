<script lang="ts">
  import { goto } from '$app/navigation';
  import Navbar from '$lib/components/Navbar.svelte';
  import type { User } from '$lib/types/user';
  import { browser } from '$app/environment';
  import { onMount } from 'svelte';
  
  export let data: { user: User };

  const user = data.user;
  const ET = { FullTime: 0, PartTime: 1, Contract: 2 };
  const WM = { Onsite: 0, Remote: 1, Hybrid: 2 };

  // initial form state
  let defaults = {
    title: '',
    description: '',
    tags: '',
    category: '',
    employmentType: 'FullTime' as keyof typeof ET,
    workMode: 'Onsite' as keyof typeof WM,
    isPublished: true,
    contactEmail: '',
    applicationUrl: '',
    currency: 'USD',
    salaryMin: null,
    salaryMax: null,
    address: '',
    city: '',
    country: '',
    expiry: '',
    deadline: ''
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

  async function submit() {
    const dto = {
      Title: form.title,
      Description: form.description,
      Tags: form.tags.split(',').map(t=>t.trim()),
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
      ApplicationDeadline: form.deadline || null
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
  <!-- Left: the form -->
  <form on:submit|preventDefault={submit} class="w-1/2 space-y-4">
    <h2 class="text-2xl font-bold">New Job Listing</h2>
    <input bind:value={form.title} placeholder="Job title" required class="input input-bordered w-full" />
    <textarea bind:value={form.description} placeholder="Description" required class="textarea textarea-bordered w-full"></textarea>
    <input bind:value={form.tags} placeholder="Tags (comma-separated)" class="input input-bordered w-full" />
    <input bind:value={form.category} placeholder="Category" class="input input-bordered w-full" />
    <select bind:value={form.employmentType} class="select select-bordered w-full">
      <option>FullTime</option><option>PartTime</option><option>Contract</option>
    </select>
    <select bind:value={form.workMode} class="select select-bordered w-full">
      <option>Onsite</option><option>Remote</option><option>Hybrid</option>
    </select>

    <!-- Salary -->
    <div class="flex space-x-2">
      <input bind:value={form.currency} placeholder="USD" class="input input-bordered w-1/4" />
      <input type="number" bind:value={form.salaryMin} placeholder="Min salary" class="input input-bordered w-1/4" />
      <input type="number" bind:value={form.salaryMax} placeholder="Max salary" class="input input-bordered w-1/4" />
    </div>

    <!-- Location -->
    <input bind:value={form.city} placeholder="City" class="input input-bordered w-full" />
    <input bind:value={form.country} placeholder="Country" class="input input-bordered w-full" />

    <!-- Dates -->
    <label>Expiry date<input type="date" bind:value={form.expiry} class="input input-bordered" /></label>
    <label>Application deadline<input type="date" bind:value={form.deadline} class="input input-bordered" /></label>

    <button type="submit" class="btn btn-primary">Create Listing</button>
  </form>

  <!-- Right: live preview box -->
  <div class="w-1/2 p-4 border rounded shadow">
    <h3 class="text-xl font-semibold">{preview.title || 'Your job title here'}</h3>
    <p class="italic">by {preview.company || 'Company name'}</p>
    <p><strong>Salary:</strong> {preview.salary}</p>
    <p><strong>Location:</strong> {preview.location}</p>
    <p class="mt-2">{preview.description || 'Job description preview…'}</p>
  </div>
</div>