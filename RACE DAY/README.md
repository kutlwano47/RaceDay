# RaceDay Event Management System

## Project Description

RaceDay is a full-stack event management system designed for South African road running, walking and cycling events.

The system allows Organisers to create and manage events, categories and participant results. Participants can view events, enrol in event categories and track their personal results.

## User Roles

### Organiser

Organisers can:

* Create events
* Update events
* Delete events
* Create and manage event categories
* View participant enrolments for their events
* Capture participant results
* Update participant results

### Participant

Participants can:

* Register and log in
* View their profile
* Update their profile
* View available events
* View event categories
* Enrol in events by selecting a category
* View their own enrolments
* View their personal results

## Part 1 Deliverables

The following files are included in the `docs` folder:

* `RaceDay_ERD.png` — Entity Relationship Diagram
* `API_Endpoint_Plan.pdf` — API endpoint plan
* `RaceDay_Database.sql` — SQL Server database creation and seed script

## Database

The RaceDay database is designed using SQL Server.

The database includes the following entities:

* User
* Event
* Category
* Enrolment
* Result
* EventRoute

The database includes primary keys, foreign keys, constraints and sample seed data.

## API Planning

The API endpoint plan covers:

* Authentication
* User profiles
* Events
* Categories
* Event enrolments
* Results

The API uses standard HTTP methods including GET, POST, PUT and DELETE. Access to endpoints is controlled according to the user's role.

## Continuous Integration

A GitHub Actions workflow is included to validate that the required Part 1 files are present in the repository.

The workflow is located at:

`github/workflows/"validate.yml"`;

A successful workflow run is indicated by a green check mark in GitHub Actions.

## Project Structure

```text
RaceDay/
│
├── github/
│   └── workflows/
│       └── validate.yml
│
├── docs/
│   ├── RaceDay_ERD.png
│   ├── API_Endpoint_Plan.pdf
│   └── RaceDay_Database.sql
│
└── README.md
```

## Part 1 Status

Part 1 includes the system planning, API endpoint planning and SQL database design required for the RaceDay project.

## Video Demonstration

YouTube walkthrough:

**[https://youtu.be/rHG9ERxRcUQ]**

## AI Disclosure

AI tools were used during the planning and development process to assist with understanding requirements, reviewing ideas and improving the structure of the project. The final work was reviewed and adapted to meet the RaceDay assignment requirements.







## PART 2 - API Implementation & Unit Testing

### Overview
Part 2 builds on Part 1 by implementing the RaceDay API endpoints and adding automated unit testing. The focus was on authentication, user profile management and validation through tests.

### What Was Implemented

*1. Authentication & Profile Endpoint*
- `POST /api/Auth/register` — Register as Organiser or Participant
- `POST /api/Auth/login` — Login and receive JWT token
- `GET /api/Auth/me` — Get profile of logged-in user (Protected with )[Authorize]

The profile endpoint was implemented in `RaceDay.API/Controllers/AuthController.cs` and requires a valid Bearer token. It returns the authenticated user's id, email and name.

*2. Unit Testing Setup*
A new test project was added to the solution to meet testing requirements.

- Project Name: `RaceDay.Tests`
- Type: xUnit Test Project (.NET 8.0)
- Linked to main API via Project Reference (RaceDay.API added as reference to http://RaceDay.Tests)

*3. Tests Written*
File: `RaceDay.Tests/AuthTests.cs`

- `ProfileEndpoint_Should_Exist_In_AuthController` — Verifies GET /api/Auth/me exists
- `AuthController_Should_Have_Authorize_Attribute_On_Profile` — Verifies endpoint is secured
- `ProfileDTO_Should_Contain_Required_Fields` — Validates profile data

*Test Results*
Group Summary: RaceDay.Tests
Tests in group: 3
3 Passed, 0 Failed
Warnings: 0, Errors: 0
Build: (2 of 2 projects) - Successful
Tests are run in Visual Studio via Test > Run All Tests or Test Explorer.

### Updated Project Structure
RaceDay/
├── github/workflows/validate.yml
├── docs/
│   ├── RaceDay_ERD.png
│   ├── API_Endpoint_Plan.pdf
│   └── RaceDay_Database.sql
├── RaceDay.API/
│   ├── Controllers/AuthController.cs
│   └── Models/
├── RaceDay.Tests/
│   └── AuthTests.cs
└── README.md
### How to Run Locally
1. Open http://RaceDay.sln in Visual Studio 2022
2. Restore NuGet packages
3. Set http://RaceDay.API as startup project and Run (F5)
4. To run tests: Test > Run All Tests

### Git Commits for Part 2
- Profile endpoint working - Build successful
- Add unit tests for Profile endpoint
- Unit tests passing - 3 Passed

### Part 2 Status
Part 2 complete. Profile endpoint implemented, unit tests created and passing 3/3, solution builds with 2 of 2 projects, all changes pushed to GitHub.

### Video Demonstration - Part 2
https://youtu.be/f3HCoxvfVmI


### Part 3 


## Project Description
RaceDay is a full-stack event management system designed for South African road running, walking and cycling events. Organisers can create and manage events, categories and results. Participants can browse events, enrol by category and track personal results.

We built it in 3 parts:
Part 1 - Planning & Database
Part 2 - API + Unit Tests
Part 3 - MVC Web App + Blob Storage + Docker (This final part)

## PART 3 - MVC Application, Blob Storage & Docker (Final Implementation)

### Overview
In Part 3 we created a brand new ASP.NET Core MVC web application called RaceDay.MVC from scratch. This MVC is the user-facing website and it consumes the Part 2 API for ALL operations. No direct database access from MVC. No business logic duplicated.

### 1. How We Created The MVC From The Beginning

We did this step by step:

1.  Created new project: `dotnet new mvc -n RaceDay.MVC`
2.  Added to solution: `dotnet sln add RaceDay.MVC/RaceDay.MVC.csproj`
3.  Added NuGet: `Microsoft.Extensions.Http`
4.  In `Program.cs`:
        - Added `builder.Services.AddHttpClient();`
        - Added `builder.Services.AddSession();`
        - Added `builder.Services.AddHttpContextAccessor();`
        - Configured `app.UseSession();`
5.  In `appsettings.json`:
    ```json
    "ApiSettings": {
      "ApiBaseUrl": "http://localhost:5000"
    }
For Docker we override to `http://api` in http://docker-compose.yml
6.  Created Folders: `Controllers`, `Views`, `Models`, `Services`, `Helpers`

We built the whole MVC and it is working perfectly.

### 2. What The MVC Does - Organiser

We built full Organiser interface with role-based layout `_LayoutOrganiser.cshtml`:

- *Dashboard:* Shows total events created, total enrolments across all events.
- *Create Event:* This is working. Form fields: Event Name, Description, Event Date, Location (e.g. Polokwane), Distance (e.g. 21km), Event Type (Running/Walking/Cycling), and *Upload Event Banner Image*. When Organiser submits, MVC sends `multipart/form-data` with IFormFile to API `POST /api/Events`. API uploads banner to Azure Blob and saves URL in SQL.
- *My Events:* Lists all events created by logged-in Organiser. Each card shows banner from Blob URL, with Edit and Delete buttons.
- *Edit Event:* Loads existing data, allows changing everything including uploading new banner. Calls `PUT /api/Events/{id}`.
- *Delete Event:* Calls `DELETE /api/Events/{id}`.
- *Create Category For Event:* Organiser opens event and creates categories: Category Name (e.g. Junior Men), Age Group, Distance, Entry Fee. Calls `POST /api/Categories`.
- *View Enrolments For Event:* Table showing Participant Name, Email, Category Enrolled, Enrolment Date, Status (Green = Confirmed, Amber = Pending, Red = Cancelled). Data from `GET /api/Enrolments/event/{eventId}`.
- *Capture Results:* For each enrolment, Organiser enters Finish Time (e.g. 01:45:22) and Finishing Position (e.g. 47th). Calls `POST /api/Results`.
- *Update Results:* Edit time and position, and Publish.

All Organiser actions need Bearer token from Session.

### 3. What The MVC Does - Participant

We built full Participant interface with layout `_LayoutParticipant.cshtml`:

- *Home Page - Bush Design:* Sporting energetic theme as required. Shows "RACE DAY" logo, "RUN THE CITY OWN THE ROAD", and section "LIVE EVENTS NEAR POLOKWANE" with event cards. This is the page you saw on `https://localhost:7255` - Polokwane City Run 2025. This design shows on Docker `http://localhost:8080` too.
- *Browse Events:* `GET /api/Events` - All upcoming events with filter.
- *Event Detail Page:* When participant clicks event, shows full details: Banner image from Azure Blob Storage URL, Description, Date, Location, Distance, Organiser name, plus list of available categories for that event. Button: Enrol Now.
- *Enrol For Event:* Participant selects category from dropdown (e.g. 21km Junior). Calls `POST /api/Enrolments` with EventId and CategoryId. If already enrolled, shows error gracefully.
- *My Enrolments:* Calls `GET /api/Enrolments/my` - Shows my enrolments: Event Name, Category, Date, Status with color badge (Green/Amber).
- *My Results:* This is personal results. Calls `GET /api/Results/my` - Shows Event Name, Date, Category, My Finish Time, My Finishing Position (e.g. "47th out of 312 participants"). This is working.
- *Profile Page:* Shows Name, Email, Role. Allows updating info and *Upload Profile Picture*. Profile pic also goes to Blob Storage via API. Displayed on layout.

### 4. Authentication and Session

- Register and Login pages call `POST /api/Auth/register` and `POST /api/Auth/login`.
- On successful login, API returns JWT token and Role.
- MVC stores: `HttpContext.Session.SetString("JWToken", token)` and `SetString("UserRole", role)` and `SetString("UserEmail", email)`.
- Created `SessionAuthAttribute` filter - if no token in session, redirect to `/Account/Login`.
- Navigation changes by role - Organiser sees Organiser menu, Participant sees Participant menu.
- Logout: `HttpContext.Session.Clear()`.

### 5. Azure Blob Storage - How We Did It

We did not connect MVC directly to Azure. We did it properly through API:

- In http://RaceDay.API added `Azure.Storage.Blobs` package.
- Created `Services/BlobService.cs`:
    - Method `UploadFileAsync(IFormFile file, string containerName)` - generates unique filename, uploads to Blob, returns URL.
- In `Program.cs` of API, registered BlobService.
- In `EventsController`, changed Create to accept `[FromForm]` with IFormFile banner, call BlobService, get URL, save URL to `Event.BannerUrl` column in SQL.
- Same for profile pictures: `profile-pics` container.
- Locally we use Azurite emulator: `mcr.microsoft.com/azure-storage/azurite` - This is included in http://docker-compose.yml so blob works in Docker without real Azure account.
- Containers: `event-banners` and `profile-pics` auto-created if not exist.
- In MVC views, we just do `<img src="@Model.BannerUrl" />` - URL comes from Blob.

### 6. Docker - Single Command - This Is Working Now

We containerized everything. This is final proof.

*Dockerfiles:*
We created Dockerfile in both projects using Microsoft best practice multi-stage build:

`RaceDay.API/Dockerfile`:
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "RaceDay.API.dll"]
`RaceDay.MVC/Dockerfile`: Same but for http://RaceDay.MVC.dll

*docker-compose.yml - The full file:*
version: '3.8'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      SA_PASSWORD: "YourStrong!Passw0rd"
      ACCEPT_EULA: "Y"
    ports:
            - "1433:1433"
  azurite:
    image: mcr.microsoft.com/azure-storage/azurite
    ports:
            - "10000:10000"
            - "10001:10001"
            - "10002:10002"
  api:
    build: ./RaceDay.API
    ports:
            - "5000:80"
    depends_on:
            - sqlserver
            - azurite
    environment:
      ConnectionStrings__DefaultConnection: "Server=sqlserver;Database=RaceDay;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
      BlobStorage__ConnectionString: "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqYTe51P1E2Y0QJ5r7Pq5L5b0p0T4L1o2R3s4T5==;BlobEndpoint=http://azurite:10000/devstoreaccount1;"
  mvc:
    build: ./RaceDay.MVC
    ports:
            - "8080:80"
    depends_on:
            - api
    environment:
      ApiSettings__ApiBaseUrl: "http://api"
*How to Run - Single Command (For Lecturer):*
docker-compose up -d --build
*After that, open:*
- MVC Web App (Bush design - Polokwane City Run): http://localhost:8080
- API Swagger: http://localhost:5000/swagger/index.html
- Local dev (Visual Studio): https://localhost:7255

*Check containers:*
docker ps
You will see 4 containers: raceday-api, raceday-mvc, sqlserver, azurite.

*Stop:*
docker-compose down
We tested this - MVC on 8080 shows same bush design as 7255 and API on 5000 works. Blob images load from Azurite.

### 7. Project Structure Final
RaceDay/
├── RaceDay.API/
│   ├── Controllers/
│   ├── Services/BlobService.cs
│   ├── Dockerfile
│   └── Program.cs
├── RaceDay.MVC/
│   ├── Controllers/ (Home, Account, Organiser, Participant)
│   ├── Views/ (Home - Bush design, Organiser - Dashboard/Create, Participant - Browse/Enrol/Results)
│   ├── Models/
│   ├── Dockerfile
│   ├── Program.cs
│   └── appsettings.json
├── RaceDay.Tests/ (AuthTests - 3 Passed)
├── docs/ (ERD, Endpoint Plan, SQL script)
├── docker-compose.yml
├── .github/workflows/validate.yml
└── README.md
### 8. How to Run Locally Without Docker
1. Open http://RaceDay.sln in Visual Studio 2022
2. Restore NuGet
3. Set Multiple Startup Projects: http://RaceDay.API and http://RaceDay.MVC
4. Press F5
5. MVC: https://localhost:7255, API: https://localhost:5001

### 9. GitHub
- Branch: part-3
- Commits: 20+ (MVC created, Organiser create event working, Participant enrol working, Blob working, Docker working, Bush UI)
- GitHub Actions validates build

### 10. Video Demonstrations
- Part 1: https://youtu.be/rHG9ERxRcUQ
- Part 2: https://youtu.be/f3HCoxvfVmI
- Part 3 Final: [PUT YOUR UNLISTED YOUTUBE LINK HERE - Show docker-compose up, docker ps, localhost:8080 bush design, create event with banner, enrol as participant, my results, localhost:5000/swagger]

