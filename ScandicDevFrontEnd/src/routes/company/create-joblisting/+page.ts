// src/routes/company/create-joblisting/+page.ts
import type { PageLoad } from './$types';
import { error } from '@sveltejs/kit';

export const load: PageLoad = async ({ parent }) => {
  const { user } = await parent();  // gets the same `user` from +layout
  if (!user || user.role !== 'Company') {
    throw error(403, 'Forbidden');
  }
  return { user };
};