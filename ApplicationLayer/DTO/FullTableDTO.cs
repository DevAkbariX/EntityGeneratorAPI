using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTO
{
    public class FullTableDTO
    {
        public TableInfo TableInfo { get; set; }
        public List<ColumnInfo> Columns { get; set; }
    }
}