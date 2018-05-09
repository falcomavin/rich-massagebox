using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace RichMessageBox
{
    /// <summary>
    /// Make object Run could be make in a thread
    /// </summary>
    internal class ContentDescription
    {
        public string Text { get; set; }
        public object FontSize { get; set; }
        public object FontFamily { get; set; }
        public object FontStyle { get; set; }
        public object FontWeight { get; set; }
        public object Foregroud { get; set; }
        public object Backgroud { get; set; }

        public ContentDescription(Run run)
        {
            if (run.ReadLocalValue(Run.BackgroundProperty) != DependencyProperty.UnsetValue)
                Backgroud = run.Background;

            if (run.ReadLocalValue(Run.FontSizeProperty) != DependencyProperty.UnsetValue)
                FontSize = run.FontSize;

            if (run.ReadLocalValue(Run.FontFamilyProperty) != DependencyProperty.UnsetValue)
                FontFamily = run.FontFamily;

            if (run.ReadLocalValue(Run.FontStyleProperty) != DependencyProperty.UnsetValue)
                FontStyle = run.FontStyle;

            if (run.ReadLocalValue(Run.FontWeightProperty) != DependencyProperty.UnsetValue)
                FontWeight = run.FontWeight;

            if (run.ReadLocalValue(Run.ForegroundProperty) != DependencyProperty.UnsetValue)
                Foregroud = run.Foreground;

            Text = run.Text;
        }

        public Run convertToRun()
        {
            Run run = new Run();

            if (Backgroud != null)
                run.Background = (Brush)Backgroud;

            if (FontSize != null)
                run.FontSize = (double)FontSize;

            if (FontFamily != null)
                run.FontFamily = (FontFamily)FontFamily;

            if (FontStyle != null)
                run.FontStyle = (FontStyle)FontStyle;

            if (FontWeight != null)
                run.FontWeight = (FontWeight)FontWeight;

            if (Foregroud != null)
                run.Foreground = (Brush)Foregroud;

            run.Text = Text;

            return run;
        }
    }
}
