// src/app/login/page.tsx
"use client";

import { useRouter } from "next/navigation";
import { mockLogin } from "@/services/auth.mock";

export default function LoginPage() {
  const router = useRouter();

  const login = async () => {
    await mockLogin();
    router.push("/dashboard");
  };

  return (
    <div className="flex h-screen items-center justify-center">
      <div className="w-80 border p-6 rounded">
        <h1 className="text-xl mb-4">Login</h1>
        <button
          onClick={login}
          className="w-full bg-blue-600 text-white py-2 rounded"
        >
          Mock Login
        </button>
      </div>
    </div>
  );
}
