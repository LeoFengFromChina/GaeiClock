using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Common.ViewModel
{
    /// <summary>
    /// 绑定到UI的ViewModel基类
    /// </summary>
    public class UIViewModelBase : ViewModelBase
    {
        private DelegateCommand _loadedCommand;

        /// <summary>
        /// UI加载完成执行的命令
        /// </summary>
        public DelegateCommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new DelegateCommand(this.Loaded);
                }
                return _loadedCommand;
            }
        }

        /// <summary>
        /// UI加载完成调用的方法
        /// </summary>
        protected virtual void Loaded()
        {

        }
    }
}
