using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Linq;
using ProjectTemplate.ViewModels;
using ReactiveUI;
using Splat;

namespace ProjectTemplate.Views
{
    /// <summary>
    /// Interaction logic for ColorPickerView.xaml
    /// </summary>
    public partial class ColorPickerView : UserControl, IViewFor<ColorPickerViewModel>
    {
        public ColorPickerView()
        {
            InitializeComponent();

            this.Bind(ViewModel, x => x.Red, x => x.Red.Text);
            this.Bind(ViewModel, x => x.Green, x => x.Green.Text);
            this.Bind(ViewModel, x => x.Blue, x => x.Blue.Text);

            this.WhenAnyValue(x => x.ViewModel.FinalColor)
                .Where(x => x != null)
                .Select(x => x.Value.ToNativeBrush())
                .BindTo(this, x => x.FinalColor.Background);
        }

        public ColorPickerViewModel ViewModel {
            get { return (ColorPickerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ColorPickerViewModel), typeof(ColorPickerView), new PropertyMetadata(null));

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (ColorPickerViewModel)value; }
        }
    }
}
