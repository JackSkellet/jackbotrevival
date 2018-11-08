using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace jack.Models.bot
{

    [Name("Owner")]
    public class name : ModuleBase
    {
        private DiscordSocketClient client;
        [Command("name", RunMode = RunMode.Async)]
        [Summary("Sets the bot's username")]
        [RequireOwner]
        public async Task avatar([Remainder] string content)
        {
            await Context.Client.CurrentUser.ModifyAsync(x => x.Username = content);
        }
    }
}