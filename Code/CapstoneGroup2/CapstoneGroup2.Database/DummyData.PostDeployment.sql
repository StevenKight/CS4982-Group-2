/*
Post-Deployment Script To Insert Dummy Data			
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
INSERT INTO [dbo].[User] (username, password)
VALUES (N'StevenG', N'Kight'),
     (N'StevenC', N'Carriger'),
     (N'Aaron', N'Hanson'),
     (N'Jonathan', N'Corley');

SET IDENTITY_INSERT [dbo].[Source] ON;
INSERT INTO [dbo].[Source] (source_id, username, name, description, type, is_link, link)
VALUES (1, N'StevenG', N'Placeholder PDF', N'Placeholder PDF for demo', N'PDF', 1, N'https://s2.q4cdn.com/175719177/files/doc_presentations/Placeholder-PDF.pdf'),
	 (2, N'StevenC', N'Placeholder PDF', N'Placeholder PDF for demo', N'PDF', 1, N'https://s2.q4cdn.com/175719177/files/doc_presentations/Placeholder-PDF.pdf'),
	 (3, N'Aaron', N'Placeholder PDF', N'Placeholder PDF for demo', N'PDF', 1, N'https://s2.q4cdn.com/175719177/files/doc_presentations/Placeholder-PDF.pdf'),
	 (4, N'Jonathan', N'Placeholder PDF', N'Placeholder PDF for demo', N'PDF', 1, N'https://s2.q4cdn.com/175719177/files/doc_presentations/Placeholder-PDF.pdf');
SET IDENTITY_INSERT [dbo].[Source] OFF;

INSERT INTO [dbo].[Note] (source_id, username, note_text, tags)
VALUES (1, N'StevenG', N'Placeholder note for demo', N'Placeholder'),
	(1, N'StevenG', N'Placeholder note for demo 2', N'Placeholder'),
	(1, N'StevenG', N'Placeholder note for demo 3', N'Placeholder'),
	(2, N'StevenC', N'Placeholder note for demo', N'Placeholder'),
	(3, N'Aaron', N'Placeholder note for demo', N'Placeholder'),
	(4, N'Jonathan', N'Placeholder note for demo', N'Placeholder');