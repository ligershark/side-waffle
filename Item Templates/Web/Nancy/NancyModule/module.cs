using Nancy;

namespace $rootnamespace$
{
    public class $safeitemname$ : NancyModule
    {
        public $safeitemname$()
        {
            Get["/"] = _ => "Hello!";
        }
    }
}