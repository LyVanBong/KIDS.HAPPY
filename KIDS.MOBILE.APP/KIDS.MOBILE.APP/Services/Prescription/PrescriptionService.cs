using KIDS.MOBILE.APP.Models.Prescription;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Prescription
{
    public class PrescriptionService : IPrescriptionService
    {
        private IRequestProvider _requestProvider;

        public PrescriptionService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> ApprovePrescription(string id, string approve, string description)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("Status","true"),
                    new RequestParameter("Approver",approve),
                    new RequestParameter("Description",description),
                };
                var data = await _requestProvider.PostAsync<string>("Prescription/Approve", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<List<PrescriptionModel>>> GetAllPrescription(string teacherId, string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("TeacherId",teacherId),
                    new RequestParameter("ClassId",classId),
                };
                var data = await _requestProvider.GetAsync<List<PrescriptionModel>>("Prescription/Select/Class", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<List<PrescriptionDetailModel>>> GetAllPrescriptionDetail(string prescriptionID)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("PrescriptionID",prescriptionID),
                };
                var data = await _requestProvider.GetAsync<List<PrescriptionDetailModel>>("Prescription/PrescriptionDetail", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
    }
}