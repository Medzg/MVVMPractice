namespace MVVM.UI.View.Services
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOkCancelDialog(string text, string Title);
        void ShowInfoDialog(string text); 
    }
}