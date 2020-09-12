using KIDS.MOBILE.APP.Models.Message;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Message
{
    public class MessageService : IMessageService
    {
        private IRequestProvider _requestProvider;

        public MessageService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> ComfirmedMessage(string communicationId, string isConfirmed, string approver, string content)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("CommunicationID",communicationId),
                    new RequestParameter("IsConfirmed",isConfirmed),
                    new RequestParameter("Approver",approver),
                    new RequestParameter("Content",content),
                };
                var data = await _requestProvider.PostAsync<int>("Communication/Approve", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<CommentModel>>> GetComment(string parent)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("Parent",parent),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<CommentModel>>("Communication/Select/Reply", para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<int>> CreateSentandComment(string classId, string teacherId, string parent, string content, string dateCreate,
            string studentId, string type)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",classId),
                    new RequestParameter("TeacherID",teacherId),
                    new RequestParameter("Parent",parent),
                    new RequestParameter("Content",content),
                    new RequestParameter("DateCreate",dateCreate),
                    new RequestParameter("StudentID",studentId),
                    new RequestParameter("Type",type),
                };
                var data = await _requestProvider.PostAsync<int>("Communication/InsertTeacher", para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public Task<ResponseModel<int>> ConfirmedMessage()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<int>> DeleteMessage(string communicationId)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("CommunicationID",communicationId),
                };
                var data = await _requestProvider.PostAsync<int>("Communication/DeleteTeacher", para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<int>> UpdateMessage(string communicationId, string content, string dateCreate)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("CommunicationID",communicationId),
                    new RequestParameter("Content",content),
                    new RequestParameter("DateCreate",dateCreate),
                };
                var data = await _requestProvider.PostAsync<int>("Communication/UpdateTeacher", para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<SentModel>>> GetInbox(string classId)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<SentModel>>("Communication/Select/Class", para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<SentModel>>> GetWasSent(string teacherId)
        {
            try
            {
                var para = new List<RequestParameter>
                {
                    new RequestParameter("TeacherID",teacherId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<SentModel>>("Communication/Select/Teacher",
                    para);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
    }
}