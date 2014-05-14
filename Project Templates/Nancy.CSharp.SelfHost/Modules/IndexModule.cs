using Nancy;

namespace Nancy.CSharp.SelfHost.Modules
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