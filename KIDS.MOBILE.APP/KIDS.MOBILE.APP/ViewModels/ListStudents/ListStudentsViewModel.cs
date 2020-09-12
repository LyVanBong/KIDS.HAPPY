using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Services.User;
using KIDS.MOBILE.APP.views.ListStudents;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.ViewModels.ListStudents
{
    public class ListStudentsViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IUserService _userService;
        private List<StudentModel> _students;
        private bool _isLoading;
        private List<StudentModel> _cache;
        private string _searchStudent;

        public string SearchStudent
        {
            get => _searchStudent;
            set
            {
                if (SetProperty(ref _searchStudent, value))
                {
                    if (string.IsNullOrWhiteSpace(SearchStudent))
                    {
                        Students = _cache;
                    }
                    else
                    {
                        Students = new List<StudentModel>(_cache.Where(x => x.Name.ToUpper().Contains(SearchStudent.ToUpper())));
                    }
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public ICommand StudentDetailCommand { get; set; }

        public ListStudentsViewModel(IUserService userService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _userService = userService;
            StudentDetailCommand = new Command<StudentModel>(async (sender) => await StudentDetail(sender));
        }

        private async Task StudentDetail(StudentModel obj)
        {
            try
            {
                var parameter = new NavigationParameters();
                parameter.Add(AppConstants.StudentDetail, obj);
                await _navigationService.NavigateAsync(nameof(StudentDetailPage), parameter, useModalNavigation: true);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitializationListStudents();
        }

        private async Task InitializationListStudents()
        {
            try
            {
                IsLoading = true;
                var data = await _userService.GetStudents(AppConstants.User.ClassID);
                if (data.Code > 0)
                {
                    _cache = Students = new List<StudentModel>(data.Data);
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