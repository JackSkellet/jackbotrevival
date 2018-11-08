using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace jack.Services
{
    public class hugcounter
    {
        private ConcurrentDictionary<ulong, int> hugDict = new ConcurrentDictionary<ulong, int>();
        private JsonSerializer jSerializer = new JsonSerializer();
        public hugcounter()
        {
            InitializeLoader();
            LoadDatabase();
        }
        public async Task Addhug(IUser user, ICommandContext context)
        {
            try
            {
                if (hugDict.ContainsKey(user.Id))
                {
                    if (context.User.Id == user.Id)
                        return;
                    int counter = 0;
                    hugDict.TryGetValue(user.Id, out counter);
                    int ignore;
                    hugDict.TryGetValue(user.Id, out ignore);
                    counter++;
                    hugDict.TryUpdate(user.Id, counter, ignore);
                }
                else
                {
                    hugDict.TryAdd(user.Id, 1);
                }
                SaveDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public async Task Checkhugs(IUser user, ICommandContext Context)
        {
            try
            {
                if (hugDict.ContainsKey(user.Id))
                {
                    int counter = 0;
                    hugDict.TryGetValue(user.Id, out counter);
                    await Context.Channel.SendMessageAsync($"{user.Mention} has received a total of {counter} hugs (◕‿◕✿)");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{user.Mention} has not received any hugs yet (⌯˃̶᷄ ﹏ ˂̶᷄⌯)ﾟ be the first!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

        }
        private void InitializeLoader()
        {
            jSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            jSerializer.NullValueHandling = NullValueHandling.Ignore;
        }
        public void SaveDatabase()
        {
            using (StreamWriter sw = File.CreateText(@"Hugs.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    jSerializer.Serialize(writer, hugDict);
                }
            }
        }
        private void LoadDatabase()
        {
            if (File.Exists("Hugs.json"))
            {
                using (StreamReader sr = File.OpenText(@"Hugs.json"))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        hugDict = jSerializer.Deserialize<ConcurrentDictionary<ulong, int>>(reader);
                    }
                }
            }
            else
            {
                File.Create("Hugs.json").Dispose();
            }
        }
    }
}