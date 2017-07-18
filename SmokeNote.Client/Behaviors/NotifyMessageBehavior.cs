using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace SmokeNote.Client.Behaviors
{
    public class NotifyMessageBehavior : Behavior<FrameworkElement>
    {
        private Storyboard NotifyStoryboard
        {
            get;
            set;
        }

        public NotifyMessageBehavior()
        {
            this.NotifyStoryboard = new Storyboard();
            this.NotifyStoryboard.Completed += NotifyStoryboard_Completed;
        }

        void NotifyStoryboard_Completed(object sender, EventArgs e)
        {
            this.AssociatedObject.Visibility = Visibility.Collapsed;
            this.AssociatedObject.Opacity = 1;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            var doubleAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(3)));
            doubleAnimation.SetValue(Storyboard.TargetProperty, this.AssociatedObject);
            doubleAnimation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));
            this.NotifyStoryboard.Children.Add(doubleAnimation);

            this.AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
        }

        void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.AssociatedObject.IsVisible)
            {
                this.NotifyStoryboard.Begin();
            }
        }
    }
}
