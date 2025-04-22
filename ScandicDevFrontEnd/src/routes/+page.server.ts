// src/routes/+page.server.ts
import type { PageServerLoad } from './$types';
import type { Joblisting } from '$lib/stores/JobListing';

export const load: PageServerLoad = async ({ fetch }) => {
  const res = await fetch('/api/Joblistings', {
    credentials: 'include'
  });

  if (!res.ok) {
    console.error('Failed to load job listings:', res.status);
    return { jobs: [] as Joblisting[] };
  }

  const jobs: Joblisting[] = await res.json();
  return { jobs };
};
