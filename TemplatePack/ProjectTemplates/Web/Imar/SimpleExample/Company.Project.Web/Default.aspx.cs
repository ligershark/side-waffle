using System;
using Company.Project.Model;

namespace Company.Project.Web
{
  public partial class Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var customer = new Customer();
    }
  }
}