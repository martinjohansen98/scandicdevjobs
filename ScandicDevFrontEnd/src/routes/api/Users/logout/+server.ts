// src/routes/api/Users/logout/+server.ts
import type { RequestHandler } from '@sveltejs/kit';

export const POST: RequestHandler = async ({ cookies }) => {
  cookies.delete('jwt', {
    path: '/',
    secure: true,
    sameSite: 'none'
  });

  return new Response(null, { status: 204 });
};
