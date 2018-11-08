using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using YoutubeExplode;
using jack.Handlers;
using Discord.WebSocket;
using jack.Services;
using jackbotV2.InnerWorkings.Extensions;

namespace jack.Modules
{
    public class Audio : ModuleBase
    {
        public static Dictionary<ulong, List<string>> Queue =
            new Dictionary<ulong, List<string>>();

        private readonly AudioService _service;
        private string _nextSong, _leftInQueue;

        public Audio(AudioService service)
        {
            _service = service;
        }

        [Command("queue", RunMode = RunMode.Async)]
        [Alias("q")]
        [Remarks("q")]
        [Summary("Lists all songs in the queue")]
        public async Task QueueList()
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);
            var songlist = new List<string>();
            if (list.Count > 0)
            {
                var i = 0;
                foreach (var item in list)
                {
                    songlist.Add($"`{i}` - {item}");
                    i++;
                }
                var embed = new EmbedBuilder();
                embed.WithTitle("Current Queue");
                await ReplyAsync(string.Join("\n", songlist.ToArray()));
            }
            else
            {
                await ReplyAsync("The Queue is empty :(");
            }
        }


        [Command("q add", RunMode = RunMode.Async)]
        [Alias("queue add", "play")]
        [Remarks("q add 'yt video'/'yt video name'")]
        [Summary("Adds a song to the queue")]
        public async Task QueueSong([Remainder] string linkOrSearchTerm)
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);
            list.Add(linkOrSearchTerm); //adds the given item to the queue, if its a URL it will be converted to a song later on

            Queue.Remove(Context.Guild.Id);
            Queue.Add(Context.Guild.Id, list);
            await ReplyAsync(
                $"**{linkOrSearchTerm}** has been added to the end of the queue. \n" +
                $"Queue Length: **{list.Count}**");
        }

        [Command("q pl", RunMode = RunMode.Async)]
        [Alias("q playlist", "queue playlist", "queue pl")]
        [Remarks("q pl 'playlist url'")]
        [Summary("Adds the given YT playlist to the Queue")]
        public async Task PlaylistCmd([Remainder] string playlistLink)
        {
            var ytc = new YoutubeClient();

            var playListInfo = await ytc.GetPlaylistInfoAsync(YoutubeClient.ParsePlaylistId(playlistLink));
            var ten = playListInfo.VideoIds.ToArray().Take(10).ToArray();
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);
            await ReplyAsync($"Attempting to add the first 10 songs of **{playListInfo.Title}** to the queue!");
            var i = 0;
            foreach (var song in ten)
            {
                var videoInfo = await ytc.GetVideoInfoAsync(song);
                var title = videoInfo.Title;
                list.Add(title);
                await ReplyAsync($"`{i}` - **{title}** added to the queue");
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id,
                    list); //ineffieient as fuck because im adding all songs one by one rather than as a group, however. it takes a long time so this is better timewise
                i++;
            }

            await PlayQueue();

            await ReplyAsync(
                $"**{playListInfo.Title}** has been added to the end of the queue. \nQueue Length: **{list.Count}**");
        }

        [Command("playing")]
        [Alias("np")]
        [Remarks("np")]
        [Summary("now playing")]
        public async Task np()
        {

            var Config = GuildHandler.GuildConfigs[Context.Guild.Id];

            var ytc = new YoutubeClient();

            var embed = new EmbedBuilder();

            var videoInfo = await ytc.SearchAsync(Config.musicid.name);
            var url = videoInfo.FirstOrDefault();


            DateTime playing = Config.musicid.added;
            DateTime now = DateTime.UtcNow;
            TimeSpan diff = now - playing;

            TimeSpan start = DateTime.UtcNow - DateTime.UtcNow;

            var c = progress_bar.bar(Context.Guild.Id);

            int endint = Convert.ToInt32(diff.TotalSeconds);
            int startint = Convert.ToInt32(Config.musicid.seconds);

            var a = progress_bar.UpdateProgress(endint, startint);
            var b = progress_bar.precent(endint, startint);

            if ((endint) > startint || Config.musicid.name == null)
            {
                await ReplyAsync("No song playing atm");
            }
            else
            {
                embed.AddField(x =>
                {
                    x.Name = "Title";
                    x.Value = $"{Config.musicid.name}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {
                    x.Name = "Duration";
                    x.Value = $"{(now - playing).Hours}:{(now - playing).Minutes}:{(now - playing).Seconds}/{Config.musicid.duration}";
                    x.IsInline = true;
                });
                embed.AddField(x =>
                {

                    x.Name = "Progress";
                    x.Value = $"[{a}](http://www.google.com){c}" + $"{b}";
                    x.IsInline = true;
                });
                embed.WithThumbnailUrl(Config.musicid.thumbnail);
                embed.WithColor(new Color(0xc000ff));
                await ReplyAsync($"", embed: embed.Build());
            }

        }

        [Command("q all", RunMode = RunMode.Async)]
        [Alias("queue all")]
        [Remarks("q all")]
        [Summary("Plays all downloaded songs")]
        public async Task Pall()
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);

            if (Directory.Exists($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/"))
            {
                var d = new DirectoryInfo($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/");
                var music = d.GetFiles("*.*");
                list.AddRange(music.Select(sng => Path.GetFileNameWithoutExtension(sng.Name)));
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id, list);
                await PlayQueue();
            }
            else
            {
                await ReplyAsync("There are no songs downloaded in this server yet");
            }
        }

        [Command("q skip", RunMode = RunMode.Async)]
        [Alias("queue skip")]
        [Remarks("q skip")]
        [Summary("Skips the current song")]
        public async Task SkipSong()
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);

            if (list.Count > 0)
            {
                list.RemoveAt(0);
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id, list);
            }
            await PlayQueue();
        }

        [Command("q del", RunMode = RunMode.Async)]
        [Alias("queue del", "q delete", "queue delete")]
        [Remarks("q del 'x'")]
        [Summary("Removes the given song from the queue")]
        public async Task Qdel(int x)
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);


            if (list.Count > 0)
            {
                await ReplyAsync($"Removed **{list.ElementAt(x)}** from the queue");
                list.RemoveAt(x);
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id, list);
            }
        }

        [Command("q clear", RunMode = RunMode.Async)]
        [Alias("queue clear")]
        [Remarks("q clear")]
        [Summary("Empties the queue")]
        public async Task ClearQue()
        {
            var list = new List<string>();
            if (Queue.ContainsKey(Context.Guild.Id))
                Queue.TryGetValue(Context.Guild.Id, out list);

            if (list.Count > 0)
            {
                list.Clear();
                await ReplyAsync("Queue has been cleared");
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id, list);
            }
        }

        [Command("q play", RunMode = RunMode.Async)]
        [Alias("queue play")]
        [Remarks("q play")]
        [Summary("Plays the queue")]
        public async Task PlayQueue()
        {
            List<string> list;
            if (Queue.ContainsKey(Context.Guild.Id))
            {
                Queue.TryGetValue(Context.Guild.Id, out list);
            }
            else
            {
                await ReplyAsync("This guilds queue is empty. Please add some songs first before playing!");
                return;
            }

            while (list.Count > 0)
            {
                await _service.LeaveAudio(Context.Guild);
                await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

                _nextSong = list.Count != 1 ? $", next song: **{list.ElementAt(1)}**" : "";
                _leftInQueue = list.Count == 1
                    ? "There is 1 song in the queue."
                    : $"There are {list.Count} songs in the queue.";
                await ReplyAsync($"Now Playing: **{list.First()}** {_nextSong}.\n{_leftInQueue}");

                await _service.SendAudioAsync(Context.Guild, Context.Channel, list.First());
                list.RemoveAt(0);
                Queue.Remove(Context.Guild.Id);
                Queue.Add(Context.Guild.Id, list);
                Queue.TryGetValue(Context.Guild.Id, out list);
            }

            await ReplyAsync($"Sorry, the queue is empty, ~>queue (or ~>q) to add more!");

            await _service.LeaveAudio(Context.Guild);
            await ReplyAsync("Leaving Audio Channel");
        }

        [Command("songs", RunMode = RunMode.Async)]
        [Remarks("songs")]
        [Summary("Lists all songs downloaded in your server")]
        public async Task SongList(int page = 0)
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            //gets the current guilds directory
            if (Directory.Exists($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/"))
            {

                var d = new DirectoryInfo($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/");
                var music = d.GetFiles("*.*");
                var songlist = new List<string>();
                var i = 0;
                foreach (var sng in music)
                {
                    songlist.Add($"`{i}` - {Path.GetFileNameWithoutExtension(sng.Name)}");
                    i++;
                }
                var list = string.Join("\n", songlist.Take(10).ToArray());
                if (i > 10)
                    if (page <= 0)
                    {
                        await ReplyAsync(
                            $"**Page 0**\nHere are the first 10 songs saved in your server (total = {i})\n" +
                            $"{list}");
                    }
                    else
                    {
                        list = string.Join("\n", songlist.Skip(page * 10).Take(10).ToArray());
                        if (list == "")
                            await ReplyAsync($"**Page {page}**\n" +
                                             "This page is empty");
                        else
                            await ReplyAsync($"**Page {page}**\n" +
                                             $"{list}");
                    }
                else
                    await ReplyAsync(list);
            }
            else
            {
                await ReplyAsync("There are currently no songs downloaded for this guild\n" +
                                 $"you can download songs using the `{gldConfig.Prefix}play` command");
            }

        }

        [Command("delete")]
        [Remarks("delete 'songnumber'")]
        [Summary("deletes the given song number's file from the servers folder")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteTask(int song)
        {

            var d = new DirectoryInfo($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/");
            var music = d.GetFiles("*.*");
            var songpath = new List<string>();
            var songname = new List<string>();
            var i = 0;
            foreach (var sng in music)
            {
                songpath.Add($"{sng.FullName}");
                songname.Add($"{Path.GetFileNameWithoutExtension(sng.Name)}");
                i++;
            }
            if (song <= i)
            {
                try
                {
                    await ReplyAsync($"**Deleted song: **{songname[song]}");
                    File.Delete(songpath[song]);
                }
                catch
                {
                    await ReplyAsync($"Unable to delete song number **{song}** from the songs directory");
                }

            }
            else
            {
                await ReplyAsync($"Unable to delete song number **{song}** from the songs directory");
            }

        }

        [Command("delall")]
        [Remarks("delete all")]
        [Summary("Deletes all downloaded song files from the servers folder (ADMIN)")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteAllTask()
        {
            var Guild = Context.Guild as SocketGuild;
            var gldConfig = GuildHandler.GuildConfigs[Guild.Id];
            var d = new DirectoryInfo($"C:/Users/jack/Documents/Visual Studio 2017/Projects/jackbot2.0/jackbot2.0/music/{Context.Guild.Id}/");
            var music = d.GetFiles("*.*");
            var i = 0;
            foreach (var sng in music)
            {
                File.Delete(sng.FullName);
                i++;
            }

            await ReplyAsync($"{Context.User} deleted all downloaded songs (total = {i}) from this server's folder\n" +
                             $"you can download more using `{gldConfig.Prefix}play 'songname or YT URL'`\n" +
                             $"for a rundown on commands type `{gldConfig.Prefix}help`");
        }

        [Command("mjoin", RunMode = RunMode.Async)]
        [Remarks("join")]
        [Summary("Joins your Voice Channel")]
        public async Task JoinCmd()
        {
            await ReplyAsync("Joining Audio Channel");
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("mleave", RunMode = RunMode.Async)]
        [Remarks("leave")]
        [Summary("Leaves your Voice Channel")]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
            await ReplyAsync("Leaving Audio Channel");
        }
    }
}