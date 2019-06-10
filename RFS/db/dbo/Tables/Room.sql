CREATE TABLE [dbo].[Room] (
    [Id]                 NVARCHAR (50)  NOT NULL,
    [RoomName]           NVARCHAR (150) NOT NULL,
    [Projector]          BIT            NOT NULL,
    [Sitting]            INT            NOT NULL,
    [location]           NVARCHAR (50)  NOT NULL,
    [VideoConferencing ] BIT            NOT NULL,
    [MonitorScreen]      BIT            NOT NULL,
    [decommission]       BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

