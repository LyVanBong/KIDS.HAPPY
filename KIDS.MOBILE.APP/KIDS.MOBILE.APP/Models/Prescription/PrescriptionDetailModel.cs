using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KIDS.MOBILE.APP.Models.Prescription
{
    public class PrescriptionDetailModel
    {
        public Guid? Id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        public Guid? StudentID { get; set; }
        public Guid? ClassID { get; set; }
        public bool? Status { get; set; }
        public Guid? Approver { get; set; }
        public string Description { get; set; }
        public List<MedicineDetailTicketModel> MedicineList { get; set; }
    }

    public class MedicineDetailTicketModel
    {
        public Guid? Id { get; set; }
        public string Picture { get; set; }
        public string Note { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public int Action { get; set; } = 1;
        [JsonIgnore]
        public string Image => $"{AppConstants.UriBaseWebForm}{Picture}";
    }
}