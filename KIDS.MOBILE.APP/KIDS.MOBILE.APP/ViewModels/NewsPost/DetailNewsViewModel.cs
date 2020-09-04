using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Services.News;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.NewsPost
{
    public class DetailNewsViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private bool _isLoading;
        private INewsService _newsService;
        private NewsModel _detailNews;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public NewsModel DetailNews
        {
            get => _detailNews;
            set => SetProperty(ref _detailNews, value);
        }

        public ICommand GoBackCommand { get; set; }

        public DetailNewsViewModel(INewsService newsService, INavigationService navigationService)
        {
            _newsService = newsService;
            _navigationService = navigationService;
            GoBackCommand = new Command(GoBack);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            if (parameters.ContainsKey(AppConstants.DetailNews))
            {
                var data = parameters[AppConstants.DetailNews] as NewsModel;
                if (data != null)
                {
                    DetailNews = data;
                }
            }

            if (parameters.ContainsKey(AppConstants.NotificationID))
            {
                var data = parameters[AppConstants.NotificationID] as string;
                if (data != "")
                {
                    await InitializationNews(data);
                }
            }

            IsLoading = false;
        }

        private async Task InitializationNews(string NewsID)
        {
            try
            {
                IsLoading = true;
                var data = await _newsService.GetAllNews(AppConstants.User.ClassID, AppConstants.User.DonVi);
                if (data.Code > 0)
                {
                    foreach (NewsModel News in data.Data)
                    {
                        if (string.Compare(News.NewsID, NewsID) == 0)
                        {
                            DetailNews = News;
                        }
                    }
                }
                else
                    DetailNews = new NewsModel();
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