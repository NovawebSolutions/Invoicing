using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Invoicing.Model
{
    public class Project : TableEntity
    {
        public int Id { get; set; }
        public int Wid { get; set; }
        public int Cid { get; set; }
        public string Name { get; set; }
        public bool Billable { get; set; }
        public bool IsPrivate { get; set; }
        public bool Active { get; set; }
        public int Rate { get; set; }
    }
}