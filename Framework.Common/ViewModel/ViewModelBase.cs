using Framework.Common.Dialog;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Framework.Common.ViewModel
{
    public class ViewModelBase : NotificationObject
    {
        #region 静态方法和属性

        /// <summary>
        /// 静态的,可在程序集内访问的IUnityContainer
        /// </summary>
        public static IUnityContainer StaticUnityContainer { get; set; }

        /// <summary>
        /// 初始化IUnityContainer
        /// </summary>
        /// <param name="unityContainer"></param>
        public static void InitUnityContainer(IUnityContainer unityContainer)
        {
            StaticUnityContainer = unityContainer;
        }
        #endregion

        #region 属性

        /// <summary>
        /// IUnityContainer实例
        /// </summary>
        public IUnityContainer UnityContainer
        {
            get
            {
                return StaticUnityContainer;
            }
        }

        private bool _isLoading;

        /// <summary>
        /// 是否在加载状态
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    this.RaisePropertyChanged("IsLoading");
                }
            }
        }

        /// <summary>
        /// 是否在设计模式
        /// </summary>
        public bool IsInDesignMode
        {
            get
            {
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(new Button());
            }
        }

        /// <summary>
        /// 事件通知中心
        /// </summary>
        public IEventAggregator EventAggregator
        {
            get
            {
                return this.UnityContainer.Resolve<IEventAggregator>();
            }
        }

        /// <summary>
        /// 对话框服务
        /// </summary>
        public IDialogService DialogService
        {
            get
            {
                return this.UnityContainer.Resolve<IDialogService>();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 在UI线程上同步执行方法
        /// </summary>
        /// <param name="action"></param>
        public void InvokeOnUIDispatcher(Action action)
        {
            if (action != null)
            {
                Application.Current.Dispatcher.Invoke(action);
            }
        }

        /// <summary>
        /// 发布一个事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        public void PublishEvent<T>(T e)
        {
            this.EventAggregator.GetEvent<CompositePresentationEvent<T>>().Publish(e);
        }

        /// <summary>
        /// 以默认的配置订阅一个事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public void SubscribeEvent<T>(Action<T> callback)
        {
            this.EventAggregator.GetEvent<CompositePresentationEvent<T>>().Subscribe(callback);
        }

        #endregion
    }
}
