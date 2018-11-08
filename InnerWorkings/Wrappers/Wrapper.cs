using System;

namespace jack.Wrappers
{
    public class Wrapper
    {
        public bool IsEnabled { get; set; }
        public ulong TextChannel { get; set; }
    }

    public class Error
    {
        public bool OnOff { get; set; }
    }

    public class text
    {
        public bool OnOff { get; set; }
    }

    public class joinrole
    {
        public bool IsEnabled { get; set; }
        public ulong roleid { get; set; }
    }

    public class music
    {

        public string name { get; set; }
        public DateTime added { get; set; }
        public string duration { get; set; }
        public double seconds { get; set; }
        public string thumbnail { get; set; }
    }

    public class CallingCard
    {
        public string CardBg { get; set; } = "https://cdn.discordapp.com/attachments/156559426009038848/336250884751097879/Original.png";
        public string UserNameColor { get; set; } = "#390b49";
        public string descriptioncolor { get; set; } = "#000000";

        public int avatarposX { get; set; } = 9;
        public int avatarposY { get; set; } = 14;
        public int avatarWidth { get; set; } = 160;
        public int avatarHeight { get; set; } = 145;

        public int ImageWidth { get; set; } = 778;
        public int ImageHeight { get; set; } = 171;

        public int WelcomeposX { get; set; } = 190;
        public int WelcomeposY { get; set; } = 20;
        public int WelcomeSize { get; set; } = 22;

        public string WelcomeColor { get; set; } = "#000000";

        public int SnameposX { get; set; } = 190;
        public int SnameposY { get; set; } = 50;
        public int SnameSize { get; set; } = 27;


        public int DiscrimPosX { get; set; } = 680;
        public int DiscrimPosY { get; set; } = 125;
        public int DiscrimSize { get; set; } = 22;

        public int UnameSize { get; set; } = 22;
        public int UnamePosX { get; set; } = 190;
        public int UnamePosY { get; set; } = 100;

        public int IdSize { get; set; } = 22;
        public int IdPosX { get; set; } = 190;
        public int IdPosY { get; set; } = 125;
    }
}
