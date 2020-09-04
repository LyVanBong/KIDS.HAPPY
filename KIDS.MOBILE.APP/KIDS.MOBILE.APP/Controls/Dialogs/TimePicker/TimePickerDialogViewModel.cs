using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.ViewModels;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Controls.Dialogs.TimePicker
{
    public class TimePickerDialogViewModel : BaseViewModel, IDialogAware
    {
        private TimeSpan _time;
        private ICommand _okCommand;
        private string _contentTime;
        public ICommand CancelCommand { get; set; }

        public string ContentTime
        {
            get => _contentTime;
            set => SetProperty(ref _contentTime, value);
        }

        public ICommand OkCommand
        {
            get => _okCommand;
            set => SetProperty(ref _okCommand, value);
        }

        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public TimePickerDialogViewModel()
        {
            OkCommand = new Command(SelectTime);
            CancelCommand = new Command(Cancel);
        }

        private void Cancel()
        {
            SelectTime();
        }

        private void SelectTime()
        {
            if (RequestClose != null)
            {
                var para = new DialogParameters();
                para.Add(AppConstants.TimeSelect, Time);
                RequestClose(para);
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("StartNapping"))
                {
                    Time = TimeSpan.Parse(parameters.GetValue<string>("StartNapping"));
                }

                if (parameters.ContainsKey("EndNapping"))
                {
                    Time = TimeSpan.Parse(parameters.GetValue<string>("EndNapping"));
                }

                if (parameters.ContainsKey("Time"))
                {
                    Time = TimeSpan.Parse(parameters.GetValue<string>("Time"));
                }
            }
        }

        public event Action<IDialogParameters> RequestClose;
    }
}