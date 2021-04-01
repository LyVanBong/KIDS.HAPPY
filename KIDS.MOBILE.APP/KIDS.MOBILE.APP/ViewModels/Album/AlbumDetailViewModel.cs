using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Controls.Dialogs.ShowImage;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Services.Album;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Album
{
    internal class AlbumDetailViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IAlbumDetailService _albumDetailService;
        private IAlbumService _albumService;
        private ObservableCollection<AlbumDetailModel> _imageData;
        private AlbumModel _albumData;
        private bool _isLoading;

        private string _albumDescription;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string AlbumDescription
        {
            get => _albumDescription;
            set => SetProperty(ref _albumDescription, value);
        }

        public AlbumModel AlbumData
        {
            get => _albumData;
            set => SetProperty(ref _albumData, value);
        }

        public ObservableCollection<AlbumDetailModel> ImageData
        {
            get => _imageData;
            set => SetProperty(ref _imageData, value);
        }

        public ICommand ImageDetailCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand EditAlbumNameCommand { get; set; }
        public ICommand DeleteAlbumCommand { get; set; }
        public ICommand InsertImageCommand { get; set; }
        private IDialogService _dialogService;

        public AlbumDetailViewModel(IAlbumDetailService albumDetailService, INavigationService navigationService, IDialogService dialogService, IAlbumService albumServices)
        {
            _albumService = albumServices;
            _dialogService = dialogService;
            _albumDetailService = albumDetailService;
            _navigationService = navigationService;
            GoBackCommand = new Command(GoBack);
            EditAlbumNameCommand = new Command(EditAlbumName);
            DeleteAlbumCommand = new Command(DeleteAlbum);
            ImageDetailCommand = new Command<AlbumDetailModel>(ImageDetail);
            InsertImageCommand = new Command(async () => await InsertImage());
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            string AlbumID = "";
            bool isNotif = false;
            if (parameters.ContainsKey(AppConstants.DetailAlbums))
            {
                var data = parameters[AppConstants.DetailAlbums] as AlbumModel;
                if (data != null)
                {
                    AlbumData = data;
                    AlbumID = data.AlbumID;
                }
                else
                {
                    return;
                }
            }
            else if (parameters.ContainsKey(AppConstants.NotificationID))
            {
                var data = parameters[AppConstants.NotificationID] as string;
                if (data != null)
                {
                    AlbumID = data;
                    isNotif = true;
                }
                else
                {
                    return;
                }
            }
            await InitializationAlbumDetail(AlbumData, isNotif);
        }

        private async Task InitializationAlbumDetail(AlbumModel dataAlbum, bool isNotif)
        {
            try
            {
                IsLoading = true;
                if (isNotif)
                {
                    var data = await _albumService.GetAllAlbum(AppConstants.User.ClassID, AppConstants.User.DonVi);
                    if (data.Code > 0)
                    {
                        AlbumData = new ObservableCollection<AlbumModel>(data.Data).FirstOrDefault();
                    }
                    else
                        AlbumData = new ObservableCollection<AlbumModel>().FirstOrDefault();
                }
                var imageData = await _albumDetailService.GetAlbumDetail(dataAlbum.AlbumID, dataAlbum.UserCreate);
                if (imageData != null)
                {
                    if (imageData.Code > 0)
                    {
                        ImageData = new ObservableCollection<AlbumDetailModel>(imageData.Data);
                    }
                    else
                        ImageData = new ObservableCollection<AlbumDetailModel>();
                }
                else
                    ImageData = new ObservableCollection<AlbumDetailModel>();
                AlbumDescription = AlbumData.TenLop + " - " + ImageData.Count.ToString() + " Pictures";
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

        private async Task InsertImage()
        {
            try
            {

            }
            catch
            {
            }
        }

        private void ImageDetail(AlbumDetailModel obj)
        {
            //var para = new DialogParameters();
            //para.Add(AppConstants.ImageDetail, obj);
            //_dialogService.ShowDialog(nameof(ShowImageDialog), para);
        }

        private void DeleteAlbum()
        {
            //_navigationService.NavigateAsync(nameof());
        }

        private void EditAlbumName()
        {
            //_navigationService.NavigateAsync(nameof());
        }

        private void GoBack()
        {
            _navigationService.GoBackAsync();
        }
    }
}