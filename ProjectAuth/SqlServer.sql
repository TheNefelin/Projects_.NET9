SELECT 
	NAME AS LoginName, 
	TYPE_DESC AS AccountType, 
	create_date, 
	modify_date,
	TYPE
FROM sys.server_principals
WHERE TYPE IN ('S', 'U', 'G');
GO

CREATE LOGIN testing WITH PASSWORD = 'testing', CHECK_POLICY = OFF;
GO
CREATE DATABASE db_testing
GO
USE db_testing
GO
CREATE USER testing FOR LOGIN testing;
GO
EXEC sp_addrolemember 'db_owner', 'testing';

-- Tables -------------------------------------------------------
-- --------------------------------------------------------------

CREATE TABLE Mae_Config (
	Config_Id INT PRIMARY KEY IDENTITY(1,1),
	ApiKey VARCHAR(256),
	IsEnableRegister BIT NOT NULL,
) 
GO

CREATE TABLE Auth_Profiles (
	Profile_Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
	UNIQUE(Name),
) 
GO

CREATE TABLE Auth_Users (
	User_Id UNIQUEIDENTIFIER PRIMARY KEY,
	Email VARCHAR(100) NOT NULL,
	HashLogin VARCHAR(256) NOT NULL,
	SaltLogin VARCHAR(256) NOT NULL,
	HashPM VARCHAR(256),
	SaltPM VARCHAR(256),
	SqlToken UNIQUEIDENTIFIER,
	Profile_Id INT NOT NULL
	UNIQUE(Email),
	FOREIGN KEY (Profile_Id) REFERENCES Auth_Profiles(Profile_Id)
) 
GO

DROP TABLE Mae_Config
GO
DROP TABLE Auth_Profiles
GO
DROP TABLE Auth_Users
GO

-- Data ---------------------------------------------------------
-- --------------------------------------------------------------

SET IDENTITY_INSERT Auth_Profiles ON
GO
INSERT INTO Auth_Profiles
	(Profile_Id, Name)
VALUES
	(1, 'ADMIN'),
	(2, 'USER')
SET IDENTITY_INSERT Auth_Profiles OFF
GO

-- Stored Procedure ---------------------------------------------
-- --------------------------------------------------------------

CREATE PROCEDURE Auth_Register
	@User_Id UNIQUEIDENTIFIER,
	@Email VARCHAR(100),
	@HashLogin VARCHAR(256),
	@SaltLogin VARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	IF 0 = (SELECT ISNULL(IsEnableRegister, 0) FROM Mae_Config WHERE Config_Id = 1)
		BEGIN
			SELECT 0 AS IsSuccess, 401 AS StatusCode, 'El Servicio de Registro No Esta Disponible' AS Message
			RETURN
		END

	IF EXISTS (SELECT User_Id FROM Auth_Users WHERE Email = @Email)
		BEGIN
			SELECT 0 AS IsSuccess, 400 AS StatusCode, 'El Usuario ya Existe' AS Message
			RETURN
		END

	BEGIN TRY
		INSERT INTO Auth_Users
			(User_Id, Email, HashLogin, SaltLogin, Profile_Id)
		VALUES
			(@User_Id, @Email, @HashLogin, @SaltLogin, 2)

		SELECT 1 AS IsSuccess, 201 AS StatusCode, 'Usuario Registrado Correctamente' AS Message
    END TRY
    BEGIN CATCH
		SELECT 0 AS IsSuccess, ERROR_STATE() AS StatusCode, ERROR_MESSAGE() AS Message
    END CATCH
END
GO

CREATE PROCEDURE Auth_Login
	@Email VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Auth_Users SET
		SqlToken = NEWID()
	WHERE 
		Email = @Email 

	SELECT 
		a.User_Id,
		a.Email,
		a.HashLogin,
		a.SaltLogin,
		a.HashPM,
		a.SaltPM,
		a.SqlToken,
		--a.Profile_Id,
		b.Name AS Role
	FROM Auth_Users a 
		INNER JOIN Auth_Profiles b ON a.Profile_Id = b.Profile_Id
	WHERE 
		a.Email = @Email 
END
GO

DROP PROCEDURE Auth_Register
GO
DROP PROCEDURE Auth_Login
GO

-- Query --------------------------------------------------------
-- --------------------------------------------------------------

SELECT * FROM Mae_Config
SELECT * FROM Auth_Profiles
SELECT * FROM Auth_Users

EXECUTE Auth_Register 'A1', 'user@example.com', 'A2', 'A3'
EXECUTE Auth_Login 'user@example.com'

SELECT
	'(''' + Email + ''',''' + HashLogin + ''',''' + SaltLogin + ''',''' + HashPM + ''',''' + SaltPM + ''',' + LTRIM(STR(Id_Profile)) + ')'
FROM Auth_Users

-- --------------------------------------------------------------
-- --------------------------------------------------------------
