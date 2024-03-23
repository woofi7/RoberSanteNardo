using System.ComponentModel.DataAnnotations;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using RoberSanteNardo.Commands;

namespace RoberSanteNardo;
public class DiscordClient(
    IOptions<DiscordClient.Options> options,
    ILogger<DiscordClient> logger,
    DiscordSocketClient client,
    InteractionService interactionService,
    CommandHandler commandHandler,
    DiscordRestClient restClient
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

    private async Task RegisterCommands(RestGuild guild)
    {
                // if (options.Value.TestGuildId.HasValue) 
        // {
        //     var guild = await restClient.GetGuildAsync((ulong)options.Value.TestGuildId);
        // }
        //
        // client.SlashCommandExecuted += async e =>
        // {
        //     Console.WriteLine(e.CommandName);
        //     switch (e.CommandName)
        //     {
        //         case "wololo":
        //             await e.RespondAsync("Wololoooooooooo");
        //
        //             _ = Task.Run(async () =>
        //             {
        //                 var channel = (e.User as IGuildUser)?.VoiceChannel;
        //                 if (channel != null)
        //                 {
        //                     try {
        //                     using var audioClient = await channel.ConnectAsync();
        //                     await using var stream = File.OpenRead("../The Coconut Song - (Da Coconut Nut) [w0AOGeqOnFY].wav");
        //                     await using var audioOutStream = audioClient.CreatePCMStream(AudioApplication.Music);
        //                     await stream.CopyToAsync(audioOutStream);
        //                     await stream.FlushAsync();
        //                     await audioOutStream.FlushAsync();
        //                     Console.WriteLine("test");
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         logger.LogError(ex, "Erro");
        //                     } 
        //                 }
        //             });
        //
        //             break;
        //         case "celine":
        //             await e.RespondAsync("Chickennnnnn");
        //
        //             _ = Task.Run(async () =>
        //             {
        //                 var channel = (e.User as IGuildUser)?.VoiceChannel;
        //                 if (channel != null)
        //                 {
        //                     try {
        //                         using var audioClient = await channel.ConnectAsync();
        //                         await using var stream = File.OpenRead("../J.Geco - Chicken Song [msSc7Mv0QHY].wav");
        //                         await using var audioOutStream = audioClient.CreatePCMStream(AudioApplication.Music);
        //                         await stream.CopyToAsync(audioOutStream);
        //                         await stream.FlushAsync();
        //                         await audioOutStream.FlushAsync();
        //                         Console.WriteLine("test");
        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         logger.LogError(ex, "Erro");
        //                     } 
        //                 }
        //             });
        //
        //             break;
        //     }
        // };
        
        var command = new SlashCommandBuilder()
            .WithName("wololo")
            .WithDescription("this is a wololo command!");
        var command2 = new SlashCommandBuilder()
            .WithName("celine")
            .WithDescription("The chicken command!");
        
        try
        {
            await guild.CreateApplicationCommandAsync(command.Build());
            await guild.CreateApplicationCommandAsync(command2.Build());
        }
        catch (ApplicationCommandException exception)
        {
            logger.LogError(exception, "An error occured while registering the command {Name}", command.Name);
        }
    }
}