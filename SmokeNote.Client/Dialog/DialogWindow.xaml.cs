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

namespace SmokeNote.Client.Dialog
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow(string caption, string description)
        {
            InitializeComponent();

            this.Caption = caption;
            this.Description = description;
            this.Owner = Application.Current.MainWindow;
        }

        #region 依赖属性

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(DialogWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(DialogWindow), new PropertyMetadata(null));

        #endregion

        #region 属性

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        #endregion

        #region 方法

        public static DialogWindow CreateAlertWindow(string title, string content, Action callback)
        {
            var window = new DialogWindow(title, content);
            window.btnCancel.Visibility = Visibility.Collapsed;
            window.Closed += (sender, e) => 
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            };
            return window;
        }

        public static DialogWindow CreateConfirmWindow(string title, string content, Action<bool> callback)
        {
            var window = new DialogWindow(title, content);
            window.btnCancel.Visibility = Visibility.Visible;
            window.Closed += (sender, e) =>
            {
                if (callback != null)
                {
                    callback.Invoke(window.DialogResult == true);
                }
            };
            return window;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        #endregion
    }
}
