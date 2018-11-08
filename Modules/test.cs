
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jack.Extensions;
using jack.Handlers;
using jack.Models;
using jack.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jackbotV2.Modules
{
    [Name("Owner")]
    public class test : ModuleBase
    {
        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.Unicode.GetBytes(value ?? ""));
        }

        private EPService epService;

        public test(EPService ep)
        {
            epService = ep;
        }

        /*[Command("Archive", RunMode = RunMode.Async), Summary("Archives a channel and uploads a JSON"), Remarks("Archive #ChannelName 50")]
        public async Task ArchiveCommand(IMessageChannel Channel, int Amount = 9000)
        {
            if (Amount >= 10000)
            {
                await ReplyAsync("Amount must by less than 9000!");
                return;
            }

            var listOfMessages = new List<IMessage>(Channel.GetMessagesAsync(Amount).FlattenAsync());

            List<ArchiveModel> list = new List<ArchiveModel>(listOfMessages.Capacity);
            foreach (var message in listOfMessages) list.Add(new ArchiveModel
            {
                Author = message.Author.Username,
                Message = message.Content,
                Timestamp = message.Timestamp,
            });
            var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(list, Formatting.Indented, jsonSettings);
            await (await Context.User.GetOrCreateDMChannelAsync()).SendFileAsync(GenerateStreamFromString(json), $"{Channel.Name}.json");
            await ReplyAsync($"{Channel.Name}'s Archive has been sent to your DM.");
        }*/

        [Command("profile", RunMode = RunMode.Async), Summary("Displays short profile image of User, if not specified it will show yours")]
        [RequireOwner]
        public async Task SendProfile(
            [Summary("User to show the picture of, if none given will show your own!")] SocketGuildUser user = null)
        {

            var User = user ?? Context.Message.Author as SocketGuildUser;
            var Config = GuildHandler.GuildConfigs[User.Guild.Id];
            var JoinChannel = User.Guild.GetChannel(Config.JoinEvent.TextChannel);
            string WelcomeMessage = "";
            ITextChannel Channel;
            WelcomeMessage = Config.WelcomeMessages;

            if (User.Guild.GetChannel(Config.JoinEvent.TextChannel) != null)
            {

                var replace = StringExtension.ReplaceWith(Config.WelcomeMessages, User.Username + "#" + User.Discriminator, User.Guild.Name);
                WelcomeMessage = Config.WelcomeMessages;

                Channel = JoinChannel as ITextChannel;


                await epService.ShowProfile("Welcome to the server", User.Guild.Id, User, JoinChannel as ITextChannel, Config.callcard.CardBg);
            }
        }

    }
}
