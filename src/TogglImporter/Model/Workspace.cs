using System;

namespace TogglImporter.Model
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Premium { get; set; }
        public bool Admin { get; set; }
        public int DefaultHourlyRate { get; set; }
        public string DefaultCurrency { get; set; }
        public bool OnlyAdminsMayCreateProjects { get; set; }
        public bool OnlyAdminsSeeBillableRates { get; set; }
        public int Rounding { get; set; }
        public int RoundingMinutes { get; set; }
        public DateTime At { get; set; }
        public string LogoUrl { get; set; }
    }
}