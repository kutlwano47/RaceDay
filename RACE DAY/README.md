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
*[Add your Part 2 YouTube link here]*