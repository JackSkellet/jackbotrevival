using Newtonsoft.Json;
using jack.Interfaces;
using jack.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace jack.Handlers
{
    public class GuildHandler
    {
        public static Dictionary<ulong, GuildModel> GuildConfigs { get; set; } = new Dictionary<ulong, GuildModel>();

        public const string configPath = "GuildConfig.json";

        public static async Task SaveAsync<T>(Dictionary<ulong, T> configs) where T : IServer
            => File.WriteAllText(configPath, await Task.Run(() => JsonConvert.SerializeObject(configs, Formatting.Indented)).ConfigureAwait(false));

        public static async Task<Dictionary<ulong, T>> LoadServerConfigsAsync<T>() where T : IServer, new()
        {
            if (File.Exists(configPath))
            {
                return JsonConvert.DeserializeObject<Dictionary<ulong, T>>(File.ReadAllText(configPath));
            }
            var newConfig = new Dictionary<ulong, T>();
            await SaveAsync(newConfig);
            return newConfig;
        }
    }
}