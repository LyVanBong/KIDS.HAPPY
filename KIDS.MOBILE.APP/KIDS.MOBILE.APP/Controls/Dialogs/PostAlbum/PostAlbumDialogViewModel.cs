using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Album;
using KIDS.MOBILE.APP.ViewModels;
using Microsoft.AppCenter.Crashes;
using Plugin.Media;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.PostAlbum
{
    internal class PostAlbumDialogViewModel : BaseViewModel, IDialogAware
    {
        private string _thumbnail;
        private string _description;
        private IAlbumService _albumService;
        private IPageDialogService _pageDialogService;
        private Image _image;
        private string _numOfPic;
        public string NumOfPic
        {
            get => _numOfPic;
            set => SetProperty(ref _numOfPic, value);
        }
        public Image image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public string Thumbnail
        {
            get => _thumbnail;
            set => SetProperty(ref _thumbnail, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ICommand GoBackCommand { get; }
        public ICommand PostAlbumCommand { get; }
        public ICommand AddImageCommand { get; }

        public PostAlbumDialogViewModel(IAlbumService albumService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _albumService = albumService;
            GoBackCommand = new Command(OnCloseTapped);
            PostAlbumCommand = new Command(async () => await PostAlbum());
            AddImageCommand = new Command(async () => await AddImage());
        }
        private async Task AddImage()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return;
                }
                //chọn 1 ảnh
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                {
                    NumOfPic = AppResources._00108;
                    return;
                }
                NumOfPic = file.AlbumPath;
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            catch
            {

            }
        }
        private async Task PostAlbum()
        {
            try
            {
                if (Description != null)
                {
                    var para = new InsertAlbumModel()
                    {
                        ClassID = AppConstants.User.ClassID,
                        Thumbnail = "19032020085941PM_400X300.jpg",
                        Description = Description,
                        DateCreate = DateTime.Now.ToString("yyyy/MM/dd hh:mm"),
                        UserCreate = AppConstants.User.NguoiSuDung,
                    };
                    var data = await _albumService.InsertAlbum(para);
                    if (data.Code > 0)
                    {
                        await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00082, "OK");
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00060, "OK");
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00060, "OK");
            }
        }

        private void OnCloseTapped()
        {
            RequestClose(new DialogParameters());
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public event Action<IDialogParameters> RequestClose;
    }
}