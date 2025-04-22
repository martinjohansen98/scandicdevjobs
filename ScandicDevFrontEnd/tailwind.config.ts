import type { Config } from "tailwindcss";
import daisyui from "daisyui";

const config: Config = {
  content: [
    "./src/**/*.{html,js,svelte,ts}",
    "./src/**/*.{svelte,html,ts,js}"
  ],
  theme: {
    extend: {},
  },
  plugins: [daisyui],
};

export default config;