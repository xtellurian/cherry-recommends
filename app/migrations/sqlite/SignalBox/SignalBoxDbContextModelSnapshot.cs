﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalBox.Infrastructure;

namespace sqlite.SignalBox
{
    [DbContext(typeof(SignalBoxDbContext))]
    partial class SignalBoxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("ExperimentOffer", b =>
                {
                    b.Property<long>("ExperimentsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("OffersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ExperimentsId", "OffersId");

                    b.HasIndex("OffersId");

                    b.ToTable("ExperimentOffer");
                });

            modelBuilder.Entity("OfferOfferRecommendation", b =>
                {
                    b.Property<long>("OffersId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("RecommendationsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("OffersId", "RecommendationsId");

                    b.HasIndex("RecommendationsId");

                    b.ToTable("OfferOfferRecommendation");
                });

            modelBuilder.Entity("ParameterParameterSetRecommender", b =>
                {
                    b.Property<long>("ParameterSetRecommendersId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ParametersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParameterSetRecommendersId", "ParametersId");

                    b.HasIndex("ParametersId");

                    b.ToTable("ParameterParameterSetRecommender");
                });

            modelBuilder.Entity("SegmentTrackedUser", b =>
                {
                    b.Property<long>("InSegmentId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SegmentsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("InSegmentId", "SegmentsId");

                    b.HasIndex("SegmentsId");

                    b.ToTable("SegmentTrackedUser");
                });

            modelBuilder.Entity("SignalBox.Core.Experiment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConcurrentOffers")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("SignalBox.Core.HashedApiKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlgorithmName")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("HashedKey")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LastExchanged")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalExchanges")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("SignalBox.Core.IntegratedSystem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApiKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cache")
                        .HasColumnType("TEXT");

                    b.Property<long?>("CacheLastRefreshed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommonId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("IntegrationStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("('NotConfigured')");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SystemType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TokenResponse")
                        .HasColumnType("TEXT");

                    b.Property<long?>("TokenResponseUpdated")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("IntegratedSystems");
                });

            modelBuilder.Entity("SignalBox.Core.ModelRegistration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("HostingType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("ModelType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScoringUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Swagger")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ModelRegistrations");
                });

            modelBuilder.Entity("SignalBox.Core.Offer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Cost")
                        .HasColumnType("REAL");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DiscountCode")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("SignalBox.Core.OfferRecommendation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonUserId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("ExperimentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Features")
                        .HasColumnType("TEXT");

                    b.Property<string>("IterationId")
                        .HasColumnType("TEXT");

                    b.Property<int>("IterationOrder")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("SignalBox.Core.Parameter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("DefaultValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParameterType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommonId")
                        .IsUnique();

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("SignalBox.Core.PresentationOutcome", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("ExperimentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IterationId")
                        .HasColumnType("TEXT");

                    b.Property<int>("IterationOrder")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("OfferId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Outcome")
                        .HasColumnType("TEXT");

                    b.Property<long?>("RecommendationId")
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SignalBox.Core.Recommenders.ParameterSetRecommendation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ParameterSetRecommendations");
                });

            modelBuilder.Entity("SignalBox.Core.Recommenders.ParameterSetRecommender", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Arguments")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommonId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParameterBounds")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScoringUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ParameterSetRecommenders");
                });

            modelBuilder.Entity("SignalBox.Core.Rule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EventKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventLogicalValue")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("SegmentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("SignalBox.Core.Segment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("SignalBox.Core.Sku", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Created")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<long?>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SkuId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Skus");
                });

            modelBuilder.Entity("SignalBox.Core.Touchpoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommonId")
                        .IsUnique();

                    b.ToTable("Touchpoints");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonId")
                        .HasColumnType("TEXT")
                        .HasColumnName("CommonUserId");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommonId")
                        .IsUnique();

                    b.ToTable("TrackedUsers");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommonUserId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EventId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Kind")
                        .HasColumnType("TEXT");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<long?>("SourceId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Timestamp")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventId")
                        .IsUnique();

                    b.HasIndex("SourceId");

                    b.ToTable("TrackedUserEvents");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserSystemMap", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("IntegratedSystemId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("TrackedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IntegratedSystemId");

                    b.HasIndex("TrackedUserId");

                    b.HasIndex("UserId", "TrackedUserId", "IntegratedSystemId")
                        .IsUnique();

                    b.ToTable("TrackUserSystemMaps");
                });

            modelBuilder.Entity("SignalBox.Core.TrackedUserTouchpoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<long?>("TouchpointId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TrackedUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Values")
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TouchpointId");

                    b.HasIndex("TrackedUserId");

                    b.ToTable("TrackedUserTouchpoints");
                });

            modelBuilder.Entity("SignalBox.Core.WebhookReceiver", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("EndpointId")
                        .HasColumnType("TEXT");

                    b.Property<long?>("IntegratedSystemId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("SharedSecret")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EndpointId")
                        .IsUnique();

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

            modelBuilder.Entity("ParameterParameterSetRecommender", b =>
                {
                    b.HasOne("SignalBox.Core.Recommenders.ParameterSetRecommender", null)
                        .WithMany()
                        .HasForeignKey("ParameterSetRecommendersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalBox.Core.Parameter", null)
                        .WithMany()
                        .HasForeignKey("ParametersId")
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
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Order")
                                .HasColumnType("INTEGER");

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
                        .HasForeignKey("IntegratedSystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalBox.Core.TrackedUser", "TrackedUser")
                        .WithMany("IntegratedSystemMaps")
                        .HasForeignKey("TrackedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                        .HasForeignKey("TrackedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Touchpoint");

                    b.Navigation("TrackedUser");
                });

            modelBuilder.Entity("SignalBox.Core.WebhookReceiver", b =>
                {
                    b.HasOne("SignalBox.Core.IntegratedSystem", "IntegratedSystem")
                        .WithMany("WebhookReceivers")
                        .HasForeignKey("IntegratedSystemId");

                    b.Navigation("IntegratedSystem");
                });

            modelBuilder.Entity("SignalBox.Core.IntegratedSystem", b =>
                {
                    b.Navigation("WebhookReceivers");
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
                    b.Navigation("IntegratedSystemMaps");

                    b.Navigation("TrackedUserTouchpoints");
                });
#pragma warning restore 612, 618
        }
    }
}
