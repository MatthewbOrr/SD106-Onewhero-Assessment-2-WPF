-- Create and use the database visitor_management
CREATE DATABASE IF NOT EXISTS visitor_management CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE visitor_management;

-- 1. Create Table for 'User'
CREATE TABLE IF NOT EXISTS `User` (
user_id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(100) NOT NULL,
email VARCHAR(150) NOT NULL UNIQUE,
password_hash VARCHAR(255) NOT NULL,
phone VARCHAR(50) NOT NULL,
role ENUM('visitor','admin') NOT NULL DEFAULT 'visitor',
created_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE = InnoDB;

-- 2. Create Table for Visitor
CREATE TABLE IF NOT EXISTS Visitor (
visitor_id INT PRIMARY KEY,
registered_date DATE,
FOREIGN KEY (visitor_id) REFERENCES `User`(user_id) ON DELETE CASCADE
) ENGINE =InnoDB;

-- 3. Create Table for Admin
CREATE TABLE IF NOT EXISTS Admin (
admin_id INT PRIMARY KEY,
role_description VARCHAR(50),
FOREIGN KEY (admin_id) REFERENCES `User`(user_id) ON DELETE CASCADE
) ENGINE =InnoDB;

-- 4. Create Table for Event
CREATE TABLE IF NOT EXISTS Event (
event_id INT AUTO_INCREMENT PRIMARY KEY,
title VARCHAR(200) NOT NULL,
description TEXT,
date DATETIME,
location VARCHAR(150),
admin_id INT,
FOREIGN KEY (admin_id) REFERENCES Admin(admin_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 5. Create Table for Booking
CREATE TABLE IF NOT EXISTS Booking (
booking_id INT AUTO_INCREMENT PRIMARY KEY,
event_id INT NOT NULL,
visitor_id INT NOT NULL,
booking_datetime DATETIME DEFAULT CURRENT_TIMESTAMP,
number_of_tickets INT DEFAULT 1,
status ENUM('pending','confirmed','cancelled') DEFAULT 'pending',
FOREIGN KEY (visitor_id) REFERENCES Visitor(visitor_id) ON DELETE CASCADE,
FOREIGN KEY (event_id) REFERENCES Event(event_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 6. Create Table for Promotion
CREATE TABLE IF NOT EXISTS Promotion (
Promotion_id INT AUTO_INCREMENT PRIMARY KEY,
admin_id INT,
message TEXT,
target_audience VARCHAR(50),
start_date DATE,
end_date DATE,
FOREIGN KEY (admin_id) REFERENCES Admin(admin_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 7. Create Table for Interest
CREATE TABLE IF NOT EXISTS Interest (
Interest_id INT AUTO_INCREMENT PRIMARY KEY,
Interest_name VARCHAR(100) NOT NULL UNIQUE
) ENGINE=InnoDB;

-- 8. Create Table for VisitorInterest 
CREATE TABLE IF NOT EXISTS VisitorInterest (
visitor_id INT NOT NULL,
interest_id INT NOT NULL,
PRIMARY KEY (visitor_id, interest_id),
FOREIGN KEY (visitor_id) REFERENCES Visitor(visitor_id) ON DELETE CASCADE,
FOREIGN KEY (interest_id) REFERENCES Visitor(visitor_id) ON DELETE CASCADE
) ENGINE=InnoDB;

DESCRIBE interest;


INSERT IGNORE INTO INTEREST (interest_name) VALUES
('Kiwi Twilight Enclosure'),
('Marae'),
('Museum'),
('Native Birds'),
('Reptile Exhibition'),
('Seasonal Event');

-- Add Admin account for HAN, Matthew (Project Group)
INSERT INTO `User` (name, email, password_hash, phone, role)
VALUES ('Matthew', 'Matthew@Youbee.ac.nz', '1234', '0211234567', 'admin');
INSERT INTO Admin (admin_id, role_description) VALUES (LAST_INSERT_ID(), 'Park Admin');
INSERT INTO `User` (name, email, password_hash, phone, role)
VALUES ('Han', 'Han@Yoobee.ac.nz', '1234', '02112345678', 'admin');
INSERT INTO Admin (admin_id, role_description) VALUES (LAST_INSERT_ID(), 'System Admin');

-- Add missing column 'Capacity' INT after location
ALTER TABLE Event
ADD COLUMN Capacity INT DEFAULT 0.00 after location;

-- event_id table fix (INT to VARCHAR(50))
-- event_id references as FK in booking_id. To alter table in event_id, need to drop FOREIGN KEY in booking_id
-- ibfk_2 "InnoDB Foreign Key" 
ALTER TABLE Booking DROP FOREIGN KEY booking_ibfk_2; 
ALTER TABLE Event MODIFY event_id VARCHAR(50);
ALTER TABLE Booking MODIFY event_id VARCHAR(50);
ALTER TABLE Booking ADD CONSTRAINT booking_ibfk_2 FOREIGN KEY (event_id) REFERENCES Event(event_id);

-- Create data in Event database
INSERT INTO Event (event_id, title, description, date, location, capacity, admin_id) Values
(1, 'Maori Flax Weaving Workshop', 'Learn traditional weaving techniques using harakeke (flax) and create your own putiputi (flower). Duration 2h30m', '2025-11-20 10:00:00', 'Marae Hall', 50, 3),
(2, 'Kiwi Twilight Encounter', 'Join a guided night tour to observe Kiwi birds in their natural setting. Includes conservation talk and quiet bush walk. Family-frinedly. Duration 1h30m', '2025-11-23 18:30:00', 'Kiwi House', 50, 3),
(3, 'Heritage Music & Food Festival', 'Enjoy live kapa haka performances, acoustic music, and local food stalls. Cultural crafts and storytelling sessions available throughout the day. Duration 5hrs', '2025-11-27 11:00:00', 'Onewhero Bay', 200, 3),
(4, 'Guided Nature Trail Walk', 'Explore native bush and wetlands with a park ranger. Learn about local flora, fauna, and conservation efforts. Great for families and photographers. Duration 1h30m', '2025-11-30 9:30:00', 'Main Trail Entrance',30, 3),
(5, 'Museum Discovery Day', 'Special guided tours with behind-the-scenes access to rare artifacts and interactive exhibits. Includes kids’ scavenger hunt and history quiz. Duration 5hrs', '2025-12-01 10:00:00', 'Heritage Museum', 1000, 3),
(6, 'Sunrise Yoga by the Lake', 'Start your day with a peaceful yoga session surrounded by nature. Mats provided. Herbal tea served afterward. Open to all levels. Duration 1hr', '2025-12-07 07:00:00', 'Lakeside Deck', 30, 3),
(7, 'Holiday Craft & Storytelling Fair', 'Celebrate the season with handmade crafts, festive storytelling, and family activities. Includes ornament-making and a visit from a bush-dwelling Santa. Duration 4hrs', '2025-12-14 10:00:00', 'Visitor Pavilion', 40, 3);

describe event;
select * from event;


DESCRIBE interest;
show columns from interest;

INSERT INTO `User` (name, email, password_hash, phone) VALUES
('TEST VISITOR', 'testvisitor@gmail.com', '$2a$11$whnEvEiRhln0TuZZSGsxO.ACt3m2OKyPesA2hfMhuXzqUCRf7Uvby', '02101234578');
INSERT INTO Visitor (visitor_id, registered_date) VALUES (LAST_INSERT_ID(), CURDATE());

-- fix error in foreign key (interest_id) REFERENCES (Visitor_id) to REFERENCES (visitor_id)
ALTER TABLE VisitorInterest
DROP FOREIGN KEY visitorinterest_ibfk_2;

ALTER TABLE VisitorInterest
ADD CONSTRAINT fk_interest
FOREIGN KEY (interest_id) REFERENCES Interest(interest_id) ON DELETE CASCADE;

-- sample visitor matching interest example (Kiwi Twilight Enclousure, Native Birds)
INSERT INTO VisitorInterest (visitor_id, interest_id) 
VALUES (LAST_INSERT_ID(), (SELECT interest_id FROM Interest WHERE interest_name='Kiwi Twilight Enclosure'));

SELECT * FROM VisitorInterest;
SELECT * FROM VisitorInterest
WHERE interest_id = (
  SELECT interest_id FROM Interest WHERE interest_name = 'Kiwi Twilight Enclosure'
);

INSERT INTO VisitorInterest (visitor_id, interest_id) 
VALUES (LAST_INSERT_ID(), (SELECT interest_id FROM Interest WHERE interest_name='Native Birds'));

Select * From VisitorInterest
WHERE interest_id = (
  SELECT interest_id FROM Interest WHERE interest_name = 'Native Birds'
);

-- Query test
SELECT 'USERS' AS what, user_id, name, email, role FROM `User` ORDER BY user_id;
SELECT 'VISITORS' AS what, v.* FROM VISITOR v LIMIT 10;

USE visitor_management;
SHOW TABLES;
SELECT * FROM Event;
SELECT * FROM Visitor;
SELECT * FROM `User`;

DESCRIBE PROMOTION;

-- 9. Sample Promotion
INSERT INTO Promotion (admin_id, message, target_audience, start_date, end_date)
VALUES (3, 'Summer family discount 20% for Museum!!!', 'Museum', '2025-11-20', '2025-12-01');

DESCRIBE PROMOTION;
SELECT * FROM PROMOTION;

-- 10. Sample booking session
SET @vID = (SELECT visitor_id FROM Visitor WHERE visitor_id = (SELECT user_id FROM User WHERE email='testvisitor@gmail.com'));
SET @eID = (SELECT event_id FROM Event WHERE title='Kiwi Twilight Encounter' LIMIT 1);
INSERT INTO Booking (visitor_id, event_id, number_of_tickets, status)
VALUES (@vID, @eID, 2, 'confirmed');

SELECT * FROM booking;
SELECT 'BOOKINGS' AS what, b.booking_id, b.visitor_id, u.name AS visitor_name, e.title, b.status FROM Booking b
JOIN `User` u ON b.visitor_id = u.user_id
JOIN Event e ON b.event_id = e.event_id;

