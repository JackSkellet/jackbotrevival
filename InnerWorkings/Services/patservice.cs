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
    public class PatService
    {
        private ConcurrentDictionary<ulong, int> patDict = new ConcurrentDictionary<ulong, int>();
        private JsonSerializer jSerializer = new JsonSerializer();
        public PatService()
        {
            InitializeLoader();
            LoadDatabase();
        }
        public async Task AddPat(IUser user, ICommandContext context)
        {
            try
            {
                if (patDict.ContainsKey(user.Id))
                {
                    if (context.User.Id == user.Id)
                        return;
                    int counter = 0;
                    patDict.TryGetValue(user.Id, out counter);
                    int ignore;
                    patDict.TryGetValue(user.Id, out ignore);
                    counter++;
                    patDict.TryUpdate(user.Id, counter, ignore);
                }
                else
                {
                    patDict.TryAdd(user.Id, 1);
                }
                SaveDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public async Task CheckPats(IUser user, ICommandContext Context)
        {
            try
            {
                if (patDict.ContainsKey(user.Id))
                {
                    int counter = 0;
                    patDict.TryGetValue(user.Id, out counter);
                    await Context.Channel.SendMessageAsync($"{user.Mention} has received a total of {counter} pats (◕‿◕✿)");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"{user.Mention} has not received any pats yet (⌯˃̶᷄ ﹏ ˂̶᷄⌯)ﾟ be the first!");
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
            using (StreamWriter sw = File.CreateText(@"Pats.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    jSerializer.Serialize(writer, patDict);
                }
            }
        }
        private void LoadDatabase()
        {
            if (File.Exists("Pats.json"))
            {
                using (StreamReader sr = File.OpenText(@"Pats.json"))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        patDict = jSerializer.Deserialize<ConcurrentDictionary<ulong, int>>(reader);
                    }
                }
            }
            else
            {
                File.Create("Pats.json").Dispose();
            }
        }
    }
}