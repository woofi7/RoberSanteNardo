using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;    
using RoberSanteNardo;
using RoberSanteNardo.Commands;

var builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddOptions<DiscordBotDbContext.Options>()
//     .BindConfiguration("Database")
//     .ValidateDataAnnotations()
//     .ValidateOnStart();
//
// builder.Services.AddDbContext<DiscordBotDbContext>((sp, dbBuilder) =>
// {
//     var options = sp.GetRequiredService<IOptions<DiscordBotDbContext.Options>>();
//     dbBuilder.UseMySQL(options.Value.ConnectionString, o => o.EnableRetryOnFailure())
//         .EnableDetailedErrors();
// });

builder.Services.AddOptions<DiscordClient.Options>()
    .BindConfiguration("DiscordClient")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHostedService<DiscordClient>();

builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<DiscordRestClient>();
builder.Services.AddSingleton<InteractionService>(sp =>
    new InteractionService(sp.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddSingleton<CommandHandler>();

var host = builder.Build();
host.Run();