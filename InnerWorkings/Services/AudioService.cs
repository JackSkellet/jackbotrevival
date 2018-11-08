using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using YoutubeExplode;
using YoutubeExplode.Models;
using jack.Handlers;

namespace jack.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels =
            new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (_connectedChannels.TryGetValue(guild.Id, out IAudioClient _))
                return;

            if (target.Guild.Id != guild.Id)
                return;

            var audioClient = await target.ConnectAsync();

            if (_connectedChannels.TryAdd(guild.Id, audioClient))
            {
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (_connectedChannels.TryRemove(guild.Id, out IAudioClient client))
                await client.StopAsync();
        }



        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string userInput)
        {
            var ytc = new YoutubeClient();

            if (userInput.ToLower().Contains("youtube.com") || userInput.ToLower().Contains("youtu.be"))
            {
                userInput = YoutubeClient.ParseVideoId(userInput);
                
            }
            else
            {
                var searchList = await ytc.SearchAsync(userInput);
                userInput = searchList.First();
                
            }

            var videoInfo = await ytc.GetVideoInfoAsync(userInput);
            

            var asi = videoInfo.AudioStreams.OrderBy(x => x.Bitrate).Last();
            

            var title = videoInfo.Title;

            var rgx = new Regex("[^a-zA-Z0-9 -]");
            title = rgx.Replace(title, "");

            var path = $@"C:\Users\jack\Documents\Visual Studio 2017\Projects\jackbot2.0\jackbot2.0\music\{guild.Id}\{title}.{asi.Container.GetFileExtension()}";
            await channel.SendMessageAsync("checking path");
            if (!Directory.Exists($@"C:\Users\jack\Documents\Visual Studio 2017\Projects\jackbot2.0\jackbot2.0\music\{guild.Id}"))
                Directory.CreateDirectory($@"C:\Users\jack\Documents\Visual Studio 2017\Projects\jackbot2.0\jackbot2.0\music\{guild.Id}");

            if (!File.Exists(path))
            {

                var embed = new EmbedBuilder()
                {
                    Title = $"Attempting to download",
                    Description = $"{title}",
                    Color = new Color(255, 0, 255)

                };



                await channel.SendMessageAsync($"", embed: embed.Build());
                using (var input = await ytc.GetMediaStreamAsync(asi))
                using (var Out = File.Create(path))
                {
                    await input.CopyToAsync(Out);
                }
            }

            if (_connectedChannels.TryGetValue(guild.Id, out IAudioClient audioClient))
            {

                var embed = new EmbedBuilder()
                {
                    Title = $"Now playing",
                    Description = $"**{title}**\n**Duration**: {videoInfo.Duration}\n**Views**: {videoInfo.ViewCount}\n<:like:325971963078115328>{videoInfo.LikeCount} | <:dislike:325971963451408386> {videoInfo.DislikeCount}",
                    Color = new Color(255, 0, 255),
                    ThumbnailUrl = videoInfo.ImageMaxResUrl
                };
                await channel.SendMessageAsync("", embed: embed.Build());
                var output = CreateStream(path).StandardOutput.BaseStream;
                var discordStream = audioClient.CreatePCMStream(AudioApplication.Music, 94208, 2000);
                var Config = GuildHandler.GuildConfigs[guild.Id];
                Config.musicid.name = title;
                Config.musicid.added = DateTime.UtcNow;
                Config.musicid.duration = videoInfo.Duration.ToString();
                Config.musicid.seconds = videoInfo.Duration.TotalSeconds;
                Config.musicid.thumbnail = $"https://img.youtube.com/vi/{videoInfo.Id}/maxresdefault.jpg";
                GuildHandler.GuildConfigs[guild.Id] = Config;
                await GuildHandler.SaveAsync(GuildHandler.GuildConfigs);
                await output.CopyToAsync(discordStream);
                await discordStream.FlushAsync();
                File.Delete(path);
            }
        }

        private static Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Users\jack\Documents\Visual Studio 2017\Projects\jackbot2.0\jackbot2.0\ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}