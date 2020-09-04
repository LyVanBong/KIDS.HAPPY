using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Evaluate;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using KIDS.MOBILE.APP.Services.Evaluate;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.Evaluate
{
    public class EvaluationCriteriaViewModel : BaseViewModel
    {
        private string _titleName;
        private StudentAssessmentModel _evaluationCriteria;
        private bool _isLoading;
        private List<EvaluationCriteriaModel> _evaluationCriteriaData;
        private IEvaluateService _evaluateService;
        private string _idBoard;
        public ICommand UpdateEvaluationCommand { get; private set; }
        public List<EvaluationCriteriaModel> EvaluationCriteriaData
        {
            get => _evaluationCriteriaData;
            set => SetProperty(ref _evaluationCriteriaData, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public StudentAssessmentModel EvaluationCriteria

        {
            get => _evaluationCriteria;
            set => SetProperty(ref _evaluationCriteria, value);
        }

        public string TitleName
        {
            get => _titleName;
            set => SetProperty(ref _titleName, value);
        }

        public EvaluationCriteriaViewModel(IEvaluateService evaluateService)
        {
            _evaluateService = evaluateService;
            UpdateEvaluationCommand = new Command<EvaluationCriteriaModel>(UpdateEvaluation);
        }

        private async void UpdateEvaluation(EvaluationCriteriaModel obj)
        {
            try
            {
                IsLoading = true;
                var data = await _evaluateService.UpdateEvaluation(obj.ID, UpdateResult(obj.Result));
                if (obj.Result)
                {
                    EvaluationCriteria.TyLe -= 25;
                    obj.Result = false;
                }
                else
                {
                    EvaluationCriteria.TyLe += 25;
                    obj.Result = true;
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
            string UpdateResult(bool res)
            {
                if (res)
                    return "false";
                else
                    return "true";
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            IsLoading = true;
            if (parameters.ContainsKey(AppConstants.EvaluationCriteria))
            {
                var data = parameters[AppConstants.EvaluationCriteria] as StudentAssessmentModel;
                _idBoard = parameters["IdDanhMucChiTieu"] as string;
                if (data != null)
                {
                    EvaluationCriteria = data;
                    TitleName = $"{data.Name} ({data.NickName})";
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InitializeEvaluationCriteria();
        }

        private async void InitializeEvaluationCriteria()
        {
            try
            {
                var data = await _evaluateService.GetEvaluationCriteria(_idBoard, EvaluationCriteria.StudentID);
                if (data != null && data.Code > 0)
                {
                    EvaluationCriteriaData = new List<EvaluationCriteriaModel>(data.Data);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
            finally { IsLoading = false; }
        }
    }
}