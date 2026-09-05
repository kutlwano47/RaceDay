CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

-- =========================================
-- 1. USER
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
    DateCreated DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_User_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO

-- =========================================
-- 2. EVENT
-- =========================================

CREATE TABLE Event
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    EventDate DATE NOT NULL,
    Location NVARCHAR(150) NOT NULL,
    DistanceKM DECIMAL(5,2) NOT NULL,
    EventType NVARCHAR(10) NOT NULL,
    DateCreated DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Event_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES [User](UserID),

    CONSTRAINT CK_Event_Distance
        CHECK (DistanceKM > 0),

    CONSTRAINT CK_Event_Type
        CHECK (EventType IN ('Run', 'Walk', 'Cycle'))
);
GO

-- =========================================
-- 3. CATEGORY
-- =========================================

CREATE TABLE Category
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryType NVARCHAR(20) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    MaxParticipants INT NOT NULL DEFAULT 0,
    Fee DECIMAL(10,2) NOT NULL DEFAULT 0.00,

    CONSTRAINT FK_Category_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT CK_Category_Type
        CHECK (CategoryType IN ('Age', 'Distance')),

    CONSTRAINT CK_Category_MaxParticipants
        CHECK (MaxParticipants >= 0),

    CONSTRAINT CK_Category_Fee
        CHECK (Fee >= 0)
);
GO

-- =========================================
-- 4. ENROLMENT
-- =========================================

CREATE TABLE Enrolment
(
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL,
    PaymentStatus NVARCHAR(20) NOT NULL,
    PaymentAmount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Notes NVARCHAR(MAX) NULL,

    CONSTRAINT FK_Enrolment_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES [User](UserID),

    CONSTRAINT FK_Enrolment_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT FK_Enrolment_Category
        FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID),

    CONSTRAINT CK_Enrolment_Status
        CHECK (Status IN ('Enrolled', 'Cancelled', 'Withdrawn')),

    CONSTRAINT CK_Enrolment_PaymentStatus
        CHECK (PaymentStatus IN ('Pending', 'Paid', 'Refunded')),

    CONSTRAINT CK_Enrolment_PaymentAmount
        CHECK (PaymentAmount >= 0)
);
GO

-- =========================================
-- 5. RESULT
-- =========================================

CREATE TABLE Result
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL UNIQUE,
    Position INT NULL,
    FinishTime TIME NULL,
    Score DECIMAL(10,2) NULL,
    Remarks NVARCHAR(MAX) NULL,
    DateRecorded DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Result_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolment(EnrolmentID),

    CONSTRAINT CK_Result_Position
        CHECK (Position IS NULL OR Position > 0),

    CONSTRAINT CK_Result_Score
        CHECK (Score IS NULL OR Score >= 0)
);
GO

-- =========================================
-- 6. EVENT ROUTE
-- =========================================

CREATE TABLE EventRoute
(
    RouteID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    RouteName NVARCHAR(100) NOT NULL,
    RouteDescription NVARCHAR(MAX) NULL,
    DistanceKM DECIMAL(5,2) NOT NULL,
    ElevationGain DECIMAL(6,2) NULL,
    MapURL NVARCHAR(255) NULL,

    CONSTRAINT FK_EventRoute_Event
        FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    CONSTRAINT CK_EventRoute_Distance
        CHECK (DistanceKM > 0),

    CONSTRAINT CK_EventRoute_Elevation
        CHECK (ElevationGain IS NULL OR ElevationGain >= 0)
);
GO

-- =========================================
-- SEED DATA: USERS
-- 2 Organisers + 2 Participants
-- =========================================

INSERT INTO [User]
    (FirstName, LastName, Email, PasswordHash, Role, PhoneNumber)
VALUES
    ('John', 'Mokoena',
     'john.mokoena@raceday.co.za',
     'HASHED_PASSWORD_1',
     'Organiser',
     '0711111111'),

    ('Thandi', 'Dlamini',
     'thandi.dlamini@raceday.co.za',
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
-- 3 Events
-- =========================================

INSERT INTO Event
    (OrganiserID, EventName, Description, EventDate,
     Location, DistanceKM, EventType)
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
-- Categories for every event
-- =========================================

INSERT INTO Category
    (EventID, CategoryName, CategoryType,
     Description, MaxParticipants, Fee)
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
    (ParticipantID, EventID, CategoryID,
     Status, PaymentStatus, PaymentAmount, Notes)
VALUES
    (3,
     1,
     1,
     'Enrolled',
     'Paid',
     50.00,
     'First-time participant.'),

    (4,
     1,
     2,
     'Enrolled',
     'Paid',
     100.00,
     'Senior category participant.'),

    (3,
     2,
     3,
     'Enrolled',
     'Pending',
     40.00,
     NULL),

    (4,
     3,
     5,
     'Enrolled',
     'Paid',
     120.00,
     'Cycling participant.');
GO

-- =========================================
-- SEED DATA: RESULTS
-- =========================================

INSERT INTO Result
    (EnrolmentID, Position, FinishTime, Score, Remarks)
VALUES
    (1,
     1,
     '00:52:30',
     100.00,
     'Excellent finish.'),

    (2,
     2,
     '01:05:45',
     95.00,
     'Strong performance.');
GO

-- =========================================
-- SEED DATA: EVENT ROUTES
-- =========================================

INSERT INTO EventRoute
    (EventID, RouteName, RouteDescription,
     DistanceKM, ElevationGain, MapURL)
VALUES
    (1,
     'City 10km Route',
     '10km road route through Polokwane.',
     10.00,
     85.50,
     'https://raceday.co.za/maps/city-10km'),

    (2,
     'Family 5km Route',
     '5km walking route suitable for families.',
     5.00,
     35.00,
     'https://raceday.co.za/maps/family-5km'),

    (3,
     'Cycle 21km Route',
     '21km road cycling route around Polokwane.',
     21.00,
     150.00,
     'https://raceday.co.za/maps/cycle-21km');
GO

-- =========================================
-- VERIFICATION
-- =========================================

SELECT * FROM [User];
SELECT * FROM Event;
SELECT * FROM Category;
SELECT * FROM Enrolment;
SELECT * FROM Result;
SELECT * FROM EventRoute;
GO