using Discord;
using Discord.Addons.Paginator;
using Discord.Commands;
using Discord.WebSocket;
using jack.Enums;
using jack.Functions;
using jack.GControllers;
using jack.Handlers;
using jack.Models;
using jack.Services;
using Microsoft.Extensions.DependencyInjection;

//using Sora_Bot_1.SoraBot.Services.Reminder;
using System;
using System.Threading.Tasks;
//using XDB.Services;
//using XDB.Services;

namespace jack
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient Client;
        private CommandHandler Commands;
        private CommandHandler Handler;

        public async Task StartAsync()
        {

            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,              // Specify console verbose information level.
                MessageCacheSize = 10000,
                AlwaysDownloadUsers = true,                    // Tell discord.net how long to store messages (per channel).
            });

            Client.Log += (Log) => Task.Run(()
                => Logger.Log(Enums.LogType.Info, Enums.LogSource.Client, Log.Message));
            GuildHandler.GuildConfigs = await GuildHandler.LoadServerConfigsAsync<GuildModel>();
            await Client.LoginAsync(TokenType.Bot, Config.Load().Token);
            await Client.StartAsync();

            var serviceProvider = ConfigureServices();
            Handler = new CommandHandler(serviceProvider);
            await Handler.ConfigureAsync();
            

            await Task.Delay(-1);                            // Prevent the console window from closing.
        }
        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(new AudioService())

                .AddSingleton<PatService>()
                .AddSingleton<EPService>()
                .AddSingleton<hugcounter>()
                .AddSingleton(new CommandService(
                    new CommandServiceConfig { CaseSensitiveCommands = false, ThrowOnError = false }))
                    .AddSingleton<SelfRoleService>()
                .AddPaginator(Client);

            var Provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            Provider.GetService<GuildHandler>();
            Provider.GetService<Events>();
            Provider.GetService<EPService>();
            return Provider;
        }

    }
}
