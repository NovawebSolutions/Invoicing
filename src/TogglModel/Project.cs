using System;

namespace TogglModel
{
    public class Project
    {
        public int Id { get; set; }
        public int Wid { get; set; }
        public int Cid { get; set; }
        public string Name { get; set; }
        public bool Billable { get; set; }
        public bool IsPrivate { get; set; }
        public bool Active { get; set; }
        public bool Template { get; set; }
        public DateTime At { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Color { get; set; }
        public bool AutoEstimates { get; set; }
        public int EstimatedHours { get; set; }
        public int ActualHours { get; set; }
        public int Rate { get; set; }
    }
}