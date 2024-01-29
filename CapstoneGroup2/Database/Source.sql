﻿CREATE TABLE [dbo].[Source]
(
	[source_id] INT NOT NULL 
		PRIMARY KEY
		IDENTITY(1,1),
	[type] CHAR(3) NOT NULL,
	[name] VARCHAR(100) NOT NULL,
	[is_link] BIT NOT NULL,
	[link] TEXT NOT NULL
)
