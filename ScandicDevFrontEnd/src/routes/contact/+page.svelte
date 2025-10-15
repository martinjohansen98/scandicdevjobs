<script lang="ts">
    import Navbar from '$lib/components/Navbar.svelte';
</script>

<div class="flex flex-col h-screen">
    <Navbar />
    <div class="contact-page p-4">
        <h1 class="text-3xl font-bold mb-4">Contact Us</h1>
        <p>If you have any questions, please email us at <a href="mailto:admin@scandicdevjob.com">admin@scandicdevjob.com</a>.</p>
    </div>
</div>
  
  <style>
    .contact-page {
      max-width: 800px;
      margin: 0 auto;
    }
  </style>
  