PRAGMA foreign_keys = ON;

DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS TimeSlot;
DROP TABLE IF EXISTS DetailService;
DROP TABLE IF EXISTS Customer;

CREATE TABLE Customer (
    CustomerId     INTEGER PRIMARY KEY,
    FullName       TEXT NOT NULL,
    IsBlocklisted  INTEGER NOT NULL CHECK (IsBlocklisted IN (0,1))
);

CREATE TABLE DetailService (
    DetailServiceId    INTEGER PRIMARY KEY,
    DetailServiceName  TEXT NOT NULL,
    IsPremium          INTEGER NOT NULL CHECK (IsPremium IN (0,1))
);

CREATE TABLE TimeSlot (
    TimeSlotId  INTEGER PRIMARY KEY,
    StartTime   TEXT NOT NULL
);

CREATE TABLE Booking (
    BookingId        INTEGER PRIMARY KEY,
    CustomerId       INTEGER NOT NULL,
    DetailServiceId  INTEGER NOT NULL,
    TimeSlotId       INTEGER NOT NULL,
    CreatedAt        TEXT NOT NULL,
    FOREIGN KEY (CustomerId)      REFERENCES Customer(CustomerId),
    FOREIGN KEY (DetailServiceId) REFERENCES DetailService(DetailServiceId),
    FOREIGN KEY (TimeSlotId)      REFERENCES TimeSlot(TimeSlotId),
    UNIQUE (TimeSlotId)
);

INSERT INTO Customer (CustomerId, FullName, IsBlocklisted) VALUES
(1, 'Mitchell Boutilier', 0),
(2, 'Mallory Flowers', 0),
(3, 'Chris London', 0),
(4, 'Aaron Mitchell', 1),
(5, 'Stephen Monk', 1),
(6, 'Chris Rendell', 0),
(7, 'Dan Shannon', 0);

INSERT INTO DetailService (DetailServiceId, DetailServiceName, IsPremium) VALUES
(1, 'Basic Wash', 0),
(2, 'Interior Detail', 0),
(3, 'Full Detail', 1),
(4, 'Ceramic Coating', 1);

--------------------------------------------------
-- Time slots
--------------------------------------------------

-- Tomorrow (good for non-premium, too soon for premium)
-- The 'Z' indicates UTC time
INSERT INTO TimeSlot (TimeSlotId, StartTime) VALUES
(1, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '09:00')),
(2, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '10:00')),
(3, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '11:00')),
(4, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '12:00')),
(5, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '13:00')),
(6, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '14:00')),
(7, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+1 day', '15:00'));

-- Three days out (good for premium success)
INSERT INTO TimeSlot (TimeSlotId, StartTime) VALUES
(8,  strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+3 day', '09:00')),
(9,  strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+3 day', '10:00')),
(10, strftime('%Y-%m-%dT%H:%M:%SZ', 'now', '+3 day', '11:00'));


--------------------------------------------------
-- Existing bookings
-- Created "now" so they are always valid
--------------------------------------------------

-- Mitch already has a booking tomorrow (tests "one booking per day")
INSERT INTO Booking (BookingId, CustomerId, DetailServiceId, TimeSlotId, CreatedAt) 
VALUES (1, 2, 1, 2, strftime('%Y-%m-%dT%H:%M:%SZ', 'now'));

-- Mallory took the 11am slot (tests slot-unavailable)
INSERT INTO Booking (BookingId, CustomerId, DetailServiceId, TimeSlotId, CreatedAt) 
VALUES (2, 1, 1, 3, strftime('%Y-%m-%dT%H:%M:%SZ', 'now'));
