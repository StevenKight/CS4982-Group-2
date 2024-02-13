CREATE TABLE [dbo].[Source]
(
	[source_id] INT NOT NULL 
		PRIMARY KEY
		IDENTITY(1,1),
	[username] VARCHAR(100) NOT NULL
		CONSTRAINT [FK_Source_User] 
		FOREIGN KEY ([username]) 
		REFERENCES [User]([username]),
	[name] VARCHAR(50) NOT NULL,
	[description] VARCHAR(100) NOT NULL,
	[type] CHAR(3) NOT NULL,
	[is_link] BIT NOT NULL,
	[link] VARCHAR(100) NOT NULL,
	[content] VARBINARY(MAX),
	[created_at] DATETIME NOT NULL
		DEFAULT GETDATE(),
	[updated_at] DATETIME NULL
)
