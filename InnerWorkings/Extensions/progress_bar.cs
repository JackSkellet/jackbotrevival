using jack.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace jackbotV2.InnerWorkings.Extensions
{
    public class progress_bar
    {
        public static string UpdateProgress(int progress, int songtime)
        {
            int percentage = (int)100.0 * progress / songtime;

            {
                var done = (new string('▬', percentage / 10));
                return done;
            }
        }

        public static string bar(ulong guild)
        {
            {
                var Config = GuildHandler.GuildConfigs[guild];
                DateTime playing = Config.musicid.added;
                DateTime now = DateTime.UtcNow;
                TimeSpan diff = now - playing;

                TimeSpan start = DateTime.UtcNow - DateTime.UtcNow;



                int endint = Convert.ToInt32(diff.TotalSeconds);
                int startint = Convert.ToInt32(Config.musicid.seconds);
                var a = progress_bar.UpdateProgress(endint, startint);
                var b = progress_bar.precent(endint, startint);
                var x = progress_bar.UpdateProgress(endint, startint);
                var done = new String('▬', 10 - x.Length);
                if (x.Length > 10)
                {
                    done = new String('▬', x.Length + 1);
                    return done;
                }
                else
                    return done;
            }
        }

        public static string precent(int progress, int songtime)
        {
            int percentage = (int)100.0 * progress / songtime;

            {
                var done = percentage + "%";
                return done;
            }
        }


    }
}
