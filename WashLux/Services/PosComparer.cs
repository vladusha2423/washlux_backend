using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WashLux.Models;

namespace WashLux.Services
{
    public class PosComparer: IComparer<RowDto>
    {
        public int Compare(RowDto firstObj, RowDto secondObj)
        {
            return firstObj.Pos.CompareTo(secondObj.Pos);
        }
    }
}
