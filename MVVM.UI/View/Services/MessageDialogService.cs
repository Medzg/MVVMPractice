using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.UI.View.Services
{
  public  class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text,string Title)
        {
            var result = MessageBox.Show(text, Title, MessageBoxButton.OKCancel);
            return result.Equals(MessageBoxResult.OK) ? MessageDialogResult.Ok : MessageDialogResult.Cancel;
        }

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text, "Info");
        }

    }
   public enum MessageDialogResult
    {
        Ok,
        Cancel
    }
}
