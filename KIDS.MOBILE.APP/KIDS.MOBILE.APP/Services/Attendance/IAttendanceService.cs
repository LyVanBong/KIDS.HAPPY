using KIDS.MOBILE.APP.Models.Attendance;
using KIDS.MOBILE.APP.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Attendance
{
    public interface IAttendanceService
    {
        /// <summary>
        /// lây danh sách điểm danh về.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<AttendanceLeaveModel>>> AttendanceLeave(string classId, string date);

        /// <summary>
        /// điểm danh về
        /// </summary>
        /// <param name="id"></param>
        /// <param name="leave"></param>
        /// <param name="leaveLate"></param>
        /// <param name="leaveLateComment"></param>
        /// <param name="nguoiDon"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> AttendanceUpdateAfternoon(string id, string leave, bool leaveLate, string leaveLateComment, string nguoiDon, string teacherId);

        /// <summary>
        /// điểm danh sáng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="coMat"></param>
        /// <param name="nghiCoPhep"></param>
        /// <param name="nghiKhongPhep"></param>
        /// <param name="diMuon"></param>
        /// <param name="comment"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> AttendanceUpdateMorning(string id, bool coMat, bool nghiCoPhep, bool nghiKhongPhep, bool diMuon, string comment, string teacherId);

        /// <summary>
        /// Điểm danh học sinh đến
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<AttendanceComeModel>>> AttendanceCome(string classId, string date);

        /// <summary>
        /// Thống kê điểm danh học trinh trong lớp học
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<CountAttendanceModel>>> CountAttendance(string classId, DateTime date);
    }
}