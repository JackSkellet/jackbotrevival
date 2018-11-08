using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using ImageSharp;
using ImageSharp.Drawing.Brushes;
using SixLabors.Fonts;
using SixLabors.Primitives;
using jack.Handlers;
using Discord;
using System.Net.Http;
using Discord.WebSocket;

namespace jack.Extensions
{

    public static class ProfileImageProcessing
    {
        private static FontCollection _fontCollection;
        private static FontFamily _titleFont;
        private static FontFamily _titleFont2;
        private static Image<Rgba32> _bgMaskImage;
        private static Image<Rgba32> _noBgMask;
        private static Image<Rgba32> _noBgMaskOverlay;

        public static void Initialize()
        {
            _fontCollection = new FontCollection();
            _titleFont = _fontCollection.Install("fonts/04B_30__.TTF");
            _titleFont2 = _fontCollection.Install("fonts/Lato-Bold.ttf");
            _bgMaskImage = ImageSharp.Image.Load("moreBGtemp.png");
            //_noBgMask = ImageSharp.Image.Load("profilecardtemplate.png");
            _noBgMaskOverlay = ImageSharp.Image.Load("backdrop.png");


        }



        public static void GenerateProfile(string welcome, ulong id, string avatarUrl, string name, string outputPath, string bgurl, SocketGuildUser user)
        {
            _noBgMask = ImageSharp.Image.Load($"{id}.png");
            var gldConfig = GuildHandler.GuildConfigs[id];
            var x = gldConfig.callcard.avatarposX;
            var y = gldConfig.callcard.avatarposY;

            var width = gldConfig.callcard.avatarWidth;
            var height = gldConfig.callcard.avatarHeight;

            var imgH = gldConfig.callcard.ImageHeight;
            var imgW = gldConfig.callcard.ImageWidth;

            var sname = user.Guild.Name;
            var snameX = gldConfig.callcard.SnameposX;
            var snameY = gldConfig.callcard.SnameposY;
            var snameSize = gldConfig.callcard.SnameSize;

            var WelcomeX = gldConfig.callcard.WelcomeposX;
            var WelcomeSize = gldConfig.callcard.WelcomeSize;
            var WelcomeY = gldConfig.callcard.WelcomeposY;
            var Welcomecolor = gldConfig.callcard.WelcomeColor;

            var discrposX = gldConfig.callcard.DiscrimPosX;
            var discrposY = gldConfig.callcard.DiscrimPosY;
            var discrSize = gldConfig.callcard.DiscrimSize;

            var UnameSize = gldConfig.callcard.UnameSize;
            var UnamePosX = gldConfig.callcard.UnamePosX;
            var UnamePosY = gldConfig.callcard.UnamePosY;

            var IdposX = gldConfig.callcard.IdPosX;
            var IdposY = gldConfig.callcard.IdPosY;
            var IdSize = gldConfig.callcard.IdSize;

            using (var output = new Image<Rgba32>(imgW, imgH))
            {

                // DrawMask(_noBgMaskOverlay, output, new Size(1000, 150));

                Drawbackdrop(_noBgMaskOverlay, output, new Size(width, height), user);

                DrawAvatar(avatarUrl, output, new Rectangle(x, y, width, height));

                DrawMask(_noBgMask, output, new Size(imgW, imgH));

                Drawwelcome("Welcome To", output, new System.Numerics.Vector2(WelcomeX, WelcomeY), Rgba32.FromHex(Welcomecolor), WelcomeSize);

                DrawwSname(sname, output, new System.Numerics.Vector2(snameX, snameY), Rgba32.FromHex(Welcomecolor), snameSize);

                DrawUserName(name, output, new System.Numerics.Vector2(UnamePosX, UnamePosY), Rgba32.FromHex(gldConfig.callcard.UserNameColor), UnameSize);

                Drawid(user.Id.ToString(), output, new System.Numerics.Vector2(IdposX, IdposY), Rgba32.FromHex(gldConfig.callcard.descriptioncolor), IdSize);

                Drawdiscrim("#" + user.Discriminator, output, new System.Numerics.Vector2(discrposX, discrposY), Rgba32.FromHex(gldConfig.callcard.descriptioncolor), discrSize);


                //DrawBackground(guild.IconUrl, output , new Size(1024, 1024));

                output.Save(outputPath);
                return;



            }//dispose of output to help save memory
        }

        private static void DrawStats(string welcome, ulong id, Image<Rgba32> output, Vector2 posRank, Vector2 posLevel, Vector2 posEP, Rgba32 color, IUser user)
        {
            var gldConfig = GuildHandler.GuildConfigs[id];
            // measure each string and split the margin between them??



            var font = new Font(_titleFont, 22, FontStyle.Bold);
            var ep = welcome;


            var rankText = $"{ep ?? "Welcome"}";
            var rankSize = TextMeasurer.Measure(rankText, font, 72);
            var left = posRank.X + rankSize.Width; // find the point the rankText stops
            var right = posEP.X - rankSize.Width; // find the point the epText starts
            var posLevel2 = new Vector2(left + (right - left) / 2, posRank.Y); // find the point halfway between the 2 other bits of text

            output.DrawText(rankText, font, color, posRank, new ImageSharp.Drawing.TextGraphicsOptions
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });




        }




        private static void DrawUserName(string name, Image<Rgba32> output, Vector2 pos, Rgba32 color, int size)
        {
            output.DrawText(name, new Font(_titleFont2, size, FontStyle.Regular), color, pos);
        }

        private static void Drawwelcome(string name, Image<Rgba32> output, Vector2 pos, Rgba32 color, int size)
        {
            output.DrawText(name, new Font(_titleFont2, size, FontStyle.Regular), color, pos);
        }

        private static void Drawdiscrim(string name, Image<Rgba32> output, Vector2 pos, Rgba32 color, int size)
        {
            output.DrawText(name, new Font(_titleFont2, size, FontStyle.Regular), color, pos);
        }

        private static void Drawid(string name, Image<Rgba32> output, Vector2 pos, Rgba32 color, int size)
        {
            output.DrawText(name, new Font(_titleFont2, size, FontStyle.Regular), color, pos);
        }

        private static void DrawwSname(string name, Image<Rgba32> output, Vector2 pos, Rgba32 color, int size)
        {
            output.DrawText(name, new Font(_titleFont2, size, FontStyle.Regular), color, pos);
        }

        private static void DrawMask(Image<Rgba32> mask, Image<Rgba32> output, Size size)
        {

            output.DrawImage(mask, 1, size, new Point(0, 0));

        }

        private static void Drawbackdrop(Image<Rgba32> mask, Image<Rgba32> output, Size size, SocketGuildUser user)
        {
            var gldConfig = GuildHandler.GuildConfigs[user.Guild.Id];
            var x = gldConfig.callcard.avatarposX;
            var y = gldConfig.callcard.avatarposY;
            output.DrawImage(mask, 1, size, new Point(x, y));

        }

        private static void DrawBackground(string backgroundUrl, Image<Rgba32> output, Size size)
        {
            using (Image<Rgba32> background = ImageSharp.Image.Load(backgroundUrl))//900x500
            {
                //draw on the background
                output.DrawImage(background, 1, size, new Point(0, 0));
            }//once draw it can be disposed as its no onger needed in memory
        }

        private static void DrawAvatar(string avatarUrl, Image<Rgba32> output, Rectangle rec)
        {
            var avatarPosition = rec;

            using (var avatar = ImageSharp.Image.Load(avatarUrl)) // 57x57
            {
                avatar.Resize(new ImageSharp.Processing.ResizeOptions
                {
                    Mode = ImageSharp.Processing.ResizeMode.Crop,
                    Size = avatarPosition.Size
                });

                output.DrawImage(avatar, 1, avatarPosition.Size, avatarPosition.Location);
            }
        }


    }
}


