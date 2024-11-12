using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static void Main(string[] args)
    {
        // Ask the user for the directory containing XML and JSON files
        Console.Write("Please enter the path to the directory containing XML and JSON files: ");
        string configDirectory = Console.ReadLine();

        // Read all XML and JSON files from the directory
        var files = Directory.EnumerateFiles(configDirectory, "*.xml").Concat(Directory.EnumerateFiles(configDirectory, "*.json")).ToList();

        // Prepare data structure for ConfigMap
        var configMap = new ConfigMap
        {
            ApiVersion = "v1",
            Kind = "ConfigMap",
            Metadata = new Metadata { Name = "my-config-map" },
            Data = new Dictionary<string, string>()
        };

        foreach (var file in files)
        {
            string fileContent = File.ReadAllText(file);
            string fileName = Path.GetFileName(file);
            configMap.Data[fileName] = fileContent;
        }

        // Serialize the config map to YAML
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        string yaml = serializer.Serialize(configMap);

        // Save the output to configmap.yaml
        string outputPath = Path.Combine(configDirectory, "configmap.yaml");
        File.WriteAllText(outputPath, yaml);

        Console.WriteLine("ConfigMap YAML has been created at: " + outputPath);
    }
}

public class ConfigMap
{
    public string ApiVersion { get; set; }
    public string Kind { get; set; }
    public Metadata Metadata { get; set; }
    public Dictionary<string, string> Data { get; set; }
}

public class Metadata
{
    public string Name { get; set; }
}
