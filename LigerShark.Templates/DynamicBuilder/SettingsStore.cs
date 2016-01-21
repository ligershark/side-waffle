using Newtonsoft.Json;
using System.IO;
using System;
using System.ComponentModel;

namespace LigerShark.Templates.DynamicBuilder
{
    public class SettingsStore
    {
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
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

        public void WriteJsonFile(string filePath, string json)
        {
            var fileInfo = new FileInfo(filePath);
            
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            File.WriteAllText(filePath, json);
        }
    }
}