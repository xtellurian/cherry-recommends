IF DATABASE_PRINCIPAL_ID('db_executor') IS NULL
BEGIN
    CREATE ROLE [db_executor] AUTHORIZATION [dbo]
END

GRANT EXECUTE TO [db_executor]

-- Admin User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(AdminUserName)')
    BEGIN
        CREATE USER $(AdminUserName) FROM LOGIN $(AdminUserName);
    END
    
EXEC sp_addrolemember 'db_datawriter', '$(AdminUserName)';
EXEC sp_addrolemember 'db_datareader', '$(AdminUserName)';
EXEC sp_addrolemember 'db_executor', '$(AdminUserName)';

-- Read User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(ReadUserName)')
    BEGIN
        CREATE USER $(ReadUserName) FROM LOGIN $(ReadUserName);
    END

EXEC sp_addrolemember 'db_datareader', '$(ReadUserName)';
