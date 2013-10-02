<%@ WebHandler Language="C#" Class="feed" %>

using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;

public class feed : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string manifest = context.Server.MapPath("~/feed/web/extension.vsixmanifest");
        string vsix = context.Server.MapPath("~/feed/web/webessentials2012.vsix");
        DateTime updated = File.GetLastAccessTimeUtc(vsix);
        
        XmlDocument doc = new XmlDocument();
        doc.Load(manifest);
                
        XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);
        xnm.AddNamespace("t", "http://schemas.microsoft.com/developer/vsx-schema/2011");
        xnm.AddNamespace("d", "http://schemas.microsoft.com/developer/vsx-schema-design/2011");

        XmlNode idNode = doc.SelectSingleNode("//t:Identity", xnm);
        string version = null;

        if (idNode != null && idNode.Attributes["Version"] != null)
        {
            version = idNode.Attributes["Version"].InnerText;
        }
         
        context.Response.ContentType = "text/html";

        WriteXml(context, version, updated);
    }

    private void WriteXml(HttpContext context, string version, DateTime updated)
    {
        using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
        {
            writer.WriteStartElement("feed", "http://www.w3.org/2005/Atom");

            writer.WriteElementString("title", "SideWaffle for Web");
            writer.WriteElementString("id", "e6e2a48e-387d-4af2-9072-86a5276da6d4");
            writer.WriteElementString("updated", updated.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            writer.WriteStartElement("entry");

            writer.WriteElementString("id", "e6e2a48e-387d-4af2-9072-86a5276da6d4");
            
            writer.WriteStartElement("title");
            writer.WriteAttributeString("type", "text");
            writer.WriteValue("SideWaffle for Web");            
            writer.WriteEndElement(); // title

            writer.WriteStartElement("summary");
            writer.WriteAttributeString("type", "text");
            writer.WriteValue("SideWaffle for Web");            
            writer.WriteEndElement(); // summary
            
            writer.WriteElementString("published", "2012-09-01T18:51:00-07:00");
            writer.WriteElementString("updated", updated.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            writer.WriteStartElement("author");
            writer.WriteElementString("name", "SideWaffle.com");
            writer.WriteEndElement(); // author

            writer.WriteStartElement("category");
            writer.WriteAttributeString("term", "SideWaffle");
            writer.WriteEndElement(); // category
            
            writer.WriteStartElement("content");
            writer.WriteAttributeString("type", "application/octet-stream");
            writer.WriteAttributeString("src", "http://sidewaffle.com/feed/web/templatepack.vsix");
            writer.WriteEndElement(); // content

            writer.WriteRaw("<Vsix xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.microsoft.com/developer/vsx-syndication-schema/2010\">");
            writer.WriteElementString("Id", "e6e2a48e-387d-4af2-9072-86a5276da6d4");
            writer.WriteElementString("Version", version);
            
            writer.WriteStartElement("References");
            writer.WriteEndElement();
            
            writer.WriteRaw("</Vsix>");// Vsix
            
            writer.WriteEndElement(); // entry

            writer.WriteEndElement(); // feed
        }

        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}