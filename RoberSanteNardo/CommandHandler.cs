using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;

namespace RoberSanteNardo;

public class CommandHandler(
    DiscordSocketClient client,
    ILogger<CommandHandler> logger,
    InteractionService interactionService
    )
{
    [SlashCommand("wololo", "this is a wololo command!")]
    public async Task Wololo()
    {
        Console.WriteLine("wololo");
    }
    
    public async Task RegisterCommands()
    {
        var guild = client.GetGuild(218147243893456898);
        
        // var command = new SlashCommandBuilder();
        // command = new SlashCommandBuilder();
        // command.WithName("wololo");
        // command.WithDescription("this is a wololo command!");
        // try
        // {
        //     await guild.CreateApplicationCommandAsync(command.Build());
        // }
        // catch (ApplicationCommandException exception)
        // {
        //     logger.LogError(exception, "An error occured while registering the command {Name}", command.Name);
        // }
    }
}