using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Services.PickMedia;
using Microsoft.AppCenter.Crashes;
using Plugin.Permissions;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Album
{
    public class AddPhotoViewModel : BaseViewModel
    {
        private string _bodyContent;
        private string _captionContent;
        private int _key;
        private IPageDialogService _pageDialogService;
        IMultiMediaPickerService _multiMediaPickerService;

        private AddPhotoModel[] _dataPhoto = new AddPhotoModel[]
        {
            new AddPhotoModel()
            {
                Title = "Thêm album ảnh",
                CaptionHeader = "Tên album",
                BodyHeader = "Thêm mô tả"
            },
            new AddPhotoModel()
            {
                Title = "Thêm bài viết",
                CaptionHeader = "Nhập tiêu đề bài viết",
                BodyHeader = "Nhập nội dung bài viết"
            },
        };

        private AddPhotoModel _photoModel;

        public AddPhotoModel PhotoModel
        {
            get => _photoModel;
            set => SetProperty(ref _photoModel, value);
        }

        public ICommand SelectFeatureCommand { get; private set; }

        public string BodyContent
        {
            get => _bodyContent;
            set => SetProperty(ref _bodyContent, value);
        }

        public string CaptionContent
        {
            get => _captionContent;
            set => SetProperty(ref _captionContent, value);
        }

        private ObservableCollection<MediaFile> _Media = new ObservableCollection<MediaFile>();
        public ObservableCollection<MediaFile> Media
        {
            get => _Media;
            set
            {
                _Media = value;
                RaisePropertyChanged(nameof(Media));
            }
        }

        public AddPhotoViewModel(IPageDialogService pageDialogService)
        {
            try
            {
                _pageDialogService = pageDialogService;
                _multiMediaPickerService = Xamarin.Forms.DependencyService.Get<IMultiMediaPickerService>();
                SelectFeatureCommand = new AsyncCommand<string>(async (key) => await SelectFeature(key));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                _key = parameters.GetValue<int>("key");
                PhotoModel = _dataPhoto[_key];
            }
            Media?.Clear();
        }

        private async Task SelectFeature(string key)
        {
            try
            {
                switch (key)
                {
                    case "0":
                        await _pageDialogService.DisplayActionSheetAsync("Chọn ảnh", ActionSheetButton.CreateCancelButton("Bỏ qua"), null, ActionSheetButton.CreateButton("Thư viên", async () => await ChonAnh()), ActionSheetButton.CreateButton("Máy ảnh", async () => await DungMayAnh()));
                        break;
                    case "1":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task DungMayAnh()
        {
            try
            {
                var capture = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = "Chụp ảnh"
                });
                if (capture != null)
                {
                    var stream = await capture.OpenReadAsync();
                    
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task ChonAnh()
        {
            try
            {
                if (!App.IsSubcribed)
                {
                    MessagingCenter.Subscribe<App, ObservableCollection<MediaFile>>((App)Xamarin.Forms.Application.Current, "PickPhotos", (sender, arg) => {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var mediaList = arg;
                            if(mediaList?.Count > 0)
                            {
                                foreach(var item in mediaList)
                                {
                                    Media.Add(item);
                                }
                            }
                        });
                    });
                }
                
                var hasPermission = await CheckPermissionsAsync();
                if (hasPermission)
                {
                    var imageList = await _multiMediaPickerService.PickPhotosAsync();
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private async Task<bool> CheckPermissionsAsync()
        {
            var retVal = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.MediaLibrary))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Need Storage permission to access to your photos.", "Ok");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    retVal = true;

                }
                else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Permission Denied. Can not continue, try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await App.Current.MainPage.DisplayAlert("Alert", "Error. Can not continue, try again.", "Ok");
            }

            return retVal;

        }
    }
}