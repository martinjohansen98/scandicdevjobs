import type { LayoutLoad } from './$types';

export const load: LayoutLoad = async ({ fetch }) => {
  // this log proves it’s running on the client
  console.log('[layout.ts] client load');

  const res = await fetch('/api/Users/me', {
    credentials: 'include'
  });
  const user = res.ok ? await res.json() : null;
  return { user };
};