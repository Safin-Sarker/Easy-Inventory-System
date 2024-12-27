# Easy Inventory System

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](link-to-ci-pipeline)

## Table of Contents
- [About the Project](#about-the-project)
- [Demo Videos](#demo-videos)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Running Tests](#running-tests)
- [Continuous Integration/Continuous Deployment(CI/CD)](#continuous-integrationcontinuous-deployment-cicd)
- [Folder Structure](#folder-structure)
- [Contributing](#contributing)
- [Future Work](#future-work)
- [Contact](#contact)

---

## About the Project

**Easy Inventory System** is a comprehensive inventory management solution designed to help users efficiently track products, inventories, and warehouses. The application emphasizes role-based and claim-based authorization while implementing clean architecture principles for scalability, maintainability, and testability.

---

## Demo Videos

### Full Demo Video

Check out the full demo of the **Easy Inventory System** below:

[Full Demo Video](https://drive.google.com/file/d/1knXmkGj3z53mVa9EgZBPJCbnIOCWZicJ/view?usp=sharing "Watch Full Demo Video")


> *Click  above to watch the full demo video .*

---

### Trailer/Short Demo Video

Here's a short trailer of the **Easy Inventory System**:

[Short Demo Video](https://drive.google.com/file/d/1FrlGbHD4T-tXeugJS8yVUQGtul6yLXPh/view?usp=sharing "Watch Short Demo Video")

> *Click  above to watch the short demo video .*

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


---



## Technologies Used

- **Framework:** ASP.NET Core MVC
- **Frontend:** Bootstrap, DataTables, jQuery, Ajax, JavaScript
- **Database:** SQL Server (with Entity Framework Core using Repository and Unit of Work pattern)
- **Architecture:** Clean Architecture
- **Testing:** NUnit (for service unit testing)
- **Containerization:** Docker
- **Security:** Claim-based authentication, cookie-based authentication and authorization
- **Utilities:** AutoMapper, Autofac, Serilog, and exception handling
- **Performance:** Stored Procedures for optimized database operations
- **Database Management:** AutoMigration
- **Rich Text Editor:** TinyMCE
- **Image Upload:** Cloudinary

---

## Getting Started

### Prerequisites

Ensure you have the following tools installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (Optional: Only needed if you want to run the application in Docker)
- **MSSQL** (SQL Server)

---

## Installation

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

## Usage
    
1. **Run the Application Locally**  
   Use the following command to start the application:  
   ```bash
   dotnet run --urls "http://localhost:5000"
2. **Run the Application with Docker**
   - You can also containerize and run the application using Docker.
   #### Option 1: Build Image and Run Container
     Prepare the application:
    - Uncomment the line from program.cs:
      ```csharp
      builder.WebHost.UseUrls("http://*:80");
    - Set the connection string empty in appsettings.json
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
     - Go to My DockerHub Repository: [Easy Inventory System Docker Image](https://hub.docker.com/repository/docker/safinsarker/easy-inventory-system-image/general)       
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

4. **Access the Application**
   - **Locally**: http://localhost:5000
   - **Dockerized**: http://localhost:8001
   - **SuperAdmin user** can manage users, roles, and inventory from the admin dashboard.
   - **SuperAdmin** (Seeded Credentials):
     - **Email**: superadmin@example.com
     - **Password**: SuperAdmin@123
     
---

## Running Tests
To run the tests for this project, follow these steps:
1. **Navigate to the DevSkill.Inventory.Application.Tests directory**:
   ```bash
   cd DevSkill.Inventory.Application.Tests
2. **Ensure you have the required dependencies**:
   Before running the tests, make sure all required dependencies are installed by running:
   ```bash
   dotnet restore
3. **Run the tests:** To run the tests for this project, use the following command:
    ```bash
   dotnet test
4. **View the results:** The test results will be displayed in the terminal. If you prefer a more detailed output, you can add the --logger option:
   ```bash
   dotnet test --logger "console;verbosity=detailed"
---

## Continuous Integration/Continuous Deployment (CI/CD)

The project integrates a CI/CD pipeline using **GitHub Actions** to automate testing, building, and Docker image pushing. Below is a summary:

### Workflow Overview

1. **Automated Testing**: Runs unit tests on every push to the repository.
2. **Build Validation**: Validates the application build for the changes.
3. **Docker Build and Push**: 
   - Builds Docker images for the application.
   - Pushes the Docker images to Docker Hub.

### GitHub Actions Workflow File

The CI/CD pipeline is defined in the `.github/workflows/main.yml` file.

---


## Folder Structure

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
---
## Contributing
Contributions are welcome! Follow these steps to contribute:

1. **Fork the project**.
   Create a new branch:
   ```bash
   git checkout -b feature/YourFeature
2. **Commit your changes**:
   ```bash
   git commit -m 'Add some feature'
3. **Push to the branch**:
   ```bash
   git push origin feature/YourFeature
   
4. **Open a pull request**.

---
## Future Work

1. **Adding reCAPTCHA for Public Forms**: 
   - Implement reCAPTCHA to prevent bots and ensure that only legitimate users can submit public forms.

2. **Integrate AWS SQS Bucket and Worker Service**:
   - After uploading an image, the image will be sent to an AWS SQS queue.
   - A worker service will process the image (e.g., resize it) and then upload the processed image back to the appropriate location (e.g., AWS S3 bucket or your desired storage).
---

## Contact
For questions or feedback, reach out to:

- Author: Md Safin Sarker
- Email:safinsarker1122@gmail.com
- Linkedin: www.linkedin.com/in/safin-sarker

