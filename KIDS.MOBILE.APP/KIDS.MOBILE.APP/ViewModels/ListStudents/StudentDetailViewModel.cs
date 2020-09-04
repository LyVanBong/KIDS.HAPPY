using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Services.User;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.ListStudents
{
    public class StudentDetailViewModel : BaseViewModel
    {
        private IUserService _userService;
        private StudentModel _student;
        private List<ParentModel> _parentData;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<ParentModel> ParentData
        {
            get => _parentData;
            set => SetProperty(ref _parentData, value);
        }

        public StudentModel Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }

        public ICommand CallParentStudentCommand { get; set; }

        public StudentDetailViewModel(IUserService userService)
        {
            _userService = userService;
            CallParentStudentCommand = new Command<string>(CallParentStudent);
        }

        private void CallParentStudent(string obj)
        {
            try
            {
                PhoneDialer.Open(obj);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsLoading = true;
            if (parameters.ContainsKey(AppConstants.StudentDetail))
            {
                var data = parameters[AppConstants.StudentDetail] as StudentModel;
                if (data != null)
                {
                    Student = data;
                    var parent = await _userService.GetParentOfStudent(data.StudentID);
                    if (parent.Code > 0)
                    {
                        ParentData = new List<ParentModel>(parent.Data);
                    }
                }
            }

            IsLoading = false;
        }
    }
}