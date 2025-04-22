<!-- src/lib/Navbar.svelte -->
<script lang="ts">
  import { onMount } from 'svelte';
  import { user, type User } from '$lib/stores/user';
  import { fade, scale } from 'svelte/transition';
  import { cubicOut } from 'svelte/easing';
  import { darkMode } from '$lib/stores/backgroundTheme';

  let activeLang = 'se';
  let showModal = false;
  let selectedTab = 'login';
  let menuOpen = false;
  let initialized  = false;
  let loading = false;

  let firstName = '';
  let lastName = '';
  let email = '';
  let confirmEmail = '';
  let password = '';
  let confirmPassword = '';
  
  function setLanguage(lang: string) {
    activeLang = lang;
  }
  
  function toggleDarkMode() {
    darkMode.update(x => !x);
  }

  function customModalTransition(node: Element, options = {}) {
  const fadeTransition = fade(node, { duration: 300 });
  const scaleTransition = scale(node, {
    duration: 300,
    easing: cubicOut,
    start: 0.9
  });

  return {
    delay: 0,
    duration: 300,
    css: (t: number, u: number) => {
      const fadeCss = fadeTransition.css ? fadeTransition.css(t, u) : '';
      const scaleCss = scaleTransition.css ? scaleTransition.css(t, u) : '';
      return `${fadeCss}; ${scaleCss}`;
    }
  };
}

async function handleSubmit() {
  loading = true;
  try {
    let res;
    if (selectedTab === 'register') {
      if (email !== confirmEmail) {
        alert("Emails do not match!");
        return;
      }
      if (password !== confirmPassword) {
        alert("Passwords do not match!");
        return;
      }
      res = await fetch('/api/Users/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ firstName, lastName, email, password })
      });
    } else {
      res = await fetch('/api/Users/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({ email, password })
      });
    }

    if (!res.ok) throw new Error(await res.text());
    user.set(await res.json());
    showModal = false;
    selectedTab = 'login';

    // Reset form
    firstName = lastName = email = confirmEmail = password = confirmPassword = '';
  } catch (err) {
    alert((err instanceof Error ? err.message : "Something went wrong"));
  } finally {
    loading = false;
  }
}

  onMount(async () => {

    try {
      const r = await fetch('/api/Users/me', {
        credentials: 'include'
      });
      if (r.ok) {
        user.set(await r.json() as User);
      } else {
        user.set(null);   // clear if not authorized
      }
  } catch {
      user.set(null);     // also clear on network error
  } finally {
      initialized = true;
  }

    document.addEventListener('click', e => {
      if (menuOpen && !(e.target as Element).closest('.avatar-menu')) {
        menuOpen = false;
      }
    });
  });

  function logout() {
  fetch('/api/Users/logout', {
    method: 'POST'
  }).finally(() => {
    user.set(null);
    menuOpen = false;
  });
}

</script>

<nav class="red p-4 shadow">
  <div class="flex justify-between items-center w-full px-4">
    <a href="/" class="flex items-center space-x-2 text-xl font-bold">
      <img src="Webicon.png" alt="Logo" class="h-12 w-12" />
      <span>ScanDevJobs</span>
    </a>
    <ul class="flex items-center space-x-4">
      <li class="flex items-center">
        <button
          on:click={toggleDarkMode}
          class="relative w-12 h-6 bg-gray-300 dark:bg-gray-600 rounded-full focus:outline-none inline-flex items-center justify-center"
        >
          <span
            class={`absolute top-0.5 left-0.5 w-5 h-5 bg-white dark:bg-gray-800 rounded-full shadow transform transition-transform duration-200 ${
              $darkMode ? 'translate-x-6' : 'translate-x-0'
            }`}
          >
            {#if !$darkMode}
              <!-- moon icon -->
              <svg xmlns="http://www.w3.org/2000/svg" class="h-3 w-3 text-yellow-500 absolute inset-0 m-auto" fill="currentColor" viewBox="0 0 20 20">
                <path d="M17.293 13.293A8 8 0 116.707 2.707a7.968 7.968 0 0010.586 10.586z" />
              </svg>
            {:else}
              <!-- sun icon -->
              <svg xmlns="http://www.w3.org/2000/svg" class="h-3 w-3 text-yellow-500 absolute inset-0 m-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M12 3v1m0 16v1m8-9h1M3 12H2
                     m15.364-7.364l.707.707
                     m-12.02 12.02l-.707.707
                     M15.364 19.364l.707-.707
                     m-12.02-12.02l-.707-.707
                     M12 8a4 4 0 100 8 4 4 0 000-8z"
                />
              </svg>
            {/if}
          </span>
        </button>
      </li>
      <!-- Navigation Links -->
      <li><a href="/" class="hover:underline">Home</a></li>
      <li><a href="/about" class="hover:underline">About</a></li>
      <li><a href="/services" class="hover:underline">Services</a></li>
      <li><a href="/contact" class="hover:underline">Contact</a></li>
      <!-- Language Buttons -->
      <li>
        <div class="flex space-x-2">
          <button on:click={() => setLanguage('se')} class={`p-0.5 border-2 border-transparent rounded transition-all transform duration-200 ${
            activeLang === 'se' ? 'border-blue-500 scale-105 bg-blue-50' : 'hover:border-blue-500'
          }`}>
            <img src="https://flagcdn.com/se.svg" alt="Swedish flag" width="24" />
          </button>
          <button on:click={() => setLanguage('no')} class={`p-0.5 border-2 border-transparent rounded transition-all transform duration-200 ${
            activeLang === 'no' ? 'border-blue-500 scale-105 bg-blue-50' : 'hover:border-blue-500'
          }`}>
            <img src="https://flagcdn.com/no.svg" alt="Norwegian flag" width="24" />
          </button>
          <button on:click={() => setLanguage('dk')} class={`p-0.5 border-2 border-transparent rounded transition-all transform duration-200 ${
            activeLang === 'dk' ? 'border-blue-500 scale-105 bg-blue-50' : 'hover:border-blue-500'
          }`}>
            <img src="https://flagcdn.com/dk.svg" alt="Danish flag" width="24" />
          </button>
        </div>
      </li>
      <!-- Register Button -->
      {#if $user}
      <li class="relative avatar-menu">
        <!-- Avatar button toggles menuOpen -->
        <button
          on:click={() => menuOpen = !menuOpen}
          class="w-8 h-8 rounded-full overflow-hidden focus:outline-none border-2 border-transparent hover:border-blue-500 transition">
          <img
            src={`https://ui-avatars.com/api/?name=${$user.firstName}+${$user.lastName}&background=0D8ABC&color=fff`}
            alt="avatar"
          />
        </button>
  
        {#if menuOpen}
          <!-- Dropdown -->
          <ul class="absolute right-0 mt-2 w-48 bg-white dark:bg-gray-800 rounded shadow-lg py-1 z-50">
            <li>
              <a
                href="/settings"
                class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">
                Settings
              </a>
            </li>
            <li>
              <button
                on:click={logout}
                class="w-full text-left px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">
                Logout
              </button>
            </li>
          </ul>
        {/if}
      </li>
    {:else}
      <li>
        <button
          on:click={() => { showModal = true; selectedTab = 'login'; }}
          class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition">
          Login / Register
        </button>
      </li>
    {/if}
  </ul>
  </div>
</nav>

{#if showModal}
  <!-- Backdrop -->
  <div
    class="fixed inset-0 bg-[rgba(0,0,0,0.05)] backdrop-blur-sm z-40 flex items-center justify-center"
    in:fade={{ duration: 200 }}
    out:fade={{ duration: 200 }}
  >

    <!-- Animated Modal Wrapper -->
    <div class="w-full max-w-md" in:customModalTransition out:customModalTransition>
      <!-- Modal Content Box -->
      <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg relative z-50">

        <!-- Close Button -->
        <button
          on:click={() => showModal = false}
          class="absolute top-2 right-2 text-gray-500 hover:text-gray-800 dark:hover:text-white text-xl font-bold">
          ×
        </button>

        <!-- Tab Navigation -->
        <div class="flex mb-4 space-x-4">
          <button
            on:click={() => selectedTab = 'login'}
            class="px-4 py-2 rounded-t border-b-2 transition-all duration-150 font-semibold"
            class:bg-gray-100={selectedTab === 'login'}
            class:border-blue-500={selectedTab === 'login'}>
            Login
          </button>
          <button
            on:click={() => selectedTab = 'register'}
            class="px-4 py-2 rounded-t border-b-2 transition-all duration-150 font-semibold"
            class:bg-gray-100={selectedTab === 'register'}
            class:border-blue-500={selectedTab === 'register'}>
            Register
          </button>
        </div>

        <!-- Form Content -->
        <form on:submit|preventDefault={handleSubmit} class="space-y-4">
          {#if selectedTab === 'register'}
            <!-- Register Form -->
            <div class="flex space-x-4">
              <input type="text" placeholder="First name" bind:value={firstName} required class="w-1/2 p-2 border rounded" />
              <input type="text" placeholder="Last name" bind:value={lastName} required class="w-1/2 p-2 border rounded" />
            </div>
            <input type="email" placeholder="Email address" bind:value={email} required class="w-full p-2 border rounded" />
            <input type="email" placeholder="Confirm email address" bind:value={confirmEmail} required class="w-full p-2 border rounded" />
            <input type="password" placeholder="Password" bind:value={password} required class="w-full p-2 border rounded" />
            <input type="password" placeholder="Confirm password" bind:value={confirmPassword} required class="w-full p-2 border rounded" />
            <button type="submit" class="w-full py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition flex items-center justify-center" disabled={loading}>
              {#if loading}
                <svg class="animate-spin h-5 w-5 text-white mr-2" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z" />
                </svg>
                Loading...
              {:else}
                Register
              {/if}
            </button>
            {:else}
            <!-- Login Form -->
            <input type="email" placeholder="Email address" bind:value={email} required class="w-full p-2 border rounded" />
            <input type="password" placeholder="Password" bind:value={password} required class="w-full p-2 border rounded" />
            <button
              type="submit"
              class="w-full py-2 bg-green-600 text-white rounded hover:bg-green-700 transition flex items-center justify-center"
              disabled={loading}
            >
              {#if loading}
                <svg class="animate-spin h-5 w-5 text-white mr-2" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8H4z" />
                </svg>
                Loading...
              {:else}
                Login
              {/if}
            </button>
          {/if}
        </form>

      </div>
    </div>
  </div>
{/if}







