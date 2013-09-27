using Microsoft.VisualStudio.Web.BrowserLink;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace $rootnamespace$
{
    [Export(typeof(IBrowserLinkExtensionFactory))]
    public class $safeitemname$Factory : IBrowserLinkExtensionFactory
    {
        public BrowserLinkExtension CreateExtensionInstance(BrowserLinkConnection connection)
        {
            return new $safeitemname$();
        }

        public string GetScript()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("$rootnamespace$.$safeitemname$.js"))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public class $safeitemname$ : BrowserLinkExtension
    {
        public override void OnConnected(BrowserLinkConnection connection)
        {
            Browsers.Client(connection).Invoke("greeting", "Hello from Visual Studio!");
        }

        [BrowserLinkCallback] // This method can be called from JavaScript
        public void SendText(string message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}