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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Threading;

namespace RichMessageBox
{
    /// <summary>
    /// MBox.xaml 的交互逻辑
    /// </summary>
    public partial class MBox : Window
    {
        private int _headImageSpanCnt = 1;
        private int _limitSec = 0;
        static private MBoxResult _result = MBoxResult.Yes;
        static public bool PlayAnimation { get; set; }
        static public Lang Lang { get; set; }

        public MBox()
        {
            InitializeComponent();

            PlayAnimation = true;
            Lang = Lang.EN_US;
        }

        #region Show
        static public MBoxResult Show(string content)
        {
            string strTitle = "提示";
            string strOk = "确定";
            if (Lang == Lang.ZH_TW)
            {
                strTitle = "提示";
                strOk = "確定";
            }
            else if (Lang == Lang.EN_US)
            {
                strTitle = "Info";
                strOk = "OK";
            }
            return doShow(strTitle, new Run[] { new Run(content) }, new string[] { strOk }, MBoxType.Normal, 0);
        }

        static public MBoxResult Show(string title, string content)
        {
            string strOk = "确定";
            if (Lang == Lang.ZH_TW)
                strOk = "確定";
            else if (Lang == Lang.EN_US)
                strOk = "OK";
            return doShow(title, new Run[] { new Run(content) }, new string[] { strOk }, MBoxType.Normal, 0);
        }

        static public MBoxResult Show(string title, string content, MBoxType type)
        {
            string strOk = "确定";
            if (Lang == Lang.ZH_TW)
                strOk = "確定";
            else if (Lang == Lang.EN_US)
                strOk = "OK";
            return doShow(title, new Run[] { new Run(content) }, new string[] { strOk }, type, 0);
        }

        static public MBoxResult Show(string title, string content, string[] operators)
        {
            return doShow(title, new Run[] { new Run(content) }, operators, MBoxType.Normal, 0);
        }

        static public MBoxResult Show(string title, string content, string[] operators, MBoxType type)
        {
            return doShow(title, new Run[] { new Run(content) }, operators, type, 0);
        }

        static public MBoxResult Show(string title, Run[] contents)
        {
            string strOk = "确定";
            if (Lang == Lang.ZH_TW)
                strOk = "確定";
            else if (Lang == Lang.EN_US)
                strOk = "OK";
            return doShow(title, contents, new string[] { strOk }, MBoxType.Normal, 0);
        }

        static public MBoxResult Show(string title, Run[] contents, MBoxType type)
        {
            string strOk = "确定";
            if (Lang == Lang.ZH_TW)
                strOk = "確定";
            else if (Lang == Lang.EN_US)
                strOk = "OK";
            return doShow(title, contents, new string[] { strOk }, type, 0);
        }

        static public MBoxResult Show(string title, Run[] contents, string[] operators)
        {
            return doShow(title, contents, operators, MBoxType.Normal, 0);
        }

        static public MBoxResult Show(string title, Run[] contents, string[] operators, MBoxType type)
        {
            return doShow(title, contents, operators, type, 0);
        }

        static public MBoxResult Show(string title, Run[] contents, string[] operators, MBoxType type, int limitSec)
        {
            return doShow(title, contents, operators, type, limitSec);
        }

        static private MBoxResult doShow(string title, Run[] contents, string[] operators, MBoxType type, int limitSec)
        {
            MBoxResult ret = MBoxResult.Yes;
            List<ContentDescription> lsContents = new List<ContentDescription>();
            foreach (var run in contents)
            {
                ContentDescription cd = new ContentDescription(run);
                lsContents.Add(cd);
            }

            Application.Current.Dispatcher.Invoke(new Action(delegate
            {
                MBox mbox = new MBox();
                mbox.setType(mbox, type);
                mbox.tbk_content.Text = "";
                mbox.tbk_head.Text = "";

                if (title == null || contents == null || operators == null)
                    mbox.ShowDialog();

                if (operators.Length == 1)
                {
                    mbox.grid_ret2.Visibility = mbox.grid_ret3.Visibility = Visibility.Hidden;
                    mbox.grid_ret1.Visibility = Visibility.Visible;
                    mbox.btn_ret1_yes.Content = operators[0];
                }
                else if (operators.Length == 2)
                {
                    mbox.grid_ret1.Visibility = mbox.grid_ret3.Visibility = Visibility.Hidden;
                    mbox.grid_ret2.Visibility = Visibility.Visible;
                    mbox.btn_ret2_yes.Content = operators[0];
                    mbox.btn_ret2_no.Content = operators[1];
                }
                else if (operators.Length == 3)
                {
                    mbox.grid_ret1.Visibility = mbox.grid_ret2.Visibility = Visibility.Hidden;
                    mbox.grid_ret3.Visibility = Visibility.Visible;
                    mbox.btn_ret3_yes.Content = operators[0];
                    mbox.btn_ret3_no.Content = operators[1];
                    mbox.btn_ret3_cancel.Content = operators[2];
                }

                mbox.tbk_head.Text = title;
                foreach (var r in lsContents)
                {
                    mbox.tbk_content.Inlines.Add(r.convertToRun());
                }

                if (limitSec > 0)
                {
                    mbox._limitSec = limitSec;
                    WaitCallback workItem = new WaitCallback(td_timer);
                    ThreadPool.QueueUserWorkItem(workItem, mbox);
                }

                mbox.ShowDialog();
                ret = _result;
            }));

            return ret;
        }

        private static void td_timer(object obj)
        {
            MBox mbox = obj as MBox;
            Button btn = null;
            string btnOriginStr = "";
            mbox.Dispatcher.Invoke(new Action(delegate
            {
                if (mbox.grid_ret1.Visibility == Visibility.Visible)
                    btn = mbox.btn_ret1_yes;
                else if (mbox.grid_ret2.Visibility == Visibility.Visible)
                    btn = mbox.btn_ret2_yes;
                else if (mbox.grid_ret3.Visibility == Visibility.Visible)
                    btn = mbox.btn_ret3_yes;
                btnOriginStr = btn.Content.ToString();
            }));

            while (mbox._limitSec > 0)
            {
                mbox.Dispatcher.Invoke(new Action(delegate
                {
                    if (mbox.IsActive)
                    {
                        btn.Content = btnOriginStr + "(" + mbox._limitSec.ToString() + ")";
                    }
                }));

                System.Threading.Thread.Sleep(1000);
                --mbox._limitSec;
            }

            mbox.Dispatcher.Invoke(new Action(delegate
            {
                if (mbox.IsActive)
                {
                    MBox._result = MBoxResult.Yes;
                    mbox.Close();
                }
            }));
        }
        #endregion

        #region beautify
        void da_Completed(object sender, EventArgs e)
        {
            Action action = (() =>
            {
                try
                {
                    System.Threading.Thread.Sleep(3000);

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DoubleAnimation da = new DoubleAnimation();
                        da.Duration = new Duration(TimeSpan.FromSeconds(1.2));
                        da.From = 0.0;
                        int itmp = _headImageSpanCnt++ % 5;
                        if (itmp == 0)
                            da.To = 720;
                        else if (itmp <= 3)
                            da.To = 360;
                        else
                            da.To = 1260;
                        da.DecelerationRatio = 0.75;
                        da.Completed += da_Completed;
                        this.aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da, HandoffBehavior.SnapshotAndReplace);
                    }));
                }
                catch (Exception )
                {
                }

            });
            action.BeginInvoke(null, null);
        }

        private void setType(MBox mBox, MBoxType type)
        {
            if (type == MBoxType.Normal)
            {
                /* icon */
                mBox.image_1.Source = new BitmapImage(new Uri("./Resources/MessageBoxLight.png", UriKind.RelativeOrAbsolute));
                mBox.image_2.Source = new BitmapImage(new Uri("./Resources/MessageBoxLight.png", UriKind.RelativeOrAbsolute));

                /* color up */
                LinearGradientBrush lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(0, 0);
                lgb.EndPoint = new Point(0, 1);
                lgb.RelativeTransform = new RotateTransform(270);
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF5DC2EC"), 0.0));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF16AAE8"), 0.261));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FEECF5FE"), 0.884));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFBFDFF"), 1.0));
                grid_up.Background = lgb;

                /* color mid */
                lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(0, 0);
                lgb.EndPoint = new Point(0, 1);
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFEEECD3"), 0.0));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFFE89C"), 1.0));
                grid_mid.Background = lgb;

                /* color down */
                lgb = new LinearGradientBrush();
                lgb.StartPoint = new Point(0, 0);
                lgb.EndPoint = new Point(0, 1);
                lgb.RelativeTransform = new RotateTransform(270);
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFFCFCFC"), 0.0));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF42B4E4"), 0.421));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF72CBF0"), 0.642));
                lgb.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFF0F0F0"), 1.0));
                grid_down.Background = lgb;

                /* color others*/
                shadow.Color = (Color)ColorConverter.ConvertFromString("#FF181819");
                tbk_head.Foreground = Brushes.White;

                /* animation */
                if (PlayAnimation)
                {
                    /* icon span */
                    DoubleAnimation da = new DoubleAnimation();
                    da.Duration = new Duration(TimeSpan.FromSeconds(1.2));
                    da.From = 0.0;
                    da.To = 1260;
                    da.DecelerationRatio = 0.75;
                    da.Completed += da_Completed;
                    this.aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da, HandoffBehavior.SnapshotAndReplace);
                }
            }
            else if (type == MBoxType.Alarm)
            {
                mBox.image_1.Source = new BitmapImage(new Uri("./Resources/alarm170619.png", UriKind.RelativeOrAbsolute));
                mBox.image_2.Source = new BitmapImage(new Uri("./Resources/alarm170619.png", UriKind.RelativeOrAbsolute));

                /* color */
                grid_up.Background = convertStrToSolidColor("#FFFFE538");
                grid_down.Background = convertStrToSolidColor("#FFFFE538");
                grid_mid.Background = convertStrToSolidColor("#FFFFFEF7");
                tbk_head.Foreground = convertStrToSolidColor("#FFFF001D");
                shadow.Color = (Color)ColorConverter.ConvertFromString("#FFFF4B00");

                if (PlayAnimation)
                {
                    /* background shadow */
                    DoubleAnimation dAnimation = new DoubleAnimation();
                    dAnimation.From = 20.0;
                    dAnimation.To = 100.0;
                    dAnimation.AutoReverse = true;
                    dAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    dAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                    shadow.BeginAnimation(System.Windows.Media.Effects.DropShadowEffect.BlurRadiusProperty, dAnimation, HandoffBehavior.SnapshotAndReplace);

                    /* icon span */
                    DoubleAnimation da = new DoubleAnimation();
                    da.Duration = new Duration(TimeSpan.FromSeconds(1.2));
                    da.From = 0.0;
                    da.To = 1260;
                    da.DecelerationRatio = 0.75;
                    da.Completed += da_Completed;
                    this.aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da, HandoffBehavior.SnapshotAndReplace);
                }
            }
        }
        #endregion

        #region button click event
        private void btn_ret1_yes_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.Yes;
            Close();
        }

        private void btn_ret2_yes_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.Yes;
            Close();
        }

        private void btn_ret2_no_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.No;
            Close();
        }

        private void btn_ret3_yes_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.Yes;
            Close();
        }

        

        private void btn_ret3_no_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.No;
            Close();
        }

        private void btn_ret3_cancel_Click(object sender, RoutedEventArgs e)
        {
            _result = MBoxResult.Cancel;
            Close();
        }
        #endregion

        static private SolidColorBrush convertStrToSolidColor(string str)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(str));
        }
    }
}
