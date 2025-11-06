# Visitor Management System – Onewhero Bay Heritage Park

This project is a desktop-based Visitor Management System developed for **Onewhero Bay Heritage Park**, a popular tourist destination in New Zealand. Built using **C# WPF** and **MySQL**, the system streamlines visitor registration, event booking, and promotional outreach, while offering administrators powerful tools to manage park operations.

# Project Overview

The system was created as part of the Diploma in Software Development (SD106 Integrated Studio II – Assessment 2) at Yoobee College. It follows Agile methodology using the Scrum framework and includes full implementation across UI, backend logic, and database integration.

# Key Features

- **Visitor Registration**  
  Visitors can register for park visits and special events by providing personal details and areas of interest.

- **Event Booking**  
  Browse and book tickets for exhibitions, animal encounters, and cultural events.

- **Visitor Tracking**  
  Stores and analyzes visitor data including bookings and interests for future insights.

- **Information Dissemination**  
  Displays park information, attractions, and upcoming events through a user-friendly interface.

- **Promotional Tools**  
  Enables targeted marketing based on visitor interests and booking history.

# Technologies Used

- **Frontend**: C# WPF (.NET Framework)
- **Backend**: C# with MySQL Connector
- **Database**: MySQL
- **Design Tools**: Figma (UI wireframes), Lucidchart (UML diagrams)
- **Version Control**: Git

# System Architecture

- **UI Layer**: Responsive desktop interface with navigation, forms, and data grids
- **Business Logic Layer**: Implements OOP principles (Abstraction, Encapsulation, Inheritance, Polymorphism)
- **Data Layer**: MySQL database with ER diagram and normalized schema
- **Connectivity**: Secure database connection using MySQL Connector for .NET

# OOP Concepts Applied

- **Abstraction**: Shared behaviors defined in abstract `User` class
- **Encapsulation**: Private attributes with public accessors
- **Inheritance**: `Visitor` and `Admin` inherit from `User`
- **Polymorphism**: Role-specific method overrides for dashboard and booking logic

# Database Schema

Includes tables for:
- `Visitor`
- `Event`
- `Booking`
- `Promotion`
- `Attraction`
- `Admin`

Relationships are defined using foreign keys and cardinality constraints to ensure data integrity.

# How to Run

1. Clone the repository
2. Set up MySQL database using provided SQL scripts
3. Configure connection string in `App.config`
4. Build and run the project in Visual Studio


# Team Roles

Product Owner (PO): Matthew

Scrum Master: Han

UX/UI Designer: Matthew

Development Team: Han, Matthew

Quality Assurance (QA) Engineer: Matthew

User Testers/Clients: Han, Matthew, Others we find

# Sprint Timeline

SPRINT 1 - WEEK 4
- Planning & Initial Design
- Backlog creation
- UI/DB design
- environment setup
- Deliverables
- Backlog
- Wireframes
- UML diagram

SPRINT 2 - WEEK 5
- Core feature Development
- Visitor registration
- event booking
- DB integration
- Deliverables
- Functional code
- test results

SPRINT 3 - WEEK 6

- ADMIN & ANALYTIICS
- Admin dashboard
- Analytics
- UI refinement
- Deliverables
- Dashboards
- Analytics
- Hi-Fi UI

SPRINT 4 - WEEK 7,8 (or else)

- QA, DEPLOYMENT & DOCUMENTATION
- QA Testing
- Documentation
- Final Deployment
- Deliverables
- Final System
- Test reports

# License

This project is for educational purposes under Yoobee College guidelines. Not intended for commercial use.

---



