using Prism.Mvvm;

namespace KIDS.MOBILE.APP.Models.Evaluate
{
    public class EvaluationCriteriaModel : BindableBase
    {
        private bool _result;
        public string ID { get; set; }
        public string NamHoc { get; set; }
        public string ClassID { get; set; }
        public string StudentID { get; set; }
        public string AssessID { get; set; }
        public string STT { get; set; }
        public string Name { get; set; }
        public string Sort { get; set; }
        public string ParentID { get; set; }

        public bool Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public string CreateDate { get; set; }
        public string UserCreate { get; set; }
    }
}