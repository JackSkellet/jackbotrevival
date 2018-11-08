using System;
using System.Collections.Generic;
using System.Text;

namespace jack.Models
{
    public class TagsModel
    {
        
        public string Name { get; set; }
        public string Response { get; set; }
        public string CreationDate { get; set; }
        public ulong Owner { get; set; }
        public int Uses { get; set; }
    }
}

