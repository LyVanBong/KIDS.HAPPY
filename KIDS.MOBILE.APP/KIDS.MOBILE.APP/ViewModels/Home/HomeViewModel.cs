using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Services.Album;
using KIDS.MOBILE.APP.Services.Database;
using KIDS.MOBILE.APP.Services.News;
using KIDS.MOBILE.APP.views.Album;
using KIDS.MOBILE.APP.views.NewsPost;
using KIDS.MOBILE.APP.views.User;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel, IActiveAware
    {
        private IDatabaseService _databaseService;
        private UserModel _userData;
        private List<AlbumModel> _albumData;
        private bool _isGoToNews;
        private bool _isGoToAlbumList;
        private bool _isGoToProfile;
        private IAlbumService _albumService;
        private List<NewsModel> _newsData;
        private INewsService _newsService;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, IsActiveChang);
        }

        public event EventHandler IsActiveChanged;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public UserModel UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        public List<AlbumModel> AlbumData
        {
            get => _albumData;
            set => SetProperty(ref _albumData, value);
        }

        public List<NewsModel> NewsData
        {
            get => _newsData;
            set => SetProperty(ref _newsData, value);
        }

        public ICommand NewsCommand { get; }
        public ICommand AlbumListCommand { get; }
        public ICommand DetailNewsCommand { get; set; }
        public ICommand AlbumDetailCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand EvaluateCommand { get; set; }

        private INavigationService _navigationService;
        private bool _isLoading;
        private bool _isActive;

        public HomeViewModel(IDatabaseService databaseService, IAlbumService albumService, INewsService newsService, INavigationService navigationService)
        {
            _databaseService = databaseService;
            _albumService = albumService;
            _newsService = newsService;
            _navigationService = navigationService;
            NewsCommand = new Command(async () => await News());
            AlbumListCommand = new Command(async () => await AlbumList());
            DetailNewsCommand = new Command<NewsModel>(DetailNews);
            AlbumDetailCommand = new Command<AlbumModel>(DetailAlbum);
            ProfileCommand = new Command(async () => await Profile());
            EvaluateCommand = new Command(async () => await Evaluate());
        }

        private async Task Evaluate()
        {
            var user = AppConstants.User;
            var uri = $"http://school.hkids.edu.vn/Teachers/AssessApp.aspx?SchoolID={user.DonVi}&GradeID={user.GradeID}&ClassID={user.ClassID}&SchoolYear={DateTime.Now.Year}&UserID={user.UserId}";
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private async void IsActiveChang()
        {
            if (IsActive)
            {
                await InitializationHome();
            }
            else
            {
            }
        }

        private async Task Profile()
        {
            if (_isGoToProfile)
                return;
            _isGoToProfile = true;
            await _navigationService.NavigateAsync(nameof(UserProfilePage), useModalNavigation: true);
            _isGoToProfile = false;
        }

        private void DetailAlbum(AlbumModel obj)
        {
            var para = new NavigationParameters();
            para.Add(AppConstants.DetailAlbums, obj);
            _navigationService.NavigateAsync(nameof(AlbumDetailPage), para, useModalNavigation: true);
        }

        private void DetailNews(NewsModel obj)
        {
            var para = new NavigationParameters();
            para.Add(AppConstants.DetailNews, obj);
            _navigationService.NavigateAsync(nameof(DetailNewsPage), para, useModalNavigation: true);
        }

        private async Task News()
        {
            if (_isGoToNews)
                return;
            _isGoToNews = true;
            await _navigationService.NavigateAsync(nameof(NewsPostPage), useModalNavigation: true);
            _isGoToNews = false;
        }

        private async Task AlbumList()
        {
            if (_isGoToAlbumList)
                return;
            _isGoToAlbumList = true;
            await _navigationService.NavigateAsync(nameof(AlbumListPage), useModalNavigation: true);
            _isGoToAlbumList = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializationHome();
        }

        private async Task InitializationHome()
        {
            try
            {
                IsLoading = true;
                UserData = AppConstants.User = await _databaseService.GetAccountUser();
                var albumData = await _albumService.GetAllAlbum(UserData.ClassID, UserData.DonVi);
                if (albumData != null)
                {
                    if (albumData.Code > 0)
                    {
                        AlbumData = new List<AlbumModel>(albumData.Data);
                    }
                    else
                        AlbumData = new List<AlbumModel>();
                }
                else
                    AlbumData = new List<AlbumModel>();

                var newsData = await _newsService.GetAllNews(UserData.ClassID, UserData.DonVi);

                if (newsData != null)
                {
                    if (newsData.Code > 0)
                    {
                        NewsData = new List<NewsModel>(newsData.Data);
                    }
                    else
                        NewsData = new List<NewsModel>();
                }
                else
                    NewsData = new List<NewsModel>();
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