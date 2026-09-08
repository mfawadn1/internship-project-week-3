# Week 4 - Part A: JWT Authentication in ASP.NET Core (Real Implementation)

## 🎯 Objectives
- Replace plain-text passwords with secure one-way hashing using `PasswordHasher<User>`.
- Implement `POST /api/auth/register` and `POST /api/auth/login`.
- Generate signed JSON Web Tokens (JWT) containing user claims (`id`, `username`, `role`) and expiry.
- Configure ASP.NET Core JWT Bearer authentication middleware (`AddJwtBearer`, `UseAuthentication()`, `UseAuthorization()`).
- Protect Book endpoints:
  - Require authentication (`[Authorize]`) for `POST /api/books` and `PUT /api/books/{id}`.
  - Require Admin role (`[Authorize(Roles = "Admin")]`) for `DELETE /api/books/{id}`.
  - Keep `GET` endpoints public.

## 🛠️ Step-by-Step Implementation Guide

### 1. Install JWT Bearer Package
In `LibraryAPI`:
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### 2. Configure JWT Settings in `appsettings.Development.json`
```json
{
  "Jwt": {
    "Key": "YOUR_VERY_STRONG_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG_HERE",
    "Issuer": "LibraryAPI",
    "Audience": "LibraryAppUsers",
    "ExpiryMinutes": 120
  }
}
```

### 3. Register Middleware in `Program.cs`
```csharp
var jwtKey = builder.Configuration["Jwt:Key"] ?? "DefaultFallbackSecretKeyMustBeLongEnough123!";
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Make sure UseAuthentication() comes before UseAuthorization()
app.UseAuthentication();
app.UseAuthorization();
```

### 4. Practice Exercises
- [ ] Register a user via Swagger and verify in SQL Server that `PasswordHash` is a hashed string, not plain text.
- [ ] Log in, copy the returned JWT token, and paste it in Swagger's **Authorize** modal (`Bearer <token>`).
- [ ] Test calling `POST /api/books` with and without the token (verify `401 Unauthorized` without token).
- [ ] Decode your token at [jwt.io](https://jwt.io) to verify claims and expiration.
- [ ] Test deleting a book as a regular `User` (should return `403 Forbidden`) vs `Admin` (should succeed).

## 🌿 Git Checkpoint
```bash
git checkout -b feature/jwt-auth-backend
git commit -m "feat: add JWT authentication with register, login, and role-based authorization"
```
