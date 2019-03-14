CREATE TABLE [dbo].[Location] (
    [Id]      NVARCHAR (50)  NOT NULL,
    [Name]    NVARCHAR (150) NOT NULL,
    [Country] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC)
);

