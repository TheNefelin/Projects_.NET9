-- =====================================================================
-- Migracion: Auth_Users.SqlToken (GUID en texto plano) -> SqlTokenHash
-- =====================================================================
-- Contexto: la API ahora genera el token de sesion en C# y persiste SOLO su
-- SHA-256 en hexadecimal minusculas. Antes la columna guardaba el GUID en
-- texto plano: quien leia la base obtenia un token de sesion reutilizable.
--
-- ESTE SCRIPT ES DESTRUCTIVO PARA LA COLUMNA SqlToken: la borra. No se
-- puede ejecutar dos veces (la columna ya no existe) y no se puede deshacer:
-- el token crudo no queda en ningun lado, asi que las sesiones abiertas
-- antes de la migracion siguen sirviendo (el hash coincide) pero no se puede
-- recuperar el valor original.
--
-- Uso (una sola vez, con la API DETENIDA):
--   sqlcmd -S <servidor> -d <base> -U <usuario> -P <password> -C -i SqlServerPM_Migrate_SqlTokenHash.sql
-- =====================================================================

-- 1) Si la columna SqlTokenHash ya existe, no hay nada que migrar.
IF COL_LENGTH('dbo.Auth_Users', 'SqlTokenHash') IS NOT NULL
BEGIN
    PRINT 'La columna SqlTokenHash ya existe: no hay nada que migrar.';
    RETURN;
END
GO

-- 2) Copia el token al hash con el mismo formato que usa el codigo:
--    GUID canonico ("D", minusculas) -> SHA-256 -> hexadecimal minusculas.
IF COL_LENGTH('dbo.Auth_Users', 'SqlToken') IS NULL
BEGIN
    PRINT 'La columna SqlToken no existe: revisa el esquema antes de migrar.';
    RETURN;
END
GO

ALTER TABLE dbo.Auth_Users ADD SqlTokenHash VARCHAR(64) NULL;
GO

UPDATE dbo.Auth_Users
SET SqlTokenHash = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CONVERT(VARBINARY(36), CONVERT(VARCHAR(36), SqlToken))), 2))
WHERE SqlToken IS NOT NULL;
GO

-- 3) Se descarta el token en texto plano.
ALTER TABLE dbo.Auth_Users DROP COLUMN SqlToken;
GO

PRINT 'Migracion aplicada: Auth_Users.SqlTokenHash lista y SqlToken eliminada.';
GO
