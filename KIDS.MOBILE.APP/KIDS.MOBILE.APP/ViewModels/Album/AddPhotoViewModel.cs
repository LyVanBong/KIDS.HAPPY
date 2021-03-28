using System;
using System.Threading.Tasks;
using System.Windows.Input;
using KIDS.MOBILE.APP.Models.Album;
using Microsoft.AppCenter.Crashes;
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

        public AddPhotoViewModel(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            SelectFeatureCommand = new AsyncCommand<string>(async (key) => await SelectFeature(key));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                _key = parameters.GetValue<int>("key");
                PhotoModel = _dataPhoto[_key];
            }
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
                var photo = await FilePicker.PickMultipleAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = PhotoModel.Title,
                });
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }
    }
}