using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Message;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Message;
using KIDS.MOBILE.APP.views.Message;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Message
{
    public class MessageViewModel : BaseViewModel
    {
        private IMessageService _messageService;
        private bool _isLoading;
        private ObservableCollection<SentModel> _sentData;
        private int _selectedIndex;
        private string _name;
        private ObservableCollection<SentModel> _inboxData;
        private string _classId;
        private string _teacharId;
        private string _searchMessageSent;
        private List<SentModel> _sentMessage;
        private List<SentModel> _inboxMessage;
        private IDialogService _dialogService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private string _searchInbox;

        public string SearchInbox
        {
            get => _searchInbox;
            set
            {
                if (SetProperty(ref _searchInbox, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchInbox))
                    {
                        InboxData = new ObservableCollection<SentModel>(_inboxMessage);
                    }
                    else
                    {
                        InboxData = new ObservableCollection<SentModel>(_inboxMessage.Where(x => x.NguoiGui.ToUpper().Contains(SearchInbox.ToUpper())));
                    }
                }
            }
        }

        public ICommand CreateNewMessageCommand { get; set; }
        public ICommand ComfirmedCommand { get; private set; }
        public ICommand CommentMessageCommand { get; private set; }
        public ICommand EditAndDeleteCommand { get; private set; }

        public string SearchMessageSent
        {
            get => _searchMessageSent;
            set
            {
                if (SetProperty(ref _searchMessageSent, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchMessageSent))
                        SentData = new ObservableCollection<SentModel>(_sentMessage);
                    else
                        SentData = new ObservableCollection<SentModel>(_sentMessage.Where(x => x.NguoiGui.ToUpper().Contains(SearchMessageSent.ToUpper())));
                }
            }
        }

        public ObservableCollection<SentModel> InboxData
        {
            get => _inboxData;
            set => SetProperty(ref _inboxData, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (SetProperty(ref _selectedIndex, value))
                {
                    Task.Run(async () => await InitializeMessage());
                }
            }
        }

        public ObservableCollection<SentModel> SentData
        {
            get => _sentData;
            set => SetProperty(ref _sentData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public MessageViewModel(IMessageService messageService, IDialogService dialogService, INavigationService navigationService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _messageService = messageService;
            EditAndDeleteCommand = new Command<SentModel>(EditAndDelete);
            CommentMessageCommand = new Command<SentModel>(CommentMessage);
            ComfirmedCommand = new Command<SentModel>(Comfirmed);
            CreateNewMessageCommand = new Command(CreateNewMessage);
        }

        private void CreateNewMessage()
        {
            try
            {
                IsLoading = true;
                _dialogService.ShowDialog(nameof(NewMessagePopup), async (result) => await InitializeMessage());
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

        private async void Comfirmed(SentModel obj)
        {
            try
            {
                if (!obj.IsConfirmed)
                {
                    var res = await _pageDialogService.DisplayAlertAsync(AppResources._00061, AppResources._00062, AppResources._00064, AppResources._00063);
                    if (res)
                    {
                        var confirm =
                            await _messageService.ComfirmedMessage(obj.CommunicationID, "true", obj.TeacherID, obj.Content);
                        if (confirm != null && confirm.Code > 0)
                        {
                            obj.IsConfirmed = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async void CommentMessage(SentModel obj)
        {
            try
            {
                IsLoading = true;
                var para = new NavigationParameters();
                para.Add(AppConstants.CommentMessage, obj);
                await _navigationService.NavigateAsync(nameof(MessageDetailPage), para, useModalNavigation: true);
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

        private void EditAndDelete(SentModel obj)
        {
            var para = new DialogParameters();
            para.Add(AppConstants.EditAndDeleteMessage, obj);
            _dialogService.ShowDialog(nameof(EditDeletePopup), para, async (result) => await InitializeMessage());
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            var user = AppConstants.User;
            _classId = user.ClassID;
            _teacharId = user.NguoiSuDung;
            Name = user.TenGV;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializeMessage();
        }

        private async Task InitializeMessage()
        {
            try
            {
                IsLoading = true;
                if (SelectedIndex == 0)
                {
                    var data = await _messageService.GetInbox(_classId);
                    if (data != null && data.Code > 0)
                    {
                        InboxData = new ObservableCollection<SentModel>(data.Data);
                        _inboxMessage = data.Data.ToList();
                    }
                }
                else
                {
                    var data = await _messageService.GetWasSent(_teacharId);
                    if (data != null && data.Code > 0)
                    {
                        SentData = new ObservableCollection<SentModel>(data.Data);
                        _sentMessage = data.Data.ToList();
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
    }
}