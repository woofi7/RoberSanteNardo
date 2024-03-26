using Discord.Interactions;
using RoberSanteNardo.Services;

namespace RoberSanteNardo.Commands;

public class MusicCommands(
    MusicService musicService
) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("now-playing", "Shows information about the song that is currently playing")]
    public async Task NowPlaying()
    {
        await Context.Interaction.RespondAsync("nowPlaying");
        throw new NotImplementedException();
    }
    
    [SlashCommand("play", "Add a song to the queue")]
    public async Task Play(string link)
    {
        await Context.Interaction.RespondAsync("play");
        await musicService.Play(Context, link);
        
        // await Context.Interaction.DeferAsync();
    }
    
    [SlashCommand("play-next", "Add a song the be played next in the queue")]
    public async Task PlayNext(string link)
    {
        await Context.Interaction.RespondAsync("playNext");
        await musicService.Play(Context, link);
    }
    
    [SlashCommand("queue", "Shows songs in the queue")]
    public async Task Queue()
    {
        await Context.Interaction.RespondAsync("queue");
        throw new NotImplementedException();
    }
    
    [SlashCommand("remove", "Remove a song from the queue")]
    public async Task Remove()
    {
        await Context.Interaction.RespondAsync("remove");
        throw new NotImplementedException();
    }
    
    [SlashCommand("clear", "Clear the song queue")]
    public async Task Clear()
    {
        await Context.Interaction.RespondAsync("clear");
        throw new NotImplementedException();
    }
    
    [SlashCommand("shuffle", "Shuffle the songs in the queue")]
    public async Task Shuffle()
    {
        await Context.Interaction.RespondAsync("shuffle");
        throw new NotImplementedException();
    }
    
    [SlashCommand("skip", "Skip the current playing song")]
    public async Task Skip()
    {
        await Context.Interaction.RespondAsync("skip");
        throw new NotImplementedException();
    }
        
    [SlashCommand("skip-to", "Skip the queue to a specific song")]
    public async Task SkipTo()
    {
        await Context.Interaction.RespondAsync("skipTo");
        throw new NotImplementedException();
    }
    
    [SlashCommand("move-song", "Change the position of a song in the queue")]
    public async Task MoveTRack()
    {
        await Context.Interaction.RespondAsync("moveTrack");
        throw new NotImplementedException();
    }
    
    [SlashCommand("repeat", "Loop the queue when it's finished")]
    public async Task Repeat()
    {
        await Context.Interaction.RespondAsync("repeat");
        throw new NotImplementedException();
    }
    
    [SlashCommand("stop", "Stop the current song and clear the queue")]
    public async Task Stop()
    {
        await Context.Interaction.RespondAsync("stop");
        throw new NotImplementedException();
    }
}