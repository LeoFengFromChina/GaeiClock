using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Commands;

namespace Framework.Common.ViewModel
{
    public class WindowViewModelBase : UIViewModelBase
    {
        public WindowViewModelBase(string windowName)
        {
            this.WindowName = windowName;
        }

        #region 属性
        /// <summary>
        /// 窗口名称
        /// </summary>
        public string WindowName
        {
            get;
            private set;
        }

        private string _title;

        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// 窗口实例
        /// </summary>
        private Window Window { get; set; }

        private bool _isShow;

        /// <summary>
        /// 窗口是否在显示中
        /// </summary>
        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged("IsShow");
                }
            }
        }
        #endregion

        #region 命令
        private DelegateCommand _showCommand;

        /// <summary>
        /// 打开窗口命令
        /// </summary>
        public DelegateCommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                {
                    _showCommand = new DelegateCommand(this.Show);
                }
                return _showCommand;
            }
        }

        private DelegateCommand _closeCommand;

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public DelegateCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new DelegateCommand(this.Close);
                }
                return _closeCommand;
            }
        }
        #endregion

        #region 方法

        private Window CreateWindow()
        {
            if (this.IsShow)
            {
                this.Window.Activate();
                return null;
            }

            var window = this.UnityContainer.Resolve<Window>(this.WindowName);
            window.Closed += OnWindowClosed;
            if (window == null)
            {
                this.DialogService.Alert("Invalid Window Name!", "Warn", null);
                return null;
            }

            window.Owner = Application.Current.MainWindow;
            window.DataContext = this;

            return window;
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        public void Show()
        {
            var window = this.CreateWindow();

            if (window != null)
            {
                this.Window = window;
                window.Show();
                this.IsShow = true;
            }
        }

        public bool? ShowDialog()
        {
            var window = this.CreateWindow();

            if (window != null)
            {
                this.Window = window;
                this.IsShow = true;
            }

            return this.Window.ShowDialog();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void Close()
        {
            if (!this.IsShow)
            {
                return;
            }
            if (this.Window != null)
            {
                this.Window.Close();
            }
            else
            {
                this.IsShow = false;
            }
        }

        protected virtual void OnWindowClosed(object sender, EventArgs e)
        {
            this.Window = null;
            this.IsShow = false;
        }

        #endregion

    }
}
