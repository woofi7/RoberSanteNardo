using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using RoberSanteNardo;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DiscordClient>();

builder.Services.AddOptions<DiscordClient.Options>()
    .BindConfiguration("DiscordClient")
    .ValidateOnStart();

builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<DiscordRestClient>();

var host = builder.Build();
host.Run();