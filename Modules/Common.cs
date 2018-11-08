using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Preconditions;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using System;
using System.Diagnostics;
using jack.Services;
using System.Collections.Generic;
//using XDB.Services.RemindService;
//using XDB.Services;
using Humanizer;
using jack.Handlers;
//using Sora_Bot_1.SoraBot.Services.Reminder;
using YoutubeExplode;
using jack.Extensions;
using jack.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace jack.Modules
{
    [Name("Common Commands")]
    public class Common : ModuleBase<SocketCommandContext>
    {
        public static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        public static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("say", RunMode = RunMode.Async), Alias("s")]
        [Summary("Make the bot say something")]

        public async Task Say([Remainder]string text)
        {
            await ReplyAsync(text);
        }

        

        [Command("about", RunMode = RunMode.Async)]
        [Summary("Get's some info about the bot, like the creator, prefix and whatnot")]
        public async Task sayasync()
        {
            var app = await Context.Client.GetApplicationInfoAsync();
            var data = new EmbedBuilder();
            data.WithColor(new Color(0xb100c1));
            data.WithAuthor(x =>
            {
                x.IconUrl = Context.Client.CurrentUser.GetAvatarUrl();
                x.Name = Context.Client.CurrentUser.Username;
            });

            data.AddField(x =>
            {
                x.WithIsInline(false);
                x.Name = $"Hello, My name is {app.Name}";
                x.Value = $"I am a multipurpose bot for discord. \nI was made using Discord.Net ({DiscordConfig.Version})\nMy creator is {app.Owner}\nI was made {app.CreatedAt}";
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Status";
                x.Value = $"Uptime: {GetUptime()}\nMemory: {GetHeapSize()}MB";
            });

            data.AddField(x =>
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
                x.WithIsInline(true);
                x.Name = $"Prefix";
                x.Value = gldConfig.Prefix;
            });
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = $"usefull links";
                x.Value = "[support/help server](https://discord.gg/NYWaXFX)\n[bot invite](https://discordapp.com/oauth2/authorize?&client_id=170218618771079168&scope=bot&permissions=284552406)";
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Total Channels";
                x.Value = $"{(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}";
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Guild's im in";
                x.Value = $"{(Context.Client as DiscordSocketClient).Guilds.Count}";
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Total Users i see";
                x.Value = $"{(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}";
            });

            data.WithTimestamp(DateTime.UtcNow);
            await ReplyAsync("", embed: data.Build());
        }

        string[] answers = {
            "It is certain",
            "It is decidedly so",
            "Without a doubt",
            "Yes, definitely",
            "You may rely on it",
            "As I see it, yes",
            "Most likely",
            "Outlook good",
            "Yes",
            "Signs point to yes",
            "Reply hazy try again",
            "Ask again later",
            "Better not tell you now",
            "Cannot predict now",
            "Concentrate and ask again",
            "Don't count on it",
            "My reply is no",
            "My sources say no",
            "Outlook not so good",
            "Very doubtful" };
        Random r = new Random();

        [Command("8ball", RunMode = RunMode.Async), Summary("Magic 8 ball"),Remarks("8ball question")]

        public async Task action([Remainder] string question)
        {
            await ReplyAsync($"`{question}`: {answers[r.Next(answers.Length - 1)]}");
        }
        [Command("roll", RunMode = RunMode.Async)]
        [Summary("Rolls any # dice"),Remarks("roll #")]
        public async Task Rolld20(int number = 6)
        {
            try
            {
                var embed = new EmbedBuilder();
                embed.WithColor(new Color(0xb100c1));
                embed.WithTitle($"Dice roll");
                Random r = new Random();
                int randomValues = r.Next(number);
                embed.WithDescription($"{Context.User.Mention} rolled the D{number} and got **{randomValues}**.");

                await ReplyAsync("", embed: embed.Build());
            }
            catch
            {
                await ReplyAsync("Error? you probably didnt use a number you idiot");
            }

        }
        [Command("getm", RunMode = RunMode.Async), Alias("getmessage")]
        [Summary("Get's message from specific channel"),Remarks("getm id channel")]
        public async Task sayasync(ulong id, IChannel cool = null)
        {

            var coolchannel = cool ?? (Context.Channel);

            var ass = coolchannel as ITextChannel;
            var message = (SocketMessage)Context.Message;
            var user = message.Channel?.GetUserAsync(message.Id);

            var msgs = await ass.GetMessageAsync(id);
            var msgss = await ass.GetMessageAsync(id);
            var data = new EmbedBuilder();
            var aaa = string.Join(" ", msgss.Content.ToString());

            Context.Channel.GetMessagesAsync(100);

            if (msgs.Attachments.Count >= 1)
            {
                data.WithAuthor(x =>
                {
                    x.Name = $"{msgs.Author.Username}";
                    x.IconUrl = msgs.Author.GetAvatarUrl();
                });

                data.WithColor(new Color(0xFFD700));
                var url = msgs.Attachments.FirstOrDefault().Url;
                var att = string.Join("", url);
                data.WithDescription(msgss.Content.ToString());

                data.WithFooter(x => x.Text = "Message sent");
                data.WithTimestamp(msgss.Timestamp.DateTime);
                data.WithImageUrl(url);
            }
            else
            {
                data.WithAuthor(x =>
                {
                    x.Name = $"{msgs.Author.Username}";
                    x.IconUrl = msgs.Author.GetAvatarUrl();
                });
                data.WithColor(new Color(0xFFD700));
                data.WithDescription(msgss.Content.ToString());


                data.WithFooter(x => x.Text = "Message sent");
                data.WithTimestamp(msgss.Timestamp.DateTime);
            }

            await ReplyAsync("", embed: data.Build());

        }

        [Command("youtube", RunMode = RunMode.Async), Summary("Search for youtube videos"), Alias("yt")]
        public async Task Pasnt([Remainder]string searchterm)
        {

            var ytc = new YoutubeClient();
            var videoInfo = await ytc.SearchAsync(searchterm);
            var url = videoInfo.FirstOrDefault();
            await ReplyAsync("https://www.youtube.com/watch?v=" + url);
        }

        [Command("emotes", RunMode = RunMode.Async), Summary("shows custom emotes in this server")]
        public async Task Patssunt()
        {

            var data = new EmbedBuilder();
            data.Title = "Custom Emotes - " + Context.Guild.Emotes.Count;
            data.WithDescription(GetEmotes(Context.Guild.Emotes));

            await ReplyAsync("", embed: data.Build());
        }



        [Command("emotes"), Summary("shows custom emotes from specific server that bot is in")]
        public async Task Pasunt(ulong id)
        {
            var client = Context.Client;

            var gld = (client.GetGuild(id) as SocketGuild);
            var data = new EmbedBuilder();
            data.Title = "Custom Emotes - " + gld.Emotes.Count;
            data.WithDescription(GetEmotes(gld.Emotes));

            await ReplyAsync("", embed: data.Build());
        }

        
        

        private string GetEmotes(IReadOnlyCollection<GuildEmote> emotes)
        {
            if (emotes.Count < 1)
                return "You dont have any custom emotes.";

            else
            {
                string emoteList = "";

                foreach (GuildEmote emote in emotes)
                {
                    emoteList += $"<:{emote.Name}:{emote.Id}>";
                }
                return emoteList;
            }
        }
    }

        

    }
    [Name("hugs")]
    public class hugs : ModuleBase<SocketCommandContext>
    {
        private hugcounter hugcount;
        public hugs(hugcounter _counter)
        {
            hugcount = _counter;
        }

        [Command("hug", RunMode = RunMode.Async)]
        [Summary("Hug someone"),Remarks("hug user")]
        public async Task GetMentionAsync(IUser user = null)
        {
            var awoo = user ?? Context.Message.Author;


            if (user == null || user == Context.Message.Author)
            {
                await ReplyAsync($"{awoo.Mention} is lonely");
                return;
            }
            else
            {
                await hugcount.Addhug(user, Context);
                await ReplyAsync($"{Context.Message.Author.Mention} hugged {awoo.Mention} (づ｡◕‿‿◕｡)づ");
                return;
            }
        }


        [Command("hugcount", RunMode = RunMode.Async), Summary("How many hugs did this User Receive (Global Number)")]
        public async Task PatCount(
            [Summary("Person to get hugcount. If not specified it will give your own")] IUser user = null)
        {
            var userInfo = user ?? Context.User; // ?? if not null return left. if null return right
            await hugcount.Checkhugs(userInfo, Context);
        }



    }
[Name("pats")]
public class pats : ModuleBase<SocketCommandContext>
{
    private PatService patService;
    public pats(PatService _patService)
    {
        patService = _patService;
    }
    [Command("pat", RunMode = RunMode.Async)]
    [Summary("pat someone"),Remarks("pat user")]
    public async Task snAsync(IUser user = null)
    {
        var awoo = user ?? Context.Message.Author;

        if (Context.Message.Content.Contains("pat"))
        {
            if (user == null || user == Context.Message.Author)
            {

                await ReplyAsync($"{awoo.Mention} did not get a pat on the head :C");
                return;
            }
            else
            {
                await patService.AddPat(user, Context);
                await ReplyAsync($"{Context.Message.Author.Mention} patted {awoo.Mention}'s head (◕‿◕)");
                return;
            }
        }

    }
    [Command("patcount", RunMode = RunMode.Async), Summary("How many pats did this User Receive (Global Number)")]
    public async Task Patssunt(
        [Summary("Person to get Patcount. If not specified it will give your own")] IUser user = null)
    {
        var userInfo = user ?? Context.User; // ?? if not null return left. if null return right
        await patService.CheckPats(userInfo, Context);
    }



}
/*[Name("Reminder")]
public class ReminderModule : ModuleBase<SocketCommandContext>
{
    private ReminderService _reminderService;

    public ReminderModule(ReminderService remService)
    {
        _reminderService = remService;
    }

    [Command("remind", RunMode = RunMode.Async), Alias("rem", "rm", "reminder"), Remarks("The time can be written in any order BUT has to be written correctly. Dont write 2 mins **AND** 30 seconds. That will fail. Just use the amount and what type after => <amount> <type(w,d,h,m,s)>")]
    [RequireOwner]
    public async Task CreateRemind([Summary("What to remind for"), Remainder] string reminder)
    {
        await _reminderService.SetReminder(Context, reminder);
    }

    [Command("reminders"), Alias("rems", "rms"), Remarks("Shows a list of the 10 MOST RECENT reminders. It will show them in descending order with time and message")]
    [RequireOwner]
    public async Task GetReminders()
    {
        await _reminderService.GetReminders(Context);
    }

    [Command("rmremind", RunMode = RunMode.Async), Alias("rmrem", "rmrm", "rmreminder"), Summary("DO THIS")]
    [RequireOwner]
    public async Task RemoveReminder()
    {
        await _reminderService.DelteReminder(Context);
    }
    
}*/







