import { writable } from 'svelte/store';
// import { Client, JobListing } from '../../api-client';
import type { JobListing } from '$lib/types/JobListing';

export const jobList = writable<JobListing[]>([]);
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
        const data: JobListing[] = await response.json();
        jobList.set(data); // ✅ Updates store with fetched jobs
    } catch (err) {
        jobListError.set(err instanceof Error ? err.message : "Unknown error");
    } finally {
        jobListLoading.set(false);
    }
}