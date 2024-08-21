using JsonToXmlConverter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace JsonToXmlConverter.Services
{
    public class JsonToXml : IJsonToXml
    {
        public void Convert(Stream input, Stream output)
        {
            try
            {
                // Read JSON content from the input stream
                using var jsonDocument = JsonDocument.Parse(input);
                var rootElement = jsonDocument.RootElement;

                // Create an XML writer
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = false
                };

                using var xmlWriter = XmlWriter.Create(output, settings);

                // Start writing the XML
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("root"); // You can change the root element name

                // Convert JSON properties to XML elements
                ConvertJsonObjectToXml(rootElement, xmlWriter);

                // End the XML document
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., invalid JSON format)
                Console.WriteLine($"Error converting JSON to XML: {ex.Message}");
            }
        }

        private void ConvertJsonObjectToXml(JsonElement jsonElement, XmlWriter xmlWriter)
        {
            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in jsonElement.EnumerateObject())
                {
                    xmlWriter.WriteStartElement(property.Name);
                    ConvertJsonObjectToXml(property.Value, xmlWriter);
                    xmlWriter.WriteEndElement();
                }
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in jsonElement.EnumerateArray())
                {
                    xmlWriter.WriteStartElement("item");
                    ConvertJsonObjectToXml(item, xmlWriter);
                    xmlWriter.WriteEndElement();
                }
            }
            else if (jsonElement.ValueKind != JsonValueKind.Null)
            {
                xmlWriter.WriteString(jsonElement.ToString());
            }
        }
    }
}