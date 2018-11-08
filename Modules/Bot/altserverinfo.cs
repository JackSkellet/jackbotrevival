using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Globalization;
using jack.Extensions;

namespace jack.Modules

{
    [Name("Owner")]
    public class altserverinfo : ModuleBase
    {
        [Command("lsi", RunMode = RunMode.Async)]
        public async Task guildinfo(ulong id)
        {

            var client = Context.Client;

            var guild = (await client.GetGuildAsync(id) as SocketGuild);

            string mnth1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(guild.CreatedAt.Month);

            String dayStr = DaySufix.DSufix(guild.CreatedAt.Day);
            var jdate = $"{mnth1} {dayStr}/{guild.CreatedAt.Year} at {guild.CreatedAt.ToString("hh:mm tt")}";
            var data = new EmbedBuilder();



            data.WithAuthor(x =>
            {
                x.IconUrl = guild.IconUrl;
                x.Name = "Server Info";
            });

            data.WithDescription(guild.Name);

            

                DateTime date1 = guild.CreatedAt.DateTime;
                int years = Int32.Parse(TimeSufix.yearSufixs(date1));
                int days = Int32.Parse(TimeSufix.daySufixs(date1));

                if (years >= 1)
                {
                    data.WithDescription($"**Joined**: {jdate}\n**{years.ToString()} years, {days.ToString()} days ago**");
                }
                else
                {
                    data.WithDescription($"**Joined**: {jdate}\n**{days.ToString()} days ago**");

                }

                await ReplyAsync("", embed: data.Build());

                
            data.WithColor(new Color(0xB642f4));
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Server Owner";
                x.Value = $"<@{guild.OwnerId.ToString()}>";
            });

            data.AddField(x =>
            {
                var users = guild.Users;
                x.WithIsInline(true);
                x.Name = "Members";

                var userid = string.Join("` `\n", users.Count);

                x.Value = userid;
            });
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "ID";
                x.Value = guild.Id.ToString();
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Region";
                x.Value = guild.VoiceRegionId;
            });


            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Security";
                x.Value = guild.MfaLevel.ToString();
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Verification Level";
                x.Value = guild.VerificationLevel.ToString();
            });
            var rolle = guild.Roles.Count;

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Roles";
                x.Value = rolle.ToString();
            });

            data.WithImageUrl($"https://images.discordapp.net/icons/{guild.Id}/{guild.IconId}.webp?size=1024");

            await ReplyAsync("", embed: data.Build());
        }
        [Command("lsu", RunMode = RunMode.Async)]
        public async Task guildusers(ulong id)
        {
            var client = Context.Client;
            var data = new EmbedBuilder();

            var guild = (await client.GetGuildAsync(id) as SocketGuild);
            var users = guild.Users;
            var userid = string.Join("` `\n", users);

            data.WithDescription(userid);

            await ReplyAsync("", embed: data.Build());
        }




    }


}