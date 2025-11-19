
-- Database Creation

CREATE DATABASE IF NOT EXISTS visitor_management_v2
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
USE visitor_management;


-- Table Definitions
-- 1. User table
CREATE TABLE IF NOT EXISTS User (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(50) NOT NULL,
    role ENUM('visitor','admin') NOT NULL DEFAULT 'visitor',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- 2. Visitor table
CREATE TABLE IF NOT EXISTS Visitor (
    visitor_id INT PRIMARY KEY,
    registered_date DATE,
    FOREIGN KEY (visitor_id) REFERENCES User(user_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 3. Admin table
CREATE TABLE IF NOT EXISTS Admin (
    admin_id INT PRIMARY KEY,
    role_description VARCHAR(50),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (admin_id) REFERENCES User(user_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 4. Event table
CREATE TABLE IF NOT EXISTS Event (
    event_id VARCHAR(50) PRIMARY KEY,   -- 변경된 타입 반영
    title VARCHAR(200) NOT NULL,
    description TEXT,
    date DATETIME,
    location VARCHAR(150),
    capacity INT DEFAULT 0,
    admin_id INT,
    FOREIGN KEY (admin_id) REFERENCES Admin(admin_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 5. Booking table
CREATE TABLE IF NOT EXISTS Booking (
    booking_id INT AUTO_INCREMENT PRIMARY KEY,
    event_id VARCHAR(50) NOT NULL,      -- 변경된 타입 반영
    visitor_id INT NOT NULL,
    booking_datetime DATETIME DEFAULT CURRENT_TIMESTAMP,
    number_of_tickets INT DEFAULT 1,
    status VARCHAR(50) DEFAULT 'pending',   -- 변경된 타입 반영
    FOREIGN KEY (visitor_id) REFERENCES Visitor(visitor_id) ON DELETE CASCADE,
    FOREIGN KEY (event_id) REFERENCES Event(event_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 6. Promotion table
CREATE TABLE IF NOT EXISTS Promotion (
    promotion_id INT AUTO_INCREMENT PRIMARY KEY,
    admin_id INT,
    message TEXT,
    target_audience VARCHAR(50),
    start_date DATE,
    end_date DATE,
    FOREIGN KEY (admin_id) REFERENCES Admin(admin_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 7. Interest table
CREATE TABLE IF NOT EXISTS Interest (
    interest_id INT AUTO_INCREMENT PRIMARY KEY,
    interest_name VARCHAR(100) NOT NULL UNIQUE
) ENGINE=InnoDB;

-- 8. VisitorInterest table
CREATE TABLE IF NOT EXISTS VisitorInterest (
    visitor_id INT NOT NULL,
    interest_id INT NOT NULL,
    PRIMARY KEY (visitor_id, interest_id),
    FOREIGN KEY (visitor_id) REFERENCES Visitor(visitor_id) ON DELETE CASCADE,
    FOREIGN KEY (interest_id) REFERENCES Interest(interest_id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- =========================================
-- Initial Data Inserts
-- =========================================

-- Admin account
INSERT INTO User (name, email, password_hash, phone, role)
VALUES ('Admin', 'Admin@Yoobee.ac.nz', '$2a$11$IFnmayPejDMcHwiSGBdzlOvoeppxZqt3cb5ee7pmZJrMzYBCFiSSC', '021123456780', 'admin');

INSERT INTO Admin (admin_id, role_description)
VALUES (LAST_INSERT_ID(), 'Admin');

-- Event data
INSERT INTO Event (event_id, title, description, date, location, capacity, admin_id) VALUES
('1', 'Maori Flax Weaving Workshop', 'Learn traditional weaving techniques using harakeke (flax) and create your own putiputi (flower). Duration 2h30m', '2025-11-20 10:00:00', 'Marae Hall', 50, 1),
('2', 'Kiwi Twilight Encounter', 'Join a guided night tour to observe Kiwi birds in their natural setting. Includes conservation talk and quiet bush walk. Family-frinedly. Duration 1h30m', '2025-11-23 18:30:00', 'Kiwi House', 50, 1),
('3', 'Heritage Music & Food Festival', 'Enjoy live kapa haka performances, acoustic music, and local food stalls. Cultural crafts and storytelling sessions available throughout the day. Duration 5hrs', '2025-11-27 11:00:00', 'Onewhero Bay', 200, 1),
('4', 'Guided Nature Trail Walk', 'Explore native bush and wetlands with a park ranger. Learn about local flora, fauna, and conservation efforts. Great for families and photographers. Duration 1h30m', '2025-11-30 09:30:00', 'Main Trail Entrance', 30, 1),
('5', 'Museum Discovery Day', 'Special guided tours with behind-the-scenes access to rare artifacts and interactive exhibits. Includes kids’ scavenger hunt and history quiz. Duration 5hrs', '2025-12-01 10:00:00', 'Heritage Museum', 1000, 1),
('6', 'Sunrise Yoga by the Lake', 'Start your day with a peaceful yoga session surrounded by nature. Mats provided. Herbal tea served afterward. Open to all levels. Duration 1hr', '2025-12-07 07:00:00', 'Lakeside Deck', 30, 1),
('7', 'Holiday Craft & Storytelling Fair', 'Celebrate the season with handmade crafts, festive storytelling, and family activities. Includes ornament-making and a visit from a bush-dwelling Santa. Duration 4hrs', '2025-12-14 10:00:00', 'Visitor Pavilion', 40, 1);

-- Interest data
INSERT IGNORE INTO Interest (interest_name) VALUES
('Kiwi Twilight Enclosure'),
('Marae'),
('Museum'),
('Native Birds'),
('Reptile Exhibition'),
('Seasonal Event');

