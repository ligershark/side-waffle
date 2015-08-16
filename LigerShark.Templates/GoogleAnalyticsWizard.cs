using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LigerShark.Templates
{
    public class GoogleAnalyticsWizard : IWizard
    {
        private string TemplateID { get; set; }
        private string TemplateName { get; set; }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        // After the project template is created, send template ID to Google
        public void ProjectFinishedGenerating(Project project)
        {

        }

        // After the item template is created, send template ID to Google
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {
            TrackTemplate(TemplateID, TemplateName);
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {

            TemplateName = replacementsDictionary["$TemplateName$"];
            TemplateID = replacementsDictionary["$TemplateID$"];
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private void TrackTemplate(string templateID, string templateName)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.google-analytics.com/collect");
            request.Method = "POST";

            // the request body we want to send
            var postData = new Dictionary<string, string>
                           {
                               { "v", "1" },
                               { "tid", "UA-62483606-3" },
                               { "cid", "555" },
                               { "t", "event" },
                               { "ec", "template" },
                               { "ea", "add" },
                               { "el", templateName },
                               { "ev", templateID }
                           };
            var postDataString = postData
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key, HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }
            }
            catch (Exception ex)
            {
                // log the exception
                System.Windows.MessageBox.Show("Failed to send data to Google Analytics\n" + ex.Message);

            }
        }
    }
}