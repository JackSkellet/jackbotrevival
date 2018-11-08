using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using ConsoleApp1;

namespace jack.Module
{
    [Name("Owner")]
    public class gstat : ModuleBase
    {
        DiscordSocketClient client = new DiscordSocketClient();
        [Command("gstat", RunMode = RunMode.Async)]

        public async Task GetMentionAsync()
        {
            var data = new EmbedBuilder();

            StringBuilder builder = new StringBuilder();

            var sss = ((DiscordSocketClient)Context.Client).Guilds.Select(g => g.Name);
            var ccc = ((DiscordSocketClient)Context.Client).Guilds.Select(g => g.Id);
            var ddd = ((DiscordSocketClient)Context.Client).Guilds.Select(g => g.Owner.Username);
            var eee = ((DiscordSocketClient)Context.Client).Guilds.Select(g => g.MemberCount);
            var a = string.Join($"`\n`", ccc);
            var b = string.Join($"`\n`", sss);
            var d = string.Join($"`\n`", ddd);
            var e = string.Join($"`\n`", eee);
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Guilds";
                x.Value = $"`{b}`";
            });
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Member Count";
                x.Value = $"`{e}`";
            });
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Owners";
                x.Value = $"`{d}`";
            });
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Id's";
                x.Value = $"`{a}`";
            });

            await ReplyAsync("", embed: data.Build());

        }
    }


}