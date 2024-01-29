CREATE TABLE [dbo].[Note]
(
	[source_id] INT NOT NULL
		CONSTRAINT FK_note_source
		REFERENCES [Source]([source_id]), 
    [username] VARCHAR(100) NOT NULL
		CONSTRAINT FK_note_creator
		REFERENCES [User]([username]),
	[note] TEXT NULL,
	[tags] VARCHAR(100) NULL,
	PRIMARY KEY ([source_id], [username]),
)
