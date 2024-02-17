use internetbankdb;

create table Users(Id int not null identity(1, 1) primary key, Name varchar(100), Email varchar(100), Password varchar(100))
create table Cards(Id int not null identity(1, 1) primary key, OwnerId int, Money decimal)
create table Transactions(Id int not null identity(1, 1) primary key, SenderIdCard int, RecieverIdCard int, TransactionTime DateTime, Money decimal)