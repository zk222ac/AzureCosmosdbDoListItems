using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosdb.Models
{
    public class TodoEntity : TableEntity
    {
        public string Content { get; set; }
        public bool Completed { get; set; }
        public string Due { get; set; }

    }
}
