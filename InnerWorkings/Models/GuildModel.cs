using Newtonsoft.Json;
using System.Collections.Generic;
using jack.Wrappers;
using jack.Interfaces;

namespace jack.Models
{
    public class GuildModel : IServer
    {
        [JsonProperty("Prefix")]
        public string Prefix { get; set; } = ";";

        [JsonProperty("WelcomeMessages")]
        public string WelcomeMessages { get; set; }

        [JsonProperty("LeaveMessages")]
        public string LeaveMessages { get; set; }

        [JsonProperty("MuteRoleID")]
        public ulong MuteRoleID { get; set; }

        [JsonProperty("Musicid")]
        public music musicid { get; set; } = new music();

        [JsonProperty("JoinRole")]
        public joinrole JoinRole { get; set; } = new joinrole();

        [JsonProperty("callcard")]
        public CallingCard callcard { get; set; } = new CallingCard();

        [JsonProperty("AdminCases")]
        public int AdminCases { get; set; }

        [JsonProperty("NoInvites")]
        public bool NoInvites { get; set; }

        [JsonProperty("TextWelcome")]
        public text textwelcome { get; set; } = new text();

        [JsonProperty("JoinLog")]
        public Wrapper JoinEvent { get; set; } = new Wrapper();

        [JsonProperty("LeaveLog")]
        public Wrapper LeaveEvent { get; set; } = new Wrapper();

        [JsonProperty("AdminLog")]
        public Wrapper AdminLog { get; set; } = new Wrapper();

        [JsonProperty("Tags")]
        public List<TagsModel> TagsList { get; set; } = new List<TagsModel>();

        [JsonProperty("AFKList")]
        public Dictionary<ulong, string> AFKList { get; set; } = new Dictionary<ulong, string>();

        [JsonProperty("MutedList")]
        public List<MutedModel> MutedList { get; set; } = new List<MutedModel>();

        [JsonProperty("Error")]
        public Error Error { get; set; } = new Error();

    }
}