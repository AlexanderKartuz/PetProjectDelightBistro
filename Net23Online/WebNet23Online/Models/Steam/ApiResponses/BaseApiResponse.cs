namespace WebNet23Online.Models.Steam.ApiResponses
{
    public abstract class BaseApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
    }
}
