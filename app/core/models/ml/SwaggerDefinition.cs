using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public partial class SwaggerDefinition
    {
        [JsonPropertyName("swagger")]
        public string Swagger { get; set; }

        [JsonPropertyName("info")]
        public Info Info { get; set; }

        [JsonPropertyName("schemes")]
        public string[] Schemes { get; set; }

        [JsonPropertyName("consumes")]
        public string[] Consumes { get; set; }

        [JsonPropertyName("produces")]
        public string[] Produces { get; set; }

        [JsonPropertyName("securityDefinitions")]
        public SecurityDefinitions SecurityDefinitions { get; set; }

        [JsonPropertyName("paths")]
        public Paths Paths { get; set; }

        [JsonPropertyName("definitions")]
        public Definitions Definitions { get; set; }
    }

    public partial class Definitions
    {
        [JsonPropertyName("ServiceInput")]
        public ServiceInput ServiceInput { get; set; }

        [JsonPropertyName("ServiceOutput")]
        public ServiceOutput ServiceOutput { get; set; }

        [JsonPropertyName("ErrorResponse")]
        public ErrorResponse ErrorResponse { get; set; }
    }

    public partial class ErrorResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public ErrorResponseProperties Properties { get; set; }
    }

    public partial class ErrorResponseProperties
    {
        [JsonPropertyName("status_code")]
        public StatusCodeClass StatusCode { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    public partial class Message
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class StatusCodeClass
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }
    }

    public partial class ServiceInput
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public ServiceInputProperties Properties { get; set; }

        [JsonPropertyName("example")]
        public AzureMLModelInput Example { get; set; }
    }

    public partial class ServiceInputProperties
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public DataItems Items { get; set; }
    }

    public partial class DataItems
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("required")]
        public string[] ItemsRequired { get; set; }

        [JsonPropertyName("properties")]
        public ItemsProperties Properties { get; set; }
    }

    public partial class ItemsProperties
    {
        [JsonPropertyName("Column2")]
        public Message Column2 { get; set; }

        [JsonPropertyName("A")]
        public StatusCodeClass A { get; set; }

        [JsonPropertyName("B")]
        public StatusCodeClass B { get; set; }
    }

    public partial class ServiceOutput
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("items")]
        public StatusCodeClass Items { get; set; }

        [JsonPropertyName("example")]
        public long[] Example { get; set; }
    }

    public partial class Info
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }

    public partial class Paths
    {
        [JsonPropertyName("/")]
        public Empty Empty { get; set; }

        [JsonPropertyName("/score")]
        public Score Score { get; set; }
    }

    public partial class Empty
    {
        [JsonPropertyName("get")]
        public Get Get { get; set; }
    }

    public partial class Get
    {
        [JsonPropertyName("operationId")]
        public string OperationId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("responses")]
        public GetResponses Responses { get; set; }
    }

    public partial class GetResponses
    {
        [JsonPropertyName("200")]
        public The200 The200 { get; set; }

        [JsonPropertyName("default")]
        public Default Default { get; set; }
    }

    public partial class Default
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("schema")]
        public Schema Schema { get; set; }
    }

    public partial class Schema
    {
        [JsonPropertyName("$ref")]
        public string Ref { get; set; }
    }

    public partial class The200
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("schema")]
        public Message Schema { get; set; }

        [JsonPropertyName("examples")]
        public Examples Examples { get; set; }
    }

    public partial class Examples
    {
        [JsonPropertyName("application/json")]
        public string ApplicationJson { get; set; }
    }

    public partial class Score
    {
        [JsonPropertyName("post")]
        public Post Post { get; set; }
    }

    public partial class Post
    {
        [JsonPropertyName("operationId")]
        public string OperationId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("security")]
        public Security[] Security { get; set; }

        [JsonPropertyName("parameters")]
        public Bearer[] Parameters { get; set; }

        [JsonPropertyName("responses")]
        public PostResponses Responses { get; set; }
    }

    public partial class Bearer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("in")]
        public string In { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("schema")]
        public Schema Schema { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class PostResponses
    {
        [JsonPropertyName("200")]
        public Default The200 { get; set; }

        [JsonPropertyName("default")]
        public Default Default { get; set; }
    }

    public partial class Security
    {
        [JsonPropertyName("Bearer")]
        public object[] Bearer { get; set; }
    }

    public partial class SecurityDefinitions
    {
        [JsonPropertyName("Bearer")]
        public Bearer Bearer { get; set; }
    }
}
