using System;
using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using Repository;
using Xunit;

namespace Api.Tests
{
    public class CommentControllerTests
    {
        [Fact]
        public void GetAll_Returns_ExistingComments()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepositorty = new Mock<IPostRepository>();

            var expectedComments = new List<Comment>
            {
                new() {Id = Guid.NewGuid(), Author = "Pedro Paulo", Content = "Very interesting topic, I like so much, excited for new ones.", CreationDate = DateTime.Now },
                new() {Id = Guid.NewGuid(), Author = "José João", Content = "Was better watch Pelé's movie.", CreationDate = DateTime.Now }
            };

            mockCommentRepository.Setup(fkRepo => fkRepo.GetAll()).Returns(expectedComments);

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepositorty.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(expectedComments, response.Value);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public void GetAll_ThrowsNotFound_WithoutComments()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepositorty = new Mock<IPostRepository>();

            mockCommentRepository.Setup(fkRepo => fkRepo.GetAll()).Throws(new Exception("No comments were found."));

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepositorty.Object);

            // Act
            var result = controller.GetAll();

            //Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public void Get_Return_ExistingComment()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepositorty = new Mock<IPostRepository>();

            var fkId = Guid.NewGuid();

            var expectedComment = new Comment { Id = fkId, Author = "Pedro Paulo", Content = "Very interesting topic, I like so much, excited for new ones.", CreationDate = DateTime.Now };

            mockCommentRepository.Setup(fkRepo => fkRepo.Get(fkId)).Returns(expectedComment);

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepositorty.Object);

            // Act
            var result = controller.Get(fkId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(expectedComment, response.Value);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public void Get_ThrowsNotFound_WithoutComment()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepositorty = new Mock<IPostRepository>();

            var fkId = Guid.NewGuid();

            mockCommentRepository.Setup(fkRepo => fkRepo.Get(fkId)).Throws(new Exception("Comment not found."));

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepositorty.Object);

            // Act
            var result = controller.Get(fkId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public void Post_Returns_CommentIsAdded()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepository = new Mock<IPostRepository>();

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Author = "Pedro Paulo",
                Content = "This is a new comment.",
                CreationDate = DateTime.Now
            };

            var savedComment = new Comment
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Author = comment.Author,
                Content = comment.Content,
                CreationDate = comment.CreationDate
            };

            mockPostRepository.Setup(fkRepo => fkRepo.Get(comment.PostId)).Returns(new Post { Id = comment.PostId });
            mockCommentRepository.Setup(fkRepo => fkRepo.Create(comment)).Returns(savedComment);

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepository.Object);

            // Act
            var result = controller.Post(comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(savedComment, response.Value);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public void Post_ThrowsPreconditionFailed_PostDoesNotExist()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepository = new Mock<IPostRepository>();

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Author = "Pedro Paulo",
                Content = "This is a new comment.",
                CreationDate = DateTime.Now
            };

            mockPostRepository.Setup(fkRepo => fkRepo.Get(comment.PostId)).Throws(new Exception("Post not found."));

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepository.Object);

            // Act
            var result = controller.Post(comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal("Post not found. Must have a valid post to add a comment.", response.Value);
            Assert.Equal(StatusCodes.Status412PreconditionFailed, response.StatusCode);
        }

        [Fact]
        public void Post_ThrowsInternalServerError_CommentCreationFails()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var mockPostRepository = new Mock<IPostRepository>();

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Author = "Pedro Paulo",
                Content = "This is a new comment.",
                CreationDate = DateTime.Now
            };

            mockPostRepository.Setup(fkRepo => fkRepo.Get(comment.PostId)).Returns(new Post { Id = comment.PostId });
            mockCommentRepository.Setup(fkRepo => fkRepo.Create(comment)).Throws(new Exception("Database failure."));

            var controller = new CommentController(mockCommentRepository.Object, mockPostRepository.Object);

            // Act
            var result = controller.Post(comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal("Database failure.", response.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Put_Returns_UpdatedComment()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Content = "Updated content" };

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Returns(new Comment());
            mockCommentRepository.Setup(repo => repo.Update(comment)).Returns(comment);

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Put(commentId, comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(comment, response.Value);
        }

        [Fact]
        public void Put_ThrowsNotFound_CommentDoesNotExist()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Content = "Updated content" };

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Throws(new Exception("Comment not found"));

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Put(commentId, comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Comment not found", response.Value);
        }

        [Fact]
        public void Put_ThrowsInternalServerError_UpdateFails()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, Content = "Updated content" };

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Returns(new Comment()); // Assume exists
            mockCommentRepository.Setup(repo => repo.Update(comment)).Throws(new Exception("Database error"));

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Put(commentId, comment);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Database error", response.Value);
        }

        [Fact]
        public void Delete_Returns_NoContent_DeletionIsSuccessful()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Returns(new Comment()); // Assume exists
            mockCommentRepository.Setup(repo => repo.Delete(commentId)).Verifiable();

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Delete(commentId);

            // Assert
            var response = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [Fact]
        public void Delete_ThrowNotFound_CommentDoesNotExist()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Throws(new Exception("Comment not found"));

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Delete(commentId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Comment not found", response.Value);
        }

        [Fact]
        public void Delete_ThrowsInternalServerError_DeletionFails()
        {
            // Arrange
            var mockCommentRepository = new Mock<ICommentRepository>();
            var commentId = Guid.NewGuid();

            mockCommentRepository.Setup(repo => repo.Get(commentId)).Returns(new Comment()); // Assume exists
            mockCommentRepository.Setup(repo => repo.Delete(commentId)).Throws(new Exception("Database error"));

            var controller = new CommentController(mockCommentRepository.Object, null);

            // Act
            var result = controller.Delete(commentId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Database error", response.Value);
        }

    }
}