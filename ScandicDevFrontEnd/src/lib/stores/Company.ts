export interface Company {
    id: number;
    name: string;
    email: string;
    isVerified: boolean;
    companySize: CompanySize;
    companyLogoUrl?: string;
    website?: string;
    contactEmail?: string;
    contactPhone?: string;
    linkedIn?: string;
    twitter?: string;
    facebook?: string;
}