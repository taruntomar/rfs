CREATE TABLE [dbo].[Booking] (
    [Id]            NVARCHAR (50) NOT NULL,
    [RoomId]        NVARCHAR (50) NOT NULL,
    [starttime]     DATETIME      NOT NULL,
    [endtime]       DATETIME      NOT NULL,
    [createdOn]     DATETIME      NULL,
    [createdBy]     NVARCHAR (50) NULL,
    [isCancelled]   BIT           NULL,
    [CancelledDate] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

