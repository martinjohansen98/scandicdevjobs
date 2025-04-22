// src/routes/+layout.server.ts
import type { LayoutServerLoad } from './$types';

/** Server-only load to check JWT cookie and fetch user */
export const load: LayoutServerLoad = async ({ fetch }) => {
    const res = await fetch('/api/Users/me', {
      credentials: 'include'  // ← ensures JWT cookie is sent
    });
  
    if (res.ok) {
      const user = await res.json();
      return { user };
    }
  
    return { user: null };
  };
  