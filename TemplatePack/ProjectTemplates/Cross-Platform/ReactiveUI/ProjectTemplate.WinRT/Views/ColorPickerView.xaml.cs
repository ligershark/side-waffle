using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ProjectTemplate.ViewModels;
using ReactiveUI;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Reactive.Linq;
using Splat;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ProjectTemplate.Views
{
    public sealed partial class ColorPickerView : UserControl, IViewFor<ColorPickerViewModel>
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
