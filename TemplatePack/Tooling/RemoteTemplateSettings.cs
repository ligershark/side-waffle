namespace TemplatePack.Tooling {
    using LigerShark.Templates.DynamicBuilder;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RemoteTemplateSettings {
        public RemoteTemplateSettings()
            : this(new List<TemplateSource>(),UpdateFrequency.OnceAWeek) {
        }
        public RemoteTemplateSettings(List<TemplateSource> sources,UpdateFrequency updateInterval) {
            this.Sources = sources;
            this.UpdateInterval = updateInterval;
        }

        [JsonProperty("$schema")]
        public string Schema { get; set; }

        //[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        [JsonProperty("updateInterval")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UpdateFrequency? UpdateInterval { get; set; }

        [JsonProperty("sources")]
        public List<TemplateSource> Sources { get; set; }

        public static RemoteTemplateSettings ReadFromJson(string filePath) {
            if (string.IsNullOrEmpty(filePath)) { throw new ArgumentNullException("filePath"); }

            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(string.Format(@"Settings json file not found at [{0}]",filePath));
            }

            return JsonConvert.DeserializeObject<RemoteTemplateSettings>(File.ReadAllText(filePath));
        }
    }
}
