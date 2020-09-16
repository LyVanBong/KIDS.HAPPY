using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.AbsenceRequest;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.Prescription;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.AbsenceRequest;
using KIDS.MOBILE.APP.Services.Prescription;
using KIDS.MOBILE.APP.views.MedicineReminders;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.TicketManagement
{
    public class TicketManagementViewModel : BaseViewModel, IActiveAware
    {
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;
        private IPrescriptionService _prescriptionService;
        private bool _isActive;
        private List<PrescriptionModel> _medicineData;
        private int _selectIndexTabview;
        private List<AbsenceRequestModel> _abseceData;

        public bool IsCloseTicketManager
        {
            get => _isCloseTicketManager;
            set => SetProperty(ref _isCloseTicketManager, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, IsActiveChange);
        }

        public event EventHandler IsActiveChanged;

        public List<PrescriptionModel> MedicineData
        {
            get => _medicineData;
            set => SetProperty(ref _medicineData, value);
        }

        public List<AbsenceRequestModel> AbseceData
        {
            get => _abseceData;
            set => SetProperty(ref _abseceData, value);
        }

        public int SelectIndexTabview
        {
            get => _selectIndexTabview;
            set => SetProperty(ref _selectIndexTabview, value, async () => await InitializationTicketmanagerment());
        }

        public UserModel UserData { get; set; }
        private IAbsenceRequestService _absenceRequestService;
        private bool _isLoading;
        private bool _isCloseTicketManager;
        public ICommand ConfirmedMedicineCommand { get; set; }
        public ICommand ShowDetailMedicineCommand { get; set; }
        public ICommand ConfirmAbsenceCommand { get; set; }

        public TicketManagementViewModel(IPrescriptionService prescriptionService, IAbsenceRequestService absenceRequestService, IPageDialogService pageDialogService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _prescriptionService = prescriptionService;
            _absenceRequestService = absenceRequestService;
            ConfirmedMedicineCommand = new Command<PrescriptionModel>(ConfirmedMedicine);
            ShowDetailMedicineCommand = new Command<PrescriptionModel>(ShowDetailMedicine);
            ConfirmAbsenceCommand = new Command<AbsenceRequestModel>(ConfirmAbsence);
        }

        private async void ConfirmAbsence(AbsenceRequestModel obj)
        {
            if (!obj.Status)
            {
                var res = await _pageDialogService.DisplayAlertAsync(AppResources._00061, AppResources._00062, AppResources._00064, AppResources._00063);
                if (res)
                {
                    var approve = await _absenceRequestService.DetailAbsenceRepuest(obj.ID, AppConstants.User.NguoiSuDung,
                        AppResources._00061);
                    if (approve.Code > 0)
                    {
                        obj.Status = true;
                    }
                }
            }
        }

        private void ShowDetailMedicine(PrescriptionModel obj)
        {
            var parameter = new NavigationParameters();
            parameter.Add(AppConstants.DetailMedicine, obj);
            _navigationService.NavigateAsync(nameof(DetailMedicineRemindersPage), parameter, useModalNavigation: true);
        }

        private async void ConfirmedMedicine(PrescriptionModel obj)
        {
            if (!obj.Status)
            {
                var res = await _pageDialogService.DisplayAlertAsync(AppResources._00061, AppResources._00062, AppResources._00064, AppResources._00063);
                if (res)
                {
                    var approve = await _prescriptionService.ApprovePrescription(obj.ID, AppConstants.User.NguoiSuDung,
                         AppResources._00061);
                    if (approve.Code > 0)
                    {
                        obj.Status = true;
                    }
                }
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(AppConstants.NotificationID))
            {
                if (parameters[AppConstants.NotificationID] != null)
                {
                    IsCloseTicketManager = true;
                    SelectIndexTabview = 1;
                }
            }
            await InitializationTicketmanagerment();
        }

        private async void IsActiveChange()
        {
            if (IsActive)
            {
                await InitializationTicketmanagerment();
            }
            else
            {
            }
        }

        private async Task InitializationTicketmanagerment()
        {
            IsLoading = true;
            try
            {
                UserData = AppConstants.User;
                if (UserData != null)
                {
                    switch (SelectIndexTabview)
                    {
                        case 0:
                            var data = await _prescriptionService.GetAllPrescription(UserData.NguoiSuDung,
                                UserData.ClassID);
                            if (data.Code > 0)
                            {
                                MedicineData = new List<PrescriptionModel>(data.Data);
                            }

                            break;

                        case 1:
                            var dataAbsence =
                                await _absenceRequestService.GetAllAbsenceRequest(UserData.NguoiSuDung,
                                    UserData.ClassID);
                            if (dataAbsence.Code > 0)
                            {
                                AbseceData = new List<AbsenceRequestModel>(dataAbsence.Data);
                            }

                            break;
                    }
                }
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
    }
}