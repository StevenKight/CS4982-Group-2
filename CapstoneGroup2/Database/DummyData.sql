/*
Dummy Data Script
--------------------------------------------------------------------------------------
	Purpose: Dummy data for testing purposes
	Creator: Steven Kight
	Date: 2024-1-28
	Version: 1.0.0
--------------------------------------------------------------------------------------
*/

-- TODO: Hash passwords
INSERT INTO [dbo].[User] ([username], [password]) 
	VALUES (N'StevenG', N'Kight'),
		(N'StevenC', N'Carriger'),
		(N'Aaron', N'Hanson')

-- TODO: PDFs, images, etc. and link vs file
SET IDENTITY_INSERT [dbo].[Source] ON
INSERT INTO [dbo].[Source] ([source_id], [type], [name], [is_link], [link]) 
	VALUES (1, N'vid', N'Sample Video 5s.mp4', 1, N'https://samplelib.com/lib/preview/mp4/sample-5s.mp4'),
		(2, N'vid', N'Sample Video 30s.mp4', 1, N'https://samplelib.com/lib/preview/mp4/sample-30s.mp4'),
		(3, N'vid', N'Sample Youtube Video', 1, N'https://youtu.be/Z_-am00EXIc?si=TtSBSOySyDDOkqsj')
SET IDENTITY_INSERT [dbo].[Source] OFF

-- TODO: Tags delimeter and parsing
INSERT INTO [dbo].[Note] ([source_id], [username], [note], [tags]) 
	VALUES (1, N'StevenG', N'Sample note on a 5 second video', N'<TAGS>'),
		(2, N'StevenG', N'Sample note on a 30 second video', N'<TAGS>'),
		(3, N'StevenG', N'Sample note on a youtube video', N'<TAGS>'),
		(1, N'StevenC', N'Sample note on a 5 second video', N'<TAGS>'),
		(2, N'StevenC', N'Sample note on a 30 second video', N'<TAGS>'),
		(3, N'StevenC', N'Sample note on a youtube video', N'<TAGS>'),
		(1, N'Aaron', N'Sample note on a 5 second video', N'<TAGS>'),
		(2, N'Aaron', N'Sample note on a 30 second video', N'<TAGS>'),
		(3, N'Aaron', N'Sample note on a youtube video', N'<TAGS>')
