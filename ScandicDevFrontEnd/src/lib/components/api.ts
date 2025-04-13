import axios from 'axios';
import type { AxiosResponse } from 'axios';

const API_BASE = 'https://localhost:5001/api/Users';

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

export async function registerUser(data: RegisterData): Promise<AxiosResponse<any>> {
  return axios.post(`${API_BASE}/register`, data);
}

export async function loginUser(data: LoginData): Promise<AxiosResponse<any>> {
  return axios.post(`${API_BASE}/login`, data);
}
