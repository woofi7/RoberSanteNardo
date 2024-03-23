using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace RoberSanteNardo.Commands;

public class CommandHandler(
    DiscordSocketClient client,
    InteractionService interactionService,
    IServiceProvider serviceProvider,
    ILogger<CommandHandler> logger
    )
{
    public async Task InitializeAsync()
    {
        // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);

        // process the InteractionCreated payloads to execute Interactions commands
        client.InteractionCreated += HandleInteraction;

        // process the command execution results 
        interactionService.SlashCommandExecuted += SlashCommandExecuted;
    }
    
    private Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
    {
        if (!result.IsSuccess)
            logger.LogError("Error while executing the slash command: `{name}`, {error}",
                commandInfo.Name, result.ErrorReason);
      
        return Task.CompletedTask;
    }
    
    private async Task HandleInteraction(SocketInteraction arg)
    {
        try
        {
            var ctx = new SocketInteractionContext(client, arg);
            await interactionService.ExecuteCommandAsync(ctx, serviceProvider);
        }
        catch (Exception e)
        {
            logger.LogError("Error while executing interaction command: {error}", e);
            if(arg.Type == InteractionType.ApplicationCommand)
            {
                await arg.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}