using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using jack.Handlers;
using jack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using jack.GControllers;
using jack.Services;
using System.Text;
//using XDB.Services;

namespace jack
{
    /// <summary> Detect whether a message is a command, then execute it. </summary>
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _cmds;
        private JsonSerializer jSerializer = new JsonSerializer();
        private CommandHandler randler => this;
        //private PatService patService;
        //private hugcounter hugcount;
        private GuildHandler GuildHandler;
        public Events EventHandler;
        private EPService _epService;

        private readonly IServiceCollection _map = new ServiceCollection();


        private void InitializeLoader()
        {
            jSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            jSerializer.NullValueHandling = NullValueHandling.Ignore;

        }






        public IServiceProvider Provider;

        public CommandHandler(IServiceProvider provider)
        {
            Provider = provider;

            _client = Provider.GetService<DiscordSocketClient>();
            _cmds = new CommandService();
            InitializeLoader();

            GuildHandler = Provider.GetService<GuildHandler>();
            EventHandler = Provider.GetService<Events>();
            _client.MessageReceived += HandleCommandAsync;
            _cmds = Provider.GetService<CommandService>();
            _epService = provider.GetService<EPService>();
            _client.UserJoined += Events.UserJoinedAsync;
            _client.UserLeft += Events.UserLeftAsync;
            _client.LeftGuild += Events.DeleteGuildConfig;
            _client.GuildAvailable += Events.HandleGuildConfigAsync;
            _client.MessageReceived += Events.HandleGuildMessagesAsync;
            _client.JoinedGuild += async (Guild) =>
            {
                await Events.JoinedGuildAsync(Guild);
            };

            _client.UserJoined += async (user) =>
            {
                var Config = GuildHandler.GuildConfigs[user.Guild.Id];
                var muted = Config.MutedList.Select(x => x.id);
                if (muted.Contains(user.Id))
                {
                    var muteRole = user.Guild.GetRole(Config.MuteRoleID);
                    await user.AddRoleAsync(muteRole);
                    await user.ModifyAsync(x => x.Mute = true);
                }
                return;
            };

            _client.UserJoined += async (user) =>
            {
                var Config = GuildHandler.GuildConfigs[user.Guild.Id];
                var role = Config.JoinRole.roleid;
                if (Config.JoinRole.IsEnabled == true)
                {
                    var joinrole = user.Guild.GetRole(role);
                    await user.AddRoleAsync(joinrole);
                    await user.ModifyAsync(x => x.Mute = true);
                }
                return;
            };

            _client.UserBanned += Events.OnUserBanned;
            _client.UserUnbanned += Events.OnUserUnBanned;

            _client.UserBanned += async (User,Guild) =>
            {
                var Config = GuildHandler.GuildConfigs[Guild.Id];
                var muted = Config.MutedList.Select(x => x.id);
                var muts = Config.MutedList.Where(x => x.id == User.Id).FirstOrDefault();
                if (muted.Contains(User.Id))
                {
                    Config.MutedList.Remove(muts);
                }
                
                

                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            };

            _client.UserJoined += async (User) =>
            {

                var Config = GuildHandler.GuildConfigs[User.Guild.Id];
                if (!Config.JoinEvent.IsEnabled == false && Config.Error.OnOff == true && Config.textwelcome.OnOff == false)
                {
                    ITextChannel Channel = null;
                    string WelcomeMessage = "Welcome to the server";

                    var JoinChannel = User.Guild.GetChannel(Config.JoinEvent.TextChannel);


                    if (User.Guild.GetChannel(Config.JoinEvent.TextChannel) != null)
                    {


                        WelcomeMessage = "Welcome to the server";

                        Channel = JoinChannel as ITextChannel;

                        await _epService.ShowProfile(WelcomeMessage, User.Guild.Id, User, JoinChannel as ITextChannel, Config.callcard.CardBg);
                        return;
                    }
                    else
                    {

                        WelcomeMessage = "Welcome to the server";

                        Channel = User.Guild.DefaultChannel as ITextChannel;

                        await _epService.ShowProfile(WelcomeMessage, User.Guild.Id, User, Channel, Config.callcard.CardBg);
                        return;
                    }
                }
                else
                {
                    return;
                }
                
            };

        }

        public async Task ConfigureAsync()
        {
            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly(),Provider);
        }

        // Load all modules from the assembly.




        private async Task HandleCommandAsync(SocketMessage s)
        {


            var msg = s as SocketUserMessage;
            if (msg == null)                                          // Check if the received message is from a user.
                return;
            if (msg.Author.Id == 156558351671623680 || msg.Author.Id == 251139273905012746)
            {
                return;
            }
            var priv = msg.Channel as IPrivateChannel;
            if (msg.Channel == priv)
            {
                if (msg.Author.Id == 156558351671623680 || msg.Author.Id == 251139273905012746)
                {
                    return;
                }
                else
                {
                    var c = _client.GetUser(140605609694199808);
                    var t = c as IUser;
                    var g = await t.GetOrCreateDMChannelAsync();
                    await g.SendMessageAsync($"{msg.Author.Username}:ID:({msg.Author.Id})\n{msg.Content} {(msg.Attachments.FirstOrDefault()?.Url)}");
                    return;
                }

            }
            
            Console.WriteLine($"\n{msg.Channel}| {msg.Author.Username}: {msg.Content}");
            var context = new SocketCommandContext(_client, msg);     // Create a new command context.
            var gld = (msg.Channel as SocketGuildChannel).Guild;
            string GuildPrefix = GuildHandler.GuildConfigs[gld.Id].Prefix;
            int argPos = 0;


            if (!(msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || msg.HasStringPrefix(GuildPrefix, ref argPos))) return;
            {                                                         // Try and execute a command with the given context.
                var result = await _cmds.ExecuteAsync(context, argPos, Provider);
                var onoff = GuildHandler.GuildConfigs[gld.Id].Error;

                
                    if (result.Error.Value != CommandError.UnknownCommand)
                        await context.Channel.SendMessageAsync(result.ToString());
                

            }
        }
    }
}
