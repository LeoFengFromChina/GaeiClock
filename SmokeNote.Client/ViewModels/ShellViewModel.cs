using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.ViewModel;
using SmokeNote.Logic.Models;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using SmokeNote.Logic.IService;
using Microsoft.Practices.Unity;

namespace SmokeNote.Client.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region 构造器

        public ShellViewModel()
        {
            if (!this.IsInDesignMode)
            {
                this.SettingsService = this.UnityContainer.Resolve<ISettingsService>();
                //this.UISettings = this.SettingsService.GetSetting<UISettings>();
                //this.NotebookListViewModel = this.UnityContainer.Resolve<NotebookListViewModel>();
                //this.NoteListViewModel = this.UnityContainer.Resolve<NoteListViewModel>();
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 设置信息服务类
        /// </summary>
        public ISettingsService SettingsService { get; private set; }

        /// <summary>
        /// 笔记本列表ViewModel
        /// </summary>
        //public NotebookListViewModel NotebookListViewModel { get; private set; }

        /// <summary>
        /// 日记列表ViewModel
        /// </summary>
        //public NoteListViewModel NoteListViewModel { get; private set; }

        #endregion

        #region 命令

        private DelegateCommand _addNotebookCommand;

        public DelegateCommand AddNotebookCommand
        {
            get
            {
                if (_addNotebookCommand == null)
                {
                    _addNotebookCommand = new DelegateCommand(this.AddNotebook);
                }
                return _addNotebookCommand;
            }
        }

        private void AddNotebook()
        {
            //var vm = new ViewModels.NotebookViewModel();
            //vm.ShowDialog();
        }

        private DelegateCommand _loadCommand;

        public DelegateCommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(this.Load);
                }
                return _loadCommand;
            }
        }

        private void Load()
        {
            var action = new Action(() =>
            {
                //this.NotebookListViewModel.InitNotebookList();
                //this.NoteListViewModel.InitNoteList();
            });
            action.BeginInvoke(ar =>
            {
                action.EndInvoke(ar);
            }, null);
        }

        private DelegateCommand _closeCommand;

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

        private void Close()
        {
            //this.SettingsService.SaveSettings(this.UISettings);
        }

        #endregion
    }
}
