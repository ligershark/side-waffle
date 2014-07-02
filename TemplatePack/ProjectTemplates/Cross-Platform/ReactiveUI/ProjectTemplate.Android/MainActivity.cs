using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ReactiveUI;
using ReactiveUI.Android;
using Splat;
using ProjectTemplate.ViewModels;

namespace ProjectTemplate
{
    [Activity(Label = "ProjectTemplate", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ReactiveActivity<ColorPickerViewModel>
    {
        public View FinalColor
        {
            get { return this.GetControl<View>(); }
        }

        public EditText Red
        {
            get { return this.GetControl<EditText>(); }
        }

        public EditText Green
        {
            get { return this.GetControl<EditText>(); }
        }

        public EditText Blue
        {
            get { return this.GetControl<EditText>(); }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.Bind(ViewModel, x => x.Red, x => x.Red.Text);
            this.Bind(ViewModel, x => x.Green, x => x.Green.Text);
            this.Bind(ViewModel, x => x.Blue, x => x.Blue.Text);

            this.WhenAnyValue(x => x.ViewModel.FinalColor)
                .Subscribe(x => FinalColor.SetBackgroundColor(x.Value.ToNative()));

            ViewModel = new ColorPickerViewModel();
        }
    }
}
