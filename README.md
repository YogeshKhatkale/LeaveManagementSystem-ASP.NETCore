# Leave Management System – ASP.NET Core API

A fully implemented Employee Leave Management API built with ASP.NET Core, EF Core, and SQL Server, featuring role-based workflows, leave approval handling, and clean layered architecture.

---

## 🚀 Features

### 👨‍💼 Employee
- Apply for leave with date range and reason
- View personal leave history
- Track leave request status (Pending/Approved/Rejected)

### 👔 Manager
- View all pending leave requests from team members
- Approve or reject leave applications
- Access team leave history and analytics

### 🛠️ Admin
- System-wide leave management
- View all employee leave records
- Manage employee accounts and permissions

### ⚙️ System Architecture
- **Entity Framework Core** with Code First approach and Migrations
- **Layered Architecture**: Clear separation of Controllers, Services, Data Access, and DTOs
- **Role-based Authorization**: Employee, Manager, and Admin roles
- **JWT Authentication**: Secure token-based authentication
- **Robust Validation**: Request validation and structured error responses
- **Production-ready Structure**: Exception handling, logging, and clean code practices

---

## 🧰 Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core Web API (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Architecture | Layered (Controllers → Services → Data → Models/DTOs) |
| Authentication | JWT (JSON Web Tokens) |
| Tools | Postman, Swagger, EF Core CLI |

---

## 📁 Project Structure

```
EmployeeLeaveManagementSys/
├── Controllers/           # API endpoints (Auth, Employee, Manager, Admin)
├── Data/                  # DbContext and database configuration
├── DTOs/                  # Data Transfer Objects (Request/Response models)
├── Migrations/            # EF Core migration history
├── Models/                # Database entities (Employee, Leave, User)
├── Services/              # Business logic layer
├── appsettings.json       # Application configuration
├── Program.cs             # Application startup and middleware
└── EmployeeLeaveManagementSys.sln
```

---

## 🔐 Authentication & Authorization

- **JWT Token-based authentication** for secure API access
- **Role-based access control (RBAC)** with three roles:
  - **Employee**: Apply for leave, view own records
  - **Manager**: Approve/reject team requests, view team data
  - **Admin**: Full system access and user management

---

## 📡 API Endpoints

### Authentication
```
POST   /api/auth/register      # Register new user
POST   /api/auth/login         # Login and receive JWT token
```

### Employee Operations
```
GET    /api/employee/leaves         # Get personal leave history
POST   /api/employee/leaves         # Submit new leave request
GET    /api/employee/leaves/{id}    # Get specific leave details
```

### Manager Operations
```
GET    /api/manager/pending         # View all pending requests
PUT    /api/manager/approve/{id}    # Approve leave request
PUT    /api/manager/reject/{id}     # Reject leave request
GET    /api/manager/team-leaves     # View team leave history
```

### Admin Operations
```
GET    /api/admin/all-leaves        # View all system leaves
GET    /api/admin/employees         # Manage employee accounts
```

---

## ⚙️ Setup & Installation

### Prerequisites
- **.NET 8 SDK** or higher
- **SQL Server** (LocalDB, Express, or Full)
- **Visual Studio 2022** or **VS Code**
- **Postman** (for API testing)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/YogeshKhatkale/LeaveManagementSystem-ASP.NETCore.git
   cd LeaveManagementSystem-ASP.NETCore
   ```

2. **Update the connection string**  
   Edit `appsettings.json` and update your SQL Server connection:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=LeaveManagementDB;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**  
   Navigate to: `https://localhost:5001/swagger` (or your configured port)

---

## 🧪 Testing

- **Swagger UI**: Interactive API documentation at `/swagger`
- **Postman**: Import endpoints and test with authentication tokens
- All endpoints require valid JWT token (except register/login)

### Sample Request Flow
1. Register a new user via `/api/auth/register`
2. Login via `/api/auth/login` to receive JWT token
3. Add token to Authorization header: `Bearer YOUR_TOKEN`
4. Make authenticated requests to protected endpoints

---

## 🏗️ Architecture Highlights

- **Layered Architecture**: Controllers → Services → Data Access → Database
- **Dependency Injection**: Services registered in `Program.cs`
- **Repository Pattern**: Data access abstraction for maintainability
- **DTO Pattern**: Separate request/response models from entities
- **Exception Handling**: Centralized error handling middleware
- **Code First Migrations**: Version-controlled database schema

---

## 💡 Key Learnings

- Implementing JWT authentication and role-based authorization in ASP.NET Core
- Building scalable layered architecture for enterprise applications
- Using Entity Framework Core migrations for database version control
- Designing RESTful APIs following industry best practices
- API testing and documentation using Swagger and Postman
- Implementing business logic with proper separation of concerns

---

## 📝 Future Enhancements

- [ ] Email notifications for leave approvals/rejections
- [ ] Leave balance tracking and calendar integration
- [ ] Export leave reports to PDF/Excel
- [ ] Admin dashboard with analytics
- [ ] Multi-level approval workflow
- [ ] Leave type management (Sick, Casual, Earned)

---

## 👤 Author

**Yogesh Khatkale**  
- LinkedIn: [yogesh-khatkale](https://linkedin.com/in/yogesh-khatkale-135ba2253)  
- GitHub: [@YogeshKhatkale](https://github.com/YogeshKhatkale)  
- Email: yogesh.khatkale1@gmail.com

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](https://github.com/YogeshKhatkale/LeaveManagementSystem-ASP.NETCore/issues).

---

⭐ **If you found this project helpful, please give it a star!** ⭐
