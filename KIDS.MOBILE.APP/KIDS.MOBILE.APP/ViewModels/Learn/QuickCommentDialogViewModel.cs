using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Learn
{
    public class QuickCommentDialogViewModel : BaseViewModel, IDialogAware
    {
        private string _commentContent;
        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public ICommand ClosePopupCommand { get; private set; }

        public string CommentContent
        {
            get => _commentContent;
            set => SetProperty(ref _commentContent, value);
        }

        public ICommand QuickCommentCommand { get; private set; }

        public QuickCommentDialogViewModel()
        {
            ClosePopupCommand = new Command(ClosePopup);
            QuickCommentCommand = new Command(QuickComment);
        }

        private void QuickComment()
        {
            if (CommentContent != null)
            {
                var para = new DialogParameters();
                para.Add(nameof(CommentContent),CommentContent);
                if (RequestClose != null)
                {
                    RequestClose(para);
                }
            }
        }

        private void ClosePopup()
        {
            if (RequestClose != null)
            {
                RequestClose(null);
            }
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}