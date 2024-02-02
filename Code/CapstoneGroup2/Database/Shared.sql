CREATE TABLE [dbo].[Shared]
(
	[source_id] INT NOT NULL, 
	[username] VARCHAR(100) NOT NULL,
	[shared_username] VARCHAR(100) NOT NULL
		CONSTRAINT FK_shared_user
		REFERENCES [User]([username])
		ON DELETE NO ACTION,
	[comment] TEXT NULL,
	PRIMARY KEY ([source_id], [username], [shared_username]),
	CONSTRAINT FK_shared_note
		FOREIGN KEY ([source_id], [username])
		REFERENCES [Note]([source_id], [username])
		ON DELETE CASCADE
)
