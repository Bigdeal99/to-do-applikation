using Xunit;
using Moq;
using SecureTodoApi.Services;
using SecureTodoApi.Repositories;
using SecureTodoApi.Models;
using SecureTodoApi.Models.DTOs;
using System.Collections.Generic;

namespace SecureTodoApi.Tests
{
    public class TodoServiceTests
    {
        [Fact]
        public void CreateTodo_ShouldReturnTodoWithCorrectUserId()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var service = new TodoService(mockRepo.Object);

            var request = new TodoCreateRequest
            {
                Title = "Test Task",
                Category = "Work",
                Description = "Test Desc"
            };

            mockRepo.Setup(r => r.Create(It.IsAny<TodoItem>()))
                .Callback<TodoItem>(t => t.Id = 1);

            var result = service.CreateTodo(5, request);

            Assert.Equal("Test Task", result.Title);
            Assert.Equal(5, result.UserId);
        }

        [Fact]
        public void UpdateTodo_ShouldReturnTrue_WhenTodoExistsAndMatchesUser()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var existing = new TodoItem
            {
                Id = 1,
                Title = "Old",
                Description = "Old",
                Category = "Old",
                UserId = 10,
                IsCompleted = false
            };

            var request = new TodoUpdateRequest
            {
                Title = "New",
                Description = "New Desc",
                Category = "Updated",
                IsCompleted = true,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            mockRepo.Setup(r => r.GetById(1)).Returns(existing);

            var service = new TodoService(mockRepo.Object);

            var result = service.UpdateTodo(10, 1, request);

            Assert.True(result);
            Assert.Equal("New", existing.Title);
            Assert.Equal("Updated", existing.Category);
            Assert.True(existing.IsCompleted);
            mockRepo.Verify(r => r.Update(existing), Times.Once);
        }

        [Fact]
        public void UpdateTodo_ShouldReturnFalse_WhenTodoNotFound()
        {
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetById(99)).Returns((TodoItem?)null);

            var service = new TodoService(mockRepo.Object);

            var result = service.UpdateTodo(1, 99, new TodoUpdateRequest());

            Assert.False(result);
        }

        [Fact]
        public void UpdateTodo_ShouldReturnFalse_WhenUserMismatch()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var todo = new TodoItem { Id = 1, UserId = 999 };
            mockRepo.Setup(r => r.GetById(1)).Returns(todo);

            var service = new TodoService(mockRepo.Object);

            var result = service.UpdateTodo(1, 1, new TodoUpdateRequest());

            Assert.False(result);
        }

        [Fact]
        public void DeleteTodo_ShouldReturnTrue_WhenTodoExistsAndMatchesUser()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var todo = new TodoItem { Id = 2, UserId = 10 };

            mockRepo.Setup(r => r.GetById(2)).Returns(todo);

            var service = new TodoService(mockRepo.Object);

            var result = service.DeleteTodo(10, 2);

            Assert.True(result);
            mockRepo.Verify(r => r.Delete(todo), Times.Once);
        }

        [Fact]
        public void DeleteTodo_ShouldReturnFalse_WhenTodoNotFound()
        {
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetById(99)).Returns((TodoItem?)null);

            var service = new TodoService(mockRepo.Object);

            var result = service.DeleteTodo(1, 99);

            Assert.False(result);
            mockRepo.Verify(r => r.Delete(It.IsAny<TodoItem>()), Times.Never);
        }

        [Fact]
        public void DeleteTodo_ShouldReturnFalse_WhenUserMismatch()
        {
            var mockRepo = new Mock<ITodoRepository>();
            var todo = new TodoItem { Id = 3, UserId = 10 };

            mockRepo.Setup(r => r.GetById(3)).Returns(todo);

            var service = new TodoService(mockRepo.Object);

            var result = service.DeleteTodo(999, 3);

            Assert.False(result);
            mockRepo.Verify(r => r.Delete(It.IsAny<TodoItem>()), Times.Never);
        }
    }
}
