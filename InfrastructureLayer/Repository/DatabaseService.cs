using DomainLayer.Entities;
using DomainLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repository
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TableInfo> GetDatabaseTables()
        {
            List<TableInfo> tables = new();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tables.Add(new TableInfo { TableName = reader.GetString(0) });
            }
            return tables;
        }
        public List<ColumnInfo> GetTableColumns(string tableName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                    throw new ArgumentException("Table name cannot be null or empty", nameof(tableName));

                List<ColumnInfo> columns = new();
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                // دریافت اطلاعات ستون‌ها
                var query = @"
            SELECT COLUMN_NAME, DATA_TYPE 
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER(@TableName)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableName", tableName);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    columns.Add(new ColumnInfo
                    {
                        ColumnName = reader.GetString(0),
                        DataType = reader.GetString(1),
                        IsPrimaryKey = false // مقدار `Primary Key` را در مرحله بعد مقداردهی می‌کنیم
                    });
                }
                reader.Close();

                // بررسی ستون‌های Primary Key
                var pkQuery = @"
            SELECT COLUMN_NAME 
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
            WHERE TABLE_NAME = @TableName";

                using var pkCommand = new SqlCommand(pkQuery, connection);
                pkCommand.Parameters.AddWithValue("@TableName", tableName);
                using var pkReader = pkCommand.ExecuteReader();

                HashSet<string> primaryKeys = new();
                while (pkReader.Read())
                {
                    primaryKeys.Add(pkReader.GetString(0));
                }
                pkReader.Close();

                // مقداردهی `IsPrimaryKey`
                foreach (var column in columns)
                {
                    if (primaryKeys.Contains(column.ColumnName))
                    {
                        column.IsPrimaryKey = true;
                    }
                }

                return columns;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطا در دریافت ستون‌های جدول {tableName}", ex);
            }
        }


        public string GenerateCSharpModel(string tableName)
        {
            var columns = GetTableColumns(tableName);
            var sb = new StringBuilder();
            sb.AppendLine("namespace GeneratedModels");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {tableName}");
            sb.AppendLine("    {");
            foreach (var column in columns)
            {
                sb.AppendLine($"        public {MapDataType(column.DataType)} {column.ColumnName} {{ get; set; }}");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        public byte[] GenerateCSharpModelFile(string tableName, string namespaceName)
        {
            var modelCode = GenerateCSharpModel(tableName, namespaceName);
            return Encoding.UTF8.GetBytes(modelCode);
        }
        public string MapDataType(string sqlType)
        {
            return sqlType switch
            {
                "int" => "int",
                "nvarchar" => "string",
                "varchar" => "string",
                "datetime" => "DateTime",
                "bit" => "bool",
                _ => "object"
            };
        }
        public Dictionary<string, byte[]> GenerateAllModels(string namespaceName)
        {
            var tables = GetDatabaseTables();
            return tables.ToDictionary(t => t.TableName + ".cs", t => GenerateCSharpModelFile(t.TableName, namespaceName));
        }
        public string GenerateCSharpModel(string tableName, string namespaceName)
        {
            var columns = GetTableColumns(tableName);
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {tableName}");
            sb.AppendLine("    {");
            foreach (var column in columns)
            {
                sb.AppendLine($"        public {MapDataType(column.DataType)} {column.ColumnName} {{ get; set; }}");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}

