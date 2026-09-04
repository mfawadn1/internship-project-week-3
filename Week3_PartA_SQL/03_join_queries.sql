-- =============================================
-- Week 3 - Part A: Relational Database Design
-- Author: Asiya
-- Script 3: JOIN Queries, Many-to-Many Queries & FK Integrity
-- =============================================

USE LibraryDb_Week3;
GO

-- ==========================================================
-- Query 1: INNER JOIN - List every book with its author's name
-- ==========================================================
SELECT 
    b.BookId,
    b.Title,
    b.PublishedYear,
    b.ISBN,
    a.AuthorId,
    a.FullName AS AuthorName
FROM Books b
INNER JOIN Authors a ON b.AuthorId = a.AuthorId
ORDER BY a.FullName, b.PublishedYear;
GO

-- ==========================================================
-- Query 2: Many-to-Many JOIN - Return all categories for a specific book
-- (e.g. '1984' or 'Clean Code')
-- ==========================================================
SELECT 
    b.Title,
    a.FullName AS Author,
    c.CategoryName
FROM Books b
INNER JOIN Authors a ON b.AuthorId = a.AuthorId
INNER JOIN BookCategories bc ON b.BookId = bc.BookId
INNER JOIN Categories c ON bc.CategoryId = c.CategoryId
WHERE b.Title = '1984';
GO

-- ==========================================================
-- Query 3: Aggregated Many-to-Many (STRING_AGG)
-- Lists all books with comma-separated categories
-- ==========================================================
SELECT 
    b.BookId,
    b.Title,
    a.FullName AS Author,
    STRING_AGG(c.CategoryName, ', ') AS Categories
FROM Books b
INNER JOIN Authors a ON b.AuthorId = a.AuthorId
LEFT JOIN BookCategories bc ON b.BookId = bc.BookId
LEFT JOIN Categories c ON bc.CategoryId = c.CategoryId
GROUP BY b.BookId, b.Title, a.FullName;
GO

-- ==========================================================
-- Query 4: Referential Integrity Test
-- Trying to delete an author who still has books linked.
-- Expected Result: SQL Server throws error 547 (FK violation)
-- ==========================================================
BEGIN TRY
    -- Try deleting George Orwell (AuthorId with linked books)
    DELETE FROM Authors WHERE FullName = 'George Orwell';
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_MESSAGE() AS ErrorMessage,
        'Explanation: SQL Server refused the delete because the Books table has foreign key rows referencing this AuthorId. To maintain data integrity, parent rows cannot be deleted while dependent children exist.' AS IntegrityExplanation;
END CATCH
GO
