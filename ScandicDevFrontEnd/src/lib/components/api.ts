import axios from 'axios';

export interface RegisterData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}
export interface LoginData {
  email: string;
  password: string;
}

// Point this at your real API
const api = axios.create({
  baseURL: 'https://localhost:7077/api/Users',  // ← correct port
  withCredentials: true,                        // sends HttpOnly cookies
});

// Registration
export function registerUser(data: RegisterData) {
  return api.post('/register', data);
}

// Login
export function loginUser(data: LoginData) {
  return api.post('/login', data);
}
