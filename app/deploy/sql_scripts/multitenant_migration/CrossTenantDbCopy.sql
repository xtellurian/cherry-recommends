-- https://docs.microsoft.com/en-us/azure/azure-sql/database/database-copy
-- this only works if the databases have the same logins

CREATE DATABASE democopy
AS COPY OF multisqlbee19077.single;
