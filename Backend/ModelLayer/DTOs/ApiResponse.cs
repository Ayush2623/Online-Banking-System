namespace ModelLayer.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse()
        {
            Success = false;
            Message = string.Empty;
        }

        public ApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> SuccessResponse(string message, T? data = default)
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>(false, message);
        }
    }

    // For responses without data
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base() { }
        
        public ApiResponse(bool success, string message) : base(success, message, null) { }

        public static ApiResponse Success(string message)
        {
            return new ApiResponse(true, message);
        }

        public static ApiResponse Error(string message)
        {
            return new ApiResponse(false, message);
        }
    }
}