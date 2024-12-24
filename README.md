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
   - Login and Registration forms
     
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

2. Navigate to the `DevSkill.Inventory.Web` directory:
   ```bash
   cd DevSkill.Inventory.Web
      
3. Restore Dependencies:
   Run the following command to restore the required dependencies:
   ```bash
   dotnet restore
 
4. Set up the database
    - Update the appsettings.json file with your database connection string.
    - The database schema will be updated automatically at runtime due to auto-migration, so no manual commands are needed for applying migrations.
---

### Usage
    
1. **Run the Application Locally**  
   Use the following command to start the application:  
   ```bash
   dotnet run --urls "http://localhost:5000"
2. **Run the Application with Docker**
   - You can also containerize and run the application using Docker.
   #### Option 1: Build Image and Run Container
     Prepare the application:
    - Uncomment the line in your code:
      ```csharp
      builder.WebHost.UseUrls("http://*:80");
    - Set the connection string in appsettings.json to
      ```csharp
      "ConnectionStrings": {
        "DefaultConnection": ""
      }
    - Navigate to the project directory
      ```csharp
      cd DevSkill.Inventory
    - Build and start the containers
      ```csharp
      docker-compose up -d
    #### Option 2:  Use Prebuilt Images
     - Pull the prebuilt images from Docker Hub:
       ```bash
       docker pull safinsarker/easy-inventory-system-image:Web
       docker pull safinsarker/easy-inventory-system-image:Database
     - Use the following docker-compose.yml file for prebuild image:
       ```bash
       version: '3.8'

       services:
         db:
           image: safinsarker/easy-inventory-system-image:Database  
           ports:
             - "1432:1433"
           environment:
             ACCEPT_EULA: "Y"
             MSSQL_SA_PASSWORD: MyPass$123
             MSSQL_DATABASE: master
      
         web:
           image: safinsarker/easy-inventory-system-image:Web
           ports:
             - "8001:80"
           environment:
             - ConnectionStrings__DefaultConnection=Server=db;Database=master;User Id=sa;Password=MyPass$123;TrustServerCertificate=True;
           depends_on:
             - db
      - Start the containers
        ```bash
        docker-compose up -d

3. **Access the Application**
   - Locally: http://localhost:5000
   - Dockerized: http://localhost:8001

        



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

