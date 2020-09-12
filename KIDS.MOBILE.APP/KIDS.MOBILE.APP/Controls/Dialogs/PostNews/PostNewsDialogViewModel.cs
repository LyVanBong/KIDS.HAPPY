using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.News;
using KIDS.MOBILE.APP.ViewModels;
using Microsoft.AppCenter.Crashes;
using Plugin.Media;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.PostNews
{
    public class PostNewsDialogViewModel : BaseViewModel, IDialogAware
    {
        private string _title;
        private string _content;
        private INewsService _newsService;
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

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand GoBackCommand { get; }
        public ICommand PostNewsCommand { get; }
        public ICommand AddImageCommand { get; }

        public PostNewsDialogViewModel(INewsService newsService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _newsService = newsService;
            GoBackCommand = new Command(OnCloseTapped);
            PostNewsCommand = new Command(async () => await PostNews());
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

        private async Task PostNews()
        {
            try
            {
                if (Title != null && Content != null)
                {
                    var para = new InsertNewsModel()
                    {
                        ClassID = AppConstants.User.ClassID,
                        Title = Title,
                        Content = Content,
                        ImageUrl = "/NewsUpload/19032020085941PM_400X300.jpg",
                        DateCreate = DateTime.Now.ToString("yyyy/MM/dd hh:mm"),
                        UserCreate = AppConstants.User.NguoiSuDung,
                    };
                    var data = await _newsService.InsertNews(para);
                    if (data.Code > 0)
                    {
                        await _pageDialogService.DisplayAlertAsync(AppResources._00007, AppResources._00054, "OK");
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
            if (RequestClose != null)
            {
                RequestClose(new DialogParameters());
            }
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