namespace DelightBistroMinimalApi.ModelsDto
{
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public string? Discription { get; set; }
        public int StatusCode { get; set; }

        public ApiErrorResponse(string message, int statusCode, string? discription = null)
        {
            Message = message;
            StatusCode = statusCode;
            Discription = discription;
        }
    }
}
