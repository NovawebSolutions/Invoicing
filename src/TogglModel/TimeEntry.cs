using System;
using Newtonsoft.Json;

namespace TogglModel
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int Wid { get; set; }
        public int Pid { get; set; }
        public bool Billable { get; set; }
        public DateTime Start { get; set; }
        public DateTime? Stop { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        [JsonProperty("duronly")]
        public bool DurationOnly { get; set; }
        public DateTime At { get; set; }
        public int Uid { get; set; }
    }
}