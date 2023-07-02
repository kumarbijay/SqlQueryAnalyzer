-- Create a stored procedure named `usp_GetProductsByCategory`
CREATE PROCEDURE usp_GetProductsByCategory @CategoryID int
AS
BEGIN

-- Create a temporary table to store the results of the query
CREATE TABLE #ProductsByCategory (
  ProductID int,
  ProductName varchar(50),
  CategoryID int
)

-- Insert the products from the specified category into the temporary table
INSERT INTO #ProductsByCategory
SELECT ProductID, ProductName, CategoryID
FROM Products
WHERE CategoryID = @CategoryID

-- Select the products from the temporary table
SELECT ProductID, ProductName
FROM #ProductsByCategory

-- Drop the temporary table
DROP TABLE #ProductsByCategory

END
