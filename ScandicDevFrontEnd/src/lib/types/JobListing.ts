// src/lib/types/JobListing.ts
import type { User } from '$lib/types/user';
import type { Tag } from '$lib/types/tag';
import type { Company } from '$lib/types/company';

export interface Joblisting {
  id: number;
  title?: string;
  description?: string;
  owner?: User;
  tags?: Array<Tag>;
  category?: string;
  employmentType: EmploymentType;
  workMode: WorkMode;
  isPublished: boolean;
  numberOfApplicants: number;

  // Company-related fields
  company?: Company;
  contactEmail?: string;
  applicationUrl?: string;

  // Salary & Money
  currency?: string;
  salaryRangeMin?: number;
  salaryRangeMax?: number;

  // Location
  address?: string;
  city?: string;
  country?: string;
  latitude?: number;
  longitude?: number;

  // Dates
  createdDate?: string; // ISO string representation of DateTimeOffset
  updatedDate?: string; // ISO string representation of DateTimeOffset
  jobListingExpiryDate?: string; // ISO string representation of DateTimeOffset
  applicationDeadline?: string; // ISO string representation of DateTimeOffset
}