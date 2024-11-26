
# D2J Clínica Odontológica

This is an API for managing dental appointments, patients, specialists, specialties, and schedules. The API was developed in C# using .NET 8 and SQL Server as the database.

## Table of Contents
  - [Features](#features)
  - [Requirements](#requirements)
  - [Business Rules](#business-rules)
  - [Technologies](#technologies)
  - [Endpoints](#endpoints)
    - [Authentication](#authentication)
    - [Patients](#patients)
    - [Consultations](#consultations)
    - [Schedules](#schedules)
    - [Specialists](#specialists)
    - [Specialties](#specialties)
  - [Database Configuration](#database-configuration)
  - [Setup](#setup)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)

## Features

* Patient management (complete CRUD).
* Dental consultation scheduling and management.
* Appointment and availability management.
* Specialist and specialty management.
* Interactive API documentation with Swagger.

## Requirements

**Core Endpoints:**
- Patients: Create, update,  and list patients.
- Consultations: Schedule, update, cancel, and list by date or specialist.
- Specialists: Manage specialist information and related appointments.
- Specialties: Register and retrieve specialties.

## Business Rules

* **Unauthenticated Users**:
  - Can create an account.
  - Can log in to the system.

* **Specialists**:
  - Can manage their own account.
  - Can view and manage specialties.
  - Can manage their own schedules.
  - Can view consultations assigned to them.

* **Patients**:
  - Can manage their own account.
  - Can view specialties.
  - Can view available schedules.
  - Can manage their own consultations.

## Technologies

* Language: C# (.NET 8)
* Database: SQL Server
* ORM: Entity Framework Core
* Authentication: JWT (Json Web Token)
* Documentation: Swagger/OpenAPI


## Endpoints

### Authentication
**User Login**
* **POST** /api/login
Body Example:
```json
{
    "email": "user@example.com",
    "password": "SecurePassword123"
}
```

### Patients
**Create Patient**
* **POST** /api/patient/create
Body Example:
```json
{
  "name": "string",
  "fone": "string",
  "address": "string",
  "email": "string",
  "password": "string",
  "birth": "2024-11-26",
  "cpf": "string",
  "createdAt": "2024-11-26T22:29:55.448Z"
}
```

**List All Patients**
* **GET** /api/patient/all

**Retrieve Patient Details**
* **GET** /api/patient/{id}

**Update Patient**
* **PUT** /api/patient/{id}/update
Body Example:
```json
{
  "address": "string",
  "fone": "string"
}
```

### Consultations
**Create Consultation**
* **POST** /api/consultation
Body Example:
```json
{
  "patientId": 0,
  "procedure": "string",
  "scheduleId": 0
}
```

**List All Consultations**
* **GET** /api/consultation/all

**List Consultation by ID**
* **GET** /api/consultation/{id}

**List Consultations by Date**
* **GET** /api/consultation/by-date/{date}

**List Consultations by Specialist**
* **GET** /api/consultation/by-specialist/{specialistId}

**Update Consultation**
* **PUT** /api/consultation/{id}
Body Example:
```json
{
    "procedure": "string",
    "scheduleId": 0
}
```

### Schedules
**Create Schedules**
* **POST** /api/schedule/create
Body Example:
```json
{
  "specialistId": 0,
  "isAvailable": true,
  "startDate": "2024-11-21T00:00:00",
  "endDate": "2024-11-25T00:00:00",
  "startTime": "08:00:00",
  "endTime": "17:00:00",
  "intervalMinutes": 60
}
```

**List All Schedules**
* **GET** /api/schedule/all

**List Schedule by ID**
* **GET** /api/schedule/{scheduleId}

**List Available Schedules**
* **GET** /api/schedule/available/{specialistId}

**Update Schedule Availability**
* **PUT** /api/schedule/{scheduleId}/availability
Body Example:
```json
{
    "isAvailable": false
}
```

### Specialists
**Create Specialist**
* **POST** /api/specialist/create
Body Example:
```json
{
  "name": "string",
  "fone": "string",
  "address": "string",
  "email": "string",
  "password": "string",
  "croNumber": "string",
  "croState": "string",
  "createdAt": "2024-11-26T22:30:44.006Z",
  "specialtyIds": [
    0
  ]
}
```

**List All Specialists**
* **GET** /api/specialist/all

**Retrieve Specialist by ID**
* **GET** /api/specialist/{id}

**Update Specialist**
* **PUT** /api/specialist/{specialistId}/update
Body Example:
```json
{
  "address": "string",
  "fone": "string",
  "specialtyIds": [
    0
  ]
}
```

### Specialties
**List All Specialties**
* **GET** /api/specialty/all

**Retrieve Specialty by ID**
* **GET** /api/specialty/{id}


## Database Configuration

The API uses Entity Framework Core with SQL Server. The connection string is specified in the `appsettings.json` file:

```json
"ConnectionStrings": {
    "Main": "Data Source=(localdb)\MSSQLLocalDB;Database=ClinicaOdonto;Integrated Security=true"
}
```

## Setup

### Prerequisites
- .NET SDK 8
- SQL Server

### Installation

1. Clone the repository:
   ```bash
   git clone <REPOSITORY_URL>
   cd <PROJECT_DIRECTORY>
   ```

2. Configure the database connection in `appsettings.json`.

3. Run migrations to set up the database:
   ```bash
   dotnet ef database update
   ```

4. Start the application:
   ```bash
   dotnet run
   ```

5. Access the interactive documentation at:
   ```
   http://localhost:<port>/swagger
   ```

---
