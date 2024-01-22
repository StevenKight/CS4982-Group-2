SET IDENTITY_INSERT [dbo].[UserNote] ON
INSERT INTO [dbo].[UserNote] ([Id], [object_link], [note], [type]) VALUES (1, N'https://samplelib.com/lib/preview/mp4/sample-5s.mp4', N'Sample Notes', N'vid')
INSERT INTO [dbo].[UserNote] ([Id], [object_link], [note], [type]) VALUES (2, N'https://samplelib.com/lib/preview/mp4/sample-30s.mp4', N'Testing Notes', N'vid')
INSERT INTO [dbo].[UserNote] ([Id], [object_link], [note], [type]) VALUES (3, N'https://youtu.be/Z_-am00EXIc?si=TtSBSOySyDDOkqsj', N'Youtube Link', N'vid')
SET IDENTITY_INSERT [dbo].[UserNote] OFF
