namespace Concordance.Model
{
    public class ServiceResult<TData>
    {
        public bool IsSuccess { get; set; }
        public TData Data { get; set; }
        public string Error { get; set; }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }
}