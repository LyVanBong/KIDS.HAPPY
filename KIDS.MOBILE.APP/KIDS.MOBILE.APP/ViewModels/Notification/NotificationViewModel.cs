using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Notification;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.Notification;
using KIDS.MOBILE.APP.views.Album;
using KIDS.MOBILE.APP.views.MedicineReminders;
using KIDS.MOBILE.APP.views.NewsPost;
using KIDS.MOBILE.APP.views.TicketManagement;
using Prism;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Notification
{
    public class NotificationViewModel : BaseViewModel, IActiveAware
    {
        private bool _isActive;
        private INotificationService _notificationService;
        private List<NotificationModel> _dataNotification;
        private IDatabaseService _databaseService;
        private bool _loadNotification;
        private INavigationService _navigationService;
        private string _count;

        public string Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, IsActiveChange);
        }

        public List<NotificationModel> DataNotification
        {
            get => _dataNotification;
            set => SetProperty(ref _dataNotification, value);
        }

        public bool LoadNotification
        {
            get => _loadNotification;
            set => SetProperty(ref _loadNotification, value);
        }

        public ICommand NotificationDetailCommand { get; set; }

        public NotificationViewModel(INotificationService notificationService, IDatabaseService databaseService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _notificationService = notificationService;
            _databaseService = databaseService;
            NotificationDetailCommand = new Command<NotificationModel>(NotificationDetail);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                new Thread(() =>
               {
                   Device.BeginInvokeOnMainThread(async () => { await GetCountNotification(); });
               }).Start();
                return true;
            });
        }

        private async Task GetCountNotification()
        {
            var data = await _notificationService.GetCountNotification();
            if (data != null && data.Data > 0)
            {
                if (data.Data > 9)
                {
                    Count = "9+";
                }

                if (data.Data < 10 && data.Data > 0)
                {
                    Count = data.Data + "";
                }
            }
        }

        private void NotificationDetail(NotificationModel obj)
        {
            var para = new NavigationParameters();
            para.Add(AppConstants.NotificationID, obj.ID);
            switch (int.Parse(obj.Type))
            {
                case 1:
                    // đơn xin nghỉ
                    _navigationService.NavigateAsync(nameof(TicketManagementPage), para, useModalNavigation: true);
                    break;

                case 2:
                    // đơn thuốc
                    _navigationService.NavigateAsync(nameof(DetailMedicineRemindersPage), para, useModalNavigation: true);
                    break;

                case 3:
                    //Tin nhắn
                    //_navigationService.NavigateAsync(nameof(trang đơn chi tiết tin nhắn),para);
                    break;

                case 4:
                    //Album lớp
                    _navigationService.NavigateAsync(nameof(AlbumDetailPage), para, useModalNavigation: true);
                    break;

                case 5:
                    //Album trường
                    _navigationService.NavigateAsync(nameof(AlbumDetailPage), para, useModalNavigation: true);
                    break;

                case 6:
                    //Bài viết lớp
                    _navigationService.NavigateAsync(nameof(DetailNewsPage), para, useModalNavigation: true);
                    break;

                case 7:
                    //bài viết trường
                    _navigationService.NavigateAsync(nameof(DetailNewsPage), para, useModalNavigation: true);
                    break;

                default:
                    return;
            }
        }

        private async void IsActiveChange()
        {
            if (IsActive)
            {
                LoadNotification = true;
                if (AppConstants.User != null)
                {
                    var data = await _notificationService.GetNotification(AppConstants.User.ClassID, AppConstants.User.DonVi);
                    if (data.Code > 0)
                    {
                        DataNotification = new List<NotificationModel>(data.Data);
                    }
                }
                await GetCountNotification();
                LoadNotification = false;
            }
            else
            {
            }
        }

        public event EventHandler IsActiveChanged;
    }
}