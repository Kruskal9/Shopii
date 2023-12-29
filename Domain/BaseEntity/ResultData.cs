namespace Domain.BaseEntity
{
    public class ResultData<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public ResultData()
        {
        }

        public ResultData<T> FalseResult(string error)
        {
            Success = false;
            ErrorMessage = error;

            return this;
        }

        public ResultData<T> SuccessResult()
        {
            Success = true;
            return this;
        }
        public ResultData<T> SuccessResult(T data)
        {
            Data = data;
            Success = true;
            return this;
        }

    }
}
