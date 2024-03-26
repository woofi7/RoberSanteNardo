using RoberSanteNardo.Models;
using RoberSanteNardo.Responses;

namespace RoberSanteNardo.Mappers;

public interface ISongMapper
{
    SongModel MapSongModel(YtDlpResponse songResponse);
}

public class SongMapper
{
    public static SongModel? MapSongModel(YtDlpResponse? songResponse)
    {
        if (songResponse == null)
            return null;
        
        return new SongModel
        {
            Id = new Guid(),
            YtDlpId = songResponse.Id,
            Title = songResponse.Title,
            ThumbnailUrl = songResponse.Thumbnail,
            Description = songResponse.Description,
            Duration = new TimeSpan(0, 0, songResponse.Duration),
            ViewCount = songResponse.ViewCount,
            SongUrl = songResponse.OriginalUrl,
            LiveStatus = songResponse.LiveStatus,
            ChannelUrl = songResponse.ChannelUrl,
            ChannelName = songResponse.Channel,
        };
    }
}