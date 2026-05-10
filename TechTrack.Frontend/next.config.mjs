/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  output: 'standalone', // Важно для оптимизированного Docker-образа
  images: {
    domains: ['localhost'],
  },
};

export default nextConfig;
