CREATE TABLE [dbo].[Booking]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[RoomId] int not null,
	[starttime] datetime not null,
	[endtime] datetime not null 
)
