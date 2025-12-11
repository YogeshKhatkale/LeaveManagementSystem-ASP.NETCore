# Leave Management System – ASP.NET Core API

A clean, scalable ASP.NET Core Web API for managing employee leave workflows.  
Supports applying for leave, approval/rejection flow, role-based actions, and a modular layered architecture using Entity Framework Core and SQL Server.

---

## 🚀 Features

### 👨‍💼 Employee
- Apply for leave  
- View leave history  
- Check leave status  

### 🛠️ Admin
- View all pending leave requests  
- Approve or reject leave  
- Manage employees (optional based on your implementation)  

### ⚙️ System
- Entity Framework Core (Code First + Migrations)  
- Separation of controllers, DTOs, services, and data access  
- Strong validation and response structure  
- Clean, production-ready API structure  

---

## 🧰 Tech Stack

| Component | Technology |
|----------|------------|
| Framework | ASP.NET Core Web API |
| Language | C# |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Architecture | Layered (Controllers → Services → Data → Models/DTOs) |
| Tools | Postman, EF Core CLI |

---

## 📁 Project Structure

EmployeeLeaveManagementSys/
│── Controllers/ # API endpoints (Admin, Auth, Employee)
│── Data/ # DbContext, DB configuration
│── DTOs/ # Request/Response models
│── Migrations/ # EF Core migrations history
│── Models/ # Database models (Entities)
│── Services/ # Business logic layer
│── logs/ # Ignored (local logs)
│── appsettings.json # Base config (no secrets)
│── Program.cs # App startup
│── EmployeeLeaveManagementSys.sln
