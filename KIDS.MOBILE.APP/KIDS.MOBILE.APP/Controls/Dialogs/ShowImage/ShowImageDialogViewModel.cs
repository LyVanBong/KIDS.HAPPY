using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.ViewModels;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.ShowImage
{
    public class ShowImageDialogViewModel : BaseViewModel, IDialogAware
    {
        private string _imageData;
        private IPageDialogService _pageDialogService;

        public string ImageData
        {
            get => _imageData;
            set => SetProperty(ref _imageData, value);
        }

        public ICommand GoBackCommand { get; }

        public ShowImageDialogViewModel(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            GoBackCommand = new Command(OnCloseTapped);
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
            if (parameters.ContainsKey(AppConstants.ImageDetail))
            {
                var albumDetail = parameters.GetValue<AlbumDetailModel>(AppConstants.ImageDetail);
                ImageData = albumDetail.UriImage;
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}