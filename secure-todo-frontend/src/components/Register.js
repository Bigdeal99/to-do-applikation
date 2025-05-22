import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';
import { checkPasswordRules } from '../utils/passwordValidation';

const Register = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errors, setErrors] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [passwordRules, setPasswordRules] = useState({
    minLength: false,
    hasUppercase: false,
    hasLowercase: false,
    hasNumber: false,
    hasSpecialChar: false,
    noRepeats: false,
    notCommon: false,
  });

  const navigate = useNavigate();
  const { register } = useAuth();

  const handlePasswordChange = (e) => {
    const value = e.target.value;
    setPassword(value);
    setPasswordRules(checkPasswordRules(value));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors([]);
    setIsLoading(true);

    if (username.length < 4) {
      setErrors(['Username must be at least 4 characters long']);
      setIsLoading(false);
      return;
    }

    if (!/^[a-zA-Z0-9_]+$/.test(username)) {
      setErrors(['Username can only contain letters, numbers, and underscores']);
      setIsLoading(false);
      return;
    }

    const allRulesPassed = Object.values(passwordRules).every((rule) => rule === true);
    if (!allRulesPassed) {
      const failedRules = [];
      if (!passwordRules.minLength) failedRules.push('Password must be at least 8 characters long');
      if (!passwordRules.hasUppercase) failedRules.push('Password must contain at least one uppercase letter');
      if (!passwordRules.hasLowercase) failedRules.push('Password must contain at least one lowercase letter');
      if (!passwordRules.hasNumber) failedRules.push('Password must contain at least one number');
      if (!passwordRules.hasSpecialChar) failedRules.push('Password must contain at least one special character');
      if (!passwordRules.noRepeats) failedRules.push('Password contains too many repeating characters');
      if (!passwordRules.notCommon) failedRules.push('Password is too common');
      setErrors(failedRules);
      setIsLoading(false);
      return;
    }

    if (password !== confirmPassword) {
      setErrors(['Passwords do not match']);
      setIsLoading(false);
      return;
    }

    try {
      const result = await register(username, password);
      if (result.success) {
        navigate('/login');
      } else {
        if (result.errors) {
          setErrors(result.errors);
        } else {
          setErrors([result.message]);
        }
      }
    } catch (error) {
      setErrors(['An error occurred during registration']);
    } finally {
      setIsLoading(false);
    }
  };

  return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
        <div className="max-w-md w-full space-y-8">
          <div>
            <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
              Create your account
            </h2>
          </div>
          <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
            <div className="rounded-md shadow-sm -space-y-px">
              <div>
                <label htmlFor="username" className="sr-only">Username</label>
                <input
                    id="username"
                    name="username"
                    type="text"
                    required
                    className="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
              </div>
              <div>
                <label htmlFor="password" className="sr-only">Password</label>
                <input
                    id="password"
                    name="password"
                    type="password"
                    required
                    className="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                    placeholder="Password"
                    value={password}
                    onChange={handlePasswordChange}
                />
              </div>
              <div>
                <label htmlFor="confirm-password" className="sr-only">Confirm Password</label>
                <input
                    id="confirm-password"
                    name="confirm-password"
                    type="password"
                    required
                    className="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
                    placeholder="Confirm Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                />
              </div>
            </div>

            {errors.length > 0 && (
                <div className="rounded-md bg-red-50 p-4">
                  <div className="flex">
                    <div className="ml-3">
                      <h3 className="text-sm font-medium text-red-800">
                        There were errors with your submission
                      </h3>
                      <div className="mt-2 text-sm text-red-700">
                        <ul className="list-disc pl-5 space-y-1">
                          {errors.map((error, index) => (
                              <li key={index}>{error}</li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
            )}

            <div>
              <button
                  type="submit"
                  disabled={isLoading}
                  className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
              >
                {isLoading ? 'Registering...' : 'Register'}
              </button>
            </div>
          </form>

          <div className="mt-4">
            <h3 className="text-sm font-medium text-gray-700">Password Requirements:</h3>
            <ul className="mt-2 text-sm text-gray-600 list-disc pl-5 space-y-1">
              <li>{passwordRules.minLength ? '✅' : '❌'} At least 8 characters long</li>
              <li>{passwordRules.hasUppercase ? '✅' : '❌'} At least one uppercase letter</li>
              <li>{passwordRules.hasLowercase ? '✅' : '❌'} At least one lowercase letter</li>
              <li>{passwordRules.hasNumber ? '✅' : '❌'} At least one number</li>
              <li>{passwordRules.hasSpecialChar ? '✅' : '❌'} At least one special character</li>
              <li>{passwordRules.noRepeats ? '✅' : '❌'} No repeating characters (more than 3 in a row)</li>
              <li>{passwordRules.notCommon ? '✅' : '❌'} Cannot be a common password</li>
            </ul>
          </div>
        </div>
      </div>
  );
};

export default Register;
