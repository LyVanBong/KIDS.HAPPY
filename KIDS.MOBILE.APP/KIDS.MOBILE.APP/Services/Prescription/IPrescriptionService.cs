using KIDS.MOBILE.APP.Models.Prescription;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Prescription
{
    public interface IPrescriptionService
    {
        /// <summary>
        /// cập nhật trạng thái đơn thuốc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approve"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> ApprovePrescription(string id, string approve, string description);

        /// <summary>
        /// Lấy toàn bộ danh sách dặn thuốc
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<List<PrescriptionModel>>> GetAllPrescription(string teacherId, string classId);

        /// <summary>
        /// chi tiet don thuoc
        /// </summary>
        /// <param name="prescriptionID"></param>
        /// <returns></returns>
        Task<ResponseModel<PrescriptionDetailModel>> GetAllPrescriptionDetail(string prescriptionID);
    }
}