using System.IO.Pipelines;
using System.Text;
using CliWrap;
using CliWrap.Buffered;
using Discord;
using Discord.Audio;
using Discord.Interactions;
using Newtonsoft.Json;
using RoberSanteNardo.Mappers;
using RoberSanteNardo.Utils;

namespace RoberSanteNardo.Services;

public class MusicService(ILogger<MusicService> logger, IHostApplicationLifetime applicationLifetime)
{
    private readonly CancellationTokenSource _cts = CancellationTokenSource.CreateLinkedTokenSource(applicationLifetime.ApplicationStopping);

    public async Task Play(SocketInteractionContext? context, string arg)
    {
        await AddSongToPlaylist(arg);
        
        // var channel = GetUserVoiceChannel(context);
        //
        // try {
        //     using var audioClient = await channel.ConnectAsync();
        //     await using var audioOutStream = audioClient.CreatePCMStream(AudioApplication.Music);
        //     await MusicPlayer(arg, audioOutStream, _cts.Token);
        //     await audioOutStream.FlushAsync();
        // }
        // catch (Exception e)
        // {
        //     logger.LogError("Error while playing sound in voice channel: {error}", e);
        // }
    }

    private async Task AddSongToPlaylist(string link)
    {
        link = "https://www.youtube.com/watch?v=h8gKJFLFOlk";
        var songInfo = await CliCommands.GetUrlInfoWithYtDlp(link, _cts.Token);
        var song = SongMapper.MapSongModel(songInfo);
        Console.WriteLine(songInfo);
    }

    private async Task<Stream> MusicPlayer(string arg, AudioOutStream audioOutStream,
        CancellationToken cancellationToken)
    {
        var ytDlPipe = new Pipe();
        var audioPipe = new Pipe();
        
        var ytDlTask = CliCommands.GetYtDlpVideoTask(arg, ytDlPipe, cancellationToken);
        var ffmpegTask = CliCommands.ConvertVideoToRawAudioTask(ytDlPipe, audioPipe, cancellationToken);
        var streamAudio = audioPipe.Reader.AsStream().CopyToAsync(audioOutStream, cancellationToken);
        
        await ytDlTask;
        await ffmpegTask;
        await streamAudio;
        
        return audioPipe.Reader.AsStream();
    }

    private static IVoiceChannel GetUserVoiceChannel(SocketInteractionContext context)
    {
        var channel = (context.User as IGuildUser)?.VoiceChannel;
        if (channel == null)
            throw new ArgumentException("Voice channel does not exist.");
        return channel;
    }
}