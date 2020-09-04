﻿using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.Network;
using KIDS.MOBILE.APP.Controls.Dialogs.NewFolder;
using KIDS.MOBILE.APP.Controls.Dialogs.PostAlbum;
using KIDS.MOBILE.APP.Controls.Dialogs.PostNews;
using KIDS.MOBILE.APP.Controls.Dialogs.ShowImage;
using KIDS.MOBILE.APP.Controls.Dialogs.TimePicker;
using KIDS.MOBILE.APP.Services.AbsenceRequest;
using KIDS.MOBILE.APP.Services.Album;
using KIDS.MOBILE.APP.Services.Assessment;
using KIDS.MOBILE.APP.Services.Attendance;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.Dining;
using KIDS.MOBILE.APP.Services.Evaluate;
using KIDS.MOBILE.APP.Services.Hygiene;
using KIDS.MOBILE.APP.Services.Learn;
using KIDS.MOBILE.APP.Services.Login;
using KIDS.MOBILE.APP.Services.Message;
using KIDS.MOBILE.APP.Services.Napping;
using KIDS.MOBILE.APP.Services.News;
using KIDS.MOBILE.APP.Services.Notification;
using KIDS.MOBILE.APP.Services.Prescription;
using KIDS.MOBILE.APP.Services.RequestProvider;
using KIDS.MOBILE.APP.Services.User;
using KIDS.MOBILE.APP.ViewModels.Album;
using KIDS.MOBILE.APP.ViewModels.Assessment;
using KIDS.MOBILE.APP.ViewModels.Attendance;
using KIDS.MOBILE.APP.ViewModels.Dining;
using KIDS.MOBILE.APP.ViewModels.Evaluate;
using KIDS.MOBILE.APP.ViewModels.Home;
using KIDS.MOBILE.APP.ViewModels.Hygiene;
using KIDS.MOBILE.APP.ViewModels.Learn;
using KIDS.MOBILE.APP.ViewModels.ListStudents;
using KIDS.MOBILE.APP.ViewModels.Login;
using KIDS.MOBILE.APP.ViewModels.Main;
using KIDS.MOBILE.APP.ViewModels.MedicineReminders;
using KIDS.MOBILE.APP.ViewModels.Menu;
using KIDS.MOBILE.APP.ViewModels.Message;
using KIDS.MOBILE.APP.ViewModels.Napping;
using KIDS.MOBILE.APP.ViewModels.NewsPost;
using KIDS.MOBILE.APP.ViewModels.Notification;
using KIDS.MOBILE.APP.ViewModels.PleaseLeaveSchool;
using KIDS.MOBILE.APP.ViewModels.TicketManagement;
using KIDS.MOBILE.APP.ViewModels.User;
using KIDS.MOBILE.APP.views.Album;
using KIDS.MOBILE.APP.views.Assessment;
using KIDS.MOBILE.APP.views.Attendance;
using KIDS.MOBILE.APP.views.Dining;
using KIDS.MOBILE.APP.views.Evaluate;
using KIDS.MOBILE.APP.views.Home;
using KIDS.MOBILE.APP.views.Hygiene;
using KIDS.MOBILE.APP.views.Learn;
using KIDS.MOBILE.APP.views.ListStudents;
using KIDS.MOBILE.APP.views.Login;
using KIDS.MOBILE.APP.views.Main;
using KIDS.MOBILE.APP.views.MedicineReminders;
using KIDS.MOBILE.APP.views.Menu;
using KIDS.MOBILE.APP.views.Message;
using KIDS.MOBILE.APP.views.Napping;
using KIDS.MOBILE.APP.views.NewsPost;
using KIDS.MOBILE.APP.views.Notification;
using KIDS.MOBILE.APP.views.PleaseLeaveSchool;
using KIDS.MOBILE.APP.views.TicketManagement;
using KIDS.MOBILE.APP.views.User;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace KIDS.MOBILE.APP
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private async void InitializeApp()
        {
            Device.SetFlags(new string[] { "RadioButton_Experimental" });

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppSettings.SyncfusionLicense);

            await NavigationService.NavigateAsync(nameof(LoginPage));

            // check mang
            Connectivity.ConnectivityChanged += CheckNetwork;

            // check theme
            Application.Current.UserAppTheme = OSAppTheme.Light;
            Application.Current.RequestedThemeChanged += (sender, args) =>
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;
            };

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
           {
               if (Connectivity.NetworkAccess != NetworkAccess.Internet)
               {
                   AppConstants.HasInternet = false;
                   if (!AppConstants.IsOpenPopupNetwork)
                   {
                       NavigationService.NavigateAsync("NetworkPage", useModalNavigation: true);
                   }

                   return true;
               }
               else
               {
                   AppConstants.HasInternet = true;
                   return false;
               }
           });
        }

        private async void CheckNetwork(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
            if (access == NetworkAccess.Internet)
            {
                AppConstants.HasInternet = true;
            }
            else
            {
                AppConstants.HasInternet = false;
                if (!AppConstants.IsOpenPopupNetwork)
                    await NavigationService.NavigateAsync("NetworkPage", useModalNavigation: true);
            }
        }

        /// <summary>
        /// đăng kỹ các trang cần điều hướng.
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Dialog

            containerRegistry.RegisterDialog<DatePickerDialog, DatePickerDialogViewModel>();
            containerRegistry.RegisterDialog<NewMessagePopup, NewMessagePopupViewModel>();
            containerRegistry.RegisterDialog<EditDeletePopup, EditDeletePopupViewModel>();
            containerRegistry.RegisterDialog<TimePickerDialog, TimePickerDialogViewModel>();
            containerRegistry.RegisterDialog<PostNewsDialog, PostNewsDialogViewModel>();
            containerRegistry.RegisterDialog<PostAlbumDialog, PostAlbumDialogViewModel>();
            containerRegistry.RegisterDialog<ShowImageDialog, ShowImageDialogViewModel>();

            #endregion Dialog

            #region Registry Page - ViewModel

            containerRegistry.RegisterForNavigation<MessageDetailPage, MessageDetailViewModel>();
            containerRegistry.RegisterForNavigation<EvaluationCriteriaPage, EvaluationCriteriaViewModel>();
            containerRegistry.RegisterForNavigation<EvaluatePage, EvaluateViewModel>();
            containerRegistry.RegisterForNavigation<AssessmentPage, AssessmentViewModel>();
            containerRegistry.RegisterForNavigation<HygienePage, HygieneViewModel>();
            containerRegistry.RegisterForNavigation<NetworkPage, NetworkViewModel>();
            containerRegistry.RegisterForNavigation<NappingPage, NappingViewModel>();
            containerRegistry.RegisterForNavigation<DiningPage, DiningViewModel>();
            containerRegistry.RegisterForNavigation<StudentDetailPage, StudentDetailViewModel>();
            containerRegistry.RegisterForNavigation<LearnPage, LearnViewModel>();
            containerRegistry.RegisterForNavigation<ListStudentsPage, ListStudentsViewModel>();
            containerRegistry.RegisterForNavigation<UserProfilePage, UserProfileViewModel>();
            containerRegistry.RegisterForNavigation<DetailMedicineRemindersPage, DetailMedicineRemindersViewModel>();
            containerRegistry.RegisterForNavigation<DetailNewsPage, DetailNewsViewModel>();
            containerRegistry.RegisterForNavigation<DetailMedicineRemindersPage, DetailMedicineRemindersViewModel>();
            containerRegistry.RegisterForNavigation<AlbumListPage, AlbumListViewModel>();
            containerRegistry.RegisterForNavigation<AlbumDetailPage, AlbumDetailViewModel>();
            containerRegistry.RegisterForNavigation<DetailNewsPage, DetailNewsViewModel>();
            containerRegistry.RegisterForNavigation<NewsPostPage, NewsPostViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<AttendancePage, AttendanceViewModel>();
            containerRegistry.RegisterForNavigation<MessagePage, MessageViewModel>();
            containerRegistry.RegisterForNavigation<PleaseLeaveSchoolPage, PleaseLeaveSchoolViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationViewModel>();
            containerRegistry.RegisterForNavigation<TicketManagementPage, TicketManagementViewModel>();

            #endregion Registry Page - ViewModel

            #region Registry Service

            containerRegistry.Register<IMessageService, MessageService>();
            containerRegistry.Register<IEvaluateService, EvaluateService>();
            containerRegistry.Register<IAssessmentService, AssessmentService>();
            containerRegistry.Register<IHygieneService, HygieneService>();
            containerRegistry.Register<INappingService, NappingService>();
            containerRegistry.Register<IDiningService, DiningService>();
            containerRegistry.Register<ILearnService, LearnService>();
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IAttendanceService, AttendanceService>();
            containerRegistry.Register<IAlbumDetailService, AlbumDetailService>();
            containerRegistry.Register<IAbsenceRequestService, AbsenceRequestService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<INewsService, NewsService>();
            containerRegistry.Register<IAlbumService, AlbumService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IPrescriptionService, PrescriptionService>();
            containerRegistry.Register<IDatabaseService, DatabaseService>();

            #endregion Registry Service
        }

        protected override void OnInitialized()
        {
            InitializeApp();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=0b0adf16-16df-4f1f-b2c2-a7b11b9b1273;" +
                            "ios=4af41406-c7eb-4bb7-aa3b-42ce26def43f;",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}