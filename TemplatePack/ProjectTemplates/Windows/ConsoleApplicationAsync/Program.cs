using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationAsync
{
  class Program
  {
    static void Main(string[] args)
    {
      Task t = mainAsync(args);
      t.Wait();
    }

    private async static Task mainAsync(string[] args)
    {
      //Add your async code here
    }
  }
}
