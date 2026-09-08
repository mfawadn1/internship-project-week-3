# 📚 internship-project-week-4 (Week 4 Milestone: Secured Library App + AI Service)

[![Repository: internship-project-week-4](https://img.shields.io/badge/Repo-internship--project--week--4-blue)](https://github.com/)
[![Framework: .NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Security: JWT & PasswordHasher](https://img.shields.io/badge/Security-JWT_%26_Identity_Hashing-green)](https://jwt.io/)
[![Frontend: Angular 18](https://img.shields.io/badge/Frontend-Angular_18-DD0031?logo=angular&logoColor=white)](https://angular.dev/)
[![AI Microservice: FastAPI](https://img.shields.io/badge/AI_Service-FastAPI_0.110-009688?logo=fastapi&logoColor=white)](https://fastapi.tiangolo.com/)
[![Database: SQL Server & EF Core](https://img.shields.io/badge/Database-SQL_Server_%26_EF_Core-CC292B?logo=microsoftsqlserver&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)

---

## 🌟 Overview & Week 4 Architecture

In **Week 4**, the system introduces **end-to-end security** and transitions the AI foundation into a standalone **FastAPI Microservice with prompt engineering**:

1. **JWT Authentication & Role Authorization (.NET 8)**:
   - Secure one-way password hashing via `PasswordHasher<User>`.
   - Signed JWT Bearer token generation with claims (`sub`, `name`, `role`) and expiration.
   - Endpoint protection (`[Authorize]` on book mutation and `[Authorize(Roles = "Admin")]` on `DELETE /api/books/{id}`).
   - Interactive Swagger testing with Bearer authorization dialog.

2. **Angular Auth Integration**:
   - `AuthService` handling token persistence and session observation.
   - Functional HTTP Interceptor (`authInterceptor`) automatically attaching `Authorization: Bearer <token>`.
   - Functional Route Guard (`authGuard`) securing `/add-book`.
   - Reactive Login & Registration UI with role-based feature display.

3. **FastAPI AI Microservice (`Week4_PartC_FastAPIService`)**:
   - Modular REST microservice with Pydantic validation and automatic interactive docs (`/docs`).
   - Prompt engineering (Few-Shot templates, system role guardrails, prompt injection resilience).
   - Defensively parsed structured JSON outputs.

4. **Git Level Up**:
   - Standardized Pull Request template (`.github/PULL_REQUEST_TEMPLATE.md`).
   - Branch protection on `main` and manual merge conflict resolution.

---

## 🔄 Architectural Diagram

```mermaid
flowchart TD
    subgraph Frontend [Angular Client - Port 4200]
        UI[Angular UI & Reactive Forms]
        Guard[AuthGuard (CanActivate)]
        Interceptor[AuthInterceptor (Bearer Token)]
    end

    subgraph BackendAPI [.NET 8 Web API - Port 5252]
        AuthController[AuthController (/api/auth/register & /login)]
        Hasher[PasswordHasher<User>]
        BooksController[BooksController ([Authorize] & [Roles='Admin'])]
        EFCore[EF Core DbContext]
        SQL[(SQL Server)]
    end

    subgraph AIService [FastAPI Microservice - Port 8000]
        FastAPIApp[FastAPI Path Operations]
        Pydantic[Pydantic Models]
        Prompts[Few-Shot & System Guardrails]
        Parser[Resilient Defensive JSON Parser]
    end

    UI -->|1. Credentials| AuthController
    AuthController -->|2. Verify Hash & Sign| Hasher
    AuthController -->|3. Return Signed JWT| UI
    UI -->|4. Authenticated HTTP Calls| Interceptor
    Interceptor -->|5. Bearer Token| BooksController
    BooksController -->|6. Data Queries| EFCore --> SQL

    Developer([API Consumer / Docs]) -->|Direct Request| FastAPIApp
    FastAPIApp --> Pydantic --> Prompts --> Parser
```

---

## 🏗️ Repository Structure

```text
├── .github/
│   └── PULL_REQUEST_TEMPLATE.md         # Standard PR checklist & template
│
├── LibraryAPI/                           # ASP.NET Core 8 Web API backend
│   ├── Controllers/
│   │   ├── AuthController.cs             # Register & Login with JWT generation
│   │   └── BooksController.cs            # [Authorize] and Admin-only DELETE
│   ├── Data/                             # LibraryDbContext (EF Core)
│   ├── Models/                           # User, Book, Author, Category
│   ├── Repositories/                     # IBookRepository, BookRepository
│   ├── Services/                         # IBookService, BookService
│   ├── appsettings.json                  # Database connection strings & JWT settings
│   └── Program.cs                        # JWT Bearer, Swagger Bearer Auth, CORS
│
├── library-frontend/                     # Angular Standalone client
│   ├── src/app/
│   │   ├── services/auth.service.ts      # Authentication & token state service
│   │   ├── interceptors/auth.interceptor.ts # Functional Bearer token interceptor
│   │   ├── guards/auth.guard.ts          # Functional route guard
│   │   ├── login/                        # Reactive login & registration component
│   │   ├── book-list/                    # Role-conditional actions (Admin delete)
│   │   ├── book-form/                    # Protected book creation form
│   │   └── app.config.ts                 # provideHttpClient with authInterceptor
│   └── src/environments/
│
├── Week4_PartA_AuthBackend/              # Practice & reference documentation for JWT
├── Week4_PartB_AngularAuth/              # Practice & reference for Angular auth
├── Week4_PartC_FastAPIService/           # FastAPI AI Microservice
│   ├── main.py                           # /health, /summarize, /genre-suggestion
│   ├── requirements.txt                  # fastapi, uvicorn, pydantic, dotenv
│   └── .env.example
├── Week4_PartD_LLMAPIs/                  # Multi-turn history, streaming & structured JSON
├── Week4_PartE_PromptEngineering/        # Few-shot templates & injection defense
├── Week4_PartF_GitPractice/              # Merge conflict & branch protection notes
└── README.md
```

---

## 🚀 How to Run the Applications

### 1. Backend (.NET 8 Web API)
```bash
cd LibraryAPI
dotnet build
dotnet run
```
> **Swagger UI**: Navigate to `http://localhost:5252/swagger` to inspect endpoints and use the **Authorize** button with your JWT token.

---

### 2. Frontend (Angular Client)
```bash
cd library-frontend
npm start
```
> Navigate to `http://localhost:4200` to access the secured library interface.

---

### 3. FastAPI AI Microservice (Python)
```bash
cd Week4_PartC_FastAPIService

# 1. Virtual environment
python -m venv .venv
.venv\Scripts\activate

# 2. Install dependencies
pip install -r requirements.txt

# 3. Start Uvicorn Server
uvicorn main:app --reload --port 8000
```
> **FastAPI Interactive Docs**: Navigate to `http://localhost:8000/docs` to test `/summarize` and `/genre-suggestion`.

---

## 📡 Key API Endpoints

| Method | Endpoint | Authorization | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | Public | Registers a new user with hashed password |
| `POST` | `/api/auth/login` | Public | Returns signed JWT token and user role |
| `GET` | `/api/books` | Public | Retrieves all books |
| `GET` | `/api/books/{id}` | Public | Retrieves a single book |
| `POST` | `/api/books` | `Bearer JWT` (Any Role) | Adds a new book |
| `PUT` | `/api/books/{id}` | `Bearer JWT` (Any Role) | Updates an existing book |
| `DELETE` | `/api/books/{id}` | `Bearer JWT` (`Admin` Only) | Deletes a book (403 for normal users) |
| `GET` | `/health` (Port 8000) | Public | FastAPI AI Service health status |
| `POST` | `/summarize` (Port 8000)| Public | Structured AI book summary & genre |
| `POST` | `/genre-suggestion` (Port 8000)| Public | AI genre classification |

---

## 🌿 Git Branches & Milestone

- **Branch Checkpoint Convention**:
  - `feature/jwt-auth-backend`
  - `feature/role-authorization`
  - `feature/angular-auth`
  - `feature/ai-fastapi-service`
  - `chore/pull-request-template`
- **Milestone Tag**: `v0.4-week4`
