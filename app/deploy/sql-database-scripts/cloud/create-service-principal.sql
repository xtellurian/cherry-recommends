-- Admin User
IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name='$(AdminUserName)')
    BEGIN
        CREATE LOGIN $(AdminUserName) WITH PASSWORD = "$(AdminPassword)";
    END
ELSE
    BEGIN
        ALTER LOGIN $(AdminUserName) WITH PASSWORD="$(AdminPassword)";
    END;

-- Read User
IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name='$(ReadUserName)')
    BEGIN
        CREATE LOGIN $(ReadUserName) WITH PASSWORD = "$(ReadPassword)";
    END
ELSE
    BEGIN
        ALTER LOGIN $(ReadUserName) WITH PASSWORD="$(ReadPassword)";
    END;
