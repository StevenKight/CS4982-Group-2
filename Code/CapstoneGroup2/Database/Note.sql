CREATE TABLE [dbo].[Note]
(
	[source_id] INT NOT NULL
		CONSTRAINT FK_note_source
		REFERENCES [Source]([source_id])
		ON DELETE CASCADE, 
    [username] VARCHAR(100) NOT NULL
		CONSTRAINT FK_note_creator
		REFERENCES [User]([username])
		ON DELETE CASCADE,
	[note] TEXT NULL,
	[tags] VARCHAR(100) NULL,
	PRIMARY KEY ([source_id], [username]),
)
