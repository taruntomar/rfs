CREATE TABLE [dbo].[UserProfilePic] (
    [Id]     NVARCHAR (50)   NOT NULL,
    [UserId] NVARCHAR (50)   NOT NULL,
    [data]   VARBINARY (MAX) NOT NULL,
    [ext]    NVARCHAR (5)    NOT NULL,
    CONSTRAINT [PK_UserProfilePic] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserProfilePic_users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[users] ([Id])
);

