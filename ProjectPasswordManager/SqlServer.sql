USE db_testing

-- Tables -------------------------------------------------------
-- --------------------------------------------------------------

CREATE TABLE PM_CoreData (
	Data_Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Data01 VARCHAR(256) NOT NULL,
	Data02 VARCHAR(256) NOT NULL,
	Data03 VARCHAR(256) NOT NULL,
	User_Id UNIQUEIDENTIFIER NOT NULL
	FOREIGN KEY (User_Id) REFERENCES Auth_Users(User_Id)
)
GO

-- Data ---------------------------------------------------------
-- --------------------------------------------------------------

-- Stored Procedure ---------------------------------------------
-- --------------------------------------------------------------

-- Query --------------------------------------------------------
-- --------------------------------------------------------------

UPDATE Auth_Users SET HashPM = NULL, SaltPM = NULL WHERE User_Id = '3fa85f64-5717-4562-b3fc-2c963f66afa6'

SELECT
	a.User_Id,
	a.Email,
	a.HashLogin,
	a.SaltLogin,
	a.HashPM,
	a.SaltPM,
	a.SqlToken,
	b.Name AS Role
FROM Auth_Users a
	INNER JOIN Auth_Profiles b ON a.Profile_Id = b.Profile_Id

SELECT Data_Id, Data01, Data02, Data03, User_Id 
FROM PM_Core 
WHERE User_Id = '3fa85f64-5717-4562-b3fc-2c963f66afa6'

SELECT 
	'(''' + Data01 + ''',''' + Data02 + ''',''' + Data03 + ''',''3fa85f64-5717-4562-b3fc-2c963f66afa6''),'
FROM PM_CoreData

-- --------------------------------------------------------------
-- --------------------------------------------------------------
