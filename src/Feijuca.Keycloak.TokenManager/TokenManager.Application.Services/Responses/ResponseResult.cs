using TokenManager.Domain.Entities;

namespace TokenManager.Application.Services.Responses
{
    public class ResponseResult
    {
        protected ResponseResult(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static ResponseResult Success() => new(true, Error.None);

        public static ResponseResult Failure(Error error) => new(false, error);
    }

    public class ResponseResult<T> : Result
    {
        private readonly T _result;

        private ResponseResult(T value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _result = value;
        }

        public T Result
        {
            get
            {
                return _result;
            }
        }

        public static ResponseResult<T> Success(T value) => new(value, true, Error.None);

        public static new ResponseResult<T> Failure(Error error) => new(default!, false, error);
    }
}
