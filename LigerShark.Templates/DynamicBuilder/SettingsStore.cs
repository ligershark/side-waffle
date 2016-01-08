using Newtonsoft.Json;
using System.IO;
using System;

namespace LigerShark.Templates.DynamicBuilder
{
    public class SettingsStore
    {
        [JsonProperty]
        public bool SendTelemetry { get; set; }

        public static SettingsStore ReadJsonFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) { throw new ArgumentNullException("filePath"); }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format(@"JSON settings file not found at [{0}]", filePath));
            }

            return JsonConvert.DeserializeObject<SettingsStore>(File.ReadAllText(filePath));
        }

        public void WriteJsonFile(string fileDirectory, string filePath, string json)
        {
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            File.WriteAllText(filePath, json);
        }
    }
}