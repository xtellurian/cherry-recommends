﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Infrastructure;

namespace sqlserver.SignalBox
{
    [DbContext(typeof(SignalBoxDbContext))]
    partial class SignalBoxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExperimentOffer", b =>
                {
                    b.Property<long>("ExperimentsId")
                        .HasColumnType("bigint");

                    b.Property<long>("OffersId")
                        .HasColumnType("bigint");

                    b.HasKey("ExperimentsId", "OffersId");

                    b.HasIndex("OffersId");

                    b.ToTable("ExperimentOffer");
                });

            modelBuilder.Entity("OfferOfferRecommendation", b =>
                {
                    b.Property<long>("OffersId")
                        .HasColumnType("bigint");

                    b.Property<long>("RecommendationsId")
                        .HasColumnType("bigint");

                    b.HasKey("OffersId", "RecommendationsId");

                    b.HasIndex("RecommendationsId");

                    b.ToTable("OfferOfferRecommendation");
                });

            modelBuilder.Entity("SegmentTrackedUser", b =>
                {
                    b.Property<long>("InSegmentId")
                        .HasColumnType("bigint");

                    b.Property<long>("SegmentsId")
                        .HasColumnType("bigint");

                    b.HasKey("InSegmentId", "SegmentsId");

                    b.HasIndex("SegmentsId");

                    b.ToTable("SegmentTrackedUser");
                });

            modelBuilder.Entity("SignalBox.Core.Experiment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConcurrentOffers")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("SignalBox.Core.HashedApiKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlgorithmName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("HashedKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("LastExchanged")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalExchanges")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("SignalBox.Core.IntegratedSystem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IntegratedSystems");
                });

            modelBuilder.Entity("SignalBox.Core.ModelRegistration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("HostingType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("ModelType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScoringUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Swagger")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ModelRegistrations");
                });

            modelBuilder.Entity("SignalBox.Core.Offer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Cost")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiscountCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("SignalBox.Core.OfferRecommendation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("ExperimentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Features")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IterationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IterationOrder")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("SignalBox.Core.PresentationOutcome", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("ExperimentId")
                        .HasColumnType("bigint");

                    b.Property<string>("IterationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IterationOrder")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("OfferId")
                        .HasColumnType("bigint");

                    b.Property<string>("Outcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("RecommendationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.HasIndex("OfferId");

                    b.HasIndex("RecommendationId");

                    b.ToTable("PresentationOutcomes");
                });

            modelBuilder.Entity("SignalBox.Core.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SignalBox.Core.Rule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EventKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventLogicalValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SegmentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("SignalBox.Core.Segment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("SignalBox.Core.Sku", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<string>("SkuId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Skus");
                });

            modelBuilder.Entity("SignalBox.Core.Touchpoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CommonId")
                        .IsUnique()
                        .HasFilter("[CommonId] IS NOT NULL");

                    b.ToTable("Touchpoints");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("CommonUserId");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CommonId")
                        .IsUnique()
                        .HasFilter("[CommonUserId] IS NOT NULL");

                    b.ToTable("TrackedUsers");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kind")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("SourceId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique()
                        .HasFilter("[EventId] IS NOT NULL");

                    b.HasIndex("SourceId");

                    b.ToTable("TrackedUserEvents");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserSystemMap", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("IntegratedSystemId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("TrackedUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IntegratedSystemId");

                    b.HasIndex("TrackedUserId");

                    b.ToTable("TrackUserSystemMaps");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserTouchpoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("TouchpointId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TrackedUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Values")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TouchpointId");

                    b.HasIndex("TrackedUserId");

                    b.ToTable("TrackedUserTouchpoints");
                });

            modelBuilder.Entity("SignalBox.Core.WebhookReceiver", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EndpointId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("IntegratedSystemId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("SharedSecret")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EndpointId")
                        .IsUnique()
                        .HasFilter("[EndpointId] IS NOT NULL");

                    b.HasIndex("IntegratedSystemId");

                    b.ToTable("WebhookReceivers");
                });

            modelBuilder.Entity("ExperimentOffer", b =>
                {
                    b.HasOne("SignalBox.Core.Experiment", null)
                        .WithMany()
                        .HasForeignKey("ExperimentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalBox.Core.Offer", null)
                        .WithMany()
                        .HasForeignKey("OffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OfferOfferRecommendation", b =>
                {
                    b.HasOne("SignalBox.Core.Offer", null)
                        .WithMany()
                        .HasForeignKey("OffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalBox.Core.OfferRecommendation", null)
                        .WithMany()
                        .HasForeignKey("RecommendationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SegmentTrackedUser", b =>
                {
                    b.HasOne("SignalBox.Core.TrackedUser", null)
                        .WithMany()
                        .HasForeignKey("InSegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalBox.Core.Segment", null)
                        .WithMany()
                        .HasForeignKey("SegmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SignalBox.Core.Experiment", b =>
                {
                    b.OwnsMany("SignalBox.Core.Iteration", "Iterations", b1 =>
                        {
                            b1.Property<long>("ExperimentId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Order")
                                .HasColumnType("int");

                            b1.HasKey("ExperimentId", "Id");

                            b1.ToTable("Iteration");

                            b1.WithOwner()
                                .HasForeignKey("ExperimentId");
                        });

                    b.Navigation("Iterations");
                });

            modelBuilder.Entity("SignalBox.Core.PresentationOutcome", b =>
                {
                    b.HasOne("SignalBox.Core.Experiment", "Experiment")
                        .WithMany()
                        .HasForeignKey("ExperimentId");

                    b.HasOne("SignalBox.Core.Offer", "Offer")
                        .WithMany("Outcomes")
                        .HasForeignKey("OfferId");

                    b.HasOne("SignalBox.Core.OfferRecommendation", "Recommendation")
                        .WithMany()
                        .HasForeignKey("RecommendationId");

                    b.Navigation("Experiment");

                    b.Navigation("Offer");

                    b.Navigation("Recommendation");
                });

            modelBuilder.Entity("SignalBox.Core.Sku", b =>
                {
                    b.HasOne("SignalBox.Core.Product", "Product")
                        .WithMany("Skus")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserEvent", b =>
                {
                    b.HasOne("SignalBox.Core.IntegratedSystem", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserSystemMap", b =>
                {
                    b.HasOne("SignalBox.Core.IntegratedSystem", "IntegratedSystem")
                        .WithMany()
                        .HasForeignKey("IntegratedSystemId");

                    b.HasOne("SignalBox.Core.TrackedUser", "TrackedUser")
                        .WithMany()
                        .HasForeignKey("TrackedUserId");

                    b.Navigation("IntegratedSystem");

                    b.Navigation("TrackedUser");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserTouchpoint", b =>
                {
                    b.HasOne("SignalBox.Core.Touchpoint", "Touchpoint")
                        .WithMany("TrackedUserTouchpoints")
                        .HasForeignKey("TouchpointId");

                    b.HasOne("SignalBox.Core.TrackedUser", "TrackedUser")
                        .WithMany("TrackedUserTouchpoints")
                        .HasForeignKey("TrackedUserId");

                    b.Navigation("Touchpoint");

                    b.Navigation("TrackedUser");
                });

            modelBuilder.Entity("SignalBox.Core.WebhookReceiver", b =>
                {
                    b.HasOne("SignalBox.Core.IntegratedSystem", "IntegratedSystem")
                        .WithMany()
                        .HasForeignKey("IntegratedSystemId");

                    b.Navigation("IntegratedSystem");
                });

            modelBuilder.Entity("SignalBox.Core.Offer", b =>
                {
                    b.Navigation("Outcomes");
                });

            modelBuilder.Entity("SignalBox.Core.Product", b =>
                {
                    b.Navigation("Skus");
                });

            modelBuilder.Entity("SignalBox.Core.Touchpoint", b =>
                {
                    b.Navigation("TrackedUserTouchpoints");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUser", b =>
                {
                    b.Navigation("TrackedUserTouchpoints");
                });
#pragma warning restore 612, 618
        }
    }
}
