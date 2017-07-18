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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmokeNote.Client.Controls
{
    /// <summary>
    /// NotifyControl.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyControl : UserControl
    {
        Storyboard notifyStoryboard;

        public NotifyControl()
        {
            InitializeComponent();
            this.Loaded += NotifyControl_Loaded;
        }

        void NotifyControl_Loaded(object sender, RoutedEventArgs e)
        {
            notifyStoryboard = this.Resources["NotifyStoryboard"] as Storyboard;
        }

        public void ShowNotifyMessage(string message)
        {
            //if (notifyStoryboard.GetCurrentState() != ClockState.Stopped)
            //{
            //    notifyStoryboard.Stop();
            //    bdContainer.Opacity = 0;
            //}

            notifyStoryboard.Stop();
            this.bdContainer.Opacity = 0;

            this.tbMessage.Text = message;
            notifyStoryboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            
        }
    }
}
