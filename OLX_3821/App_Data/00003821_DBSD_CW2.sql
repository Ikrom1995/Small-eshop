
/*******************************************************************************
   DBSD_CW2_3821 Database - Version 1.4
   Description: Creates and populates the DBSD_CW2_3821 database.
   DB Server: SqlServer
   Author: 00003821
********************************************************************************/

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'DBSD_CW2_3821')
BEGIN
	ALTER DATABASE [DBSD_CW2_3821] SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE [DBSD_CW2_3821] SET ONLINE;
	DROP DATABASE [DBSD_CW2_3821];
END

GO

/*******************************************************************************
   Create database
********************************************************************************/
CREATE DATABASE [DBSD_CW2_3821];
GO

USE [DBSD_CW2_3821];
GO

/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE [dbo].[Users]
(
    [userID] int IDENTITY(1,1) NOT NULL,
	[UTitle] nvarchar(10) NOT NULL,
	[firstName] nvarchar(30) NOT NULL,
	[lastName] nvarchar(40) NOT NULL,
	[phone] nvarchar(12) NOT NULL,
	[eMail] nvarchar(60),
	[username] nvarchar(20) NOT NULL,
	[uPassword] nvarchar(16) NOT NULL,
	--constraints
	CONSTRAINT [pk_user_id] PRIMARY KEY ([userID]),
	CONSTRAINT [uc_username] UNIQUE ([username])
);
GO
CREATE TABLE [dbo].[Poster]
(
    [posterID] int IDENTITY(1,1) NOT NULL,
	[title] nvarchar(30) NOT NULL,
	[category] nvarchar(30) NOT NULL,
	[subcategory] nvarchar(30) NOT NULL,
	[price] float NOT NULL,
	[pDescription] nvarchar(1000),
	[PAddress] nvarchar(100) NOT NULL,
	[startDate] datetime NOT NULL CONSTRAINT [df_poster_datepublished]  DEFAULT getdate(),
	[cancelDate] datetime NOT NULL CONSTRAINT [df_poster_canceldate] DEFAULT DATEADD(day, 30, getDate()),
	[quantity] int NOT NULL,
	[userID] int NOT NULL,
	--constraints
	CONSTRAINT [pk_poster_id] PRIMARY KEY ([posterID]),
	CONSTRAINT [ck_poster_price] CHECK ([price] > 0 OR [price] = 0),
	CONSTRAINT [fk_poster_userID] FOREIGN KEY ([userID]) REFERENCES [Users] ([userID]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [ck_poster_quantity] CHECK ([quantity] >= 0)
);
GO

CREATE TABLE [dbo].[poster_log]
(
    [posterID] int NOT NULL,
	[title] nvarchar(30) NOT NULL,
	[category] nvarchar(30) NOT NULL,
	[subcategory] nvarchar(30) NOT NULL,
	[price] float NOT NULL,
	[pDescription] nvarchar(1000),
	[PAddress] nvarchar(100) NOT NULL,
	[startDate] datetime NOT NULL,
	[cancelDate] datetime NOT NULL,
	[quantity] int NOT NULL,
	[userID] int NOT NULL,
	[operation] nvarchar(10),
	[opDate] datetime,
	[user] nvarchar(100)
);
GO

CREATE TABLE [dbo].[Payment]
(
    [paymentID] int IDENTITY(1,1) NOT NULL,
	[amount] float NOT NULL,
	[userID] int NOT NULL,
	[payDate] datetime NOT NULL CONSTRAINT [df_PAYMENT_payDATE] DEFAULT getdate(),
	--constraints
	CONSTRAINT [pk_payment_id] PRIMARY KEY ([paymentID]),
	CONSTRAINT [fk_payment_userID] FOREIGN KEY ([userID]) REFERENCES [Users]([userID]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [ck_payment_checkamount] CHECK ([amount] > 1000)
);
GO
CREATE TABLE [dbo].[Balance]
(
    [currentAmount] float CONSTRAINT [df_userBalance_current] DEFAULT 0,
	[userID] int NOT NULL,
	--constraints
	CONSTRAINT [fk_balance_userID] FOREIGN KEY ([userID]) REFERENCES [Users]([userID]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [pk_balance_userid] PRIMARY KEY ([userID]),
	CONSTRAINT [ck_balance_amount] CHECK ([currentAmount] >= 0)
);
GO
CREATE TABLE [dbo].[Purchases]
(
    [purchaseID] int IDENTITY(1,1) NOT NULL,
	[userID] int NOT NULL,
	[posterID] int NOT NULL,
	[quantity] int NOT NULL,
	[purchaseDate] datetime NOT NULL CONSTRAINT [df_purchase_purchaseDATE] DEFAULT getdate(),
	--constraints
	CONSTRAINT [pk_purchases_purchaseid] PRIMARY KEY ([purchaseID]),
	CONSTRAINT [fk_purchase_userID] FOREIGN KEY ([userID]) REFERENCES [Users]([userID]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [fk_purchase_posterID] FOREIGN KEY ([posterID]) REFERENCES [Poster]([posterID]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [ck_purchase_quantity] CHECK ([quantity] >= 1)
);
GO

CREATE INDEX [IFK_PURCHASES_userID] ON [dbo].[Purchases] ([userID])
CREATE INDEX [IFK_PURCHASES_posterID] ON [dbo].[Purchases] ([posterID])
CREATE INDEX [IFK_BALANCE_userID] ON [dbo].[Balance] ([userID])

/*******************************************************************************
   Populate Tables
********************************************************************************/


--Inserting data to Users table
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Ms.' ,'Jacqueline', 'Fuller', '998979771418', 'jfuller0@buzzfeed.com', 'Jacqueline1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Mr.' ,'Ashley', 'Bishop', '998972215829', 'abishop1@typepad.com', 'Ashley1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Mr.' ,'Larry', 'Harper', '998908759895', 'lharper2@a8.net', 'Larry1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Mr.' ,'Daniel', 'Gilbert', '998907353873', 'dgilbert3@ebay.com', 'Daniel1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Mr.' ,'Willie', 'Gilbert', '998909798653', 'wgilbert4@sakura.ne.jp', 'Willie1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Ms.' ,'Theresa', 'Allen', '998907663768', 'tallen5@cloudflare.com', 'Theresa1', '111111');
INSERT INTO Users (UTitle, firstName, lastName, phone, eMail, username, uPassword) VALUES ('Mr.' ,'Zafar', 'Lawrence', '998909030224', 'jlawrence6@edublogs.org', 'zafar1', '111111');

--Inserting data to Poster table
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Isuzu', 'Transport', 'Buses', 2500, 'Isuzu bus 20', 'Tashkent, Mirobod', 1, 1);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Man', 'Transport', 'Trucks', 5000, 'Man Monster Truck', 'Tashkent, Mirzo-Uligbek', 2, 2);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('SamAuto', 'Transport', 'Agricultural transport', 4500, 'SamAuto 2', 'Tashkent, Yunus Abad', 3, 3);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Chevrolet', 'Transport', 'Trailers', 4000, 'Chevrolet super tralier', 'Tashkent, Chilionzor', 4, 4);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Wheels', 'Transport', 'Transport parts', 200, 'HanCock 4', 'Tashkent, Sergrli', 5, 5);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Spoiler', 'Transport', 'Transport accessories', 100, 'Crazy spolier', 'Tashkent, Chorsu', 6, 6);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Delphin', 'Transport', 'Water transport', 6000, 'Delphin 500L', 'Tashkent, TTZ', 7, 7);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Ducatti', 'Transport', 'Motorcycles', 12000, 'Ducatti Black Monster', 'Tashkent, Erkin', 8, 1);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Chevrolet', 'Transport', 'Cars', 3000, 'Chevrolet Matiz', 'Tashkent, Yakkasaray', 9, 2);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Lada', 'Transport', 'Cars', 5000, 'Lada Vesta Car', 'Tashkent, Darhan', 10, 3);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Mango', 'Clothes', 'Women', 20, 'Night Dress', 'Tashkent, C5', 1, 4);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Victorias Secret', 'Clothes', 'Women', 50, 'Super Bra', 'Tashkent, Mustaqillik str.', 2, 5);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Nike', 'Clothes', 'Men', 100, 'Nike Lite Jacket', 'Tashkent, Gafur Gulyam', 3, 6);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Boss', 'Clothes', 'Men', 200, 'Fashion Lather Jacket', 'Tashkent, C1', 4, 7);
INSERT INTO Poster (title, category, subcategory, price, pDescription, PAddress, quantity, userID) VALUES ('Hipster', 'Clothes', 'Unisex', 10, 'Hipster socks', 'Tashkent, Bunyodkor', 100, 1);

INSERT INTO Payment (amount, userID) VALUES (5000, 1);
INSERT INTO Payment (amount, userID) VALUES (4500, 2);
INSERT INTO Payment (amount, userID) VALUES (12000, 3);
INSERT INTO Payment (amount, userID) VALUES (12500, 4);
INSERT INTO Payment (amount, userID) VALUES (18000, 5);
INSERT INTO Payment (amount, userID) VALUES (2000, 6);
INSERT INTO Payment (amount, userID) VALUES (2500, 7);

INSERT INTO Balance (currentAmount, userID) VALUES (5000, 1);
INSERT INTO Balance (currentAmount, userID) VALUES (4500, 2);
INSERT INTO Balance (currentAmount, userID) VALUES (12000, 3);
INSERT INTO Balance (currentAmount, userID) VALUES (12500, 4);
INSERT INTO Balance (currentAmount, userID) VALUES (18000, 5);
INSERT INTO Balance (currentAmount, userID) VALUES (2000, 6);
INSERT INTO Balance (currentAmount, userID) VALUES (2500, 7);

IF OBJECT_ID('[dbo].[udpUpdateUser]') IS not NULL
    DROP PROCEDURE dbo.udpUpdateUser
go

CREATE PROCEDURE udpUpdateUser 
         @UserID int
       , @UTitle nvarchar(10)
		, @FirstName nvarchar(30)		
		, @LastName nvarchar(40)
		, @Phone nvarchar(12)		
		, @Email nvarchar(60)
		, @Username nvarchar (20)
		, @UPassword nvarchar(16)
AS
BEGIN
	UPDATE [dbo].Users
	SET 	[UTitle] = @UTitle
		,[FirstName] = @FirstName
		,[lastName] = @LastName
		,[phone] = @Phone		
		,[Email] = @Email
		,[username] = @Username
		,[uPassword] = @UPassword
	WHERE [userID] = @UserID;
END

IF OBJECT_ID('[dbo].[udpUpdatePoster]') IS not NULL
    DROP PROCEDURE dbo.udpUpdatePoster
go

CREATE PROCEDURE udpUpdatePoster 
         @title nvarchar(12)
       , @price float
		, @description nvarchar(30)		
		, @pAddress nvarchar(40)
		, @quantity int
		, @PosterID int
AS
BEGIN
	UPDATE [dbo].Poster
	SET  [title] = @title
		,[price] = @price
		,[pDescription] = @description
		,[PAddress] = @pAddress
		,[quantity] = @quantity
	WHERE [posterID] = @PosterID;
END;

IF OBJECT_ID('[dbo].[udpCreateUser]') IS not NULL
    DROP PROCEDURE dbo.udpCreateUser
go

CREATE PROCEDURE udpCreateUser 
         @UTitle nvarchar(10)
       , @firstName nvarchar(30)
		, @lastName nvarchar(40)	
		, @phone nvarchar(12)
		, @eMail nvarchar(60)
		, @username nvarchar(20)
		, @uPassword nvarchar(16)
AS
BEGIN
	INSERT INTO Users(
    [UTitle],
    [firstName],
    [lastName],
    [phone],
    [eMail],
    [username],
    [uPassword])
VALUES (
    @UTitle,
    @firstName,
    @lastName,
    @phone,
    @eMail,
    @username,
    @uPassword)
END;

IF OBJECT_ID('[dbo].[udpUpdateBalance]') IS not NULL
    DROP PROCEDURE dbo.udpUpdateBalance
go

CREATE PROCEDURE udpUpdateBalance 
         @UserID int
       , @newAmount float
AS
BEGIN
	UPDATE [dbo].Balance
	SET 	[currentAmount] = @newAmount
	WHERE [userID] = @UserID;
END;

--IF OBJECT_ID('[dbo].[udfInvoicesReport]') IS not NULL
--    DROP FUNCTION dbo.udfInvoicesReport
--go

--CREATE FUNCTION udfInvoicesReport(@userID int, @min float, @max float) 
--RETURNS TABLE AS 
--RETURN (
--	select U.userID, U.UTitle, U.firstName, U.lastName, Pos.category, sum(P.quantity * Pos.price) as Total 
--	from [dbo].[Users] U join [dbo].[Purchases] P on U.userID = P.userID
--	join [dbo].[Poster] Pos on P.posterID = Pos.posterID
--	WHERE P.userID = @userID and (Pos.price BETWEEN @min and @max)
--	GROUP BY Pos.category, U.UTitle, U.firstName, U.lastName, U.userID
--)
--GO

IF OBJECT_ID('[dbo].[udfLogin]') IS not NULL
    DROP FUNCTION dbo.udfLogin
go

CREATE FUNCTION udfLogin (@u varchar(100), @p varchar(30))
        RETURNS bit
        AS
        BEGIN
	     declare @result bit = 0;
            IF exists(select 1 from Users where [username] = @u and [uPassword] = @p) 
	          set @result = 1;    

return @result;
END
GO

IF OBJECT_ID('[dbo].[udfInvoicesReport]') IS not NULL
    DROP FUNCTION dbo.udfInvoicesReport
go

CREATE FUNCTION udfInvoicesReport(@userID int, @min float, @max float) 
RETURNS TABLE AS 
RETURN (
	select U.userID, U.UTitle, U.firstName, U.lastName, Pos.category, sum(P.quantity * Pos.price) as Total 
	from [dbo].[Users] U join [dbo].[Purchases] P on U.userID = P.userID
	join [dbo].[Poster] Pos on P.posterID = Pos.posterID
	WHERE P.userID = @userID and (Pos.price BETWEEN @min and @max)
	GROUP BY Pos.category, U.UTitle, U.firstName, U.lastName, U.userID
)
GO

--CREATE TRIGGER [dbo].[tr_poster_audit]
--ON [dbo].poster AFTER UPDATE, DELETE, INSERT
--AS 
--BEGIN
--    IF exists (select 1 from inserted) AND exists (select 1 from deleted)  
--  INSERT INTO Poster_log (title, category, subcategory, price, pDescription, PAddress, quantity, userID, operation, opdate, [user]) SELECT title, category, subcategory, price, pDescription, PAddress, quantity, userID, 'Update', getdate(), user FROM DELETED
--ELSE IF NOT exists (select 1 from inserted) AND exists (select 1 from deleted) 
--  INSERT INTO Poster_log (title, category, subcategory, price, pDescription, PAddress, quantity, userID, operation, opdate, [user]) SELECT title, category, subcategory, price, pDescription, PAddress, quantity, userID, 'Delete', getdate(), user FROM DELETED
--ELSE IF exists (select 1 from inserted) AND NOT exists (select 1 from deleted) 
--  INSERT INTO Poster_log (title, category, subcategory, price, pDescription, PAddress, quantity, userID, operation, opdate, [user]) SELECT title, category, subcategory, price, pDescription, PAddress, quantity, userID, 'Insert', getdate(), user FROM inserted
--END

--select * into Poster_log from Poster where 1<> 1;
--alter table Poster_log add opdate datetime;
--alter table Poster_log add op varchar(10);
--alter table Poster_log add [user] varchar(100);

--select * from Poster_log

--DROP TRIGGER [tr_poster_audit]


create trigger tr_poster_audit
on [dbo].poster
after insert, update, delete
as 
begin
 declare @insertedRows int
 declare @deletedRows int

 select @insertedRows = count(*) from inserted
 select @deletedRows = count(*) from deleted 

 if @deletedRows = 0 -- insert action
  --copy inserted client to log table
  begin
   insert into poster_log
   select [posterID] ,[title], [category], [subcategory], [price], [pDescription], [PAddress], [startDate], [cancelDate], [quantity], [userID], 'INSERT', GETDATE(), SYSTEM_USER  from inserted
   print 'New client inserted'
  end
 else if  @insertedRows = 0 -- delete action
  --copy deleted row to log table
  begin
   insert into poster_log
   select [posterID] ,[title], [category], [subcategory], [price], [pDescription], [PAddress], [startDate], [cancelDate], [quantity], [userID], 'DELETE', GETDATE(), SYSTEM_USER from deleted
   print 'Client got deleted'
  end
 else -- update action
  --copy old client row to log table
  begin
   insert into poster_log
   select [posterID] ,[title], [category], [subcategory], [price], [pDescription], [PAddress], [startDate], [cancelDate], [quantity], [userID], 'UPDATE', GETDATE(), SYSTEM_USER from deleted
   print 'Client got updated'
  end
end

--DROP trigger tr_poster_audit
--DROP TABLE poster_log
