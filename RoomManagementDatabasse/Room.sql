CREATE TABLE [dbo].[Room]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[RoomName] nvarchar(150) not null,
	[Projector] bit  null,
	[Sitting] int not null,
	[location] nvarchar(50)

)
