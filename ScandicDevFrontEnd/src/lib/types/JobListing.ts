import type { User } from '$lib/types/user';
import type { Tag } from '$lib/types/Tag';
import type { Company } from '$lib/types/Company';

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
  logoFileId?: string;

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
  createdDate?: string;
  updatedDate?: string;
  jobListingExpiryDate?: string;
  applicationDeadline?: string;
}