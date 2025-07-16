namespace Challenge.Skopia.TaskManagement.Application.DataTransferObjects;

public sealed class ApiResponse<T>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<ApiError>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operação realizada com sucesso", int status = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Status = status,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, int status = 400, List<ApiError>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Status = status,
            Message = message,
            Errors = errors ?? []
        };
    }
}

public sealed class ApiError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}