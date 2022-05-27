-- Admin User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(AdminUserName)')
    BEGIN
        CREATE USER $(AdminUserName) FROM LOGIN $(AdminUserName);
    END
    
EXEC sp_addrolemember 'db_datawriter', '$(AdminUserName)';
EXEC sp_addrolemember 'db_datareader', '$(AdminUserName)';

-- Read User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(ReadUserName)')
    BEGIN
        CREATE USER $(ReadUserName) FROM LOGIN $(ReadUserName);
    END

EXEC sp_addrolemember 'db_datareader', '$(ReadUserName)';
