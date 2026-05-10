import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// Отключаем middleware полностью - вся логика на клиенте
export function middleware(request: NextRequest) {
  return NextResponse.next();
}

export const config = {
  matcher: [],
};
