CREATE TABLE [dbo].[RoomProfilePic] (
    [Id]     NVARCHAR (50)   NOT NULL,
    [RoomId] NVARCHAR (50)   NOT NULL,
    [data]   VARBINARY (MAX) NOT NULL,
    [ext]    NVARCHAR (5)    NOT NULL,
    CONSTRAINT [PK_RoomProfilePic] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoomProfilePic_Room] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([Id])
);

