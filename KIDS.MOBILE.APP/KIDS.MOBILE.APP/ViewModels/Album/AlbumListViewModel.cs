using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.PostAlbum;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Services.Album;
using KIDS.MOBILE.APP.views.Album;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Album
{
    internal class AlbumListViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private ObservableCollection<AlbumModel> _albumData;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<AlbumModel> AlbumData
        {
            get => _albumData;
            set => SetProperty(ref _albumData, value);
        }

        private UserModel _user;
        private IAlbumService _albumService;
        public ICommand GoBackCommand { get; set; }
        public ICommand DetailAlbumCommand { get; set; }
        public ICommand AddNewAlbumCommand { get; set; }
        private IDialogService _dialogService;

        public AlbumListViewModel(INavigationService navigationService, IAlbumService albumService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            AddNewAlbumCommand = new Command(AddNewAlbum);
            DetailAlbumCommand = new Command<AlbumModel>(AlbumDetail);
            GoBackCommand = new Command(GoBack);
            _albumService = albumService;
            _navigationService = navigationService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializationAlbumpost();
        }

        private async Task InitializationAlbumpost()
        {
            try
            {
                IsLoading = true;
                _user = AppConstants.User;
                var data = await _albumService.GetAllAlbum(_user.ClassID, _user.DonVi);
                if (data.Code > 0)
                {
                    AlbumData = new ObservableCollection<AlbumModel>(data.Data);
                }
                else
                    AlbumData = new ObservableCollection<AlbumModel>();
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

        private void AddNewAlbum()
        {
            _dialogService.ShowDialog(nameof(PostAlbumDialog));
        }

        private void AlbumDetail(AlbumModel obj)
        {
            var para = new NavigationParameters();
            para.Add(AppConstants.DetailAlbums, obj);
            _navigationService.NavigateAsync(nameof(AlbumDetailPage), para, useModalNavigation: true);
        }

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
    }
}