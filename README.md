# Simple Banking System API

## Table of Contents
- [Simple Banking System API](#simple-banking-system-api)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Expected Features](#expected-features)
    - [User Management](#user-management)
    - [Account Management](#account-management)
    - [Transaction Management](#transaction-management)
    - [Loan Management](#loan-management)
    - [Administrative Functions (Admin User)](#administrative-functions-admin-user)
  - [Technologies Used](#technologies-used)
  - [Project Structure](#project-structure)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Configuration](#configuration)
    - [Running the Application](#running-the-application)
  - [Docker and Docker Compose Configuration](#docker-and-docker-compose-configuration)
    - [Database Service (db)](#database-service-db)
    - [API Service (yourprojectapi)](#api-service-yourprojectapi)
    - [Dockerfile](#dockerfile)
  - [API Endpoints](#api-endpoints)
    - [Account Endpoints](#account-endpoints)
    - [Admin Endpoints](#admin-endpoints)
    - [Auth Endpoints](#auth-endpoints)
    - [Loan Endpoints](#loan-endpoints)
    - [Transaction Endpoints](#transaction-endpoints)
    - [User Endpoints](#user-endpoints)
    - [UserProfileUpdate Endpoints](#userprofileupdate-endpoints)
  - [Testing](#testing)
    - [Running Tests](#running-tests)
    - [Coverage Report](#coverage-report)
  - [Deployment to Azure](#deployment-to-azure)
    - [Prerequisites](#prerequisites-1)
    - [Steps](#steps)
      - [Docker](#docker)
      - [Azure](#azure)
  - [Relevant Repo](#relevant-repo)

## Overview
The **Simple Banking System** is a web-based backend application built using ASP.NET Core Web API for creating RESTful services. It provides essential banking functionalities such as user management, account management, transaction management, and loan management. The application uses Entity Framework Core for data management, a Service layer for business logic, and incorporates JSON Web Tokens (JWT) for authentication. API documentation can be created through Swagger and Postman. 

[simplebankingsystemwebapi.azurewebsites.net](
https://simplebankingsystemwebapi.azurewebsites.net/swagger) - hosted in Azure 

## Expected Features

### User Management
- **User Registration**: Users can register for a new account by providing necessary details. The account will be activated by an admin after background verification.
- **User Login/Sessions**: Users can log in to their accounts securely, and sessions are handled with cookies. Users can also log out.
- **View/Edit User Profile**: Users can view and update their profile information, including updating each account's password and profile details (e.g., address) with admin approval.

### Account Management
- **Open a New Account**: Users can open various types of accounts such as savings, salary, etc.
- **View Account Details**: Users can view details of their accounts, including balance and transaction history.
- **Request to Close an Account**: Users can raise a request to close an account if they no longer need it.
- **List All Accounts for a User**: Users can see a list of all their accounts.
- **Manage Account Details**: Users can change transaction passwords and other details.

### Transaction Management
- **Deposit Money**: Users can deposit money into their accounts.
- **Withdraw Money**: Users can withdraw money from their accounts.
- **Transfer Money Between Accounts**: Users can transfer money between their own accounts or to other users (Instant/NEFT/RTFS).
- **Repay Loan**: Users can make repayments towards their outstanding loans (with interests).
- **View Transaction History**: Users can view a history of transactions made on their accounts.

### Loan Management
- **Apply for a Loan**: Users can apply for a loan with specified details such as amount and repayment terms.
- **View Loan Status**: Users can view the status of their loan applications and approved loans.
- **Manage Loans**: Users can track the loans they have applied for.
- **View Loan Repayment History**: Users can view a history of loan repayments.

### Administrative Functions (Admin User)
- **View All Users**: Admin users can view a list of all registered users.
- **Manage User Accounts**: Admin users can manage user accounts, such as opening, deleting, or closing user accounts.
- **Assign Roles and Permissions**: Admin users can assign roles and permissions to other users.
- **View All Accounts**: Admin users can view a list of all accounts in the system.
- **Manage Accounts**: Admin users can delete or close accounts related to users.
- **View All Transactions**: Admin users can view a list of all transactions in the system.
- **Approve Certain Transactions**: Admins can approve RTFS/NEFT transactions if users have submitted any.
- **Approve/Deny Loan Applications**: Admin users can approve or deny loan applications submitted by users.
- **Manage Loan Repayments**: Admin users can manage loan repayments, including marking them as received and charging for late payments.



## Technologies Used
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **JSON Web Tokens (JWT) for authentication**
- **Swagger & Postman for API documentation**

## Project Structure
The project follows a layered architecture to separate concerns and improve maintainability:
- **Controllers**: Handle HTTP requests and responses.
- **Services**: Business logic layer.
- **Repositories**: Data access layer.
- **Models**: Define database entities.
- **DTOs**: Data Transfer Objects for communication between layers.
- **Validators**: Input validation.

## Getting Started

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation
1. **Clone the Repository**
   ```sh
   git clone https://github.com/JaivigneshJv/SimpleBankingSystemAPI.git
   cd SimpleBankingSystemAPI
   ```

2. **Restore Dependencies**
   ```sh
   dotnet restore
   ```

3. **Update Database**
   ```sh
   dotnet ef database update
   ```

### Configuration
Configure your application settings in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "<DB>",
    "WatchManConnection": "<WathcDog - DB>"
  },
  "TokenKey": {
    "JWT": "<Key>"
  },
  "Email": {
    "mail": "<mail>",
    "password": "xxxx xxxx xxxx xxxx" //2FA with apps password [gmail]
  },
  "WatchDog": {
    "username": "admin",
    "password": "admin"
  },
    "AllowedHosts": "*"
  }

```

### Running the Application
1. **Run the API**
   ```sh
   dotnet run --project SimpleBankingSystemAPI
   ```

2. **Access the API**
   Open your browser and navigate to `https://localhost:7080 /http://localhost:5010` to explore the API documentation.

3. **Access WatchDog**
   `https://localhost:7080/watchdog /http://localhost:5010/watchdog` 


## Docker and Docker Compose Configuration

This project uses Docker and Docker Compose for containerization and orchestration. The `docker-compose.yml` file defines two services: `db` and `yourprojectapi`.

### Database Service (db)

The `db` service uses the `mcr.microsoft.com/mssql/server` image and exposes port 1433. It uses a health check to ensure the database is running correctly. The SA password and acceptance of the End User License Agreement (EULA) are set via environment variables.

### API Service (yourprojectapi)

The `yourprojectapi` service uses a custom image `todorokishoto1/simplebankwebapigenspark` and exposes port 8000. It depends on the `db` service and will only start once the `db` service is healthy.

### Dockerfile

The Dockerfile uses multi-stage builds to create a lean production image. It starts from the `mcr.microsoft.com/dotnet/aspnet:6.0` base image, copies the project files, restores any necessary dependencies, builds the application, and publishes it. The final image is built from the base image, and the published files are copied into it. The application is then started with `dotnet SimpleBankingSystemAPI.dll`.

This Docker configuration is hosted on Azure Web Services.

## API Endpoints

### Account Endpoints
- **Open a New Account**
  ```http
  POST /api/v1/Accounts/open-account
  ```
- **View Account Details**
  ```http
  GET /api/v1/Accounts/get-account/{accountId}
  ```
- **Request to Close an Account**
  ```http
  POST /api/v1/Accounts/close-request/{accountId}
  ```
- **List All Accounts for a User**
  ```http
  GET /api/v1/Accounts/get-all-accounts
  ```
- **Change Transaction Password**
  ```http
  PUT /change-transaction-password{accountId}
  ```
- **Request to Close an Account**
  ```http
  POST /api/v1/Accounts/request/close-account/{accountId}
  ```

### Admin Endpoints
- **View All Users**
  ```http
  GET /api/v1/Admin/get-all-user
  ```
- **View All Active Users**
  ```http
  GET /api/v1/Admin/get-all-active-user
  ```
- **View All Inactive Users**
  ```http
  GET /api/v1/Admin/get-all-inactive-user
  ```
- **Activate User**
  ```http
  GET /activate/{userId}
  ```
- **View All Accounts**
  ```http
  GET /api/v1/Admin/get-all-accounts
  ```
- **View All Account Close Requests**
  ```http
  GET /api/v1/Admin/get-all-accounts-close-request
  ```
- **Approve Account Close Request**
  ```http
  POST /api/v1/Admin/request/approve/close-account/{requestId}
  ```
- **Reject Account Close Request**
  ```http
  POST /api/v1/Admin/request/reject/close-account/{requestId}
  ```
- **View All Transaction Requests**
  ```http
  POST /api/v1/Admin/transaction/request/all
  ```
- **View Pending Transaction Requests**
  ```http
  GET /api/v1/Admin/transaction/request/pending
  ```
- **View Approved Transaction Requests**
  ```http
  GET /api/v1/Admin/transaction/request/approved
  ```
- **View Rejected Transaction Requests**
  ```http
  GET /api/v1/Admin/transaction/request/rejected
  ```
- **Approve Transaction Request**
  ```http
  POST /api/v1/Admin/transaction/request/approve/{requestId}
  ```
- **Reject Transaction Request**
  ```http
  POST /api/v1/Admin/transaction/request/reject/{requestId}
  ```
- **View Pending Loan Requests**
  ```http
  GET /api/v1/Admin/loans/request/pending
  ```
- **Approve Loan Request**
  ```http
  POST /api/v1/Admin/loans/request/approve/{loanId}
  ```
- **Reject Loan Request**
  ```http


  POST /api/v1/Admin/loans/request/reject/{loanId}
  ```
- **View Rejected Loan Requests**
  ```http
  GET /api/v1/Admin/loans/request/rejected
  ```
- **View Opened Loans**
  ```http
  GET /api/v1/Admin/loans/request/opened
  ```
- **View Closed Loans**
  ```http
  GET /api/v1/Admin/loans/request/closed
  ```

### Auth Endpoints
- **Register**
  ```http
  POST /api/v1/Auth/register
  ```
- **Login**
  ```http
  POST /api/v1/Auth/login
  ```
- **Logout**
  ```http
  POST /api/v1/Auth/logout
  ```

### Loan Endpoints
- **Get Loan Details**
  ```http
  POST /api/v1/Loan/get-loan-details
  ```
- **Apply for a Loan**
  ```http
  POST /api/v1/Loan/apply-loan
  ```
- **Get All Account Loans**
  ```http
  GET /api/v1/Loan/get-all-account-loans/{accountId}
  ```
- **Repay Loan**
  ```http
  POST /api/v1/Loan/repay-loan/{loanId}
  ```

### Transaction Endpoints
- **Deposit Money**
  ```http
  POST /api/v1/Transaction/deposit/{accountId}
  ```
- **Withdraw Money**
  ```http
  POST /api/v1/Transaction/withdraw/{accountId}
  ```
- **Bank Transfer**
  ```http
  POST /api/v1/Transaction/bank-transfer/{accountId}/{receiverId}
  ```
- **Verify Transaction**
  ```http
  POST /api/v1/Transaction/transfer/verify-transaction/{accountId}/{verificationCode}
  ```
- **Get Transactions**
  ```http
  GET /api/v1/Transaction/get-transactions/{accountId}
  ```
- **Get Transaction Requests**
  ```http
  GET /api/v1/Transaction/get-transaction-request/{accountId}
  ```

### User Endpoints
- **View User Profile**
  ```http
  GET /api/v1/User/profile
  ```
- **Update User Profile**
  ```http
  PUT /api/v1/User/profile
  ```

### UserProfileUpdate Endpoints
- **Update Password**
  ```http
  PUT /api/v1/UserProfileUpdate/update-password
  ```
- **Request Email Update**
  ```http
  PUT /api/v1/UserProfileUpdate/email/request-update
  ```
- **Verify Email Update**
  ```http
  PUT /api/v1/UserProfileUpdate/email/verify-update
  ```

## Testing
Unit tests are written using xUnit and Moq for mocking dependencies.

### Running Tests
```sh
dotnet test
```

### Coverage Report
To generate a code coverage report:
```sh
dotnet test /p:CollectCoverage=true
```


## Deployment to Azure

This project can be deployed to Azure using Azure App Service and Azure SQL Database.

### Prerequisites

- An Azure account
- Azure CLI installed on your local machine

### Steps

#### Docker 

1. **Login to Docker Hub**: Use the Docker CLI to login to your Docker Hub account.

    ```sh
    docker login
    ```

    You will be prompted to enter your Docker Hub username and password.

2. **Build the Docker Image**: Replace `<docker-image-name>` with your preferred Docker image name and `<dockerfile-path>` with the path to your Dockerfile.

    ```sh
    docker build -t <docker-image-name> <dockerfile-path>
    ```

3. **Tag the Docker Image**: Replace `<docker-image-name>`, `<docker-hub-username>`, and `<tag>` with your Docker image name, Docker Hub username, and preferred tag respectively.

    ```sh
    docker tag <docker-image-name> <docker-hub-username>/<docker-image-name>:<tag>
    ```

4. **Push the Docker Image to Docker Hub**: 

    ```sh
    docker push <docker-hub-username>/<docker-image-name>:<tag>
    ```

#### Azure
1. **Login to Azure**: Use the Azure CLI to login to your Azure account.

  ```sh
  az login
  ```

2. **Create a Resource Group**: Replace `<resource-group-name>` and `<location>` with your preferred resource group name and location respectively.

  ```sh
  az group create --name <resource-group-name> --location <location>
  ```

3. **Create an App Service Plan**: Replace `<app-service-plan-name>` with your preferred app service plan name.

  ```sh
  az appservice plan create --name <app-service-plan-name> --resource-group <resource-group-name> --sku B2 --is-linux
  ```

4. **Create a Web App**: Replace `<web-app-name>` with your preferred web app name.

  ```sh
  az webapp create --resource-group <resource-group-name> --plan <app-service-plan-name> --name <web-app-name> --deployment-container-image-name <docker-image-name>
  ```

5. **Configure the Web App to use the Docker Compose file**: Replace `<docker-compose-file-path>` with the path to your Docker Compose file. which has the image for asp.net core web api

  ```sh
  az webapp config container set --name <web-app-name> --resource-group <resource-group-name> --multicontainer-config-type compose --multicontainer-config-file <docker-compose-file-path>
  ```


After following these steps, your application should be deployed to Azure and accessible at `https://<web-app-name>.azurewebsites.net`.


## Relevant Repo

[Genspark Training](https://github.com/JaivigneshJv/GenSpark)
