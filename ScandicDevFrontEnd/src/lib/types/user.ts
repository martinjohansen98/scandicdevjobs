import { writable } from 'svelte/store';

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

// Create a custom writable that syncs to localStorage
function createUserStore() {
  // On load, try to read from localStorage
  let initial: User|null = null;
  if (typeof localStorage !== 'undefined') {
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
      if (u) {
        localStorage.setItem('user', JSON.stringify(u));
      } else {
        localStorage.removeItem('user');
      }
      set(u);
    }
  };
}

export const user = createUserStore();

