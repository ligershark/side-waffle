using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;

namespace $rootnamespace$
{
    public class $safeitemname$ : TableServiceEntity
    {
        public $safeitemname$()
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = "default";
        }

        public string PropertyA { get; set; }
        public DateTime PropertyB { get; set; }
        public int PropertyC { get; set; }
    }
}