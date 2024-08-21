using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Storage.Blobs;
using JsonToXmlConverter.Services;

namespace JsonToXmlConverter
{
    [StorageAccount("BlobConnectionString")]
    public class JsonToXmlConverter
    {
        private readonly ILogger<JsonToXmlConverter> _logger;
        private readonly IJsonToXml _ijsonToXml;
        public JsonToXmlConverter(IJsonToXml ijsonToXml)
        {
            this._ijsonToXml = ijsonToXml;
        }     
        public JsonToXmlConverter(ILogger<JsonToXmlConverter> logger)
        {
            _logger = logger;
        }

        [Function(nameof(JsonToXmlConverter))]
        public void Run([Microsoft.Azure.Functions.Worker.BlobTrigger("jsonfiles/{name}")] Stream inputfile,
            [Blob("xmlfiles/{name}", FileAccess.Write)] Stream outputfile,
            string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Size: {inputfile.Length} Bytes");
            try
            {
                this._ijsonToXml.Convert(inputfile, outputfile);
                _logger.LogInformation("Json File converted into XML file");
            }
            catch (Exception e)
            {
                _logger.LogError("Conversion Failed : ", e);
            }
        }
    }
}
