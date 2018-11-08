using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jack.Enums;
using jack.Extensions;
using jack.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jack.Modules.Guild
{
    [Name("Guild Module's")]
    public class GuildModule : ModuleBase
    {
        [Command("Prefix", RunMode = RunMode.Async), Summary("Sets guild prefix"), Remarks("Prefix .")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task SetPrefixAsync(string prefix)
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            gldConfig.Prefix = prefix;
            GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync($"Guild Prefix has been set to: **{prefix}**");
        }

        [Command("Prefix", RunMode = RunMode.Async), Summary("show's guild prefix"), Remarks("Prefix")]
        public async Task PrefixAsync()
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];

            await ReplyAsync($"Guild Prefix is: **{gldConfig.Prefix}**");
        }

        [Command("Welcome", RunMode = RunMode.Async),
            Summary("Sets a welcome message for your server."),
            Remarks("Welcome hi")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task WelcomeMessageAsync([Remainder]string msg = null)
        {


            var Guild = Context.Guild as SocketGuild;
            
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            if (msg == null)
            {
                await ReplyAsync("Remember that you can use {user} to get the username and {guild} for server name\nCurrent Welcome message for this guild\n" + gldConfig.WelcomeMessages);
            }
            else
            {
                gldConfig.WelcomeMessages = msg;
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await ReplyAsync($"Guild Welcome Message has been set to:\n```{msg}```");
            }
        }

        [Command("Leave", RunMode = RunMode.Async),
            Summary("Sets a Leaave message for your server."),
            Remarks("Leave MSG")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task LeaveMessageAsync([Remainder]string msg = null)
        {

            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            if (msg == null)
            {
                await ReplyAsync("Remember that you can use {user} to get the username and {guild} for server name\nCurrent Leave message for this guild\n"+gldConfig.WelcomeMessages);
            }
            else
            {
                gldConfig.LeaveMessages = msg;
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await ReplyAsync($"Guild Leave Message has been set to:\n```{msg}```");
            }
                
        }

        [Command("Settings", RunMode = RunMode.Async), Summary("Displays all settings for your Guild.")]
        public async Task SettingsAsync()
        {
            var SB = new StringBuilder();
            var SD = new StringBuilder();
            var GConfig = GuildHandler.GuildConfigs[Context.Guild.Id];

            string AFKList = null;
            if (GConfig.AFKList.Count <= 0)
                AFKList = $"{Context.Guild.Name}'s AFK list is empty.";
            else
                AFKList = $"{Context.Guild.Name}'s AFK list contains {GConfig.AFKList.Count} members.";

            string TagList = null;
            if (GConfig.TagsList.Count <= 0)
                TagList = $"{Context.Guild.Name}'s Tag list is empty.";
            else
                TagList = $"{Context.Guild.Name}'s Tag list contains {GConfig.MutedList.Count} tags.";

            string MuteList = null;
            if (GConfig.TagsList.Count <= 0)
                MuteList = $"{Context.Guild.Name}'s Mute list is empty.";
            else
                MuteList = $"{Context.Guild.Name}'s Mute list contains {GConfig.MutedList.Count.ToString()} user's.";

            /*string Roles = null;
            if (GConfig.AssignableRoles.Count <= 0)
                Roles = $"There are no assignable roles for {Context.Guild.Name}.";
            else
                Roles = $"{Context.Guild.Name}'s has {GConfig.AssignableRoles.Count} assignable roles!";*/

            var Joins = GConfig.JoinEvent.IsEnabled ? "Enabled" : "Disabled";
            var Leaves = GConfig.LeaveEvent.IsEnabled ? "Enabled" : "Disabled";
            var Bans = GConfig.AdminLog.IsEnabled ? "Enabled" : "Disabled";
            var joinrole = GConfig.JoinRole.IsEnabled ? "Yes" : "No";


            SocketGuildChannel JoinChannel;
            SocketGuildChannel LeaveChannel;
            SocketGuildChannel BanChannel;




            if (GConfig.WelcomeMessages == null)
            {
                SB = SB.Append("Guild has no welcome message!");
            }
            else
            {
                SB = SB.Append(GConfig.WelcomeMessages.ToString());
            }

            if (GConfig.LeaveMessages == null)
            {
                SD = SD.Append("Guild has no Leave message!");
            }
            else
            {
                SD = SD.Append(GConfig.LeaveMessages.ToString());
            }


            if (GConfig.JoinEvent.TextChannel != 0 || GConfig.LeaveEvent.TextChannel != 0 || GConfig.AdminLog.TextChannel != 0)
            {
                JoinChannel = await Context.Guild.GetChannelAsync(GConfig.JoinEvent.TextChannel) as SocketGuildChannel;
                LeaveChannel = await Context.Guild.GetChannelAsync(GConfig.LeaveEvent.TextChannel) as SocketGuildChannel;
                BanChannel = await Context.Guild.GetChannelAsync(GConfig.AdminLog.TextChannel) as SocketGuildChannel;

            }
            else
            {
                JoinChannel = null;
                LeaveChannel = null;
                BanChannel = null;

            }


            string Description = $"**Prefix:** {GConfig.Prefix}\n" +
                $"**Mute Role:** {GConfig.MuteRoleID}\n" +
                $"**JoinRole ID**{GConfig.JoinRole.roleid}\n" +
                $"**Admin Cases:** {GConfig.AdminCases}\n" +
                $"**Ban Logging:** {Bans} [{BanChannel}]\n" +
                $"**Join Logging:** {Joins} [{JoinChannel}]\n" +
                $"**Leave Logging:** {Leaves} [{LeaveChannel}]\n" +
                $"**Admin/Log Channel:** [{Context.Guild.GetChannelAsync(GConfig.AdminLog.TextChannel).Result}]\n" +
                $"**Card Welcome** {GConfig.Error.OnOff}\n"+

                $"**Join Role Enabled?:** {joinrole}\n" +
                $"**Welcome Message:**\n{SB.ToString()}\n" +
                $"**Leave Message:**\n{SD.ToString()}\n" +
                //$"**AFK List:** {AFKList}\n" +
                //$"**Tags List** {TagList}\n" +
                $"**Mute List:** {MuteList}\n";
                

            var embed = EmbedExtension.Embed(EmbedColors.Teal, Context.Guild.Name, Context.Guild.IconUrl, Description: Description, ThumbUrl: Context.Guild.IconUrl);
            await ReplyAsync("", embed: embed.Build());
        }


        [Command("SetChannel", RunMode = RunMode.Async), Summary("Sets channel for events/logs"), Remarks("SetChannel AdminChannel #Channelname")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task SetChannelAsync(GlobalEnums ConfigChannel = GlobalEnums.none, SocketGuildChannel Channel = null)
        {
            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            if (ConfigChannel == GlobalEnums.none && Channel == null)
            {
                await ReplyAsync($"Usage: {Config.Prefix}setchannel AdminChannel #channelname\nChannels you can set\n`AdminChannel`,`JoinChannel`,`LeaveChannel`");
            }

            switch (ConfigChannel)
            {
                case GlobalEnums.AdminChannel:
                    Config.AdminLog.TextChannel = Channel.Id;
                    await ReplyAsync($"Admin log channel has been set to: **{Channel.Name}**");
                    break;

                case GlobalEnums.JoinChannel:
                    Config.JoinEvent.TextChannel = Channel.Id;
                    await ReplyAsync($"Join log channel has been set to: **{Channel.Name}**");
                    break;

                case GlobalEnums.LeaveChannel:
                    Config.LeaveEvent.TextChannel = Channel.Id;
                    await ReplyAsync($"Leave log channel has been set to: **{Channel.Name}**");
                    break;

            }

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        [Command("SetMuteRole"), Summary("set's up a mute role"), Remarks("setmuterole rolename")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task muterole(IRole role = null)
        {
            if (role == null)
            {
                await ReplyAsync("you must make also add a name of a role that you are going to use as muterole\nDo NOT include the @ of the role. just the name\n i'll take care of the rest\nAlso, Remember to drag the role above the other roles you want it to affect, but dont drag it over jackbot's role, or he cant add it to others");
                return;
            }

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var guild = Context.Guild as IGuild;
            var channels = await guild.GetChannelsAsync();
            GuildPermissions perms = GuildPermissions.None;
            perms = guild.EveryoneRole.Permissions.Modify(addReactions: false, sendMessages: false, embedLinks: false, attachFiles: false);
            await role.ModifyAsync(z => z.Permissions = perms);
            IRole muteRole = null;

            muteRole = role;
            Config.MuteRoleID = role.Id;
            var ow = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, embedLinks: PermValue.Deny, attachFiles: PermValue.Deny);
            foreach (IGuildChannel chan in channels)
            {
                await chan.AddPermissionOverwriteAsync(muteRole, ow);
                await Task.Delay(100);
            }

            await ReplyAsync("Mute Role was Successfully set up");

            
            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        [Command("SetJoinRole"), Summary("adds join role"), Remarks("setjoinrole rolename")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task joinrole(IRole role = null)
        {
            if (role == null)
            {
                await ReplyAsync("give me a rolename that is in the guild, only 1 can be joinrole atm\nMake sure jackbot's role is above the join role");
                return;
            }

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];
            var guild = Context.Guild as IGuild;

            Config.JoinRole.roleid = role.Id;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        [Group("Toggle")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public class ToggleModule : ModuleBase
        {

            [Command,Summary("show's all you can Toggle, use toggle before every command below this one")]
            public async Task toggle()
            {
                await ReplyAsync("`Errors`\n`AntInv`\n`bans`\n`joins`\n`leaves`\n`Joinrole`");
            }

            [Command("Card", RunMode = RunMode.Async), Summary("Toggles Welcome card, will overwrite welcome msg"), Remarks("Toggle Card")]
            public async Task toggleErrorasync()
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];


                if (!gldConfig.Error.OnOff)
                {
                    gldConfig.Error.OnOff = true;
                    gldConfig.textwelcome.OnOff = false;

                    await ReplyAsync(":gear: Card Enabled!");
                    await ReplyAsync(":skull_crossbones: welcome/leave text Disabled.");
                }
                else
                {

                    gldConfig.Error.OnOff = false;
                    await ReplyAsync(":skull_crossbones: Card Disabled.");

                }

                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);

            }

            [Command("text", RunMode = RunMode.Async), Summary("Toggles Welcome text, will overwrite welcome card"), Remarks("Toggle text")]
            public async Task toggleErzrasync()
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];


                if (!gldConfig.textwelcome.OnOff)
                {
                    gldConfig.textwelcome.OnOff = true;
                    gldConfig.Error.OnOff = false;

                    await ReplyAsync(":gear: welcome/leave text Enabled!");
                    await ReplyAsync(":skull_crossbones: Card Disabled.");
                }
                else
                {

                    gldConfig.textwelcome.OnOff = false;
                    await ReplyAsync(":skull_crossbones: welcome/leave text Disabled.");

                }

                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);

            }

            [Command("Joinrole", RunMode = RunMode.Async), Summary("Toggles joinroles on or off"), Remarks("Toggle joinrole")]
            public async Task togglejoinroleasync()
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];


                if (!gldConfig.JoinRole.IsEnabled)
                {
                    gldConfig.JoinRole.IsEnabled = true;


                    await ReplyAsync(":gear: joinrole enable!");
                }
                else
                {

                    gldConfig.JoinRole.IsEnabled = false;
                    await ReplyAsync(":skull_crossbones: Joinrole Disabled.");

                }

                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);

            }

            [Command("AntInv", RunMode = RunMode.Async), Summary("Enables/Disables NoInvites. If user posts an invite link it will be removed"),Remarks("Toggle antinv")]
            public async Task ToggleAntInv()
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
                if (!gldConfig.NoInvites)
                {
                    gldConfig.NoInvites = true;
                    await ReplyAsync(":gear: Anti Invites has now been enabled!");
                }
                else
                {
                    gldConfig.NoInvites = false;
                    await ReplyAsync(":skull_crossbones: Anti Invites has been disabled!.");
                }
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            }

            [Command("Bans", RunMode = RunMode.Async), Summary("Enables/Disables logging admin actions such as Kick/Ban."), Remarks("Toggle bans")]
            public async Task ToggleBansAsync()
            {
                var Guild = Context.Guild as SocketGuild;
                var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
                if (!gldConfig.AdminLog.IsEnabled)
                {
                    gldConfig.AdminLog.IsEnabled = true;
                    await ReplyAsync(":gear:   Now logging bans.");
                }
                else
                {
                    gldConfig.AdminLog.IsEnabled = false;
                    await ReplyAsync(":skull_crossbones:  No longer logging bans.");
                }
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            }

            [Command("Joins", RunMode = RunMode.Async), Summary("Enables/Disables logging joins."), Remarks("Toggle joins")]
            public async Task ToggleJoinsAsync()
            {
                var gldConfig = GuildHandler.GuildConfigs[Context.Guild.Id];
                if (!gldConfig.JoinEvent.IsEnabled)
                {
                    gldConfig.JoinEvent.IsEnabled = true;
                    await ReplyAsync(":gear: Joins logging enabled!");
                }
                else
                {
                    gldConfig.JoinEvent.IsEnabled = false;
                    await ReplyAsync(":skull_crossbones:   No longer logging joins.");
                }
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            }

            [Command("Leaves", RunMode = RunMode.Async), Summary("Enables/Disables logging leaves."), Remarks("Toggle leaves")]
            public async Task ToggleLeavesAsync()
            {
                var gldConfig = GuildHandler.GuildConfigs[Context.Guild.Id];
                if (!gldConfig.LeaveEvent.IsEnabled)
                {
                    gldConfig.LeaveEvent.IsEnabled = true;
                    await ReplyAsync(":gear:   Now logging leaves.");
                }
                else
                {
                    gldConfig.LeaveEvent.IsEnabled = false;
                    await ReplyAsync(":skull_crossbones:  No longer logging leaves.");
                }
                GuildHandler.GuildConfigs[Context.Guild.Id] = gldConfig;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            }
        }
    }
}
