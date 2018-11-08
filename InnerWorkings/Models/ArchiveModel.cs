using System;

namespace jack.Models
{
    public class ArchiveModel
    {
        public string Author { get; set; }
        public string Message { get; set; }
        public string links { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
