using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LigerShark.SideWaffleOptions
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    class OptionsPageCustom : DialogPage
    {
        private string _sidewaffleUrl = "https://github.com/ligershark/side-waffle.git";
        private string _contosoUrl = "https://github.com/sayedihashimi/contoso-templatepack.git";

        [Category("Remote Sources")]
        [DisplayName("SideWaffle")]
        //[Description("General SideWaffle settings")]
        public string SideWaffleSourceUrl
        {
            get { return _sidewaffleUrl; }
            set { _sidewaffleUrl = value; }
        }

        [Category("Remote Sources")]
        [DisplayName("Contoso")]
        //[Description("General SideWaffle settings")]
        public string ContosoSourceUrl
        {
            get { return _contosoUrl; }
            set { _contosoUrl = value; }
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    class OptionPageGrid : DialogPage
    {
        private string _sidewaffleUrl = "https://github.com/ligershark/side-waffle.git";
        private string _contosoUrl = "https://github.com/sayedihashimi/contoso-templatepack.git";

        [Category("Remote Sources")]
        [DisplayName("SideWaffle")]
        //[Description("General SideWaffle settings")]
        public string SideWaffleSourceUrl
        {
            get { return _sidewaffleUrl; }
            set { _sidewaffleUrl = value; }
        }

        [Category("Remote Sources")]
        [DisplayName("Contoso")]
        //[Description("General SideWaffle settings")]
        public string ContosoSourceUrl
        {
            get { return _contosoUrl; }
            set { _contosoUrl = value; }
        }
    }
}
