using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SmokeNote.Client.Controls
{
    public class LoadingButton : Button
    {
        /// <summary>
        /// 图片对象
        /// </summary>
        private Image img;

        /// <summary>
        /// 是否在加载状态
        /// </summary>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingButton), new PropertyMetadata(false));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(LoadingButton), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.img = (Image)this.GetTemplateChild("Img");
            if (img != null)
            {
                var center = this.Width / 2;
                var r = (RotateTransform)img.RenderTransform;
                r.CenterX = center;
                r.CenterY = center;
            }
        }
    }
}
