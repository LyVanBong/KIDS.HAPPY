using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Evaluate;
using KIDS.MOBILE.APP.Services.Evaluate;
using KIDS.MOBILE.APP.views.Evaluate;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.ViewModels.Evaluate
{
    public class EvaluateViewModel : BaseViewModel
    {
        private IEvaluateService _evaluateService;
        private List<EvaluationBoardModel> _evaluationBoardData;
        private EvaluationBoardModel _selectedItemComboBox;
        private ObservableCollection<StudentAssessmentModel> _studentAssessmentData;
        private bool _isLoading;
        private StudentAssessmentModel _selectedItemDataGrid;
        private INavigationService _navigationService;

        public StudentAssessmentModel SelectedItemDataGrid
        {
            get => _selectedItemDataGrid;
            set
            {
                if (SetProperty(ref _selectedItemDataGrid, value))
                {
                    if (SelectedItemDataGrid != null)
                    {
                        GetEvaluationCriteria(SelectedItemDataGrid);
                    }
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<StudentAssessmentModel> StudentAssessmentData
        {
            get => _studentAssessmentData;
            set => SetProperty(ref _studentAssessmentData, value);
        }

        public EvaluationBoardModel SelectedItemComboBox
        {
            get => _selectedItemComboBox;
            set
            {
                if (SetProperty(ref _selectedItemComboBox, value))
                {
                    GetStudentAssessment(SelectedItemComboBox);
                }
            }
        }

        public List<EvaluationBoardModel> EvaluationBoardData
        {
            get => _evaluationBoardData;
            set => SetProperty(ref _evaluationBoardData, value);
        }

        public EvaluateViewModel(IEvaluateService evaluateService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _evaluateService = evaluateService;
        }

        private void GetEvaluationCriteria(StudentAssessmentModel item)
        {
            try
            {
                IsLoading = true;
                var para = new NavigationParameters();
                para.Add(AppConstants.EvaluationCriteria, item);
                para.Add("IdDanhMucChiTieu", SelectedItemComboBox.ID);
                _navigationService.NavigateAsync(nameof(EvaluationCriteriaPage), para, useModalNavigation: true);
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

        private async void GetStudentAssessment(EvaluationBoardModel item)
        {
            try
            {
                IsLoading = true;
                var data = await _evaluateService.GetStudentAssessment(item.ID);
                if (data != null && data.Code > 0)
                {
                    StudentAssessmentData = new ObservableCollection<StudentAssessmentModel>(data.Data);
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializeEvaluate();
        }

        private async Task InitializeEvaluate()
        {
            try
            {
                IsLoading = true;
                var user = AppConstants.User;
                var data = await _evaluateService.GetEvaluationBoard(user.DonVi,
                    user.ClassID);
                if (data != null && data.Code > 0)
                {
                    EvaluationBoardData = new List<EvaluationBoardModel>(data.Data);
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