// src/lib/types/JobListing.ts
export interface JobListing {
  id: number;
  title: string;
  city: string;
  category: string;
  description: string;
  companyLogo: string;
  salaryRange: string;
  tags: string[];
  filterJob: string[];
  address: string;
  lat: number;
  lng: number;
}