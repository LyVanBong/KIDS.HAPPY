using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Message;
using KIDS.MOBILE.APP.Services.Message;
using KIDS.MOBILE.APP.views.Message;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Message
{
    public class MessageDetailViewModel : BaseViewModel
    {
        private SentModel _messageData;
        private string _name;
        private bool _isLoading;
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private ObservableCollection<CommentModel> _commentData;
        private IMessageService _messageService;
        private string _comment;

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public ICommand CommentCommand { get; set; }

        public ObservableCollection<CommentModel> CommentData
        {
            get => _commentData;
            set => SetProperty(ref _commentData, value);
        }

        public ICommand EditAndDeleteCommand { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public SentModel MessageData
        {
            get => _messageData;
            set => SetProperty(ref _messageData, value);
        }

        public MessageDetailViewModel(IDialogService dialogService, INavigationService navigationService, IMessageService messageService)
        {
            _messageService = messageService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            EditAndDeleteCommand = new Command(EditAndDelete);
            CommentCommand = new Command(CommentMessage);
        }

        private async void CommentMessage()
        {
            try
            {
                IsLoading = true;
                if (Comment != null)
                {
                    var data = await _messageService.CreateSentandComment(MessageData.ClassID, MessageData.TeacherID, MessageData.CommunicationID, Comment, DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), MessageData.StudentID, "1");
                    if (data != null && data.Code > 0)
                    {
                        await InitialzerDetailMessage(MessageData);
                        Comment = null;
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void EditAndDelete()
        {
            var para = new DialogParameters();
            para.Add(AppConstants.EditAndDeleteMessage, MessageData);
            _dialogService.ShowDialog(nameof(EditDeletePopup), para, result =>
            {
                RaisePropertyChanged(nameof(MessageData));
                if (result.Parameters.ContainsKey(AppConstants.DeleteMessage))
                {
                    if (result.Parameters.TryGetValue(AppConstants.DeleteMessage, out bool res))
                    {
                        _navigationService.GoBackAsync();
                    }
                }
            });
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
            var user = AppConstants.User;
            Name = user.TenGV;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(AppConstants.CommentMessage))
            {
                if (parameters.TryGetValue(AppConstants.CommentMessage, out SentModel message))
                {
                    MessageData = message;
                    await InitialzerDetailMessage(message);
                }
            }

            IsLoading = false;
        }

        private async Task InitialzerDetailMessage(SentModel message)
        {
            try
            {
                var data = await _messageService.GetComment(message.CommunicationID);
                if (data != null && data.Code > 0)
                {
                    CommentData = new ObservableCollection<CommentModel>(data.Data);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}