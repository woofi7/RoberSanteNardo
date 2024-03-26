namespace RoberSanteNardo.Models;

public class SongModel
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required TimeSpan Duration { get; set; }
    public int ViewCount { get; set; }
    public string YtDlpId { get; set; } = "";
    public string ThumbnailUrl { get; set; } = "";
    public string Description { get; set; } = "";
    public required string SongUrl { get; set; }
    public string LiveStatus { get; set; } = "";
    public string ChannelUrl { get; set; } = "";
    public string ChannelName { get; set; } = "";
}