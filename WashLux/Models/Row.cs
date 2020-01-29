using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashLux.Models
{
    public class Row
    {
        public int Id { get; set; }
        public string Table { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<ServiceName> Names { get; set; }
        public string Time { get; set; }
        public string Sedan { get; set; }
        public string Universal { get; set; }
        public string Jeep { get; set; }
        public string Minivan { get; set; }
        public string Van { get; set; }
        public int Pos { get; set; }
    }
}
