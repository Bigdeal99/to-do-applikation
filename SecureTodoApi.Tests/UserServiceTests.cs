using Xunit;
using Moq;
using SecureTodoApi.Services;
using SecureTodoApi.Repositories;
using SecureTodoApi.Models;
using SecureTodoApi.Security;
using System;

namespace SecureTodoApi.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void Register_ShouldSucceed_WithValidPassword()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByUsername("newuser")).Returns((User)null);

            var hasher = new PasswordHasher();
            var service = new UserService(mockRepo.Object, hasher);

            var (user, errors) = service.Register("newuser", "ValidPass123!");

            Assert.NotNull(user);
            Assert.Null(errors);
            Assert.Equal("newuser", user.Username);
        }

        [Fact]
        public void Register_ShouldFail_WithWeakPassword()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByUsername("newuser")).Returns((User)null);

            var hasher = new PasswordHasher();
            var service = new UserService(mockRepo.Object, hasher);

            var (user, errors) = service.Register("newuser", "weak");

            Assert.Null(user);
            Assert.NotNull(errors);
            Assert.Contains(errors, e => e.Contains("at least 8 characters"));
        }

        [Fact]
        public void Register_ShouldFail_WhenUsernameExists()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByUsername("existing")).Returns(new User { Username = "existing" });

            var hasher = new PasswordHasher();
            var service = new UserService(mockRepo.Object, hasher);

            var (user, errors) = service.Register("existing", "ValidPass123!");

            Assert.Null(user);
            Assert.NotNull(errors);
            Assert.Contains(errors, e => e.Contains("already exists"));
        }
    }
}