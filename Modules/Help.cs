using Discord;
using Discord.Addons.Paginator;
using Discord.Commands;
using Discord.WebSocket;
using jack.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jack.Modules
{
    public class Help : ModuleBase
    {
        private readonly PaginationService _paginator;
        private readonly CommandService _service;

        public Help(PaginationService paginator, CommandService service)
        {
            _paginator = paginator;
            _service = service;
        }

        [Command("?", RunMode = RunMode.Async)]
        [Summary("info about command")]
        [Remarks("help commandname")]
        public async Task HelpAsync([Remainder] string command = null)
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            var result = _service.Search(Context, command);

            if (command == null)
            {
                await ReplyAsync($"Please specify a command, ie '{gldConfig.Prefix}? kick'");
                return;
            }

            if (!result.IsSuccess)
                await ReplyAsync($"**Command Name:** {command}\n**Error:** Not Found!\n**Reason:** ¯\\_(ツ)_/¯");
            var builder = new EmbedBuilder
            {
                Color = new Color(179, 56, 216)
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;
                builder.Title = cmd.Name.ToUpper();
                builder.Description =
                    $"**Aliases:** {string.Join(", ", cmd.Aliases)}\n**Parameters:** {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                    $"**Remarks:** `{gldConfig.Prefix}{cmd.Remarks}`\n**Summary:** `{cmd.Summary}`";
            }
            await ReplyAsync("", false, builder.Build());
        }

        [Command("help", RunMode = RunMode.Async)]
        [Summary("all help commands")]

        public async Task Help2Async()
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            if (Context.Channel is IDMChannel)
            {
                var builder = new EmbedBuilder
                {
                    Color = new Color(114, 137, 218),
                    Title = $"Here are my commands:"
                };

                foreach (var module in _service.Modules)
                {
                    var description =
                        (from cmd in module.Commands let result = module.Name where result != "Owner" select cmd)
                        .Aggregate<CommandInfo, string>(null,
                            (current, cmd) => current + $"{gldConfig.Prefix}{cmd.Name} - {cmd.Summary}\n");

                    if (!string.IsNullOrWhiteSpace(description))
                        builder.AddField(x =>
                        {
                            x.Name = module.Name;
                            x.Value = description;
                        });
                }
                await ReplyAsync("", false, builder.Build());
            }
            else
            {
                var pages = new List<string>();

                foreach (var module in _service.Modules)
                {
                    string description = null;
                    foreach (var cmd in module.Commands)
                    {
                        var result = module.Name;

                        if (result != "Owner")
                            description += $"{gldConfig.Prefix}{cmd.Name} - {cmd.Summary}\n";
                    }

                    if (!string.IsNullOrWhiteSpace(description))
                        pages.Add($"**{module.Name}**\n{description}");

                }
                var message = new PaginatedMessage(pages, "Help Commands", new Color(0xb100c1), Context.User);

                await _paginator.SendPaginatedMessageAsync(Context.Channel, message);

            }
        }
    }
}
