<!-- This is the layout file that runs on every route and returns data that all child layouts 
 and pages inherit. -->
 <script lang="ts">
  import "../app.css";
  import { user, type User } from '$lib/types/user';
  import { onMount, onDestroy } from 'svelte';
  import { darkMode, themeName } from '$lib/stores/backgroundThemeStore';

  export let data: { user: User | null };

  user.set(data.user);

  // Unsubscribe handles
  let off1: () => void;
  let off2: () => void;
  
  onMount(() => {
    // Subscribe to the derived themeName store and set the DaisyUI data-theme
    off1 = themeName.subscribe((name: string) => {
      document.documentElement.setAttribute('data-theme', name);
    });
    // Subscribe to darkMode to toggle the Tailwind dark class
    off2 = darkMode.subscribe((isDark: boolean) => {
      document.documentElement.classList.toggle('dark', isDark);
    });
  });

  onDestroy(() => {
    // Clean up subscriptions on unmount
    off1 && off1();
    off2 && off2();
  });
</script>

<slot />