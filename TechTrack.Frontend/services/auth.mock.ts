// src/services/auth.mock.ts
export async function mockLogin() {
  localStorage.setItem("token", "mock-token");
}
