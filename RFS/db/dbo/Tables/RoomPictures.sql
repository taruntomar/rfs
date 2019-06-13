CREATE TABLE [dbo].[RoomPictures] (
    [RoomId]      NVARCHAR (50)   NOT NULL,
    [PictureName] NVARCHAR (50)   NOT NULL,
    [picture]     VARBINARY (MAX) NULL,
    CONSTRAINT [PK_RoomPictures] PRIMARY KEY CLUSTERED ([PictureName] ASC)
);

