CREATE TABLE [dbo].[Note]
(
	[note_id] INT NOT NULL
		PRIMARY KEY
		IDENTITY(1,1),
	[source_id] INT NOT NULL
		CONSTRAINT [FK_Note_Source] 
		FOREIGN KEY
		REFERENCES [dbo].[Source]([source_id]),
	[username] VARCHAR(100) NOT NULL
		CONSTRAINT [FK_Note_User] 
		FOREIGN KEY
		REFERENCES [dbo].[User]([username]),
	[note_text] NVARCHAR(MAX) NOT NULL,
	[note_date] DATETIME NOT NULL
		DEFAULT GETDATE(),
	[tags] NVARCHAR(MAX) NULL
)
