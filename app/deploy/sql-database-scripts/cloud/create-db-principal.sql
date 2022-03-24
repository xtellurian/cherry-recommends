-- Admin User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(AdminUserName)')
    BEGIN
        CREATE USER $(AdminUserName) FROM LOGIN $(AdminUserName);
        EXEC sp_addrolemember 'db_datawriter', '$(AdminUserName)';
    END

-- Read User
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name='$(ReadUserName)')
    BEGIN
        CREATE USER $(ReadUserName) FROM LOGIN $(ReadUserName);
        EXEC sp_addrolemember 'db_datareader', '$(ReadUserName)';
    END
