using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using KIDS.MOBILE.APP.Models.RequestProvider;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Learn
{
    public class QuickCommentDialogViewModel : BaseViewModel, IDialogAware
    {
        private string _commentContent;
        public event Action<IDialogParameters> RequestClose;
        private List<RequestParameter> _commentData;
        public List<RequestParameter> CommentData
        {
            get => _commentData;
            set => SetProperty(ref _commentData, value);
        }
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
                para.Add(nameof(CommentContent), CommentContent);
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
            var para = parameters.GetValue<string>("key");
            if (para != null)
            {
                switch (para)
                {
                    case "0":
                        CommentData = new List<RequestParameter>
                        {
                            new RequestParameter("1", "Biết giúp bạn học tập"),
                            new RequestParameter("2", "Biết giữ gìn đồ dùng học tập"),
                            new RequestParameter("3",
                                "Hôm nay có tổ chức cho con làm thí nghiệm, con rất chú ý và làm thí nghiệm đạt kết quả tốt"),
                            new RequestParameter("4", "Con có nhiều tiến bộ tích cực tham gia vào các hoạt động ở lớp"),
                            new RequestParameter("5",
                                "Con tập trung chú ý, hứng thú tham gia giờ học cùng các cô và các bạn"),
                            new RequestParameter("6", "Chuẩn bị đầy đủ đò dùng học tập"),
                        };
                        break;
                    case "1":
                        CommentData = new List<RequestParameter>
                        {
                            new RequestParameter("1","Con hôm nay ăn hết xuất ăn bố mẹ ạ!"),
                            new RequestParameter("2","Con hôm nay đã uống hết sữa rồi bố mẹ nhé!"),
                            new RequestParameter("3","Hôm nay con uống còn thừa nhiều sứa"),
                            new RequestParameter("4","Hôm nay con đã biết cầm thìa và tự xúc cơm ăn hết bát cơm!"),
                        };
                        break;
                    default:
                        break;
                }
            }
        }
    }
}