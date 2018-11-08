using Discord;
using Discord.Commands;
using jack.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jack.Modules.Guild
{
    [Group("card")]
    [RequireUserPermission(GuildPermission.ManageChannels)]
    public class Card : ModuleBase
    {
        [Command, Summary("show's card dimmensions"), Remarks("card")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task blank()
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            var Height = Config.callcard.ImageHeight;
            var Width = Config.callcard.ImageWidth;

            await ReplyAsync($"Card Dimmensions:\nWidth: {Width}\nHeight: {Height}");
            return;


        }

        [Command("Background"), Alias("bg"), Summary("Adds Card BG"), Remarks("Background link")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bg(string url)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.CardBg = url;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Card Background set");
            return;


        }

        [Command("UserColor"), Alias("uc"), Summary("Changes Username color on card"), Remarks("Card UserColor #Hex")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bhex(string hex)
        {
            if (!hex.Contains("#") || hex.Length > 7)
            {
                await ReplyAsync("Has a color hex starting with #");
                return;
            }
            else
            {
                var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


                Config.callcard.UserNameColor = hex;

                GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await ReplyAsync("Card UserNameColor set");
            }
        }

        [Command("Userpos"), Alias("upos"), Summary("Changes Username position on card"), Remarks("Card Userpos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task buserposhex(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.UnamePosX = X;
            Config.callcard.UnamePosY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("UserName position set");

        }

        [Command("Usersize"), Alias("usize"), Summary("Changes Username size on card"), Remarks("Card Usersize s")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task buserpzex(int s)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.UnameSize = s;


            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("UserName position set");

        }

        [Command("InfoColor"), Alias("ifc"), Summary("Changes Info color on card"), Remarks("Card infocolor #Hex")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bshex(string hex)
        {
            if (!hex.Contains("#") || hex.Length > 7)
            {
                await ReplyAsync("Has a color hex starting with #");
                return;
            }
            else
            {
                var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


                Config.callcard.descriptioncolor = hex;

                GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await ReplyAsync("Card info Color set");
            }
        }

        [Command("avatarpos"), Alias("avpos"), Summary("Changes position of avatar on card"), Remarks("Card avatarpos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssex(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.avatarposX = X;
            Config.callcard.avatarposY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Avatar Position set");

        }

        [Command("avatarsize"), Alias("avsize"), Summary("Changes size of avatar on card"), Remarks("Card avatarsize W H")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssesx(int W, int H)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.avatarWidth = W;
            Config.callcard.avatarHeight = H;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Avatar Size set");

        }

        [Command("size"), Summary("Changes size of the card"), Remarks("Card size W H")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssesjjjx(int W, int H)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.ImageWidth = W;
            Config.callcard.ImageHeight = H;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Card size set");

        }

        [Command("welcomesize"), Summary("Changes welcome size on the card"), Remarks("Card welcomesize s")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssessjjjx(int s)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.WelcomeSize = s;
            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("welcome Size set");

        }

        [Command("welcomepos"), Summary("Changes welcome position on the card"), Remarks("Card welcomepos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssssssssessjjjx(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            Config.callcard.WelcomeposX = X;
            Config.callcard.WelcomeposY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("welcome Position set");

        }

        [Command("welcomecolor"), Summary("Changes welcome color on the card"), Remarks("Card welcomecolor #hex")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssssssessjjjx(string hex)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            if (!hex.Contains("#") || hex.Length > 7)
            {
                await ReplyAsync("Has a color hex starting with #");
                return;
            }
            else
            {
                Config.callcard.WelcomeColor = hex;

                GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await ReplyAsync("Card welcome Color set");
            }
        }

        [Command("serversize"), Summary("Changes servername size on the card"), Remarks("Card serversize s")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bzsessjjjx(int s)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.SnameSize = s;
            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("servername Size set");

        }

        [Command("serverpos"), Summary("Changes servername position on the card"), Remarks("Card serverpos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssssssszssjjjx(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            Config.callcard.SnameposX = X;
            Config.callcard.SnameposY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Servername Position set");

        }


        [Command("discrimsize"), Summary("Changes discriminator size on the card"), Remarks("Card discrimsize s")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task zzzzzz(int s)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.DiscrimSize = s;
            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("discriminator Size set");

        }

        [Command("discrimpos"), Summary("Changes discriminator position on the card"), Remarks("Card discrimpos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bssssssszzzzssjjjx(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            Config.callcard.DiscrimPosX = X;
            Config.callcard.DiscrimPosY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("discriminator Position set");

        }

        [Command("idize"), Summary("Changes ID size on the card"), Remarks("Card id s")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task zzzzxxzz(int s)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];


            Config.callcard.IdSize = s;
            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("ID Size set");

        }

        [Command("idpos"), Summary("Changes ID position on the card"), Remarks("Card idpos X Y")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task bsssszzzzzxxxsszzzzssjjjx(int X, int Y)
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            Config.callcard.IdPosX = X;
            Config.callcard.IdPosY = Y;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("discriminator Position set");

        }

        [Command("reset"), Summary("Reset's card to default"), Remarks("Card reset")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task reset()
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            Config.callcard.IdSize = 22;
            Config.callcard.IdPosX = 190;
            Config.callcard.IdPosY = 125;
            Config.callcard.DiscrimPosX = 680;
            Config.callcard.DiscrimPosY = 125;
            Config.callcard.DiscrimSize = 22;
            Config.callcard.SnameposX = 190;
            Config.callcard.SnameposY = 50;
            Config.callcard.SnameSize = 27;
            Config.callcard.WelcomeColor = "#000000";
            Config.callcard.WelcomeposX = 190;
            Config.callcard.WelcomeposY = 20;
            Config.callcard.WelcomeSize = 22;
            Config.callcard.ImageWidth = 778;
            Config.callcard.ImageHeight = 171;
            Config.callcard.avatarWidth = 160;
            Config.callcard.avatarHeight = 145;
            Config.callcard.avatarposX = 9;
            Config.callcard.avatarposY = 14;
            Config.callcard.descriptioncolor = "#000000";
            Config.callcard.UserNameColor = "#390b49";
            Config.callcard.CardBg = "https://cdn.discordapp.com/attachments/156559426009038848/336250884751097879/Original.png";
            Config.callcard.UnamePosX = 190;
            Config.callcard.UnamePosY = 100;
            Config.callcard.UnameSize = 22;

            GuildHandler.GuildConfigs[Context.Guild.Id] = Config;
            await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
            await ReplyAsync("Card Reset");

        }

    }
}
