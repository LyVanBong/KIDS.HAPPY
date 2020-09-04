using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.Prescription;
using KIDS.MOBILE.APP.Resources;
using KIDS.MOBILE.APP.Services.Prescription;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.MedicineReminders
{
    public class DetailMedicineRemindersViewModel : BaseViewModel
    {
        private IPageDialogService _pageDialogService;
        private IPrescriptionService _prescriptionService;
        private PrescriptionModel _medicineData;
        private UserModel _userData;
        private List<PrescriptionDetailModel> _detailMeicineData;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<PrescriptionDetailModel> DetailMeicineData
        {
            get => _detailMeicineData;
            set => SetProperty(ref _detailMeicineData, value);
        }

        public UserModel UserData
        {
            get => _userData;
            set => SetProperty(ref _userData, value);
        }

        public PrescriptionModel MedicineData
        {
            get => _medicineData;
            set => SetProperty(ref _medicineData, value);
        }

        public ICommand ConfirmedMedicineCommand { get; set; }

        public DetailMedicineRemindersViewModel(IPrescriptionService prescriptionService, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _prescriptionService = prescriptionService;
            ConfirmedMedicineCommand = new Command(ConfirmedMedicine);
        }

        private async void ConfirmedMedicine()
        {
            if (!MedicineData.Status)
            {
                var res = await _pageDialogService.DisplayAlertAsync(AppResources._00061, AppResources._00062, AppResources._00064, AppResources._00063);
                if (res)
                {
                    var approve = await _prescriptionService.ApprovePrescription(MedicineData.ID, AppConstants.User.NguoiSuDung,
                        AppResources._00061);
                    if (approve.Code > 0)
                    {
                        MedicineData.Status = true;
                    }
                }
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            string MedicalID = "";
            bool isNotif = false;
            if (parameters.ContainsKey(AppConstants.DetailMedicine))
            {
                var data = parameters[AppConstants.DetailMedicine] as PrescriptionModel;
                if (data != null)
                    MedicineData = data;
            }
            if (parameters.ContainsKey(AppConstants.NotificationID))
            {
                var data = parameters[AppConstants.NotificationID] as string;
                if (data != "")
                {
                    isNotif = true;
                    MedicalID = data;
                }
            }

            await InitializationDetailMedicine(MedicalID, isNotif);
        }

        private async Task InitializationDetailMedicine(string MedicalID, bool isNotif)
        {
            try
            {
                UserData = AppConstants.User;
                if (isNotif)
                {
                    var data = await _prescriptionService.GetAllPrescription(UserData.NguoiSuDung,
                              UserData.ClassID);
                    if (data.Code > 0)
                    {
                        foreach (PrescriptionModel item in data.Data)
                        {
                            if (string.Compare(item.ID, MedicalID) == 0)
                                MedicineData = item;
                        }
                    }
                }
                var detail = await _prescriptionService.GetAllPrescriptionDetail(MedicineData.ID);
                if (detail.Code > 0)
                {
                    DetailMeicineData = new List<PrescriptionDetailModel>(detail.Data);
                }
                else
                {
                    DetailMeicineData = new List<PrescriptionDetailModel>();
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