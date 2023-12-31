﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_destinations_to_channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrate RecommenderDestinations to Channels
            migrationBuilder.Sql(
                @"
                INSERT INTO [dbo].Channels (Name, LinkedIntegratedSystemId, ChannelType, Discriminator, Endpoint, Created, LastUpdated, EnvironmentId)
                SELECT Discriminator
                    ,[ConnectedSystemId]
                    ,'Webhook'
                    ,'WebhookChannel'
                    ,[WebhookDestination_Endpoint]
                    ,[Created]
                    ,[LastUpdated]
                    ,[EnvironmentId]
                FROM [dbo].[RecommendationDestinations]
                where Discriminator='WebhookDestination'"
            );

            // add RecommenderChannel relationship
            migrationBuilder.Sql(
                @"
                INSERT INTO [dbo].[RecommenderChannel] (RecommendersId, ChannelsId)
                SELECT RecommenderId, c.Id
                FROM [dbo].[RecommendationDestinations] p
                JOIN [dbo].[Channels] c ON c.Endpoint = p.WebhookDestination_Endpoint"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE from [dbo].Channels
                Where Name='WebhookDestination'"
            );

            migrationBuilder.Sql(
                @"
                DELETE FROM [dbo].[RecommenderChannel]
                SELECT RecommenderId, c.Id
                FROM [dbo].[RecommendationDestinations] p
                JOIN [dbo].[Channels] c ON c.Endpoint = p.WebhookDestination_Endpoint"
            );
        }
    }
}
