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
using System.IO;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ConsoleApp1.Modules.Module.image_commands

{
    [Name("Images")]
    public class images : ModuleBase
    {

        [Command("mokou")]
        [Alias("moko")]
        [Summary("random mokou image")]
        public async Task sxasync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\mokou");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("kasen")]
        [Summary("random kasen image")]
        public async Task ssync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\kasen");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("okuu")]
        [Summary("random okuu image")]
        public async Task sadddnc()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\u");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("reisen")]
        [Summary("random amazing tank image")]
        public async Task vvsync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\r");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("sakuya")]
        [Alias("maid")]
        [Summary("random sakuya image")]
        public async Task gggggg()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\sakuya");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        
            [Command("tenshi")]
            [Summary("random tenshi image")]
            public async Task sgync()
            {
                var rand = new Random();
                var extensions = new string[] { ".png", ".jpg", ".gif" };
                var di = new DirectoryInfo(path: @"C:\tenshi");
                var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
                var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
                await Context.Channel.SendFileAsync(file);
            }
        

        [Command("tewi")]
        [Summary("random random tewi image")]
        public async Task sssync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\tewi");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("think")]
        [Summary("random random thinking emote")]
        public async Task saynync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\Think");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("yomu")]
        [Summary("random yomu image")]
        public async Task saysssssync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\yomus\yomu");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }
        [Command("kogasa")]
        [Summary("random kosaga image")]
        public async Task sazzzzzc()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\kosaga");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("mamizou")]
        [Summary("random racoon")]
        public async Task sccync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\mamizou");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }
        [Command("dog")]
        [Summary("Gets a random dog image from random.dog")]
        public async Task Cat()
        {

            var img = ("http://random.dog/" + await SearchHelper.GetResponseStringAsync("http://random.dog/woof").ConfigureAwait(false));
            var embed = new EmbedBuilder()
            {
                Color = new Color(0xb942f4),
                ImageUrl = img
            };
            await ReplyAsync("", embed: embed.Build());

        }
        [Command("cat")]
        [Summary("Gets a random cat image from random.cat")]
        public async Task Caat()
        {

            XDocument xDoc = JsonConvert.DeserializeXNode(await (new HttpClient().GetStringAsync("http://random.cat/meow")), "root");

            var embed = new EmbedBuilder()
            {
                Color = new Color(0xb942f4),
                ImageUrl = xDoc.Element("root").Element("file").Value
            };

            await ReplyAsync("", embed: embed.Build());

        }
        [Command("mashiro")]
        [Summary("random mashiro image")]
        public async Task saggc()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\mash");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("mask")]
        [Alias("kokoro")]
        [Summary("random mask")]
        public async Task szync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\reimuu");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("naz")]
        [Alias("nazarin")]
        [Summary("random nazarin image")]
        public async Task sasadsaync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\m");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("dweem")]
        [Alias("doremy sweet")]
        [Summary("random dream catcher")]
        public async Task sayasync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\d");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("flowertank"), Alias("flt")]
        [Summary("random amazing tank image")]
        public async Task sxsync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\flower");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("hag")]
        [Alias("yukari")]
        [Summary("some random hag")]
        public async Task sanc()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\yukair");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("hina")]
        [Summary("some random hina")]
        public async Task sasadsad()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\hina");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("jeanne")]
        [Summary("random jeanne image")]
        public async Task sahghgf()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\jeanne");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("kero")]
        [Alias("suwako")]
        [Summary("random suwako")]
        public async Task sadfdsfdssync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\suwac");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }

        [Command("koishi")]
        [Summary("random random koishi image")]
        public async Task saasync()
        {
            var rand = new Random();
            var extensions = new string[] { ".png", ".jpg", ".gif" };
            var di = new DirectoryInfo(path: @"C:\y");
            var files = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));
            var file = files.ElementAt(rand.Next(0, files.Count())).FullName;
            await Context.Channel.SendFileAsync(file);
        }
    }
    
}

