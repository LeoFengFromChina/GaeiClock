using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Windows.Data;

namespace SmokeNote.Client
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        #region 字段

        /// <summary>
        /// 是否真的关闭窗口
        /// </summary>
        private bool isReallyExit = false;

        /// <summary>
        /// 记录上一次WindowState
        /// </summary>
        private WindowState lastWindowState = WindowState.Normal;

        #endregion

        #region 属性

        public ViewModels.ShellViewModel ShellViewModel
        {
            get
            {
                return this.DataContext as ViewModels.ShellViewModel;
            }
        }

        public IUnityContainer Container
        {
            get;
            private set;
        }

        //public Logic.Models.NoteQueryCondition NoteQueryCondition
        //{
        //    get;
        //    private set;
        //}

        #endregion

        #region 方法

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="condition"></param>
        public Shell(IUnityContainer container)
        {
            Framework.Common.Helpers.FullScreenHelper.RepairWpfWindowFullScreenBehavior(this);
            this.Container = container;
            //this.NoteQueryCondition = condition;
            this.KeyDown += Shell_KeyDown;

            InitializeComponent();
        }

        /// <summary>
        /// 点击设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miConfig_Click(object sender, RoutedEventArgs e)
        {
            //var vm = this.Container.Resolve<ViewModels.ConfigViewModel>();
            //vm.ShowDialog();
        }

        /// <summary>
        /// 点击托盘退出程序按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            isReallyExit = true;
            this.Close();
        }

        /// <summary>
        /// 点击新建笔记本按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miNewNotebook_Click(object sender, RoutedEventArgs e)
        {
            //var vm = new ViewModels.NotebookViewModel();
            //vm.ShowDialog();
        }

        /// <summary>
        /// 选中笔记本时,筛选日记列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvNotebookList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvNotebookList.SelectedItem != null && tvRecycle != null)
            {
                ((TreeViewItem)tvRecycle.Items[0]).IsSelected = false;
            }

            //var vm = tvNotebookList.SelectedItem as ViewModels.NotebookViewModel;

            //if (vm == null)
            //{
            //    this.NoteQueryCondition.CurrentNotebook = null;
            //}
            //else
            //{
            //    this.NoteQueryCondition.CurrentNotebook = vm.Notebook;
            //}

        }

        /// <summary>
        /// 选中回收站时,筛选日记列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvRecycle_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvRecycle.SelectedItem != null)
            {
                var treeViewItem = tvNotebookList.Items[0] as TreeViewItem;
                treeViewItem.IsSelected = false;

                //foreach (ViewModels.NotebookViewModel item in treeViewItem.Items)
                //{
                //    item.IsChecked = false;
                //}
            }

            //this.NoteQueryCondition.IsRecycle = tvRecycle.SelectedItem != null;
        }

        /// <summary>
        /// 这个方法是为了在右键点击树时,选中子项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tvItem = (TreeViewItem)sender;
            tvItem.IsSelected = true;
        }

        /// <summary>
        /// 如果在主窗口按下Ctrl+S,保存日记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Shell_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                //this.ShellViewModel.NoteListViewModel.SaveAllCommand.Execute();
            }
        }

        /// <summary>
        /// 关闭时,判断是缩小到托盘还是退出程序
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!isReallyExit)
            {
                e.Cancel = true;
                lastWindowState = this.WindowState;
                this.Hide();
            }
        }

        /// <summary>
        /// 菜单项"显示主窗口"点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miShowWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsVisible)
            {
                this.Show();
            }

            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = lastWindowState;
            }

            this.Activate();
        }

        /// <summary>
        /// 在笔记列表上按下Delete,删除笔记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbNote_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                //this.ShellViewModel.NoteListViewModel.DeleteNoteCommand.Execute();
            }
        }

        /// <summary>
        /// 点击了使用量菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miTotal_Click(object sender, RoutedEventArgs e)
        {
            //var vm = new ViewModels.TotalViewModel();
            //vm.ShowDialog();
        }

        #endregion
    }
}
