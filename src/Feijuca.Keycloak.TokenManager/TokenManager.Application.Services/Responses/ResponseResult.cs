namespace TokenManager.Application.Services.Responses
{
    public class ResponseResult<T>
    {
        public T? Result { get; set; }
        public string? DetailMessage { get; set; }
    }
}
