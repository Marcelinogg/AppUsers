CREATE DATABASE [dbUsers]
WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

USE [master]
GO
/****** Object:  Login [userApp]    Script Date: 15/05/2023 12:40:31 p. m. ******/
CREATE LOGIN [userApp] WITH PASSWORD=N'u5er4pp', DEFAULT_DATABASE=[dbUsers], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [dbUsers]
GO
CREATE TABLE dbo.Profiles (
	Id		int PRIMARY KEY,
	Name	varchar(255)
)

INSERT INTO dbo.Profiles
VALUES	(1, 'ADMINISTRADOR'),
		(2, 'REPORTEADOR'),
		(3, 'GERENTE'),
		(4, 'SUPERVISOR'),
		(5, 'USUARIO GENERAL')

CREATE TABLE dbo.Users (
	UserId			int not null,
	LoginName		varchar(255) not null,
	FullName		varchar(255) not null,
	Email			varchar(255),
	PasswordHash	binary(64) not null,
	Avatar			varchar(max),
	ProfileId		int not null References dbo.Profiles (Id),
	CONSTRAINT PK_UserId PRIMARY KEY CLUSTERED (
		UserId ASC
	),
	CONSTRAINT UK_LoginName UNIQUE NONCLUSTERED (
		FullName ASC
	)
)

CREATE TABLE dbo.Users_Log (
	Id			int PRIMARY KEY IDENTITY,
	UserId		int,
	LoginName	varchar(255),
	FullName	varchar(255),
	Email			varchar(255),
	Avatar		varchar(max),
	ProfileId	int References dbo.Profiles (Id),
	Action		varchar(50),
	CreatedDate	datetime
)

/****** Object:  User [userApp]    Script Date: 15/05/2023 12:32:17 p. m. ******/
CREATE USER [userApp] FOR LOGIN [userApp] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [userApp]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetUserIdByLoginName]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date, May 15th 2023			>
-- Description:	<Description,	Retrives the user ID by login name is used when a new user is created	>
-- Exectuion:	<Exectuion,			Select dbo.fn_GetUserIdByLoginName('sss') as UserId					>
-- =============================================
CREATE FUNCTION [dbo].[fn_GetUserIdByLoginName]
(
	@loginName	varchar(255)
)
RETURNS int
AS
BEGIN

	RETURN (Select UserId From dbo.Users Where LoginName = LTRIM(RTRIM(@loginName)))
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsLoginNameAvailable]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date, May 13th 2023			>
-- Description:	<Description,	Checks if the login name exists				>
-- Exectuion:	<Exectuion,			Select dbo.fn_IsLoginNameAvailable('') as LoginName			>
-- =============================================
CREATE FUNCTION [dbo].[fn_IsLoginNameAvailable]
(
	@loginName	varchar(255)
)
RETURNS bit
AS
BEGIN

	RETURN IIF(EXISTS(Select 1 From dbo.Users Where LoginName = LTRIM(RTRIM(@loginName))), 0, 1)
END
GO
/****** Object:  StoredProcedure [dbo].[AddUser_In]    Script Date: 15/05/2023 01:01:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Adds a new user		>
-- =============================================
ALTER PROCEDURE [dbo].[AddUser_In]
	@loginName	varchar(255),
	@fullName	varchar(255),
	@email		varchar(255) = null,
	@password	varchar(64),
	@avatar		varchar(max) = null,
	@profileId	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    Declare @minUserId	int = 100000
	Declare @userId		int = ISNULL((
								Select
										max(Id)
								From	(
									Select
											max(UserId) as Id
									From	dbo.Users
									Union
									Select
											max(UserId)	as Id
									From	dbo.Users_Log
									) x
								), @minUserId) +1

	-- Implicit transaction, because I just one table is used
	INSERT INTO dbo.Users
	(
		UserId
	  ,LoginName
	  ,FullName
	  ,Email
	  ,PasswordHash
	  ,Avatar
	  ,ProfileId
	)
	VALUES (
		@userId
		,LTRIM(RTRIM(@loginName))
		,@fullName
		,@email
		,HASHBYTES('SHA2_512', @password)
		,@avatar
		,@profileId
	)

END
/****** Object:  StoredProcedure [dbo].[ChangeDataUser_Up]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Update the overall data user		>
-- =============================================
CREATE PROCEDURE [dbo].[ChangeDataUser_Up]
	@userId		int,
	@loginName	varchar(255),
	@fullName	varchar(255),
	@email		varchar(255) = null,
	@avatar		varchar(max) = null,
	@profileId	int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	-- Implicit transaction, because I just one table is used
    UPDATE
	dbo.Users
	SET	LoginName	= LTRIM(RTRIM(@loginName))
		,FullName	= @fullName
		,Email		= @email
		,Avatar		= @avatar
		,ProfileId	= @profileId
	WHERE	UserId = @userId

END
GO
/****** Object:  StoredProcedure [dbo].[ChangePasswordUser_Up]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Update just the password user		>
-- =============================================
CREATE PROCEDURE [dbo].[ChangePasswordUser_Up]
	@userId		int,
	@password	varchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	-- Implicit transaction, because I just one table is used
	UPDATE
	dbo.Users
	SET	PasswordHash	= HASHBYTES('SHA2_512', @password)
	WHERE	UserId = @userId

END
GO
/****** Object:  StoredProcedure [dbo].[GetProfiles_Se]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Retrive all users	>
-- =============================================
CREATE PROCEDURE [dbo].[GetProfiles_Se]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
			Id
			,Name
	FROM	dbo.Profiles

END
GO
/****** Object:  StoredProcedure [dbo].[GetUsers_Se]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Retrive all users	>
-- Execution:	<Execution.		dbo.GetUsers_Se		>
-- =============================================
CREATE PROCEDURE [dbo].[GetUsers_Se]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
			U.UserId
			,U.LoginName
			,U.FullName
			,U.Email
			,CAST(U.PasswordHash as varchar(60))	AS [Password]
			,U.Avatar
			,U.ProfileId
			,P.Name		AS [Profile]
	FROM	dbo.Users U INNER JOIN
			dbo.Profiles P ON U.ProfileId = P.Id

END
GO
/****** Object:  StoredProcedure [dbo].[RemoveUser_De]    Script Date: 15/05/2023 12:32:17 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Remove a user physically		>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveUser_De]
	@userId		int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	-- Implicit transaction, because I just one table is used
    DELETE
	FROM	dbo.Users
	WHERE	UserId = @userId

END
GO
/****** Object:  Trigger [dbo].[Tgr_User_AD]    Script Date: 15/05/2023 12:34:40 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Saves the updated row in the log		>
-- =============================================
CREATE TRIGGER [dbo].[Tgr_User_AD]
   ON  [dbo].[Users] 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.Users_Log
	(
		UserId
		,LoginName
		,FullName
		,Email
		,Avatar
		,ProfileId
		,[Action]
		,CreatedDate
	)
	SELECT
			UserId
			,LoginName
			,FullName
			,Email
			,Avatar
			,ProfileId
			,'DELETED'	AS[Action]
			,GETDATE()	AS CreatedDate
	FROM	deleted

END
GO
ALTER TABLE [dbo].[Users] ENABLE TRIGGER [Tgr_User_AD]
GO
/****** Object:  Trigger [dbo].[Tgr_User_AI]    Script Date: 15/05/2023 12:34:40 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Saves the new row in the log		>
-- =============================================
CREATE TRIGGER [dbo].[Tgr_User_AI]
   ON  [dbo].[Users] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.Users_Log
	(
		UserId
		,LoginName
		,FullName
		,Email
		,Avatar
		,ProfileId
		,[Action]
		,CreatedDate
	)
	SELECT
			UserId
			,LoginName
			,FullName
			,Email
			,Avatar
			,ProfileId
			,'NEW'		AS[Action]
			,GETDATE()	AS CreatedDate
	FROM	inserted

END
GO
ALTER TABLE [dbo].[Users] ENABLE TRIGGER [Tgr_User_AI]
GO
/****** Object:  Trigger [dbo].[Tgr_User_AU]    Script Date: 15/05/2023 12:34:40 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,	Marcelino Guzmán García	>
-- Create date: <Create Date,	May 13th 2023		>
-- Description:	<Description,	Saves the updated row in the log		>
-- =============================================
CREATE TRIGGER [dbo].[Tgr_User_AU]
   ON  [dbo].[Users] 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO dbo.Users_Log
	(
		UserId
		,LoginName
		,FullName
		,Email
		,Avatar
		,ProfileId
		,[Action]
		,CreatedDate
	)
	SELECT
			UserId
			,LoginName
			,FullName
			,Email
			,Avatar
			,ProfileId
			,'UPDATED'		AS[Action]
			,GETDATE()	AS CreatedDate
	FROM	deleted

END
GO
ALTER TABLE [dbo].[Users] ENABLE TRIGGER [Tgr_User_AU]
GO
