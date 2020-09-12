using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Models.User;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.User
{
    public class UserService : IUserService
    {
        private IRequestProvider _requestProvider;

        public UserService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
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
            catch (Exception e)
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
                    new RequestParameter("Picture",update.Picture)
                };
                var data = await _requestProvider.PostAsync<int>("Teacher/Update", parameters);
                return data;
            }
            catch (Exception e)
            {
                
                return null;
            }
        }
    }
}