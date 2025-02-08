using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class TableInfo
    {
        public string TableName { get; set; } = string.Empty;
        public List<ColumnInfo> Columns { get; set; } = new();
    }
}
