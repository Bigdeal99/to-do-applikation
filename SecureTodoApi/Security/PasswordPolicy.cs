using System.Text.RegularExpressions;

namespace SecureTodoApi.Security
{
    public class PasswordPolicy
    {
        public bool HasMinimumLength(string password) => password.Length >= 8;
        public bool HasUpperCase(string password) => password.Any(char.IsUpper);
        public bool HasLowerCase(string password) => password.Any(char.IsLower);
        public bool HasDigit(string password) => password.Any(char.IsDigit);
        public bool HasSpecialCharacter(string password) => password.Any(c => !char.IsLetterOrDigit(c));
        public bool HasNoCommonPasswords(string password) => !IsCommonPassword(password);
        public bool HasNoRepeatingCharacters(string password) => !HasRepeatingChars(password, 3);

        private bool IsCommonPassword(string password)
        {
            var commonPasswords = new[]
            {
                "password", "123456", "12345678", "qwerty", "abc123",
                "monkey", "letmein", "dragon", "111111", "baseball",
                "iloveyou", "trustno1", "sunshine", "master", "welcome",
                "shadow", "ashley", "football", "jesus", "michael",
                "ninja", "mustang", "password1", "123456789", "password123"
            };

            return commonPasswords.Contains(password.ToLower());
        }

        private bool HasRepeatingChars(string str, int maxRepeating)
        {
            for (int i = 0; i < str.Length - maxRepeating + 1; i++)
            {
                bool allSame = true;
                for (int j = 1; j < maxRepeating; j++)
                {
                    if (str[i] != str[i + j])
                    {
                        allSame = false;
                        break;
                    }
                }
                if (allSame) return true;
            }
            return false;
        }

        public (bool IsValid, string[] Errors) ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (!HasMinimumLength(password))
                errors.Add("Password must be at least 8 characters long");
            if (!HasUpperCase(password))
                errors.Add("Password must contain at least one uppercase letter");
            if (!HasLowerCase(password))
                errors.Add("Password must contain at least one lowercase letter");
            if (!HasDigit(password))
                errors.Add("Password must contain at least one number");
            if (!HasSpecialCharacter(password))
                errors.Add("Password must contain at least one special character");
            if (!HasNoCommonPasswords(password))
                errors.Add("Password is too common");
            if (!HasNoRepeatingCharacters(password))
                errors.Add("Password contains too many repeating characters");

            return (!errors.Any(), errors.ToArray());
        }
    }
} 