using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private IRequestProvider _requestProvider;

        public AttendanceService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<IEnumerable<AttendanceLeaveModel>>> AttendanceLeave(string classId, string date)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("classId",classId),
                    new RequestParameter("date",date),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<AttendanceLeaveModel>>("Attendance/Afternoon", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<int>> AttendanceUpdateAfternoon(string id, string leave, bool leaveLate, string leaveLateComment, string nguoiDon,
            string teacherId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("Id",id),
                    new RequestParameter("Leave",leave),
                    new RequestParameter("LeaveLate",leaveLate.ToString()),
                    new RequestParameter("LeaveComment",leaveLateComment),
                    new RequestParameter("NguoiDon",nguoiDon),
                    new RequestParameter("TeacherId",teacherId),
                };
                var data = await _requestProvider.PostAsync<int>("Attendance/UpdateAfternoon", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<int>> AttendanceUpdateMorning(string id, bool coMat, bool nghiCoPhep, bool nghiKhongPhep, bool diMuon, string comment,
            string teacherId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("Id",id),
                    new RequestParameter("CoMat",coMat.ToString()),
                    new RequestParameter("NghiCoPhep",nghiCoPhep.ToString()),
                    new RequestParameter("NghiKhongPhep",nghiKhongPhep.ToString()),
                    new RequestParameter("DiMuon",diMuon.ToString()),
                    new RequestParameter("Comment",comment),
                    new RequestParameter("TeacherId",teacherId),
                };
                var data = await _requestProvider.PostAsync<int>("Attendance/UpdateMorning", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<AttendanceComeModel>>> AttendanceCome(string classId, string date)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassId",classId),
                    new RequestParameter("Date",date),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<AttendanceComeModel>>("Attendance/Morning", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<CountAttendanceModel>>> CountAttendance(string classId, DateTime date)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassId",classId),
                    new RequestParameter("Date",date.ToString("yyyy/MM/dd")),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<CountAttendanceModel>>("Attendance/Count", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
    }
}