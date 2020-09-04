using System.Collections.Generic;
using System.Threading.Tasks;
using KIDS.MOBILE.APP.Models.Message;
using KIDS.MOBILE.APP.Models.Response;

namespace KIDS.MOBILE.APP.Services.Message
{
    public interface IMessageService
    {
        /// <summary>
        /// xac nhan tin nhan cho hoc sinh
        /// </summary>
        /// <param name="communicationId"></param>
        /// <param name="isConfirmed"></param>
        /// <param name="approver"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> ComfirmedMessage(string communicationId,string isConfirmed,string approver,string Content);
        /// <summary>
        /// lấy danh ra danh sách bình luận trong tin nhăn
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<CommentModel>>> GetComment(string parent);
        /// <summary>
        /// tạo tin nhăn cho giao viên và chức năng bình luận
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="teacherId"></param>
        /// <param name="parent"></param>
        /// <param name="content"></param>
        /// <param name="dateCreate"></param>
        /// <param name="studentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> CreateSentandComment(string classId, string teacherId, string parent, string content, string dateCreate, string studentId, string type);
        Task<ResponseModel<int>> ConfirmedMessage();
        /// <summary>
        /// Xóa tin nhăn của giao viên tạo
        /// </summary>
        /// <param name="communicationId"></param>
        /// <param name="content"></param>
        /// <param name="dateCreate"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> DeleteMessage(string communicationId);
        /// <summary>
        /// cập nhật lại tin nhăn giao viên tạo
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateMessage(string communicationId, string content, string dateCreate);
        /// <summary>
        /// lấy danh sách tin nhăn của của phụ huynh
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<SentModel>>> GetInbox(string classId);
        /// <summary>
        /// lấy danh sách tin nhăn đã gửi
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<SentModel>>> GetWasSent(string teacherId);
    }
}