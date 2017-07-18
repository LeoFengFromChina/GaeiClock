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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SmokeNote.Client.Controls
{
    /// <summary>
    /// RichTextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class RichTextEditor : UserControl
    {
        public RichTextEditor()
        {
            InitializeComponent();

            this.DataContextChanged += RichTextEditor_DataContextChanged;
            this.Loaded += RichTextEditor_Loaded;
            this.IsEnabledChanged += RichTextEditor_IsEnabledChanged;
        }

        

        #region 依赖属性

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(RichTextEditor), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowEditorToolbarProperty =
            DependencyProperty.Register("ShowEditorToolbar", typeof(bool), typeof(RichTextEditor), new PropertyMetadata(true, OnShowEditorToolbarChanged));

        private static void OnShowEditorToolbarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var editor = sender as RichTextEditor;
            editor.ChangeToolbarVisibility();
        }

        #endregion

        #region 属性

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool ShowEditorToolbar
        {
            get { return (bool)GetValue(ShowEditorToolbarProperty); }
            set { SetValue(ShowEditorToolbarProperty, value); }
        }

        #endregion

        #region 方法

        void RichTextEditor_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ChangeToolbarVisibility();
        }

        private void ChangeToolbarVisibility()
        {
            if (this.ShowEditorToolbar && this.IsEnabled == true)
            {
                toolbar.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                toolbar.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private TextRange CreateTextRange()
        {
            var range = new TextRange(this.rtbContent.Selection.Start, this.rtbContent.Selection.End);
            return range;            
        }

        private void rtbContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                var binding = rtbContent.GetBindingExpression(Xceed.Wpf.Toolkit.RichTextBox.TextProperty);
                binding.UpdateSource();
            }
        }

        private void rtbContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var range = this.CreateTextRange();

            #region FontFamily

            var fontFamily = range.GetPropertyValue(TextBlock.FontFamilyProperty) as FontFamily;
            cbxFontFamily.SelectionChanged -= cbxFontFamily_SelectionChanged;
            if (fontFamily != null)
            {
                cbxFontFamily.SelectedValue = fontFamily.ToString();                
            }
            else
            {
                cbxFontFamily.SelectedIndex = -1;
            }
            cbxFontFamily.SelectionChanged += cbxFontFamily_SelectionChanged;

            #endregion

            #region FontSize

            var fontSize = range.GetPropertyValue(TextBlock.FontSizeProperty);
            cbxFontSize.SelectionChanged -= cbxFontSize_SelectionChanged;
            if (fontSize != DependencyProperty.UnsetValue)
            {
                cbxFontSize.SelectedValue = fontSize;
            }
            else
            {
                cbxFontSize.SelectedIndex = -1;
            }
            cbxFontSize.SelectionChanged += cbxFontSize_SelectionChanged;

            #endregion

            #region FontWeight

            var fontWeight = range.GetPropertyValue(TextBlock.FontWeightProperty);
            if (fontWeight != DependencyProperty.UnsetValue)
            {
                tbBold.IsChecked = (FontWeight)fontWeight == FontWeights.Bold;
            }

            #endregion

            #region FontStyle

            var fontStyle = range.GetPropertyValue(TextBlock.FontStyleProperty);
            if (fontStyle != DependencyProperty.UnsetValue)
            {
                tbItalic.IsChecked = (FontStyle)fontStyle == FontStyles.Italic;
            }

            #endregion

            #region TextDecoration

            var textDecorationCollection = range.GetPropertyValue(TextBlock.TextDecorationsProperty) as TextDecorationCollection;

            if (textDecorationCollection != null)
            {
                bool isUnderline = false;
                foreach (var textDecoration in textDecorationCollection)
                {
                    if (textDecoration.Location == TextDecorationLocation.Underline)
                    {
                        isUnderline = true;
                        break;
                    }
                }
                tbUnderline.IsChecked = isUnderline;
            }

            #endregion

            #region TextAlign

            var align = range.GetPropertyValue(TextBlock.TextAlignmentProperty);

            if (align != DependencyProperty.UnsetValue)
            {
                tbAlignCenter.IsChecked = false;
                tbAlignLeft.IsChecked = false;
                tbAlignRight.IsChecked = false;

                var alignment = (TextAlignment)align;
                switch (alignment)
                {
                    case TextAlignment.Left:
                        tbAlignLeft.IsChecked = true;
                        break;
                    case TextAlignment.Right:
                        tbAlignRight.IsChecked = true;
                        break;
                    case TextAlignment.Center:
                        tbAlignCenter.IsChecked = true;
                        break;
                }
            }

            #endregion
        }

        private void cbxFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var range = this.CreateTextRange();
            range.ApplyPropertyValue(TextBlock.FontFamilyProperty, cbxFontFamily.SelectedValue);
        }

        private void cbxFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var range = this.CreateTextRange();
            range.ApplyPropertyValue(TextBlock.FontSizeProperty, cbxFontSize.SelectedValue);
        }

        private void tbAlignLeft_Checked(object sender, RoutedEventArgs e)
        {
            tbAlignCenter.IsChecked = false;
            tbAlignRight.IsChecked = false;
        }

        private void tbAlignCenter_Checked(object sender, RoutedEventArgs e)
        {
            tbAlignLeft.IsChecked = false;
            tbAlignRight.IsChecked = false;
        }

        private void tbAlignRight_Checked(object sender, RoutedEventArgs e)
        {
            tbAlignCenter.IsChecked = false;
            tbAlignLeft.IsChecked = false;
        }

        void rtbContent_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Keyboard.Focus(this.rtbContent);
        }

        void RichTextEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                //Keyboard.Focus(this.rtbContent);
            }
        }

        void RichTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbxFontFamily.SelectionChanged += this.cbxFontFamily_SelectionChanged;
            this.cbxFontSize.SelectionChanged += this.cbxFontSize_SelectionChanged;
            this.ChangeToolbarVisibility();
        }

        private void tbHyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = new Hyperlink(new Run() { Text = "百度" });
            hyperlink.NavigateUri = new Uri("http://www.baodi/com", UriKind.Absolute);

            var span = new Span(hyperlink, this.rtbContent.Selection.Start);
        }

        #endregion

        
    }
}
