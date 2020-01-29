using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashLux.Models;

namespace WashLux.Converters
{
    public class RowConverter
    {
        public static RowDto RowConvert(Row item)
        {
            return new RowDto
            {
                Id = item.Id,
                Table = item.Table,
                Type = item.Type,
                Name = item.Name,
                Names = new List<string>(),
                Time = item.Time,
                Sedan = item.Sedan,
                Universal = item.Universal,
                Jeep = item.Jeep,
                Minivan = item.Minivan,
                Van = item.Van,
                Pos = item.Pos
            };
        }
        public static Row RowConvert(RowDto item)
        {
            var serviceNames = new List<ServiceName>();
            if (item.Names != null && item.Names.Count != 0)
                foreach (var name in item.Names)
                    serviceNames.Add(new ServiceName { Name = name });
            return new Row
            {
                Id = item.Id,
                Table = item.Table,
                Type = item.Type,
                Name = item.Name,
                Names = serviceNames,
                Time = item.Time,
                Sedan = item.Sedan,
                Universal = item.Universal,
                Jeep = item.Jeep,
                Minivan = item.Minivan,
                Van = item.Van,
                Pos = item.Pos
            };
        }
        public static List<RowDto> RowConvert(List<Row> items)
        {
            return items.Select(a => RowConvert(a)).ToList();
        }
    }
}
