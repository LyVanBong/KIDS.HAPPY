using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace KIDS.MOBILE.APP.Models.Prescription
{
    public class PrescriptionModel : BindableBase
    {
        private bool _status;

        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("StudentID")]
        public string StudentID { get; set; }

        [JsonProperty("FromDate")]
        public DateTime? FromDate { get; set; }

        public string TmpFromDate => FromDate?.ToString("dd/MM/yyyy");

        [JsonProperty("ToDate")]
        public DateTime? ToDate { get; set; }

        public string TmpToDate => ToDate?.ToString("dd/MM/yyyy");

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Date")]
        public DateTime? Date { get; set; }

        public string TmpDate => Date?.ToString("hh:mm:ss dd/MM/yyyy");

        [JsonProperty("Status")]
        public bool Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        [JsonProperty("Approver")]
        public string Approver { get; set; }

        [JsonProperty("DateApprove")]
        public DateTime? DateApprove { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("NguoiGui")]
        public string NguoiGui { get; set; }

        [JsonProperty("Picture")]
        public string Picture { get; set; }

        public string TmpPicture => AppConstants.UriBaseWebForm + Picture;

        [JsonProperty("StatusName")]
        public string StatusName { get; set; }
    }
}