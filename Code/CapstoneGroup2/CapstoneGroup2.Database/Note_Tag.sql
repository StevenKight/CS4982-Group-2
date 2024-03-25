CREATE TABLE [dbo].[Note_Tag]
(
    [note_id] INT NOT NULL,
    [tag_id] INT NOT NULL,
    CONSTRAINT [PK_Note_Tag] PRIMARY KEY CLUSTERED ([note_id], [tag_id]),
    CONSTRAINT [FK_Note_Tag_Note] FOREIGN KEY ([note_id]) REFERENCES [dbo].[Note]([note_id]),
    CONSTRAINT [FK_Note_Tag_Tag] FOREIGN KEY ([tag_id]) REFERENCES [dbo].[Tag]([tag_id])
);
