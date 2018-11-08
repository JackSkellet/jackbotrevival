using jack.Models;
using jack.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace jack.Interfaces
{
    public interface IServer
    {
        string Prefix { get; set; }
        string WelcomeMessages { get; set; }
        string LeaveMessages { get; set; }
        ulong MuteRoleID { get; set; }
        joinrole JoinRole { get; set; }
        int AdminCases { get; set; }
        bool NoInvites { get; set; }
        Wrapper JoinEvent { get; set; }
        Wrapper LeaveEvent { get; set; }
        text textwelcome { get; set; }
        Wrapper AdminLog { get; set; }
        music musicid { get; set; }
        List<TagsModel> TagsList { get; set; }
        List<MutedModel> MutedList { get; set; }
        Dictionary<ulong, string> AFKList { get; set; }
        
    }
}
