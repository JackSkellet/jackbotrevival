using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Net.Http;

namespace jack.Modules.bot
{
    [Name("Owner")]
    public class bAvatar : ModuleBase
    {
        private string[] _imageExtensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        [Command("bavatar", RunMode = RunMode.Async)]
        [Summary("Sets the bot's avatar")]
        [RequireOwner]
        public async Task avatar([Summary("A direct image link to the bot's new avatar")] string content)
        {
            if (!_imageExtensions.Any(x => content.EndsWith(x)))
            {
                await ReplyAsync(":x: Please enter a valid direct image link to the bot's new avatar.");
                return;
            }
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(content);
                var image = await response.Content.ReadAsStreamAsync();
                await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(image));
            }
        }
    }


    
}