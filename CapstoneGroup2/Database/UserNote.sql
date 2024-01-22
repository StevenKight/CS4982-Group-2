CREATE TABLE [dbo].[UserNote]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [object_link] VARCHAR(100) NULL, 
    [note] TEXT NULL, 
    [type] CHAR(3) NOT NULL,
)
