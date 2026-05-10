import { HydrationProvider } from '@/components/providers/HydrationProvider';
import './globals.css';

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="ru">
      <body>
        <HydrationProvider>{children}</HydrationProvider>
      </body>
    </html>
  );
}
