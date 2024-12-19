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
- [License](#license)
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
   cd EasyInventorySystem

 2.Restore Dependencies:
    Run the following command to restore the required dependencies:    
    ```bash
    dotnet restore

### Set up the database
  1.Update the appsettings.json file with your database connection string.
  2.Apply migrations and seed data by running the following command:
    ```bash
    dotnet ef database update
    
### Run the Application Locally
  Use the following command to start the application:
   ```dotnet run```
Run the Application in Docker
To run the application using Docker, execute the following command:

bash
Copy code
docker-compose up --build
Usage
Access the application at https://localhost:5001 or http://localhost:5000.
Use the Swagger UI at /swagger for testing API endpoints.
Admin users can manage users, roles, and inventory from the admin dashboard.
Running Tests
Run unit tests to verify the functionality of the application:

bash
Copy code
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
├── LICENSE                                   # License file
└── .gitignore                                # Git ignore rules

```
Contributing
Contributions are welcome! Follow these steps to contribute:

Fork the project.
Create a new branch:
bash
Copy code
git checkout -b feature/YourFeature
Commit your changes:
bash
Copy code
git commit -m 'Add some feature'
Push to the branch:
bash
Copy code
git push origin feature/YourFeature
Open a pull request.
License
This project is licensed under the MIT License. See the LICENSE file for more details.

### Contact
For questions or feedback, reach out to:

Author: Md Safin Sarker
Email: your-email@example.com
GitHub: MdSafinSarker

