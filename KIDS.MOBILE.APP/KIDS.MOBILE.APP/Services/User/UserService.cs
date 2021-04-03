using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace KIDS.MOBILE.APP.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;

        public UserService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> ChangePasswd(string userName, string passwd)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("UserName",userName),
                    new RequestParameter("PassWord",passwd),
                };
                var data = await _requestProvider.PostAsync<string>("ChangePassWord", parameters);
                return data;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        public async Task<ResponseModel<IEnumerable<StudentModel>>> GetStudents(string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<StudentModel>>("Student/Select/Student", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<ParentModel>>> GetParentOfStudent(string studentId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("StudentID",studentId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<ParentModel>>("Student/Select/Parent", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<TeacherModel>>> GetTeacher(string teacherid)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("teacherid",teacherid),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<TeacherModel>>("Teacher/Profile", parameters);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ResponseModel<int>> UpdateTeacher(UpdateTeacherModel update)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("TeacherId",update.TeacherId),
                    new RequestParameter("Name",update.Name),
                    new RequestParameter("Sex",update.Sex),
                    new RequestParameter("DOB",update.DOB),
                    new RequestParameter("Email",update.Email),
                    new RequestParameter("Phone",update.Phone),
                    new RequestParameter("Address",update.Address),
                };
                Dictionary<string, FileResult> paraFile = new Dictionary<string, FileResult>();
                if (update.Picture != null && update.Picture.FileName != null)
                {
                    paraFile.Add("Picture", update.Picture);
                }
                var data = await _requestProvider.PostAsync<int>("Teacher/Update", parameters, paraFile);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}