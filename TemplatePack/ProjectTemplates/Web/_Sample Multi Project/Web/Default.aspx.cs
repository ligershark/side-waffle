using My.Cool.Company.SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace $safeprojectname$ {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Contact contact = new Contact {
                Firstname = "Sayed",
                Lastname = "Hashimi"
            };
        }
    }
}