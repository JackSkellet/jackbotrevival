using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ImageSharp;
using ImageSharp.Processing;
using jack.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SixLabors.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace jack.Services
{
    public class EPService
    {

        ConcurrentDictionary<ulong, bool> userBG = new ConcurrentDictionary<ulong, bool>();
        ConcurrentDictionary<ulong, DateTime> userBGUpdateCD = new ConcurrentDictionary<ulong, DateTime>();
        private DiscordSocketClient client;
        private JsonSerializer jSerializer = new JsonSerializer();
        private int timeToUpdate = Environment.TickCount + 30000;



        public EPService(DiscordSocketClient c)
        {
            client = c;
            InitializeLoader();
            LoadDatabaseBG();

            ProfileImageProcessing.Initialize();
        }



        public async Task ShowProfile(string welcome, ulong id, IUser user, ITextChannel channel, string cardbg)
        {
            try
            {

                if (userBG.ContainsKey(user.Id))
                {
                    //await DrawText(user.GetAvatarUrl(), user, Context);
                    await DrawProfile(user.GetAvatarUrl(), user, id, welcome, cardbg);
                }
                else
                {
                    //await DrawText2(user.GetAvatarUrl(), user, Context);
                    await DrawProfile(user.GetAvatarUrl(), user, id, welcome, cardbg);
                }
                //await Context.Channel.SendMessageAsync($"Image \n{img}");
                if (File.Exists($"{user.Id}.png"))
                {
                    
                    await channel.SendFileAsync($"{user.Id}.png", $"<@{user.Id}>", false, null);
                    
                    
                    File.Delete($"{user.Id}.png");
                    File.Delete($"{user.Id}Avatar.png");
                    File.Delete($"{user.Id}AvatarF.png");

                    
                    return;
                }
                else
                {
                    await channel.SendMessageAsync($"Failed to create Image! This may be due to the Image you linked is damaged or unsupported. Try a new Custom Pic or use the default Image (p setbg with no parameter sets it to the default image)\n\n Welcome <@{user.Id}> to The server");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            
        }



        private async Task DrawProfile(String AvatarUrl, IUser userInfo, ulong guildid, string welcome, string cardbg)
        {
            if (String.IsNullOrEmpty(AvatarUrl))
                AvatarUrl =
                    "http://is2.mzstatic.com/image/pf/us/r30/Purple7/v4/89/51/05/89510540-66df-9f6f-5c91-afa5e48af4e8/mzl.sbwqpbfh.png";

            Uri requestUri = new Uri(AvatarUrl);
            Uri requestUri2 = new Uri(cardbg);

            if (File.Exists($"{userInfo.Id}Avatar.png"))
            {
                File.Delete($"{userInfo.Id}Avatar.png");
            }

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (
                Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                    stream = new FileStream($"{userInfo.Id}Avatar.png", FileMode.Create, FileAccess.Write,
                        FileShare.None, 3145728, true))
            {
                await contentStream.CopyToAsync(stream);
                await contentStream.FlushAsync();
                contentStream.Dispose();
                await stream.FlushAsync();
                stream.Dispose();
                Console.WriteLine("DONE STREAM");
                
            }
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri2))
            using (
                Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                    stream = new FileStream($"{guildid}.png", FileMode.Create, FileAccess.Write,
                        FileShare.None, 3145728, true))
            {
                await contentStream.CopyToAsync(stream);
                await contentStream.FlushAsync();
                contentStream.Dispose();
                await stream.FlushAsync();
                stream.Dispose();
                Console.WriteLine("DONE STREAM");
                
            }

            var x = userInfo as SocketGuildUser;
            var username = x.Nickname ?? userInfo.Username;



            ProfileImageProcessing.GenerateProfile(welcome, guildid, $"{userInfo.Id}Avatar.png", username, $"{userInfo.Id}.png", $"{userInfo.Id}.png", x);
            return;
        }



        private void InitializeLoader()
        {
            jSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            jSerializer.NullValueHandling = NullValueHandling.Ignore;
        }



        public void SaveDatabaseBG()
        {
            using (StreamWriter sw = File.CreateText(@"UserCustomBG.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    jSerializer.Serialize(writer, userBG);
                }
            }
        }

        private void LoadDatabaseBG()
        {
            if (File.Exists("UserCustomBG.json"))
            {
                using (StreamReader sr = File.OpenText(@"UserCustomBG.json"))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        var userBGTemp = jSerializer.Deserialize<ConcurrentDictionary<ulong, bool>>(reader);
                        if (userBGTemp == null)
                            return;
                        userBG = userBGTemp;
                    }
                }
            }
            else
            {
                File.Create("UserCustomBG.json").Dispose();
            }
        }

    }

}
