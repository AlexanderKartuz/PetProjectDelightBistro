namespace DelightBistroMinimalApi.ModelsDto
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
