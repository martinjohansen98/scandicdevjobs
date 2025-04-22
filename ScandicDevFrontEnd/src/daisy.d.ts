declare module "daisyui" {
  const daisyui: any;
  export default daisyui;
}

import "tailwindcss";

declare module "tailwindcss" {
  interface Config {
    daisyui?: {
      themes?: string[];
      [key: string]: unknown; // Allow other DaisyUI-specific options
    };
  }
}