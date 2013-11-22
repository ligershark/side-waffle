using Microsoft.VisualStudio.Web.BrowserLink;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace $safeprojectname$
{
    [Export(typeof(IBrowserLinkExtensionFactory))]
    public class MyExtensionFactory : IBrowserLinkExtensionFactory
    {
        public BrowserLinkExtension CreateExtensionInstance(BrowserLinkConnection connection)
        {
            return new MyExtension();
        }

        public string GetScript()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("$safeprojectname$.Scripts.$safeprojectname$Extension.js"))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    public class MyExtension : BrowserLinkExtension
    {
        public override void OnConnected(BrowserLinkConnection connection)
        {
            Browsers.Client(connection).Invoke("greeting", "Hello from Visual Studio!");
        }

        [BrowserLinkCallback] // This method can be called from JavaScript
        public void SendText(string message)
        {
            MessageBox.Show(message);
        }
    }
}