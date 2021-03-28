using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.PostNews;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Services.News;
using KIDS.MOBILE.APP.views.NewsPost;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.views.Album;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.NewsPost
{
    public class NewsPostViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private ObservableCollection<NewsModel> _newsData;
        private bool _isLoading;

        public ObservableCollection<NewsModel> NewsData
        {
            get => _newsData;
            set => SetProperty(ref _newsData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand GoBackCommand { get; set; }
        private UserModel _user;
        private INewsService _newsService;
        public ICommand AddNewsPostCommand { get; set; }
        private IDialogService _dialogService;
        public ICommand DetailNewsCommand { get; set; }

        public NewsPostViewModel(INavigationService navigationService, INewsService newsService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _newsService = newsService;
            _navigationService = navigationService;
            GoBackCommand = new Command(GoBack);
            AddNewsPostCommand = new Command(AddNewsPost);
            DetailNewsCommand = new Command<NewsModel>(DetailNews);
        }

        private void DetailNews(NewsModel obj)
        {
            var para = new NavigationParameters();
            para.Add(AppConstants.DetailNews, obj);
            _navigationService.NavigateAsync(nameof(DetailNewsPage), para, useModalNavigation: true);
        }

        private void AddNewsPost()
        {
            if (IsLoading)
                return;
            IsLoading = true;
            _navigationService.NavigateAsync(nameof(AddPhotoPage), new NavigationParameters("?key=1"), true);
            IsLoading = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializationNewspost();
        }

        private async Task InitializationNewspost()
        {
            try
            {
                IsLoading = true;
                _user = AppConstants.User;
                var data = await _newsService.GetAllNews(_user.ClassID, _user.DonVi);
                if (data.Code > 0)
                {
                    NewsData = new ObservableCollection<NewsModel>(data.Data);
                }
                else
                    NewsData = new ObservableCollection<NewsModel>();
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

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
    }
}