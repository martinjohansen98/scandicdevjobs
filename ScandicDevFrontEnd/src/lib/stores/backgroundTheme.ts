import { writable, derived } from 'svelte/store';

export const darkMode = writable<boolean>(false);

export const themeName = derived(darkMode, (darkModeValue: boolean) => darkModeValue ? 'winter' : 'dark');