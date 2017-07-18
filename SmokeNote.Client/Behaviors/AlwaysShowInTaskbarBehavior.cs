using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace SmokeNote.Client.Behaviors
{
    /// <summary>
    /// 这个行为是为了解决窗口在最小化后,点击任务栏图标还原,会使任务栏图标消失的情况
    /// </summary>
    public class AlwaysShowInTaskbarBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.StateChanged += AssociatedObject_StateChanged;
        }

        void AssociatedObject_StateChanged(object sender, EventArgs e)
        {
            if (this.AssociatedObject.WindowState != WindowState.Minimized)
            {
                this.AssociatedObject.Visibility = Visibility.Visible;
            }
        }
    }
}
