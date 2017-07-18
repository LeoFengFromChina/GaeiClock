using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace SmokeNote.Logic.Models
{
    public abstract class ModelBase : NotificationObject
    {
        private Guid _id;

        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }

        private DateTime _createDate;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate != value)
                {
                    _createDate = value;
                    this.RaisePropertyChanged("CreateDate");
                }
            }
        }

        private DateTime _modifyDate;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set
            {
                if (_modifyDate != value)
                {
                    _modifyDate = value;
                    this.RaisePropertyChanged("ModifyDate");
                }
            }
        }
    }
}
