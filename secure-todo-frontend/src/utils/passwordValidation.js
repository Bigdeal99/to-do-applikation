export const checkPasswordRules = (password) => {
  const rules = {
    minLength: password.length >= 8,
    hasUppercase: /[A-Z]/.test(password),
    hasLowercase: /[a-z]/.test(password),
    hasNumber: /[0-9]/.test(password),
    hasSpecialChar: /[!@#$%^&*(),.?":{}|<>]/.test(password),
    noRepeats: !(/(.)\1{2,}/.test(password)),
    notCommon: ![
      "password", "123456", "12345678", "qwerty", "abc123",
      "monkey", "letmein", "dragon", "111111", "baseball",
      "iloveyou", "trustno1", "sunshine", "master", "welcome",
      "shadow", "ashley", "football", "jesus", "michael",
      "ninja", "mustang", "password1", "123456789", "password123"
    ].includes(password.toLowerCase())
  };

  return rules;
};
