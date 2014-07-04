namespace LigerShark.Templates {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for DownloadZipWindow.xaml
    /// </summary>
    public partial class DownloadZipWindow : Window {
        public DownloadZipWindow() {
            InitializeComponent();

            this.TempFolder = Environment.ExpandEnvironmentVariables(@"%APPDATA%\LigerShark\SideWaffle\Temp\DownloadZipWindow\");
        }
        public string TempFolder { get; set; }
        public string DownloadedFile { get; private set; }

        private async Task DownloadFile(string uri) {
            if (string.IsNullOrEmpty(uri)) { throw new ArgumentNullException("uri"); }

            string dest = System.IO.Path.Combine(this.TempFolder, DateTime.Now.Ticks.ToString() + ".zip");

            await new Downloader().Download(uri, dest);

            this.DownloadedFile = dest;

            this.DialogResult = true;
            this.Close();
        }

        // TODO: is it OK to use async void here? 
        // According to http://stackoverflow.com/questions/19415646/should-i-avoid-async-void-event-handlers
        // its OK.
        private async void OnNavigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e) {
            if (e != null && e.Uri != null && e.Uri.AbsoluteUri.EndsWith("/download")) {
                e.Cancel = true;
                // download the file now
                await DownloadFile(e.Uri.ToString());
            }
        }

        private async void ButtonDownload_Click(object sender, RoutedEventArgs e) {
            string url = this.CtrlTextUrl.Text;
            if (!string.IsNullOrEmpty(url)) {
                await DownloadFile(this.CtrlTextUrl.Text);
            }
            else {
                MessageBox.Show("Please enter a URL to a .zip file in the text box");
            }
        }
    }
}
