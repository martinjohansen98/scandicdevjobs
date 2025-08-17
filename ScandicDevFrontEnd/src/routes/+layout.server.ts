// src/routes/+layout.server.ts
import type { LayoutServerLoad } from './$types';

/** Server-only load to check JWT cookie and fetch user */
export const load: LayoutServerLoad = async ({ fetch }) => {
    console.log('[layout.ts] running client load…');
    const res = await fetch('/api/Users/me', {
      credentials: 'include'  // ← ensures JWT cookie is sent
    });
  
    const user = res.ok ? await res.json() : null;
    return { user };
  };
  