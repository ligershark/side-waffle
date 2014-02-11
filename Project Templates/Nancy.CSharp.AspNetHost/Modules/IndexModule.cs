using Nancy;

namespace Nancy.CSharp.AspNetHost.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => {
                return View["index"];
            };
        }
    }
}