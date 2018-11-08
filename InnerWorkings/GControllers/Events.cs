using Discord;
using Discord.WebSocket;
using jack.Enums;
using jack.Extensions;
using jack.Functions;
using jack.Handlers;
using jack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jack.GControllers
{
    public class Events
    {
        public static async Task UserJoinedAsync(SocketGuildUser User)
        {
            var Config = GuildHandler.GuildConfigs[User.Guild.Id];
            if (!Config.JoinEvent.IsEnabled && Config.Error.OnOff == false) return;

            ITextChannel Channel = null;
            string WelcomeMessage = null;

            var JoinChannel = User.Guild.GetChannel(Config.JoinEvent.TextChannel);

            if (Config.WelcomeMessages == null)
                WelcomeMessage = $"{User.Mention} just joined {User.Guild.Name}! WELCOME!";


            if (User.Guild.GetChannel(Config.JoinEvent.TextChannel) != null && Config.Error.OnOff == false)
            {
                Channel = JoinChannel as ITextChannel;
                var replace = StringExtension.ReplaceWith(Config.WelcomeMessages, User.Mention, User.Guild.Name);
                if (Config.textwelcome.OnOff == true)
                {
                   
                    WelcomeMessage = replace;
                    await Channel.SendMessageAsync(WelcomeMessage);
                    return;
                }
                else
                {

                    
                    WelcomeMessage = replace;
                    var embed = new EmbedBuilder()
                    {
                        Title = "=== User Joined ===",
                        Description = $"{WelcomeMessage}",
                        Color = new Color(0xAD33FF),
                        ThumbnailUrl = User.GetAvatarUrl(),
                        Timestamp = DateTime.UtcNow
                    };
                    await Channel.SendMessageAsync("", embed: embed.Build());
                    return;

                }
                
            }
            else if (User.Guild.GetChannel(Config.JoinEvent.TextChannel) == null && Config.Error.OnOff == false)
            {
                var replace = StringExtension.ReplaceWith(Config.WelcomeMessages, User.Mention, User.Guild.Name);
                Channel = User.Guild.DefaultChannel as ITextChannel;
                if (Config.textwelcome.OnOff == true)
                {
                    WelcomeMessage = replace;
                    await Channel.SendMessageAsync(WelcomeMessage);
                    return;
                }
                else
                {
                    WelcomeMessage = replace;
                    var embed = new EmbedBuilder()
                    {
                        Title = "=== User Joined ===",
                        Description = $"{WelcomeMessage}",
                        Color = new Color(0xAD33FF),
                        ThumbnailUrl = User.GetAvatarUrl(),
                        Timestamp = DateTime.UtcNow
                    };
                    await Channel.SendMessageAsync("", embed: embed.Build());
                    return;
                }
            }
            return;
        }

        public static async Task UserLeftAsync(SocketGuildUser User)
        {
            await CleanUpAsync(User);

            var Config = GuildHandler.GuildConfigs[User.Guild.Id];
            if (!Config.LeaveEvent.IsEnabled) return;

            ITextChannel Channel = null;
            string LeaveMessages = null;

            var LeaveChannel = User.Guild.GetChannel(Config.LeaveEvent.TextChannel);

            if (Config.LeaveMessages == null)
                LeaveMessages = $"{User.Mention} just left {User.Guild.Name} :wave:";



            if (User.Guild.GetChannel(Config.LeaveEvent.TextChannel) != null)
            {
                Channel = LeaveChannel as ITextChannel;
                var replace = StringExtension.ReplaceWith(Config.LeaveMessages, User.Username + "#" + User.Discriminator, User.Guild.Name);

                if (Config.textwelcome.OnOff == true)
                {
                    LeaveMessages = replace;
                    await Channel.SendMessageAsync(LeaveMessages);
                    return;
                }
                else
                {
                    LeaveMessages = replace;
                    var embed = new EmbedBuilder()
                    {
                        Title = "=== User Left ===",
                        Description = $"{LeaveMessages}",
                        Color = new Color(255, 0, 0),
                        ThumbnailUrl = User.GetAvatarUrl(),
                        Timestamp = DateTime.UtcNow
                    };
                    
                    await Channel.SendMessageAsync("", embed: embed.Build());
                    return;
                }
            }
            else
            {
                Channel = User.Guild.DefaultChannel as ITextChannel;
                var replace = StringExtension.ReplaceWith(Config.LeaveMessages, User.Username + "#" + User.Discriminator, User.Guild.Name);

                if (Config.textwelcome.OnOff == true)
                {
                    LeaveMessages = replace;
                    await Channel.SendMessageAsync(LeaveMessages);
                    return;
                }
                else
                {
                    LeaveMessages = replace;
                    var embed = new EmbedBuilder()
                    {
                        Title = "=== User Left ===",
                        Description = $"{LeaveMessages}",
                        Color = new Color(255, 0, 0),
                        ThumbnailUrl = User.GetAvatarUrl(),
                        Timestamp = DateTime.UtcNow
                    };

                    await Channel.SendMessageAsync("", embed: embed.Build());
                    return;
                }
            }
            
        }

        public static async Task OnUserBanned(SocketUser User, SocketGuild Guild)
        {
            var Config = GuildHandler.GuildConfigs[Guild.Id];
            if (!Config.AdminLog.IsEnabled && Config.AdminLog.TextChannel == 0) return;
            var AdminChannel = Guild.GetChannel(Config.AdminLog.TextChannel) as ITextChannel;

            var embed = new EmbedBuilder()
            {
                Title = "=== User Banned ===",
                Description = $"**Case #{Config.AdminCases}**\n{User.Username}#{User.Discriminator} ({User.Id}) was banned from {Guild.Name}",
                Color = new Color(0xE9287D),
                ThumbnailUrl = User.GetAvatarUrl(),
                Timestamp = DateTime.UtcNow
            };
            ++Config.AdminCases;
            await AdminChannel.SendMessageAsync("", embed: embed.Build());
        }

        public static async Task OnUserUnBanned(SocketUser User, SocketGuild Guild)
        {
            var Config = GuildHandler.GuildConfigs[Guild.Id];
            if (!Config.AdminLog.IsEnabled && Config.AdminLog.TextChannel == 0) return;
            var AdminChannel = Guild.GetChannel(Config.AdminLog.TextChannel) as ITextChannel;
            ++Config.AdminCases;

            var embed = new EmbedBuilder()
            {
                Title = "=== User UnBanned ===",
                Description = $"**Case #{Config.AdminCases}**\n{User.Username}#{User.Discriminator} ({User.Id}) was UnBanned from {Guild.Name}",
                Color = new Color(0xE9287D),
                ThumbnailUrl = User.GetAvatarUrl(),
                Timestamp = DateTime.UtcNow
            };

            await AdminChannel.SendMessageAsync("", embed: embed.Build());
        }

        internal static async Task DeleteGuildConfig(SocketGuild Guild)
        {
            if (GuildHandler.GuildConfigs.ContainsKey(Guild.Id))
            {
                GuildHandler.GuildConfigs.Remove(Guild.Id);
            }
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        internal static async Task HandleGuildConfigAsync(SocketGuild Guild)
        {
            var CreateConfig = new GuildModel();
            if (!GuildHandler.GuildConfigs.ContainsKey(Guild.Id))
            {
                GuildHandler.GuildConfigs.Add(Guild.Id, CreateConfig);
            }
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        internal static async Task HandleGuildMessagesAsync(SocketMessage Message)
        {
            var Guild = (Message.Channel as SocketGuildChannel).Guild;
            await AFKHandlerAsync(Guild, Message);
            await AntiAdvertisementAsync(Guild, Message);
        }

        #region Event Methods
        static async Task CleanUpAsync(SocketGuildUser User)
        {
            var GuildConfig = GuildHandler.GuildConfigs[User.Guild.Id];
            
            if (GuildConfig.AFKList.ContainsKey(User.Id))
            {
                GuildConfig.AFKList.Remove(User.Id);
                Logger.Log(LogType.Warning, LogSource.Configuration, $"{User.Username} removed from {User.Guild.Name}'s AFK List.");
            }
            foreach (var tag in GuildConfig.TagsList)
            {
                if (tag.Owner == User.Id)
                {
                    GuildConfig.TagsList.Remove(tag);
                    Logger.Log(LogType.Warning, LogSource.Configuration, $"Removed {tag.Name} by {User.Username}.");
                }
            }

            GuildHandler.GuildConfigs[User.Guild.Id] = GuildConfig;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }

        
        static async Task AFKHandlerAsync(SocketGuild Guild, SocketMessage Message)
        {
            var AfkList = GuildHandler.GuildConfigs[Guild.Id].AFKList;
            string afkReason = null;
            SocketUser gldUser = Message.MentionedUsers.FirstOrDefault(u => AfkList.TryGetValue(u.Id, out afkReason));
            if (gldUser != null)
                await Message.Channel.SendMessageAsync($"**Message left from {gldUser.Username}:** {afkReason}");
        }

        
        static async Task AntiAdvertisementAsync(SocketGuild Guild, SocketMessage Message)
        {
            var Config = GuildHandler.GuildConfigs[Guild.Id];
            if (!Config.NoInvites || Guild == null) return;
            if (Functions.Funciton.Advertisement(Message.Content))
            {
                await Message.DeleteAsync();
            }
        }
        
        public static async Task JoinedGuildAsync(SocketGuild Guild)
        {
            var CreateConfig = new GuildModel();
            if (!GuildHandler.GuildConfigs.ContainsKey(Guild.Id))
            {
                GuildHandler.GuildConfigs.Add(Guild.Id, CreateConfig);
            }
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
        }
        #endregion
    }
}
