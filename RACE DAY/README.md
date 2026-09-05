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

`.github/workflows/validate.yml`

A successful workflow run is indicated by a green check mark in GitHub Actions.

## Project Structure

```text
RaceDay/
│
├── .github/
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

