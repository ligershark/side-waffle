using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.JSON.Core.Schema;

namespace $rootnamespace$
{
    [Export(typeof(IJSONSchemaSelector))]
    internal class $safeitemname$ : IJSONSchemaSelector
    {
        private static Dictionary<string, string> _schemas = new Dictionary<string, string> 
        { 
            // Files named 'myfilepattern.json' will automatically have the schema applied to it.
            {"myfilepattern.json", "http://mywebsite.com/schemas/v1/myschema.json"},
        };

        public IEnumerable<string> GetAvailableSchemas()
        {
            // Populates the schema dropdown at the top of the JSON editor
            return _schemas.Values;
        }

        public string GetSchemaFor(string fileLocation)
        {
            string fileName = Path.GetFileName(fileLocation).ToLowerInvariant();

            // Matches the file name 'myfilepattern.json' with the correct schema file
            if (_schemas.ContainsKey(fileName))
                return _schemas[fileName];

            return null;
        }
    }
}