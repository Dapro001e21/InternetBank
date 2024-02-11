use internetbankdb;

create table Users(Id int not null identity(1, 1) primary key, Name varchar(100), Email varchar(100), Password varchar(100))