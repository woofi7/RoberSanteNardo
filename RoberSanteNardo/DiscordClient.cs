using System.ComponentModel.DataAnnotations;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using RoberSanteNardo.Commands;
using RoberSanteNardo.Services;

namespace RoberSanteNardo;
public class DiscordClient(
    IOptions<DiscordClient.Options> options,
    ILogger<DiscordClient> logger,
    DiscordSocketClient client,
    InteractionService interactionService,
    CommandHandler commandHandler,
    DiscordRestClient restClient,
    IHostApplicationLifetime applicationLifetime,
    MusicService musicService
) : BackgroundService
{
    [Serializable]
    public class Options
    {
        [Required] public required string ClientId { get; set; }
        [Required] public required string Token { get; set; }
        public ulong? TestGuildId { get; set; }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await musicService.Play(null, "https://www.youtube.com/watch?v=O9wG5jGP32M");
        applicationLifetime.ApplicationStarted.Register(() =>
        {
            Console.WriteLine("Started");
        });
        applicationLifetime.ApplicationStopping.Register(() =>
        {
            Console.WriteLine("Stopping");
        });
        applicationLifetime.ApplicationStopped.Register(() =>
        {
            Console.WriteLine("Stopped");
        });
        client.Log += LogAsync;
        interactionService.Log += LogAsync;
        client.Ready += ReadyAsync;
        
        await client.LoginAsync(TokenType.Bot, options.Value.Token);
        await client.StartAsync();
        
        await restClient.LoginAsync(TokenType.Bot, options.Value.Token);

        await commandHandler.InitializeAsync();
    }
    
    private Task LogAsync(LogMessage log)
    {
        logger.LogInformation(log.ToString());
        return Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        if (IsDebug())
        {
            var testGuildId = options.Value.TestGuildId;
            if (testGuildId == null)
            {
                logger.LogError("Missing `{field}` in the options.",
                    nameof(Options.TestGuildId));
                throw new MissingFieldException();
            }
            
            logger.LogInformation("In debug mode, adding commands to {guild}...", testGuildId);
            await interactionService.RegisterCommandsToGuildAsync((ulong)testGuildId);
        }
        else
        {
            // Adding globally, can take up to 1 hour
            await interactionService.RegisterCommandsGloballyAsync();
        }
        logger.LogInformation("Connected as -> [{currentUser}]", client.CurrentUser);
    }

    private static bool IsDebug ()
    {
        #if DEBUG
            return true;
        #endif
    }
}