using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace ProjectTemplate.ViewModels
{
    public class ColorPickerViewModel : ReactiveObject
    {
        int red;
        public int Red {
            get { return red; }
            set { this.RaiseAndSetIfChanged(ref red, value); }
        }

        int green;
        public int Green {
            get { return green; }
            set { this.RaiseAndSetIfChanged(ref green, value); }
        }

        int blue;
        public int Blue {
            get { return blue; }
            set { this.RaiseAndSetIfChanged(ref blue, value); }
        }

        ObservableAsPropertyHelper<Color?> finalColor;
        public Color? FinalColor {
            get { return finalColor.Value; }
        }

        public ColorPickerViewModel()
        {
            this.WhenAnyValue(x => x.Red, x => x.Green, x => x.Blue,
                    (r, g, b) => new { Red = r, Green = g, Blue = blue})
                .Where(rgb =>
                    InRange(rgb.Red) && InRange(rgb.Green) && InRange(rgb.Blue))
                .Select(rgb =>
                    (Color?)Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue))
                .ToProperty(this, x => x.FinalColor, out finalColor, Color.Black);
        }

        bool InRange(int colorVal)
        {
            return colorVal >= 0 && colorVal <= 255;
        }
    }
}