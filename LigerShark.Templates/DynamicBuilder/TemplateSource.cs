namespace LigerShark.Templates.DynamicBuilder {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TemplateSource {
        public TemplateSource() {
            Name = "unnamed";
            Enabled = true;
        }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public Uri Location { get; set; }
        
        /// <summary>
        /// Only used for git:// or http:// or https:// URI scheme values
        /// </summary>
        public string Branch { get; set; }
    }    
}
