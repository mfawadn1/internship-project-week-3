-- =============================================
-- Week 3 - Part A: Relational Database Design
-- Author: Asiya
-- Script 1: Database and Table Creation (DDL)
-- =============================================

-- 1. Create Database if it does not exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LibraryDb_Week3')
BEGIN
    CREATE DATABASE LibraryDb_Week3;
END
GO

USE LibraryDb_Week3;
GO

-- 2. Create Authors Table
IF OBJECT_ID('dbo.BookCategories', 'U') IS NOT NULL DROP TABLE dbo.BookCategories;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Authors', 'U') IS NOT NULL DROP TABLE dbo.Authors;
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
GO

CREATE TABLE Authors (
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Bio NVARCHAR(500) NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
GO

-- 3. Create Books Table (with Foreign Key to Authors)
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    AuthorId INT NOT NULL,
    PublishedYear INT NULL,
    ISBN NVARCHAR(50) NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Books_Authors FOREIGN KEY (AuthorId) 
        REFERENCES Authors(AuthorId)
        ON DELETE NO ACTION -- Enforces referential integrity
);
GO

-- 4. Create Categories Table
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- 5. Create BookCategories Link Table (Many-to-Many Relationship)
CREATE TABLE BookCategories (
    BookId INT NOT NULL,
    CategoryId INT NOT NULL,
    AssignedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT PK_BookCategories PRIMARY KEY (BookId, CategoryId),
    CONSTRAINT FK_BookCategories_Books FOREIGN KEY (BookId) 
        REFERENCES Books(BookId) ON DELETE CASCADE,
    CONSTRAINT FK_BookCategories_Categories FOREIGN KEY (CategoryId) 
        REFERENCES Categories(CategoryId) ON DELETE CASCADE
);
GO
