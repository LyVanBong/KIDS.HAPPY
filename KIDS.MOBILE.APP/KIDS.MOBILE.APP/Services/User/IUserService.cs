using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> ChangePasswd(string userName, string passwd);
        /// <summary>
        /// lấy danh sách học sinh trong một lớp học
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<StudentModel>>> GetStudents(string classId);

        /// <summary>
        /// lấy thông tin danh sách phụ huynh
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<ParentModel>>> GetParentOfStudent(string studentId);

        /// <summary>
        /// lấy thông tin giáo viên
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<TeacherModel>>> GetTeacher(string teacherid);

        /// <summary>
        /// cập nhật thông tin giáo viên
        /// </summary>
        /// <param name="TeacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateTeacher(UpdateTeacherModel update);
    }
}