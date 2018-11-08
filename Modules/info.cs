using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using jack;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using Discord.Addons.Preconditions;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using jack.Extensions;
using jack.Models;

namespace jack.Module
{
    [Name("Info")]
    public class info : ModuleBase
    {
        [Command("inrole", RunMode = RunMode.Async)]
        [Summary("shows the amount of users in a role"),Remarks("inrole user")]
        public async Task InRole([Remainder] string roles)
        {
            var embed = new EmbedBuilder();
            if (string.IsNullOrWhiteSpace(roles))
                return;
            var arg = roles.Split(',').Select(r => r.Trim().ToUpperInvariant());

            try
            {
                foreach (var roleStr in arg.Where(str => !string.IsNullOrWhiteSpace(str) && str != "@EVERYONE" && str != "EVERYONE"))
                {
                    var role = Context.Guild.Roles.Where(r => r.Name.ToUpperInvariant() == roleStr).FirstOrDefault();
                    if (role == null) continue;
                    embed.Color = role.Color;
                    embed.AddField(async x =>
                    {
                        x.Name = $"Here is a list of users in the {role.Name} role:";
                        x.Value = string.Join(", ", (await Context.Guild.GetUsersAsync()).Where(u => u.RoleIds.Contains(role.Id)).Select(u => u.Mention));
                    });
                    var count = (await Context.Guild.GetUsersAsync()).Where(u => u.RoleIds.Contains(role.Id)).Select(u => u.Id).Count();
                    if (count > 62)
                    {
                        await ReplyAsync($"Sorry, there are too many users in this role. max 30, current ammount {count}\nif there arent 30 in that role but you still get this message, their bloody username is too big");
                        return;
                    }
                    if (count <= 0)
                    {
                        await ReplyAsync($"No users in that role");
                        return;
                    }
                }

                var usr = Context.User as IGuildUser;

            }
            catch (Exception z)
            {
                if (z.Message == "The server responded with error 400(BadRequest)" || z.Source == "The server responded with error 400(BadRequest)" || z.StackTrace.Contains("The server responded with error 400(BadRequest)"))
                    await ReplyAsync("Too many users in that role");
                return;
            }


            await ReplyAsync("", embed: embed.Build());
        }

        DiscordSocketClient client = new DiscordSocketClient();
        [Command("avatar", RunMode = RunMode.Async)]
        [Summary("Finds users avatar"),Remarks("avatar user")]
        public async Task GetMentionAsync(IUser auser = null)
        {
            var av = auser ?? Context.Message.Author;
            var data = new EmbedBuilder();
            var user = av as SocketUser;


            data.WithColor(new Color(0xB642f4));
            data.WithTitle($"{av?.Username ?? Context.User.Username}'s avatar");
            if (av.AvatarId.StartsWith("a_"))
            {
                data.WithImageUrl($"https://images.discordapp.net/avatars/{av.Id}/{av.AvatarId}.gif?size=1024");
            }
            else
            {
                data.WithImageUrl($"https://images.discordapp.net/avatars/{av.Id}/{av.AvatarId}.png?size=1024");
            }

            await ReplyAsync("", embed: data.Build());
        }


        [Command("ping", RunMode = RunMode.Async), Alias("pong"), Summary("Returns the estimated round-trip latency over the WebSocket.")]
        public async Task Ping()
        {
            ulong target = 0;
            CancellationTokenSource source = new CancellationTokenSource();

            Task WaitTarget(SocketMessage message)
            {
                if (message.Id != target) return Task.CompletedTask;
                source.Cancel();
                return Task.CompletedTask;
            }

            var latency = (Context.Client as DiscordSocketClient).Latency;
            var sw = Stopwatch.StartNew();
            var reply = await ReplyAsync($"Calculating... 10s");
            var init = sw.ElapsedMilliseconds;
            target = reply.Id;
            sw.Restart();
            (Context.Client as DiscordSocketClient).MessageReceived += WaitTarget;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(10), source.Token);
            }
            catch (TaskCanceledException)
            {

                sw.Stop();
                var datas = new EmbedBuilder();
                if (Context.Message.Content.Contains("ping"))
                {
                    datas.WithTitle("PONG!                                                                                        ");
                    datas.WithThumbnailUrl("http://i.imgur.com/Vztzea3.png");
                }
                else
                {
                    if (Context.Message.Content.Contains("pong"))
                    {
                        datas.WithTitle("PING");
                        datas.WithThumbnailUrl("http://i.imgur.com/7Lg7r9Z.png");
                    }
                }
                datas.WithDescription($"Heartbeat: {latency}ms\ninit: {init}ms");


                datas.WithFooter(x =>
                {
                    if (latency < 300)

                    {
                        x.Text = "Fast :D";
                        x.IconUrl = "https://s-media-cache-ak0.pinimg.com/236x/63/62/79/636279b10193e8521a06b6717ebccf14.jpg";
                        datas.WithColor(new Color(0x49ff00));
                    }
                    else
                    {
                        x.Text = "Slow D:";
                        x.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8c/SadSmiley.svg/1024px-SadSmiley.svg.png";
                        datas.WithColor(new Color(0xFF0000));
                    }

                });
                datas.WithTimestamp(DateTime.UtcNow);
                await reply.DeleteAsync();
                await ReplyAsync("Results:", embed: datas.Build());
                return;
            }
            finally
            {
                (Context.Client as DiscordSocketClient).MessageReceived -= WaitTarget;
            }
            sw.Stop();
            var data = new EmbedBuilder();
            if (Context.Message.Content.Contains("ping"))
            {
                data.WithTitle("PONG!                                                                                        ");
                data.WithThumbnailUrl("http://i.imgur.com/Vztzea3.png");
            }
            else
            {
                if (Context.Message.Content.Contains("pong"))
                {
                    data.WithTitle("PING");
                    data.WithThumbnailUrl("http://i.imgur.com/7Lg7r9Z.png");
                }
            }
            data.WithDescription($"Heartbeat: {latency}ms\ninit: {init}ms");


            data.WithFooter(x =>
            {
                if (latency < 300)

                {
                    x.Text = "Fast :D";
                    x.IconUrl = "https://s-media-cache-ak0.pinimg.com/236x/63/62/79/636279b10193e8521a06b6717ebccf14.jpg";
                    data.WithColor(new Color(0x49ff00));
                }
                else
                {
                    x.Text = "Slow D:";
                    x.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8c/SadSmiley.svg/1024px-SadSmiley.svg.png";
                    data.WithColor(new Color(0xFF0000));
                }

            });
            data.WithTimestamp(DateTime.UtcNow);
            await reply.DeleteAsync();
            await ReplyAsync("Results:", embed: data.Build());
        }

        [Command("roles", RunMode = RunMode.Async)]
        [Summary("Finds and posts the users roles"),Remarks("roles user")]
        public async Task sayasync(IUser user = null)
        {
            var guild = (IGuild)Context.Guild;
            var guildss = guild as IGuild;
            var oser = user ?? Context.Message.Author;
            var ruser = oser as IGuildUser;
            StringBuilder builder = new StringBuilder();
            var rolle = ruser.RoleIds.Select(e => guild.GetRole(e).Name);
            var aaa = string.Join("` `", rolle);




            var data = new EmbedBuilder();
            data.WithColor(new Color(0xB642f4));

            data.WithAuthor(x =>
            {
                x.IconUrl = oser.GetAvatarUrl();
                x.Name = $"{oser.Username}'s Roles";
            });

            data.WithDescription($"`{aaa}`");


            await ReplyAsync("", embed: data.Build());
        }

        [Command("roleinfo", RunMode = RunMode.Async), Summary("Returns info about the role."),Remarks("roleinfo rolename")]
        [RequireContext(ContextType.Guild)]
        public async Task Role([Remainder, Summary("The role to return information about.")] string roleName)
        {
            EmbedBuilder embed = new EmbedBuilder();
            StringBuilder builder = new StringBuilder();

            IMessage message = Context.Message;
            IGuild guild = Context.Guild;
            IMessageChannel channel = Context.Channel;
            IReadOnlyCollection<IRole> rolesReadOnly = guild.Roles;
            IRole tgt = null;
            List<IRole> roleList = rolesReadOnly.ToList();

            foreach (IRole role in roleList)
            {
                if (role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                {
                    tgt = role;
                    break;
                }
                else
                {
                    continue;
                }
            }

            if (tgt == null)
            {
                await Context.Channel.SendMessageAsync($"Unable to find role by the name of `{roleName}`.");
                return;
            }
            else
            {
                

                embed.Title = $"{tgt.Name}";
                embed.Color = tgt.Color;
                embed.Footer = new EmbedFooterBuilder()
                {
                    Text = "Role created on " + tgt.CreatedAt.UtcDateTime.ToString()
                };
                embed.Title = "=== ROLE INFORMATION ===";
                embed.Timestamp = DateTime.UtcNow;

                embed.AddField(x =>
                {
                    x.Name = $"Name";
                    x.Value = $"{tgt.Name}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = $"ID";
                    x.Value = $"{tgt.Id}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = $"Seperated";
                    x.Value = $"{tgt.IsHoisted}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = $"Managed";
                    x.Value = $"{tgt.IsManaged}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = $"Mentionable";
                    x.Value = $"{tgt.IsMentionable}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = $"Position";
                    x.Value = $"{tgt.Position}";
                    x.IsInline = true;
                });

                string hex = tgt.Color.R.ToString("X2") + tgt.Color.G.ToString("X2") + tgt.Color.B.ToString("X2");
                embed.AddField(x =>
                {
                    x.Name = $"Color";
                    x.Value = $"RGB: {tgt.Color.R},{tgt.Color.G},{tgt.Color.B}| Hex: {hex}";
                    x.IsInline = true;
                });

                var arg = roleName.Split(',').Select(r => r.Trim().ToUpperInvariant());
                foreach (var roleStr in arg.Where(str => !string.IsNullOrWhiteSpace(str) && str != "@EVERYONE" && str != "EVERYONE"))
                {
                    var role = Context.Guild.Roles.Where(r => r.Name.ToUpperInvariant() == roleStr).FirstOrDefault();
                    if (role == null) continue;

                    embed.AddField(async x =>
                    {
                        x.IsInline = true;
                        x.Name = "Amount of users in this role";
                        x.Value = (await Context.Guild.GetUsersAsync()).Where(u => u.RoleIds.Contains(role.Id)).Select(u => u.Id).Count().ToString();
                    });
                }


                builder.Clear();
                foreach (GuildPermission perm in tgt.Permissions.ToList())
                {
                    builder.Append($"`{perm.ToString()}` ");
                }
                embed.AddField(x =>
                {
                    x.Name = $"Permissions";
                    if (builder.Length != 0)
                    {
                        x.Value = $"{builder.ToString()}";
                    }
                    else
                    {
                        x.Value = "No perms at all";
                    }
                    x.IsInline = false;
                });

                await channel.SendMessageAsync("", false, embed.Build());
            }
        }

        [Command("serverinfo", RunMode = RunMode.Async), Alias("sinfo")]
        [Summary("Shows the server information"),Remarks("serverinfo")]
        public async Task sayxync()
        {
            string mnth1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Context.Guild.CreatedAt.Month);

            String dayStr = DaySufix.DSufix(Context.Guild.CreatedAt.Day);
            var jdate = $"{mnth1} {dayStr}/{Context.Guild.CreatedAt.Year} at {Context.Guild.CreatedAt.ToString("hh:mm tt")}";
            var data = new EmbedBuilder();

            data.WithAuthor(x =>
            {
                x.IconUrl = Context.Guild.IconUrl;
                x.Name = "Server Info";
            });

            data.WithDescription(Context.Guild.Name);

            DateTime date1 = Context.Guild.CreatedAt.DateTime;
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

            data.WithColor(new Color(0xB642f4));
            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Server Owner";
                x.Value = $"<@{Context.Guild.OwnerId.ToString()}>";
            });

            data.AddField(x =>
            {
                var users = Context.Guild.GetUsersAsync();
                x.WithIsInline(true);
                x.Name = "Members";
                x.Value = users.Result.Count.ToString();
            });
            data.AddField(x =>
            {
                var users = Context.Guild.GetUsersAsync();
                x.WithIsInline(true);
                x.Name = "ID";
                x.Value = Context.Guild.Id.ToString();
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Region";
                x.Value = Context.Guild.VoiceRegionId;
            });


            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Security";
                x.Value = Context.Guild.MfaLevel.ToString();
            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Verification Level";
                x.Value = Context.Guild.VerificationLevel.ToString();
            });
            var rolle = Context.Guild.Roles.Count;

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = "Roles";
                x.Value = rolle.ToString();
            });

            data.WithImageUrl($"https://images.discordapp.net/icons/{Context.Guild.Id}/{Context.Guild.IconId}.webp?size=1024");

            await ReplyAsync("", embed: data.Build());
        }


        [Command("time")]
        [Summary("Shows the persons time on the server"), Remarks("time user")]
        public async Task GetMensnc(IUser user = null)
        {

            try
            {
                var User = user ?? Context.Message.Author;
                var joined = (User as SocketGuildUser);
                var data = new EmbedBuilder();

                DateTime date1 = joined.JoinedAt.Value.Date;
                DateTime date2 = DateTime.UtcNow.Date;

                var tim = (date1);
                string mnth1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(joined.JoinedAt.Value.Month);
                String dayStr = DaySufix.DSufix(joined.JoinedAt.Value.Day);
                var jdate = $"{mnth1} {dayStr}/{joined.JoinedAt.Value.Year} at {joined.JoinedAt.Value.ToString("hh:mm tt")}";

                data.WithColor(new Color(0xB642f4));
                data.WithAuthor(x =>
                {
                    x.WithIconUrl(User.GetAvatarUrl());
                    x.Name = $"{User.Username}'s Time on the server";
                });


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

            }
            catch (Exception)
            {
                var time = user ?? Context.Message.Author;
                var joined = (time as SocketGuildUser);
                var data = new EmbedBuilder();

                DateTime date1 = joined.JoinedAt.Value.Date;
                DateTime date2 = DateTime.UtcNow.Date;
                DateTime date3 = joined.JoinedAt.Value.Date;

                var tim = (date3);
                string mnth1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(joined.JoinedAt.Value.Month);

                var jdate = $"{mnth1} {joined.JoinedAt.Value.Day}/{joined.JoinedAt.Value.Year} at {joined.JoinedAt.Value.Hour}:{joined.JoinedAt.Value.Minute}";

                data.WithColor(new Color(0xB642f4));
                data.WithAuthor(x =>
                {
                    x.WithIconUrl(time.GetAvatarUrl());
                    x.Name = $"{time.Username}'s Time on the server";
                });

                data.AddField(x =>
                {
                    x.Name = "Joined";
                    x.Value = jdate;
                });

                data.AddField(x =>
                {
                    x.Name = " ";
                    x.Value = "Joined Today";
                });
                await ReplyAsync("", embed: data.Build());

            }
        }

        [Command("userinfo", RunMode = RunMode.Async)]
        [Summary("Shows users info"),Remarks("Userinfo user")]
        public async Task GetMzzzzzzzentionAsync(IUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;
            var data = new EmbedBuilder();
            var joined = userInfo as SocketGuildUser;
            var role = userInfo as IGuildUser;
            var guild = user as IGuild;

            var rell = role.RoleIds.Select(e => guild.GetRole(e).Name);

            
            String dayaaa = DaySufix.DSufix(userInfo.CreatedAt.Day);
            String dayStr = DaySufix.DSufix(joined.JoinedAt.Value.Day);
            string mnth1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(joined.JoinedAt.Value.Month);
            string mnth2 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(userInfo.CreatedAt.Month);
            var cdate = $"{mnth2} {dayaaa}/{userInfo.CreatedAt.Year}";
            var jdate = $"{mnth1} {dayStr}/{joined.JoinedAt.Value.Year}";
            data.WithColor(new Color(0xB642f4));
            data.WithTitle($"{userInfo.Username}'s info");

            data.WithThumbnailUrl(userInfo.GetAvatarUrl());
            data.WithDescription($"**Joined this server** {jdate} {joined.JoinedAt.Value.ToString("hh:mm tt")}\n**Joined Discord** {cdate} {userInfo.CreatedAt.ToString("hh:mm tt")}\n\n**Name**: {userInfo.Username}#{userInfo.Discriminator}\n**ID**:{userInfo.Id}");


            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = $"Status";
                if (userInfo.Status == UserStatus.Offline | userInfo.Status == UserStatus.Invisible)
                {
                    x.Value = $"Offline/Invis";

                }
                else
                {
                    x.Value = userInfo.Status.ToString();
                }

            });

            data.AddField(x =>
            {
                x.WithIsInline(true);
                x.Name = $"Nic";
                if ((userInfo as IGuildUser).Nickname == null)
                {
                    x.Value = "No Nic";
                }
                else
                {
                    x.Value = $"{(userInfo as IGuildUser).Nickname}";
                }

            });

            
            await ReplyAsync("", embed: data.Build());
        }

        [Command("weather", RunMode = RunMode.Async)]
        [Summary("Get's weather info for specified city")]
        public async Task sayasync([Remainder]string query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                await ReplyAsync("Kinda need a place to find weather for @_@");
                return;
            }


            string response;
            using (var http = new HttpClient())
                response = await http.GetStringAsync($"http://api.openweathermap.org/data/2.5/weather?q={query}&appid=42cd627dd60debf25a5739e50a217d74&units=metric").ConfigureAwait(false);

            var data = JsonConvert.DeserializeObject<WeatherData>(response);

            var embed = new EmbedBuilder()
                .AddField(fb => fb.WithName("**Country**🗾").WithValue(data.name + ", " + data.sys.country).WithIsInline(true))
                .AddField(fb => fb.WithName("**Lat,Long**🗺").WithValue($"{data.coord.lat}, {data.coord.lon}").WithIsInline(true))
                .AddField(fb => fb.WithName("**Condition**🚧").WithValue(String.Join(", ", data.weather.Select(w => w.main))).WithIsInline(true))
                .AddField(fb => fb.WithName("**Humidity**☔").WithValue($"{data.main.humidity}%").WithIsInline(true))
                .AddField(fb => fb.WithName("**Wind Speed**🚩").WithValue(data.wind.speed + " km/h").WithIsInline(true))
                .AddField(fb => fb.WithName("**Temperature**🌡").WithValue(data.main.temp + "°C").WithIsInline(true))
                .AddField(fb => fb.WithName("**Min Temp - Max Temp**🌡").WithValue($"{data.main.temp_min}°C - {data.main.temp_max}°C").WithIsInline(true))
                .AddField(fb => fb.WithName("**Sunrise (utc)**⏱").WithValue($"{data.sys.sunrise.ToUnixTimestamp():HH:mm}").WithIsInline(true))
                .AddField(fb => fb.WithName("**Sunset (utc)**⏲").WithValue($"{data.sys.sunset.ToUnixTimestamp():HH:mm}").WithIsInline(true))
                .WithColor(new Color(0xB642f4))
                .WithFooter(efb => efb.WithText("Powered by http://openweathermap.org"));
            await ReplyAsync("", embed: embed.Build());
        }
    }
    public static class aaaaaaa
    {
        public static DateTime ToUnixTimestamp(this double number) => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(number);
    }

}