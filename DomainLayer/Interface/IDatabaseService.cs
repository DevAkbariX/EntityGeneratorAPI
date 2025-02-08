using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interface
{
    public interface IDatabaseService
    {
        List<TableInfo> GetDatabaseTables();
        List<ColumnInfo> GetTableColumns(string tableName);
        string GenerateCSharpModel(string tableName);
        string MapDataType(string sqlType);
        byte[] GenerateCSharpModelFile(string tableName, string namespaceName);
        string GenerateCSharpModel(string tableName, string namespaceName);
        Dictionary<string, byte[]> GenerateAllModels(string namespaceName);
        
    }
}
