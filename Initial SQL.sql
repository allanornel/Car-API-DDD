DROP TABLE IF EXISTS Car
DROP TABLE IF EXISTS Photo;

CREATE TABLE Photo(
	Id int identity primary key,
	[Base64] nvarchar(max) not null,	
)

CREATE TABLE Car(
	Id int identity primary key,
	PhotoId int REFERENCES Photo(Id) NOT NULL,
	Name VARCHAR(100) not null,
	Status int not null DEFAULT 1,
);