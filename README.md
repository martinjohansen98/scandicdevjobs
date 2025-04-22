![Image alt](https://github.com/Olivolja/ScandicDevJobs/blob/6d077923c9a9fbbe97651895f93dee8ef1223824/ScanDevJobs-readme.png)

An interactive, real-time platform for Scandinavian software developers to discover, apply for, and engage with job opportunities. Beyond static listings, ScandevJobs emphasizes user-generated data, live notifications, and community-driven insights to create a dynamic and engaging job-hunting experience. Our primary objective is to create a software‑developer job market that’s both highly transparent and easily accessible.

## Table of Contents
- [Description](#description)
- [Technologies & Frameworks](#technologies--frameworks)
- [Architecture Overview](#architecture-overview)
- [Main Features](#main-features)
- [Data Model](#data-model)
- [API Endpoints](#api-endpoints)
- [Setup & Installation](#setup--installation)

## Description

ScandevJobs allows developers to:
- Create and customize profiles (skills, CV, preferences).
- Browse and filter job listings in real time.
- Track application status with live updates.
- Chat with recruiters and peers.
- View job locations on an interactive map.

## Technologies & Frameworks

**Frontend**
- **Svelte** (TypeScript): Core reactive components and state management.
- **SvelteKit**: Server-side rendering (SSR) and rich client-side interactivity.
- **Tailwind CSS**: Utility-first styling.
- **SignalR**: Real-time updates and notifications.
- **Leaflet.js**: Interactive maps for geospatial job displays.

**Backend**
- **ASP.NET Core 7** (C#): Web API for RESTful and GraphQL endpoints.
- **Entity Framework Core** with **PostgreSQL**: Relational data storage.
- **Azure Blob Storage**: Storing company-uploaded images and media.
- **ASP.NET Identity** with JWT tokens: Authentication and authorization.
- **SignalR Hub**: Real-time messaging and notifications.

**APIs & Integrations**
- **HotChocolate GraphQL**: Advanced querying, filtering, and subscriptions.
- **Stripe**: Stripe for managing payments.

## Architecture Overview

1. **Server-Side Rendering (SSR):** SvelteKit renders initial pages on the server for SEO and fast load times.
2. **API Layer:** ASP.NET Core exposes REST under `/api/` and GraphQL under `/graphql`.
3. **Real-Time Layer:** SignalR Hubs manage live chat, notifications, and application pipeline updates.
4. **Data Layer:** PostgreSQL database managed via EF Core migrations.
5. **Storage Layer:** Azure Blob Storage (or emulator) for media assets.

## Main Features

- **User Accounts & Profiles**  
  - Email verification, profile editing, CV uploads.
- **Job Listings**  
  - Companies post jobs via a secure dashboard.  
  - Users search, filter, bookmark, and subscribe to listings.
- **Application Pipeline**  
  - Track statuses (Applied, Interviewing, Offer, Rejected) with live notifications.
- **Company Reviews & Ratings**  
  - Submit reviews, view aggregate scores.
- **Real-Time Chat**  
  - Peer-to-peer messaging.
- **Interactive Map**  
  - Display job locations, filter by region or distance.
- **Notifications**  
  - Toasts for updates.
- **Live Filtered Feed** (using GraphQL Subscriptions)  
  - Receive only relevant new job postings matching user criteria.

## Data Model

**Entities**
- User
- Job
- Application
- Company
- Review
- Message
- Notification
- Tags

**Relationships**
- `User ↔ Application ↔ Job`
- `User ↔ Review ↔ Company`
- `User ↔ Message` (SignalR conversations)
- `User ↔ Notification`
- `User ↔ Badge`

## API Endpoints

**Jobs**
```http
GET    /api/jobs
POST   /api/jobs
PUT    /api/jobs/{id}
DELETE /api/jobs/{id}
```

**Applications**
```http
GET   /api/users/{userId}/applications
POST  /api/jobs/{jobId}/apply
PATCH /api/applications/{id}
```

**Reviews**
```http
GET  /api/companies/{companyId}/reviews
POST /api/companies/{companyId}/reviews
```

**Auth**
```http
POST /api/auth/register
POST /api/auth/login
GET  /api/auth/me
```

**GraphQL**
```
POST /graphql
```

## Setup & Installation

1. **Prerequisites**
   - .NET 7 SDK
   - Node.js v18+
   - PostgreSQL
   - (Optional) Docker & Docker Compose

2. **Clone & Configure**
   ```bash
   git clone https://github.com/your-username/scandevjobs.git
   cd scandevjobs
   cp .env.example .env
   # Update DB and JWT secrets in .env
   ```

3. **Database Migration & Seed**
   ```bash
   dotnet ef database update
   dotnet run --project src/ScandevJobs.Api -- seed
   ```

4. **Development**
   ```bash
   docker-compose up --build
   npm install --workspace frontend
   npm run --workspace frontend dev
   dotnet run --project src/ScandevJobs.Api
   ```
