import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  /* config options here */
  watchOptions: {
    pollIntervalMs: 1000, // some sensible number
  },
};

export default nextConfig;