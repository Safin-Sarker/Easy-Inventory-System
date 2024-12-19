# Easy Inventory System

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](link-to-ci-pipeline)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

## Table of Contents
- [About the Project](#about-the-project)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Running Tests](#running-tests)
- [Folder Structure](#folder-structure)
- [Contributing](#contributing)
- [Contact](#contact)

---

## About the Project

**Easy Inventory System** is a comprehensive inventory management solution designed to help users efficiently track products, inventories, and warehouses. The application emphasizes role-based and claim-based authorization while implementing clean architecture principles for scalability, maintainability, and testability.

---

## Features

### Functional Features
1. **Authentication & Authorization**
   - Role-based and Claim-based authorization.
   - Cookie-based authentication.
   - Login and Registration forms secured with reCAPTCHA.

2. **User Management**
   - SuperAdmin can manage users and roles through CRUD operations.
   - Member users have restricted access until authorized by the SuperAdmin.

3. **Inventory Workflow**
   - CRUD operations for managing products.
   - Advanced search functionality for finding inventory and products efficiently.
   - User profile update capabilities.

4. **Scalability**
   - Background tasks handled by worker services.
   - AWS S3 and AWS Queue integration for sustainable workflows.

---

## Technologies Used

- **Framework:** ASP.NET Core
- **Frontend:** Bootstrap, DataTables
- **Database:** SQL Server (with Entity Framework Core using Repository and Unit of Work pattern)
- **Architecture:** Clean Architecture
- **Testing:** NUnit (for service and model unit testing)
- **Containerization:** Docker
- **Cloud Integration:** AWS S3 for storage, AWS Queue for task handling
- **Security:** reCAPTCHA for public forms, claim-based authentication, and authorization
- **Utilities:** AutoMapper, Autofac, logging, and exception handling
- **Performance:** Stored Procedures for optimized database operations

---

## Getting Started

### Prerequisites

Ensure you have the following tools installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- SQL Server
- AWS CLI (for AWS integration)

---

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/EasyInventorySystem.git
   
2.  Go to DevSkill.Inventory.Web folder
    ```bash
    cd DevSkill.Inventory.Web
   
3. Restore Dependencies:
    Run the following command to restore the required dependencies:
     ```bash
    dotnet restore
4. Set up the database:
   - Update the appsettings.json file with your database connection string.
   - Apply migrations and seed data by running the following command
      ```bash
     dotnet ef database update
    
5. Run the Application Locally
   Use the following command to start the application:
    ```bash
    dotnet run --urls http://localhost:5158

6. Run the Application in Docker 
   To run the application using Docker, execute the following command:
    ```bash
    docker-compose up --build

### Usage
- Access the application at http://localhost:5158
- SuperAdmin user can manage:
 - users, 
 - roles,
 - inventory from the admin dashboard.

### Run the Application Locally

- Use the following command to start the application:
  ```bash
    dotnet test

### Folder Structure

Below is the folder structure of the project:

```
.
├── Devskill.Inventory
│   ├── Devskill.Inventory.Application       # Application layer (Services)
│   ├── Devskill.Inventory.Domain            # Domain layer (Entities, Interfaces)
│   ├── Devskill.Inventory.Infrastructure    # Infrastructure layer (Data, Repositories, AWS integration)
│   ├── Devskill.Inventory.Web                # Web AFrontend project
│   ├── Devskill.Inventory.Application.Tests   # Unit  tests
|   ├── docker-compose.yml                     # Docker configuration
├── README.md                                 # Project documentation
└── .gitignore                                # Git ignore rules

```
### Contributing
- Contributions are welcome! Follow these steps to contribute:

1. Fork the project.
2. Create a new branch:
   ```bash
   git checkout -b feature/YourFeature
3. Commit your changes:
   ```bash
   git commit -m 'Add some feature'
   
4. Push to the branch:
   ```bash
   git push origin feature/YourFeature
   
5. Create a pull request.


### Contact
For questions or feedback, reach out to:

Author: Md Safin Sarker
Email: safinsarker1122@gmail.com

