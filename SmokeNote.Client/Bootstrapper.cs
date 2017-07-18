using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Windows;
using SmokeNote.Logic.IService;
using SmokeNote.Logic.Services;
using Framework.Common.Dialog;

namespace SmokeNote.Client
{
    public class Bootstrapper : Framework.Common.Bootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            var shell = this.Container.Resolve<Shell>();
            return shell;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            //var shell = (Window)this.Shell;
            //shell.Show();
            Clock clock = new Clock();
            clock.Show();
            Application.Current.MainWindow = clock;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            //注册单例
            //this.Container.RegisterInstance<Logic.Models.NoteQueryCondition>(new Logic.Models.NoteQueryCondition());

            //注册对话框
            this.Container.RegisterType<IDialogService, Dialog.CustomDialogService>();

            //注册服务
            this.Container.RegisterType<ISettingsService, SettingsService>();
            //this.Container.RegisterType<INotebookService, NotebookService>();
            //this.Container.RegisterType<INoteService, NoteService>();

            //注册窗口
            //this.Container.RegisterType<Window, Views.ModifyNotebook>(Consts.WindowNames.ModifyNotebook);
            this.Container.RegisterType<Window, Views.Config>(Consts.WindowNames.Config);
            //this.Container.RegisterType<Window, Views.NoteHistory>(Consts.WindowNames.NoteHistory);
            //this.Container.RegisterType<Window, Views.Total>(Consts.WindowNames.Total);
        }
    }
}
