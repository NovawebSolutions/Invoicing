using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Invoicing.Model
{
    public class Client : TableEntity
    {
        public int Id { get; set; }

        public int Wid { get; set; }

        public string Name { get; set; }
    }
}
