CREATE TABLE [dbo].[UserNote]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [object_link] VARCHAR(100) NULL, 
    [note] TEXT NULL
)
