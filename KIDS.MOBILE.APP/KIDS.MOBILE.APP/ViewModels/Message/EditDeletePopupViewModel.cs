using System;
using System.Windows.Input;
using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Message;
using KIDS.MOBILE.APP.Services.Message;
using Microsoft.AppCenter.Crashes;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Message
{
    public class EditDeletePopupViewModel : BaseViewModel, IDialogAware
    {
        private SentModel _message;
        private IMessageService _messageService;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CancelCommand { get; set; }
        public SentModel Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public EditDeletePopupViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            CancelCommand = new Command(Cancel);
            DeleteCommand = new Command(Delete);
            UpdateCommand = new Command(Update);
        }

        private async void Update()
        {
            try
            {
                IsLoading = true;
                var data = await _messageService.UpdateMessage(Message.CommunicationID, Message.Content,
                    DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                if (RequestClose != null)
                {
                    RequestClose(new DialogParameters());
                }
                IsLoading = false;
            }
        }

        private async void Delete()
        {
            try
            {
                IsLoading = true;
                var data = await _messageService.DeleteMessage(Message.CommunicationID);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                if (RequestClose != null)
                {
                    var para = new DialogParameters();
                    para.Add(AppConstants.DeleteMessage,true);
                    RequestClose(para);
                }
                IsLoading = false;
            }
        }

        private void Cancel()
        {
            IsLoading = true;
            if (RequestClose != null)
            {
                RequestClose(new DialogParameters());
            }

            IsLoading = false;
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(AppConstants.EditAndDeleteMessage))
            {
                if (parameters.TryGetValue(AppConstants.EditAndDeleteMessage, out SentModel res))
                {
                    Message = res;
                }
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}