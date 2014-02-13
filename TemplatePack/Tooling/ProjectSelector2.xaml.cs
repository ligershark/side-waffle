using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace TemplatePack
{
    public partial class ProjectSelector : Window
    {
        private IEnumerable<string> _projectNames;

        public ProjectSelector(IEnumerable<string> projectNames)
        {
            InitializeComponent();

            _projectNames = projectNames;
            
            if (projectNames.Any())
            {
                names.ItemsSource = _projectNames;
                names.SelectedIndex = 0;
            }
            else
            {
                names.IsEnabled = false;
                btnOk.IsEnabled = false;
            }
        }

        public string SelectedProjectName { get; set; }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedProjectName = names.SelectedItem.ToString();
            DialogResult = true;
        }
    }
}
