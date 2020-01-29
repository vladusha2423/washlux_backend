using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashLux.Models
{
    public class ServiceName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Row Row { get; set; }
        public int RowId {get; set;}
    }
}
