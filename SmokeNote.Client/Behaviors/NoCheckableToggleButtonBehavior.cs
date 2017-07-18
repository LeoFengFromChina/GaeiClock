using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace SmokeNote.Client.Behaviors
{
    /// <summary>
    /// 不允许ToggleButton选中的行为
    /// 主要用于编辑器中,避免重复写Style
    /// </summary>
    public class NoCheckableToggleButtonBehavior : Behavior<ToggleButton>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Checked += AssociatedObject_Checked;
        }

        void AssociatedObject_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.AssociatedObject.IsChecked = false;
        }
    }
}
