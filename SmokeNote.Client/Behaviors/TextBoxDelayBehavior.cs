using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace SmokeNote.Client.Behaviors
{
    public class TextBoxDelayBehavior : Behavior<TextBox>
    {
        #region 依赖属性

        public static readonly DependencyProperty MillisecondsProperty =
            DependencyProperty.Register("Milliseconds", typeof(double), typeof(TextBoxDelayBehavior), new PropertyMetadata(500D, OnMillisecondsChanged));

        #endregion

        #region 属性

        public double Milliseconds
        {
            get { return (double)GetValue(MillisecondsProperty); }
            set { SetValue(MillisecondsProperty, value); }
        }

        public DispatcherTimer Timer
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        public TextBoxDelayBehavior()
        {
            this.Timer = new DispatcherTimer();
            this.Timer.Interval = TimeSpan.FromMilliseconds(this.Milliseconds);
            this.Timer.Tick += Timer_Tick;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            this.Timer.Stop();
            var binding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Timer.Stop();
            this.Timer.Start();
        }

        private static void OnMillisecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = sender as TextBoxDelayBehavior;
            behavior.Timer.Interval = TimeSpan.FromMilliseconds(Convert.ToDouble(e.NewValue));
        }

        #endregion
    }
}
