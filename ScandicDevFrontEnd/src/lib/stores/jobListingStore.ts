import { writable } from 'svelte/store';
// import { Client, JobListing   6} from '../../api-client';
import type { Joblisting } from '$lib/stores/JobListing';

export const jobList = writable<Joblisting[]>([]);
export const jobListLoading = writable<boolean>(false);
export const jobListError = writable<string | null>(null);
const API_BASE_URL = 'https://localhost:7077'; // Adjust port if needed
// const api = new Client(API_BASE_URL);

export async function fetchJobs() {
    jobListLoading.set(true);
    jobListError.set(null);
    try {
        const response = await fetch(API_BASE_URL + '/api/JobListings');
        if (!response.ok) {
            throw new Error(`Error fetching jobs: ${response.statusText}`);
        }
        const data: Joblisting[] = await response.json();
        jobList.set(data);
    } catch (err) {
        jobListError.set(err instanceof Error ? err.message : "Unknown error");
    } finally {
        jobListLoading.set(false);
    }
}