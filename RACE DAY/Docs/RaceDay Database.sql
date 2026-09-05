CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

-- =========================================
-- 1. USER TABLE
-- =========================================

CREATE TABLE [User]
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_User_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO

-- =========================================
-- 2. EVENT TABLE
-- =========================================

CREATE TABLE Event
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    EventDate DATE NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    EventType NVARCHAR(10) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES [User](UserID),

    CONSTRAINT CK_Event_Distance
        CHECK (Distance > 0),

    CONSTRAINT CK_Event_Type
        CHECK (EventType IN ('Run', 'Walk', 'Cycle'))
);
GO

-- =========================================
-- 3. CATEGORY TABLE
-- =========================================

CREATE TABLE Category
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryType NVARCHAR(20) NOT NULL,
    Description NVARCHAR(255) NULL,
    MaximumParticipants INT NULL,
    EntryFee DECIMAL(10,2) NOT NULL DEFAULT 0.00,

    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT CK_Category_Type
        CHECK (CategoryType IN ('Age', 'Distance')),

    CONSTRAINT CK_Category_MaxParticipants
        CHECK (
            MaximumParticipants IS NULL
            OR MaximumParticipants > 0
        ),

    CONSTRAINT CK_Category_EntryFee
        CHECK (EntryFee >= 0),

    CONSTRAINT UQ_Category_Event_Name
        UNIQUE (EventID, CategoryName)
);
GO

-- =========================================
-- 4. ENROLMENT TABLE
-- =========================================

CREATE TABLE Enrolment
(
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Enrolment_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES [User](UserID),

    CONSTRAINT FK_Enrolment_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT FK_Enrolment_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID),

    CONSTRAINT UQ_Enrolment_Participant_Event
        UNIQUE (ParticipantID, EventID)
);
GO

-- =========================================
-- 5. RESULT TABLE
-- =========================================

CREATE TABLE Result
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    FinishingPosition INT NOT NULL,
    RecordedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Result_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolment(EnrolmentID),

    CONSTRAINT UQ_Result_Enrolment
        UNIQUE (EnrolmentID),

    CONSTRAINT CK_Result_Position
        CHECK (FinishingPosition > 0)
);
GO

-- =========================================
-- 6. EVENT ROUTE TABLE
-- =========================================

CREATE TABLE EventRoute
(
    EventRouteID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    RouteName NVARCHAR(100) NOT NULL,
    RouteDescription NVARCHAR(500) NULL,

    CONSTRAINT FK_EventRoute_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID)
);
GO

-- =========================================
-- SEED DATA: USERS
-- =========================================

INSERT INTO [User]
    (FirstName, LastName, Email, PasswordHash, Role, PhoneNumber)
VALUES
    ('John', 'Mokoena',
     'john.mokoena@RaceDay.co.za',
     'HASHED_PASSWORD_1',
     'Organiser',
     '0711111111'),

    ('Thandi', 'Dlamini',
     'thandi.dlamini@RaceDay.co.za',
     'HASHED_PASSWORD_2',
     'Organiser',
     '0722222222'),

    ('Sipho', 'Nkosi',
     'sipho.nkosi@example.com',
     'HASHED_PASSWORD_3',
     'Participant',
     '0733333333'),

    ('Lerato', 'Molefe',
     'lerato.molefe@example.com',
     'HASHED_PASSWORD_4',
     'Participant',
     '0744444444');
GO

-- =========================================
-- SEED DATA: EVENTS
-- =========================================

INSERT INTO Event
    (OrganiserID, EventName, Description, EventDate,
     Location, Distance, EventType)
VALUES
    (1,
     'Polokwane City Run',
     'Annual road running event.',
     '2026-10-10',
     'Polokwane',
     10.00,
     'Run'),

    (1,
     'Limpopo Family Walk',
     'Family-friendly walking event.',
     '2026-10-24',
     'Polokwane',
     5.00,
     'Walk'),

    (2,
     'Limpopo Cycle Challenge',
     'Road cycling challenge for participants.',
     '2026-11-07',
     'Polokwane',
     21.00,
     'Cycle');
GO

-- =========================================
-- SEED DATA: CATEGORIES
-- =========================================

INSERT INTO Category
    (EventID, CategoryName, CategoryType,
     Description, MaximumParticipants, EntryFee)
VALUES
    (1,
     'Under 20',
     'Age',
     'Participants under 20 years old.',
     100,
     50.00),

    (1,
     'Senior',
     'Age',
     'Senior participant category.',
     200,
     100.00),

    (2,
     '5km Walk',
     'Distance',
     'Five kilometre walking category.',
     150,
     40.00),

    (2,
     'Family Walk',
     'Distance',
     'Family walking category.',
     100,
     60.00),

    (3,
     '21km Cycle',
     'Distance',
     'Twenty-one kilometre cycling category.',
     200,
     120.00),

    (3,
     '10km Cycle',
     'Distance',
     'Ten kilometre cycling category.',
     150,
     80.00);
GO

-- =========================================
-- SEED DATA: ENROLMENTS
-- =========================================

INSERT INTO Enrolment
    (ParticipantID, EventID, CategoryID)
VALUES
    (3, 1, 1),
    (4, 1, 2),
    (3, 2, 3),
    (4, 3, 5);
GO

-- =========================================
-- SEED DATA: RESULTS
-- =========================================

INSERT INTO Result
    (EnrolmentID, FinishTime, FinishingPosition)
VALUES
    (1, '00:52:30', 1),
    (2, '01:05:45', 2);
GO

-- =========================================
-- SEED DATA: EVENT ROUTES
-- =========================================

INSERT INTO EventRoute
    (EventID, RouteName, RouteDescription)
VALUES
    (1,
     'City 10km Route',
     '10km road route through Polokwane.'),

    (2,
     'Family 5km Route',
     '5km walking route suitable for families.'),

    (3,
     'Cycle 21km Route',
     '21km road cycling route around Polokwane.');
GO

-- =========================================
-- VERIFICATION QUERIES
-- =========================================

SELECT * FROM [User];
SELECT * FROM Event;
SELECT * FROM Category;
SELECT * FROM Enrolment;
SELECT * FROM Result;
SELECT * FROM EventRoute;
GO