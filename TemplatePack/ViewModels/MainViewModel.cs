using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LigerShark.TemplatePack.ViewModels
{
    class MainViewModel
    {
        private DelegateCommand _okCommand;
        private DelegateCommand _cancelCommand;

        public ICommand OKCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new DelegateCommand((o) => this.SaveAllSettings());
                }

                return _okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand((o) => this.CloseWindow());
                }

                return _cancelCommand;
            }
        }

        public void SaveAllSettings()
        {

        }

        public void CloseWindow()
        {
            Application.Current.MainWindow.Close();

            //var window = Application.Current.Windows.OfType<Window>().Where(w => w.Name == "OptionsWindow").FirstOrDefault();
            //if (window != null)
            //{
            //    window.Close();
            //}
        }
    }
}
