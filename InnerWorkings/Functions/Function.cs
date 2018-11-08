using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace jack.Functions
{
    public static class Funciton
    {


        public static bool Advertisement(string Message)
        {


            List<string> URLS = new List<string>
            {
                "https://discordapp.com/invite/",
                "https://discord.com/invite/",
                "https://discord.me/invite/",
                "https://discordapp.gg/invite/",
                "https://discord.gg/invite/",
                "https://discord.me/",
                "https://discord.gg/",
                "https://www.discordapp.com/invite/",
                "https://www.discord.com/invite/",
                "https://www.discord.me/invite/",
                "https://www.discordapp.gg/invite/",
                "https://www.discord.gg/invite/",
                "https://www.discord.me/",
            };
            return (URLS.Any(x => Message.Contains(x) | Message.StartsWith(x)));
        }

        public static string Youtube(string Search)
        {
            var Service = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = aaaaaa
            });
            var SearchRequest = Service.Search.List("snippet");
            SearchRequest.Q = Search;
            SearchRequest.MaxResults = 1;
            SearchRequest.Type = "video";
            return (SearchRequest.Execute()).Items.Select(x => x.Id.VideoId).FirstOrDefault();
        }
    }
}
