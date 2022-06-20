WITH cte AS (
    SELECT 
        Id
        ,EventId
        ,EnvironmentId
        ,ROW_NUMBER() OVER (
            PARTITION BY 
                EventId
                ,EnvironmentId
            ORDER BY 
                Id
        ) row_num
     FROM 
        TrackedUserEvents
)

/* Delete existing records violating the unique constraint 
for EventId and EnvironmentId while retaining the first records */

DELETE FROM cte
WHERE row_num > 1;