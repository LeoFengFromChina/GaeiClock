using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Framework.Common.Behaviors
{
    public class ChangeWindowStateBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// 更改类型
        /// </summary>
        public ChangeWindowType Type
        {
            get;
            set;
        }

        /// <summary>
        /// 操作的Window对象
        /// </summary>
        public Window Window
        {
            get { return (Window)GetValue(WindowProperty); }
            set { SetValue(WindowProperty, value); }
        }

        public static readonly DependencyProperty WindowProperty =
            DependencyProperty.Register("Window", typeof(Window), typeof(ChangeWindowStateBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Window == null)
            {
                return;
            }

            switch (this.Type)
            {
                case ChangeWindowType.Minimize:
                    this.Window.WindowState = WindowState.Minimized;
                    break;
                case ChangeWindowType.Maximize:
                    this.Window.WindowState = WindowState.Maximized;
                    break;
                case ChangeWindowType.MinimizeToNotifyIcon:
                    //this.Window.Activated += Window_Activated;
                    //this.Window.ShowInTaskbar = false;
                    //this.Window.WindowState = WindowState.Minimized;
                    break;
                default:
                    this.Window.Close();
                    break;
            }
        }

        void Window_Activated(object sender, EventArgs e)
        {
            this.Window.ShowInTaskbar = true;
            this.Window.Activated -= Window_Activated;
        }
    }

    /// <summary>
    /// 更改窗口的方式
    /// </summary>
    public enum ChangeWindowType
    {
        /// <summary>
        /// 最小化
        /// </summary>
        Minimize = 1,

        /// <summary>
        /// 最大化
        /// </summary>
        Maximize = 2,

        /// <summary>
        /// 关闭
        /// </summary>
        Close = 3,

        /// <summary>
        /// 最小化到托盘
        /// </summary>
        MinimizeToNotifyIcon
    }
}
