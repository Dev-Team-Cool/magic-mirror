namespace MirrorOfErised.models
{
    public class RunnerResult
    {

        private string _errors;
        public string Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                if (!string.IsNullOrEmpty(value))
                    Failed = true;
            }
        }

        public string Output { get; set; }
        public bool Failed { get; set; }
        public TrainJob TrainJob { get; set; }
    }
}