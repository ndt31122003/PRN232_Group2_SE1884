import daisyui from 'daisyui';

export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
    "./node_modules/@ebay/ui-core-react/**/*.{js,jsx,ts,tsx}",
    "./node_modules/@ebay/skin/dist/**/*.css"
  ],
  theme: {
    extend: {},
  },
  plugins: [daisyui],
  corePlugins: {
    preflight: false,
  },
  daisyui: {
    themes: ["light"], // 👈 chỉ dùng theme light
  },
};
