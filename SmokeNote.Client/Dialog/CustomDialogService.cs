using Framework.Common.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SmokeNote.Client.Dialog
{
    public class CustomDialogService : DialogBase
    {
        public override void Alert(string message, string title, Action callback)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var window = DialogWindow.CreateAlertWindow(title, message, callback);
                window.ShowDialog();
            }));
        }

        public override void Confirm(string message, string title, Action<bool> callback)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var window = DialogWindow.CreateConfirmWindow(title, message, callback);
                window.ShowDialog();
            }));
        }

        public override void Notify(string message, string title, MessageType type, Action callback)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var shell = Application.Current.MainWindow as Shell;
                shell.notifyControl.ShowNotifyMessage(message);
            }));
        }
    }
}
