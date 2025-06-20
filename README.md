# 🔐 User Management API

## 🚀 Features

- 👤 **User CRUD Operations**
  - Create, Read, Update, Delete users
  - Validation and error handling
  - Unique email and role enforcement

- 🔑 **Authentication**
  - JWT Bearer Token support
  - Secure login & token-based access
  - Expiry handling and token validation

- 🛡️ **Authorization Permissions**
  - Role-based access (Read, Create, Update, Delete)
  - Custom policy handlers for permissions

- 🧠 **CQRS Pattern**
  - Clean separation between Commands & Queries
  - Scalable and testable logic organization

- 🧱 **Clean Architecture**
  - Organized layers: Application, Domain, Infrastructure, WebAPI
  - SOLID principles applied across layers

## 🧰 Tech Stack

| Technology            | Purpose                                |
|------------------------|----------------------------------------|
| ASP.NET Core 9         | Web API Framework                      |
| Entity Framework Core  | ORM and database access                |
| SQL Server             | Database                               |
| JWT                    | Authentication Token                   |
| CQRS                   | Command Query Responsibility Segregation |
| Clean Architecture     | Project structure & separation of concerns |
| SOLID Principles       | Maintainable & scalable code design    |


## 📌 API Endpoints

| Method | Endpoint             | Description              |
|--------|----------------------|--------------------------|
| POST   | /api/Auth/login      | User login               |
| GET    | /api/users           | Get all users (Read)     |
| GET    | /api/users/{id}      | Get user by Id (Read)    |
| POST   | /api/users           | Create new user (Write)  |
| PUT    | /api/users/{id}      | Update user (Update)     |
| DELETE | /api/users/{id}      | Delete user (Delete)     |
