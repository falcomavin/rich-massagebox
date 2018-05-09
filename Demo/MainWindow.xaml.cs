using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RichMessageBox;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MBox.Show("this is a normal mbox");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MBox.Show("Alert", "this is a alert mbox", MBoxType.Alarm);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MBox.Show("tips", "you can diy the chose option button content", new string[] { "Yes!" });  
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MBox.Show("tips", "two option: one for MBoxResult.Yes, one for MBoxResult.No", new string[]{"yes", "no"});
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MBox.Show("tips", "three option: the third option is for MBoxResult.Cancel", new string[] { "yes", "no", "cancel" });
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MBox.Show("tips", new Run[] { new Run("you can set the count down sec") }, new string[] { "ok", "ignore" }, MBoxType.Alarm, 10);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MBox.Show("Bingo", new Run[] { new Run() { Text = "font, ", Foreground = Brushes.Red, FontSize = 25 }, new Run() { Text = "size, ", Foreground = Brushes.Green, FontSize = 28 }, new Run() { Text = "color, ", Foreground = Brushes.Blue, FontSize = 35 }, new Run() { Text = "...is all ", Foreground = Brushes.Black, FontSize = 24, FontFamily = new FontFamily("Segoe Script") }, new Run() { Text = "up to you", Foreground = Brushes.Red, FontSize = 30, FontWeight = FontWeights.Bold } }, MBoxType.Normal);
        }


    }
}
