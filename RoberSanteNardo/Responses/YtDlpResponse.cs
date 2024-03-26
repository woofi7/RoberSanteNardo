using Newtonsoft.Json;

namespace RoberSanteNardo.Responses;

[Serializable]
public class YtDlpResponse
{
    [JsonProperty("id")] public required string Id { get; set; }
    [JsonProperty("title")] public required string Title { get; set; }
    [JsonProperty("thumbnail")] public required string Thumbnail { get; set; }
    [JsonProperty("description")] public required string Description { get; set; }
    [JsonProperty("duration")] public required int Duration { get; set; }
    [JsonProperty("view_count")] public required int ViewCount { get; set; }
    [JsonProperty("webpage_url")] public required string WebpageUrl { get; set; }
    [JsonProperty("original_url")] public required string OriginalUrl { get; set; }
    [JsonProperty("live_status")] public required string LiveStatus { get; set; }
    [JsonProperty("channel_id")] public required string ChannelId { get; set; }
    [JsonProperty("channel_url")] public required string ChannelUrl { get; set; }
    [JsonProperty("channel")] public required string Channel { get; set; }
}