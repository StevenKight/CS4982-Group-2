-- Create Tags table
CREATE TABLE [dbo].[Tag]
(
    [tag_id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [tag_name] NVARCHAR(100) NOT NULL
);