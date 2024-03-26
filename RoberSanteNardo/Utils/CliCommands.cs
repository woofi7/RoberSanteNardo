using System.IO.Pipelines;
using System.Text;
using CliWrap;
using CliWrap.Buffered;
using Newtonsoft.Json;
using RoberSanteNardo.Responses;

namespace RoberSanteNardo.Utils;

public static class CliCommands
{
    public static async Task<YtDlpResponse?> GetUrlInfoWithYtDlp(
        string link,
        CancellationToken cancellationToken)
    {
        var stringBuilder = new StringBuilder();
        await Cli.Wrap(@"..\yt-dlp.exe")
            .WithArguments(args => args
                .Add("--no-download")
                .Add("--dump-json")
                .Add(link))
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stringBuilder))
            .ExecuteBufferedAsync(cancellationToken);
        return JsonConvert.DeserializeObject<YtDlpResponse>(stringBuilder.ToString());
    }
    
    public static CommandTask<BufferedCommandResult> GetYtDlpVideoTask(
        string arg,
        Pipe outputPipe,
        CancellationToken cancellationToken)
    {
        return Cli.Wrap(@"..\yt-dlp.exe")
            .WithArguments(args => args
                .AddOption("-f", "bestaudio")
                .AddOption("-o", "-")
                .AddOption("--ffmpeg-location", @"..\ffmpeg\bin\ffmpeg.exe")
                .Add(arg))
            .WithStandardOutputPipe(PipeTarget.ToStream(outputPipe.Writer.AsStream()))
            .ExecuteBufferedAsync(cancellationToken);
    }
    
    public static CommandTask<BufferedCommandResult> ConvertVideoToRawAudioTask(
        Pipe inputPipe,
        Pipe outputPipe,
        CancellationToken cancellationToken)
    {
        return Cli.Wrap(@"..\ffmpeg\bin\ffmpeg.exe")
            .WithArguments(args => args
                .AddOption("-i", "-")
                .AddOption("-acodec", "pcm_s16le")
                .AddOption("-ar", "44100")
                .AddOption("-ac", "2")
                .Add("-vn")
                .AddOption("-f", "wav")
                .Add("-"))
            .WithStandardInputPipe(PipeSource.FromStream(inputPipe.Reader.AsStream()))
            .WithStandardOutputPipe(PipeTarget.ToStream(outputPipe.Writer.AsStream()))
            .ExecuteBufferedAsync(cancellationToken);
    }
}