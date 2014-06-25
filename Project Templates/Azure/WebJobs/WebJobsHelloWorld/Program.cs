using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Jobs;

namespace WebJobsHelloWorld
{
    // To learn more about Windows Azure WebJobs start here http://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connectionstring values in app.config
        // AzureJobsDashboard and AzureJobsStorage
        private static void Main()
        {
            var host = new JobHost();
            host.RunAndBlock();
        }

        public static void ProcessQueueMessage(
            [QueueTrigger("webjobsqueue")] string inputText,
            [Blob("containername/blobname")] TextWriter writer)
        {
            writer.WriteLine(inputText);
        }
    }
}