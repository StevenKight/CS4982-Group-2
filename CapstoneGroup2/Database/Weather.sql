CREATE TABLE [dbo].[Weather]
(
	[date] DATE NOT NULL PRIMARY KEY, 
    [temperature] DECIMAL(4, 2) NOT NULL, 
    [summary] VARCHAR(25) NOT NULL
)
