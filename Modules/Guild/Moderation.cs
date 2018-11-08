using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jack.Handlers;
using jack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jack.Modules.Guild
{
    [Name("Moderation")]
    public class Moderation : ModuleBase
    {
        [Command("forceban", RunMode = RunMode.Async), Alias("fban"), Summary("Bans a user from the server when the user is not present in the server.")]
        [Remarks("Forceban id")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task ForceBan(ulong id, [Remainder] string reason = "Not specified")
        {
            var author = Context.User as SocketGuildUser;
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var edited = $"{author.Nickname ?? author.Username} forcebaned {id} for " + reason;
            await Context.Guild.AddBanAsync(id, 0, edited);


            await Context.Message.DeleteAsync();
            await ReplyAsync($"{author.Nickname ?? author.Username} Forcebanned {id} for reason {reason}");
        }

        [Command("kick", RunMode = RunMode.Async)]
        [Summary("Kick the specified user."), Remarks("kick user")]
        [RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task Kick(SocketGuildUser user, [Remainder] string reason = "Not Specified")
        {
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var admin = user.Guild.GetChannel(Config.AdminLog.TextChannel) as ITextChannel;
            var User = Context.Message.Author as SocketGuildUser;

            var author = Context.User as IGuildUser;
            var authorsHighestRole = author.RoleIds.Select(x => Context.Guild.GetRole(x)).OrderBy(x => x.Position).First();
            var usersHighestRole = author.RoleIds.Select(x => Context.Guild.GetRole(x)).OrderBy(x => x.Position).First();

            if (usersHighestRole.Position > authorsHighestRole.Position)
            {
                var data = new EmbedBuilder();
                data.WithDescription(":x: You cannot ban someone above you in the role hierarchy.");
                await ReplyAsync("", embed: data.Build());
                return;
            }

            if (!Config.AdminLog.IsEnabled && Config.AdminLog.TextChannel == 0)
            {
                await ReplyAsync($"{User.Nickname ?? User.Username} kicked {user.Username} for {reason}");
                await user.KickAsync(reason);
            }
            else
            {
                ++Config.AdminCases;
                var embed = new EmbedBuilder()
                {
                    Title = "=== User Kicked ===",
                    Description = $"**Case #{Config.AdminCases}**\n{User.Nickname ?? User.Username}Kicked {user.Username}#{user.Discriminator} ({user.Id}) for `{reason}`",
                    Color = new Color(255, 0, 0),
                    ThumbnailUrl = user.GetAvatarUrl(),
                    Timestamp = DateTime.UtcNow
                };
                var edited = $"{User.Nickname ?? User.Username} Kicked {user.Username} for {reason}";
                await user.KickAsync(edited);
                await admin.SendMessageAsync("", embed: embed.Build());
            }
        }
        /*
        [Command("prune", RunMode = RunMode.Async)]
        [Alias("purge", "clean")]
        [Summary("Deletes the messages of one individual")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task DeleteMessages(int number, IUser user = null)
        {
            if (user == null)
            {
                
                await Context.Channel.DeleteMessageAsync(
                (await Context.Channel.GetMessagesAsync(number).Flatten())
                    .Where(x => DateTimeOffset.UtcNow - x.CreatedAt < TimeSpan.FromDays(14)));
            }
            else
            {
                await Context.Channel.DeleteMessagesAsync(
                (await Context.Channel.GetMessagesAsync(number).Flatten())
                    .Where(x => x.Author == user && DateTimeOffset.UtcNow - x.CreatedAt < TimeSpan.FromDays(14)));
            }
        }

        [Command("prune", RunMode = RunMode.Async)]
        [Alias("purge", "clean")]
        [Summary("Deletes the messages multiple users")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task DeleteMsessages(int num, params IUser[] users)
        {

            await Context.Channel.DeleteMessagesAsync(
            (await Context.Channel.GetMessagesAsync(num).Flatten())
                .Where(x => users.Select(y => y.Id).Contains(x.Author.Id)
                    && DateTimeOffset.UtcNow - x.CreatedAt < TimeSpan.FromDays(14)));
        }

        [Command("ahh", RunMode = RunMode.Async)]
        [Summary("Deletes the bot messages")]
        public async Task Deletzsages(int num = 35)
        {
            var channel = Context.Channel as ITextChannel;
            var usr = Context.Guild.GetUserAsync(156558351671623680).Result;
            IUser User = usr;
            var messagesToDelete = await channel.GetMessagesAsync(num + 1).Flatten();
            var usrmsgs = messagesToDelete.Where(msg => msg.Author == User && DateTimeOffset.UtcNow - msg.CreatedAt < TimeSpan.FromDays(14));
            await channel.DeleteMessagesAsync(usrmsgs);
            var author = Context.User as IGuildUser;
        }*/

        [Command("ban", RunMode = RunMode.Async), Alias("permban", "permaban")]
        [Summary("Bans a User"),Remarks("ban user")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task PermaBan([Summary("The user to ban")] IGuildUser user, [Remainder] string reason = "Not Specified")
        {

            var data = new EmbedBuilder();
            data.WithColor(new Color(0x000000));
            var author = Context.User as IGuildUser;
            var authorsHighestRole = author.RoleIds.Select(x => Context.Guild.GetRole(x))
                                                   .OrderBy(x => x.Position)
                                                   .First();
            var usersHighestRole = user.RoleIds.Select(x => Context.Guild.GetRole(x))
                                               .OrderBy(x => x.Position)
                                               .First();

            if (usersHighestRole.Position > authorsHighestRole.Position)
            {
                data.WithDescription(":x: You cannot ban someone above you in the role hierarchy.");
                await ReplyAsync("", embed: data.Build());
                return;
            }
            var edited = $"{author.Nickname ?? author.Username} forcebaned {user.Username} for " + reason;
            await Context.Guild.AddBanAsync(user, 0, edited);
            var name = user.Nickname == null
                ? user.Username
                : $"{user.Username} (nickname: {user.Nickname})";
            var timestamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            var guild = Context.Guild.GetChannelAsync(245680529557422080);
            var chan = (ITextChannel)guild.Result;





            data.WithDescription($"{author.Mention} Banned {name}");
            await Context.Channel.SendMessageAsync("", embed: data.Build());
            await ReplyAsync(":ok_hand:");

        }

        [Command("lock", RunMode = RunMode.Async), Summary("Locks a channel to disallow messages being sent.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task LockChannel(IChannel channel = null)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var chan = channel as SocketGuildChannel ?? Context.Channel as SocketTextChannel;
            var ow = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, embedLinks: PermValue.Deny, attachFiles: PermValue.Deny);
            var everyone = Context.Guild.Roles.First(x => x.Id == x.Guild.Id);
            var c = chan as ITextChannel;
            await c.SendMessageAsync(":lock: Channel Locked.");
            await Context.Message.DeleteAsync();
            await chan.AddPermissionOverwriteAsync(everyone, ow);
            return;


        }

        [Command("unlock", RunMode = RunMode.Async), Summary("Unlocks a channel to allow messages being sent.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task UnlockChannel(IChannel channel = null)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var chan = channel as SocketGuildChannel ?? Context.Channel as SocketTextChannel;
            var everyone = Context.Guild.Roles.First(x => x.Id == x.Guild.Id);
            await chan.RemovePermissionOverwriteAsync(everyone);
            await Context.Message.DeleteAsync();
            var c = chan as ITextChannel;
            await c.SendMessageAsync(":unlock:  Channel UnLocked.");
            return;

        }

        [Command("slock", RunMode = RunMode.Async), Summary("Locks a Server to disallow messages being sent.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task sLockChannel()
        {

            await ReplyAsync(":lock: Server lockdown in progress.");
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var channel = Context.Channel as SocketTextChannel;
            var ow = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, embedLinks: PermValue.Deny, attachFiles: PermValue.Deny);
            var User = Context.Client as SocketGuildUser;
            var everyone = Context.Guild.Roles.First(x => x.Id == x.Guild.Id);
            var guild = Context.Guild as IGuild;
            var channels = await guild.GetChannelsAsync();
            foreach (IGuildChannel chan in channels)
            {
                await chan.AddPermissionOverwriteAsync(everyone, ow);
                await Task.Delay(100);
            }

 
            await Context.Message.DeleteAsync();

            return;


        }

        [Command("sunlock", RunMode = RunMode.Async), Summary("Unlocks a Server to allow messages being sent.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task sUnlockChannel()
        {
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var channel = Context.Channel as SocketTextChannel;
            var User = Context.Client as SocketGuildUser;
            var everyone = Context.Guild.Roles.First(x => x.Id == x.Guild.Id);
            var guild = Context.Guild as IGuild;
            var channels = await guild.GetChannelsAsync();
            foreach (IGuildChannel chan in channels)
            {
                await chan.RemovePermissionOverwriteAsync(everyone);
                await Task.Delay(100);
            }
            await channel.RemovePermissionOverwriteAsync(everyone);
            await Context.Message.DeleteAsync();
            await ReplyAsync(":unlock: Server UnLocked.");
            return;

        }

        [Command("Mute", RunMode = RunMode.Async), Summary("Adds muted role to User"), Remarks("Mute @user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task mute(IUser user, string reason = "Not specified")
        {

            await Context.Message.DeleteAsync();
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var muted = Config.MutedList.Select(x => x.id);

            if (user == null)
            {
                await ReplyAsync("Cant mute someone that dont exist");
                return;
            }

            if (Config.MuteRoleID == 0)
            {
                await ReplyAsync($"No Mute role set up for this server\nUse `{Config.Prefix}SetMuteRole rolename` to set one up");
                return;
            }


            if (muted.Contains(user.Id))
            {
                await ReplyAsync("This person is already muted");
                return;
            }

            var User = (user as SocketGuildUser);
            var mute = new MutedModel()
            {
                date = DateTime.UtcNow.ToString(),
                id = user.Id,
                Name = user.Username,
                reason = reason
            };

            var MuteRole = User.Guild.GetRole(Config.MuteRoleID);

            Config.MutedList.Add(mute);
            await User.AddRoleAsync(MuteRole);
            await User.ModifyAsync(x => x.Mute = true);

            var SUser = Context.Message.Author as SocketGuildUser;
            var xx = user as SocketGuildUser;
            await ReplyAsync($"{SUser.Nickname ?? SUser.Username} successfully muted {xx.Nickname ?? xx.Username} for reason: {reason}");

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        [Command("UnMute", RunMode = RunMode.Async), Summary("Removes muted role from User"), Remarks("UnMute @user")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task UnMute(IUser user)
        {
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var muted = Config.MutedList.Select(x => x.id);
            var muts = Config.MutedList.Where(x => x.id == user.Id).FirstOrDefault();


            if (user == null)
            {
                await ReplyAsync("Cant UnMute someone that dont exist");
                return;
            }

            if (!muted.Contains(user.Id))
            {
                await ReplyAsync("This person is not muted");
                return;
            }

            var User = (user as SocketGuildUser);

            var MuteRole = User.Guild.GetRole(Config.MuteRoleID);
            await User.RemoveRoleAsync(MuteRole);
            await User.ModifyAsync(x => x.Mute = false);


            if (muted.Contains(user.Id))
            {
                Config.MutedList.Remove(muts);
            }



            var SUser = Context.Message.Author as SocketGuildUser;
            var xx = user as SocketGuildUser;
            await ReplyAsync($"{SUser.Nickname ?? SUser.Username} successfully UnMuted {xx.Nickname ?? xx.Username}");

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }
    }
}
