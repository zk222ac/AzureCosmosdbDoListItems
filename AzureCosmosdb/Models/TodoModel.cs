using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosdb.Models
{
    public class TodoModel
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Content { get; set; }
        public string Due { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

    }
}
