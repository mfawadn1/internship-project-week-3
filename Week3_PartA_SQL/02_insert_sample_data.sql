-- =============================================
-- Week 3 - Part A: Relational Database Design
-- Author: Asiya
-- Script 2: Sample Data Insertion (DML)
-- =============================================

USE LibraryDb_Week3;
GO

-- 1. Insert 3 Authors
INSERT INTO Authors (FullName, Bio)
VALUES 
('George Orwell', 'English novelist and essayist, famous for 1984 and Animal Farm.'),
('J.K. Rowling', 'British author, best known for the Harry Potter fantasy series.'),
('Robert C. Martin', 'Software engineer and author, also known as Uncle Bob (Clean Code).');
GO

-- Retrieve Author IDs for mapping
DECLARE @OrwellId INT = (SELECT AuthorId FROM Authors WHERE FullName = 'George Orwell');
DECLARE @RowlingId INT = (SELECT AuthorId FROM Authors WHERE FullName = 'J.K. Rowling');
DECLARE @UncleBobId INT = (SELECT AuthorId FROM Authors WHERE FullName = 'Robert C. Martin');

-- 2. Insert 6 Books (2 for each Author)
INSERT INTO Books (Title, AuthorId, PublishedYear, ISBN)
VALUES 
('1984', @OrwellId, 1949, '978-0451524935'),
('Animal Farm', @OrwellId, 1945, '978-0451526342'),
('Harry Potter and the Sorcerer''s Stone', @RowlingId, 1997, '978-0590353427'),
('Harry Potter and the Chamber of Secrets', @RowlingId, 1998, '978-0439064873'),
('Clean Code: A Handbook of Agile Software Craftsmanship', @UncleBobId, 2008, '978-0132350884'),
('Clean Architecture: A Craftsman''s Guide to Software Structure', @UncleBobId, 2017, '978-0134494166');
GO

-- 3. Insert Categories
INSERT INTO Categories (CategoryName)
VALUES 
('Classic Fiction'),
('Dystopian'),
('Fantasy'),
('Magic'),
('Software Engineering'),
('Programming & Design');
GO

-- 4. Map Books to Categories in BookCategories Link Table (Many-to-Many)
DECLARE @Book1984 INT = (SELECT BookId FROM Books WHERE Title = '1984');
DECLARE @BookAnimalFarm INT = (SELECT BookId FROM Books WHERE Title = 'Animal Farm');
DECLARE @BookHP1 INT = (SELECT BookId FROM Books WHERE Title = 'Harry Potter and the Sorcerer''s Stone');
DECLARE @BookCleanCode INT = (SELECT BookId FROM Books WHERE Title = 'Clean Code: A Handbook of Agile Software Craftsmanship');

DECLARE @CatClassic INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Classic Fiction');
DECLARE @CatDystopian INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Dystopian');
DECLARE @CatFantasy INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Fantasy');
DECLARE @CatMagic INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Magic');
DECLARE @CatSE INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Software Engineering');
DECLARE @CatDesign INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Programming & Design');

INSERT INTO BookCategories (BookId, CategoryId)
VALUES 
(@Book1984, @CatClassic),
(@Book1984, @CatDystopian),
(@BookAnimalFarm, @CatClassic),
(@BookAnimalFarm, @CatDystopian),
(@BookHP1, @CatFantasy),
(@BookHP1, @CatMagic),
(@BookCleanCode, @CatSE),
(@BookCleanCode, @CatDesign);
GO
