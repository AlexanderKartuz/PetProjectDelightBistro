namespace WebNet23Online.Models.Steam.ApiResponses
{
    public class ErrorApiResponse : BaseApiResponse
    {
        public ErrorApiResponse(string error)
        {
            IsSuccess = false;
            Error = error;
        }
    }
}
