using ApplicationLayer.DTO;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratorController : ControllerBase
    {
        private readonly DatabaseServiceHandler _serviceHandler;

        public GeneratorController(DatabaseServiceHandler serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        [HttpGet("schema")]
        public ActionResult<List<TableInfo>> GetDatabaseSchema()
        {
            var schema = _serviceHandler.RetrieveDatabaseSchema();
            return Ok(schema);
        }
        [HttpGet("GetFullTables")]
        public ActionResult<List<FullTableDTO>> GetFullTable()
        {
            var full = _serviceHandler.FullTable();
            return Ok(full);
        }
        [HttpGet("generate-model/{tableName}")]
        public ActionResult<string> GenerateModel(string tableName)
        {
            var modelCode = _serviceHandler.GenerateModelForTable(tableName);
            return Ok(modelCode);
        }

        [HttpGet("download-all-models/{namespaceName}")]
        public IActionResult DownloadAllModels(string namespaceName)
        {
            var files = _serviceHandler.GenerateAllModels(namespaceName);
            var zipBytes = CreateZip(files);
            return File(zipBytes, "application/zip", "CSharpModels.zip");
        }

        private byte[] CreateZip(Dictionary<string, byte[]> files)
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var entry = archive.CreateEntry(file.Key);
                    using var entryStream = entry.Open();
                    entryStream.Write(file.Value, 0, file.Value.Length);
                }
            }
            return memoryStream.ToArray();
        }
    }
}
