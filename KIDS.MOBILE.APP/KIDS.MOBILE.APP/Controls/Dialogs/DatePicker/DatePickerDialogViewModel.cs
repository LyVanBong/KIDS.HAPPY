using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.ViewModels;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.NewFolder
{
    public class DatePickerDialogViewModel : BaseViewModel,IDialogAware
    {
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private IPageDialogService _pageDialogService;
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public DatePickerDialogViewModel(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            OkCommand = new Command(SelectDate);
            CancelCommand = new Command(Cancel);
        }
        private void SelectDate()
        {
            if (RequestClose != null)
            {
                var para = new DialogParameters();
                para.Add(AppConstants.ChoosedDate, Date);
                RequestClose(para);
            }
        }
        private void Cancel()
        {
            SelectDate();
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
            if (parameters != null)
            {
                if (parameters.ContainsKey("ChoosedDate"))
                {
                    Date = DateTime.Parse(parameters.GetValue<string>("ChoosedDate"));
                }
            }
        }
        public event Action<IDialogParameters> RequestClose;
    }
}