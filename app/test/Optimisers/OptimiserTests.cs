using Xunit;
using SignalBox.Functions;
using System.Text.Json;
using System;
using SignalBox.Core;
using SignalBox.Core.Optimisers;

namespace SignalBox.Test.Stores
{
    public class OptimiserTests
    {
        readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private string CreateCategoricalOptimiserBody()
        {
            return
            @"
             {
                ""id"": ""Id1"",
                ""name"": ""NewRecommender"",
                ""nItemsToRecommend"": 1,
                ""baselineItem"": {""commonId"": ""Test1""},
                ""items"": [{
                    ""commonId"": ""Test1""
                    }, 
                    {
                        ""commonId"": ""Test2""
                    },
                    {
                        ""commonId"": ""Test3""
                    }
                ]
            }";
        }

        private string CreateCategoricalOptimiserPayload()
        {
            return
            @"
            {
                ""id"": 3,
	            ""name"": ""NameOfRecommender"",
                ""version"": ""prod_1"",
                ""payload"": {
                    ""features"": {},
                    ""arguments"": {},
                    ""parameterBounds"": [],
                    ""items"": [{
                        ""commonId"": ""Test1""
                        }, {
                        ""commonId"": ""Test2""
                        }, {
                        ""commonId"": ""Test3""
                    }]
                }
            }";
        }

        private string CreateCategoricalOptimiserCollection()
        {
            return
            @"
            {
                ""populations"": [
                    {
                    ""population_id"": 1,
                    ""default_item"": { ""commonId"": ""Test1"" },
                    ""items"": [
                        { ""commonId"": ""Test1"" },
                        { ""commonId"": ""Test2"" },
                        { ""commonId"": ""Test3"" }
                    ],
                    ""probabilities"": { ""Test1"": 0.6, ""Test2"": 0.2, ""Test3"": 0.2 }
                    }
                ],
                ""nItemsToRecommend"": 1,
                ""defaultItem"": { ""commonId"": ""Test1"" }
                
            }";
        }

        [Fact]
        public void CreateCategoricalOptimiserInvalidTest()
        {
            OptimiserJobs jobs = new OptimiserJobs();
            var jsonString = @"
            {
                ""items"": [{
                    ""commonId"": ""bcjadbf""
                }]
            }";

            var info = JsonSerializer.Deserialize<CategoricalOptimiser>(jsonString, serializerOptions);
            Action act = () => jobs.CreateCategoricalOptimiser(info, "tenant1");
            BadRequestException exception = Assert.Throws<BadRequestException>(act);
        }

        [Fact]
        public void CreateCategoricalOptimiserValidTest()
        {
            OptimiserJobs jobs = new OptimiserJobs();
            var jsonString = CreateCategoricalOptimiserBody();
            var info = JsonSerializer.Deserialize<CategoricalOptimiser>(jsonString, serializerOptions);

            var data = jobs.CreateCategoricalOptimiser(info, "tenant1");

            Assert.NotNull(data);
            Assert.NotNull(data.Populations);
            Assert.NotEmpty(data.Populations);
        }

        [Fact]
        public void InvokeCategoricalOptimiserValidTest()
        {
            OptimiserJobs jobs = new OptimiserJobs();

            var jsonString = CreateCategoricalOptimiserPayload();
            InvokeCategoricalOptimiserModel payload = null;

            var jsonCollectionString = CreateCategoricalOptimiserCollection();
            PopulationDistributionCollection collection = null;

            try
            {
                payload = JsonSerializer.Deserialize<InvokeCategoricalOptimiserModel>(jsonString, serializerOptions);

                collection = JsonSerializer.Deserialize<PopulationDistributionCollection>(jsonCollectionString, serializerOptions);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            var data = jobs.InvokeCategoricalOptimiser(collection, payload);
            Assert.NotNull(data);
        }
    }
}