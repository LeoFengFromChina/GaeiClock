using System;
using System.Collections;
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

namespace SmokeNote.Client
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        private IList selectedItems = null;

        public TestWindow()
        {
            InitializeComponent();
        }

        private void lbDrag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedItems = lbDrag.SelectedItems;
        }

        private void lbDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && selectedItems != null)
            {                
                DragDrop.DoDragDrop(lbDrag, selectedItems, DragDropEffects.All);
            }
        }

        private void lbDrop_Drop(object sender, DragEventArgs e)
        {
            var ui = (IList)e.Data.GetData("System.Windows.Controls.SelectedItemCollection");
            if (ui != null)
            {
                for (var i = ui.Count; i > 0; i--)
                {
                    var item = ui[i - 1];
                    
                    lbDrag.Items.Remove(item);
                    lbDrop.Items.Add(item);
                }
            }
        }

        private void LoadingButton_Click(object sender, RoutedEventArgs e)
        {
            lbTest.IsLoading = !lbTest.IsLoading;
            ncTest.ShowNotifyMessage("保存成功!");
        }
    }
}
