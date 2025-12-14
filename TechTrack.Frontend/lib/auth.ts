import {jwtDecode} from "jwt-decode";
import api from "./api";

export async function login(email: string, password: string) {
  const { data } = await api.post("/auth/login", { email, password });
  localStorage.setItem("token", data.accessToken);
  return jwtDecode(data.accessToken);
}

export function logout() {
  localStorage.removeItem("token");
  window.location.href = "/login";
}

export function getToken() {
  return typeof window !== "undefined" ? localStorage.getItem("token") : null;
}
