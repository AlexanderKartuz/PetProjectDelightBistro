namespace DelightBistroMvc.Data.Services.PasswordHasher
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private const int WORKFACTOR = 11;
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Пароль не должен быть пустым");
            }

            return BCrypt.Net.BCrypt.HashPassword(password, WORKFACTOR);

        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}
