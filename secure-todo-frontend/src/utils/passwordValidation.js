export const validatePassword = (password) => {
  const errors = [];

  if (password.length < 8) {
    errors.push("Password must be at least 8 characters long");
  }
  if (!/[A-Z]/.test(password)) {
    errors.push("Password must contain at least one uppercase letter");
  }
  if (!/[a-z]/.test(password)) {
    errors.push("Password must contain at least one lowercase letter");
  }
  if (!/[0-9]/.test(password)) {
    errors.push("Password must contain at least one number");
  }
  if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
    errors.push("Password must contain at least one special character");
  }
  if (/(.)\1{2,}/.test(password)) {
    errors.push("Password contains too many repeating characters");
  }

  const commonPasswords = [
    "password", "123456", "12345678", "qwerty", "abc123",
    "monkey", "letmein", "dragon", "111111", "baseball",
    "iloveyou", "trustno1", "sunshine", "master", "welcome",
    "shadow", "ashley", "football", "jesus", "michael",
    "ninja", "mustang", "password1", "123456789", "password123"
  ];

  if (commonPasswords.includes(password.toLowerCase())) {
    errors.push("Password is too common");
  }

  return {
    isValid: errors.length === 0,
    errors
  };
}; 