using ApplicationLayer.DTO;
using DomainLayer.Entities;
using InfrastructureLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class DatabaseServiceHandler
    {
        private readonly DatabaseService _databaseService;

        public DatabaseServiceHandler(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<TableInfo> RetrieveDatabaseSchema()
        {
            return _databaseService.GetDatabaseTables();
        }

        public string GenerateModelForTable(string tableName)
        {
            return _databaseService.GenerateCSharpModel(tableName);
        }
        public string GenerateCSharpModel(string tableName, string namespaceName)
        {
            return _databaseService.GenerateCSharpModel(tableName, namespaceName);
        }
        public byte[] GenerateCSharpModelFile(string tableName, string namespaceName)
        {
            return _databaseService.GenerateCSharpModelFile(tableName,namespaceName);
        }
        public Dictionary<string, byte[]> GenerateAllModels(string namespaceName)
        {
            return _databaseService.GenerateAllModels(namespaceName);
        }

        public List<FullTableDTO> FullTable()
        {
            try
            {
                List<FullTableDTO> fullTables = new List<FullTableDTO>();
                var table = _databaseService.GetDatabaseTables();
                foreach(var tb in table)
                {
                    List<ColumnInfo> columns = _databaseService.GetTableColumns(tb.TableName);
                    var ft = new FullTableDTO()
                    {
                        Columns = columns,
                        TableInfo = tb
                    };
                    fullTables.Add(ft);
                }
                return fullTables;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error retrieving table data", ex);
            }
        }
    }
}
