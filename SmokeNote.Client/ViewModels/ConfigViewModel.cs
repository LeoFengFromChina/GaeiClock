using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmokeNote.Client.ViewModels
{
    public class ConfigViewModel : NotificationObject
    {
        public ConfigViewModel()
        {
        }

        private bool _topmost;
        /// <summary>
        /// 回收站日记数
        /// </summary>
        public bool Topmost
        {
            get { return _topmost; }
            set
            {
                if (_topmost != value)
                {
                    _topmost = value;
                    this.RaisePropertyChanged("Topmost");
                }
            }
        }
    }
}
