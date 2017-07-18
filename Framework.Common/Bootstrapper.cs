using Framework.Common.Dialog;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Framework.Common
{
    public abstract class Bootstrapper : UnityBootstrapper
    {
        public IEventAggregator EventAggregator
        {
            get
            {
                return this.Container.Resolve<IEventAggregator>();
            }
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            Application.Current.Exit += Current_Exit;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            base.Run(runWithDefaultConfiguration);            
        }

        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.EventAggregator.GetEvent<CompositePresentationEvent<DispatcherUnhandledExceptionEventArgs>>().Publish(e);
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            this.EventAggregator.GetEvent<CompositePresentationEvent<ExitEventArgs>>().Publish(e);
        }

        protected override IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
            ViewModel.ViewModelBase.InitUnityContainer(container);
            return container;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //this.Container.RegisterType<IDialogService, NormalDialogService>();
        }
    }
}
