import { writable } from 'svelte/store';
import { browser } from '$app/environment';

export type UserRole = 'User' | 'Company';

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
}

function createUserStore() {
  let initial: User | null = null;

  if (browser) {
    const json = localStorage.getItem('user');
    if (json) {
      try {
        initial = JSON.parse(json);
      } catch {
        localStorage.removeItem('user');
      }
    }
  }

  const { subscribe, set } = writable<User | null>(initial);

  return {
    subscribe,
    set: (u: User | null) => {
      if (browser) {
        if (u) {
          localStorage.setItem('user', JSON.stringify(u));
        } else {
          localStorage.removeItem('user');
        }
      }
      set(u);
    }
  };
}

export const user = createUserStore();
