using System.ComponentModel.DataAnnotations;
using Discord;
using Discord.Audio;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace RoberSanteNardo;

public class DiscordClient(
    IOptions<DiscordClient.Options> options,
    ILogger<DiscordClient> logger,
    DiscordSocketClient client,
    DiscordRestClient restClient
) : BackgroundService
{
    [Serializable]
    public class Options
    {
        [Required] public required string ClientId { get; set; }
        [Required] public required string Token { get; set; }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await client.LoginAsync(TokenType.Bot, options.Value.Token);
        await client.StartAsync();
        
        await restClient.LoginAsync(TokenType.Bot, options.Value.Token);
        var guild = await restClient.GetGuildAsync(218147243893456898);
        
        var command = new SlashCommandBuilder()
            .WithName("wololo")
            .WithDescription("this is a wololo command!");
        
        try
        {
            await guild.CreateApplicationCommandAsync(command.Build());
        }
        catch (ApplicationCommandException exception)
        {
            logger.LogError(exception, "An error occured while registering the command {Name}", command.Name);
        }
        
        client.SlashCommandExecuted += async e =>
        {
            Console.WriteLine(e.CommandName);
            switch (e.CommandName)
            {
                case "wololo":
                    await e.RespondAsync("Wololoooooooooo");

                    _ = Task.Run(async () =>
                    {
                        var channel = (e.User as IGuildUser)?.VoiceChannel;
                        if (channel != null)
                        {
                            try {
                            using var audioClient = await channel.ConnectAsync();
                            await using var stream = File.OpenRead("../output");
                            await using var audioOutStream = audioClient.CreatePCMStream(AudioApplication.Music);
                            await stream.CopyToAsync(audioOutStream);
                            await stream.FlushAsync();
                            await audioOutStream.FlushAsync();
                            Console.WriteLine("test");
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Erro");
                            } 
                        }
                    });

                    break;
            }
        };
    }
}