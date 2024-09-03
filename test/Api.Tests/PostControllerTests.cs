using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using Xunit;

namespace Api.Tests
{
    public class PostControllerTests
    {
        [Fact]
        public void GetAll_Returns_PostsExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var expectedPosts = new List<Post>
            {
                new() { Id = Guid.NewGuid(), Title = "First Post", Content = "Content of the first post" },
                new() { Id = Guid.NewGuid(), Title = "Second Post", Content = "Content of the second post" }
            };

            mockPostRepository.Setup(repo => repo.GetAll()).Returns(expectedPosts);

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(expectedPosts, response.Value);
        }

        [Fact]
        public void GetAll_ThrowsNotFound_NoPostsExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();

            mockPostRepository.Setup(repo => repo.GetAll()).Throws(new Exception("No posts found"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("No posts found", response.Value);
        }

        [Fact]
        public void Get_Return_Post_PostExists()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();
            var expectedPost = new Post { Id = postId, Title = "Existing Post", Content = "Content here" };

            mockPostRepository.Setup(repo => repo.Get(postId)).Returns(expectedPost);

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Get(postId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(expectedPost, response.Value);
        }

        [Fact]
        public void Get_ThrowsNotFound_PostDoesNotExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();

            mockPostRepository.Setup(repo => repo.Get(postId)).Throws(new Exception("Post not found"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Get(postId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Post not found", response.Value);
        }

        [Fact]
        public void Post_CreatedPost_SaveIsSuccessful()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var newPost = new Post { Title = "New Post", Content = "New post content" };
            var savedPost = new Post { Id = Guid.NewGuid(), Title = "New Post", Content = "New post content" };

            mockPostRepository.Setup(repo => repo.Create(newPost)).Returns(savedPost);

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Post(newPost);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(savedPost, response.Value);
        }

        [Fact]
        public void Post_ThrowsInternalServerError_SaveFails()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var newPost = new Post { Title = "New Post", Content = "New post content" };

            mockPostRepository.Setup(repo => repo.Create(newPost)).Throws(new Exception("Database error"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Post(newPost);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Database error", response.Value);
        }

        [Fact]
        public void Put_Returns_UpdatedPost_UpdateIsSuccessful()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();
            var updatedPost = new Post { Id = postId, Title = "Updated Title", Content = "Updated content" };

            mockPostRepository.Setup(repo => repo.Get(postId)).Returns(new Post()); // Assuming existing post
            mockPostRepository.Setup(repo => repo.Update(updatedPost)).Returns(updatedPost);

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Put(postId, updatedPost);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(updatedPost, response.Value);
        }

        [Fact]
        public void Put_ThrowsNotFound_PostDoesNotExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();
            var updatedPost = new Post { Id = postId, Title = "Updated Title", Content = "Updated content" };

            mockPostRepository.Setup(repo => repo.Get(postId)).Throws(new Exception("Post not found"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Put(postId, updatedPost);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Post not found", response.Value);
        }

        [Fact]
        public void Delete_Returns_NoContent_DeletionIsSuccessful()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();

            mockPostRepository.Setup(repo => repo.Get(postId)).Returns(new Post()); // Assume the post exists
            mockPostRepository.Setup(repo => repo.Delete(postId)).Verifiable();

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Delete(postId);

            // Assert
            var response = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [Fact]
        public void Delete_ThrowsNotFound_PostDoesNotExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();

            mockPostRepository.Setup(repo => repo.Get(postId)).Throws(new Exception("Post not found"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.Delete(postId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal("Post not found", response.Value);
        }

        [Fact]
        public void GetComments_Returns_CommentsForPost_CommentsExist()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();
            var expectedComments = new List<Comment>
            {
                new() { Id = Guid.NewGuid(), PostId = postId, Content = "Comment 1" },
                new() { Id = Guid.NewGuid(), PostId = postId, Content = "Comment 2" }
            };

            mockCommentRepository.Setup(repo => repo.GetByPostId(postId)).Returns(expectedComments);

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.GetComments(postId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(expectedComments, response.Value);
        }

        [Fact]
        public void GetComments_ThrowsInternalServerError_FetchingCommentsFails()
        {
            // Arrange
            var mockPostRepository = new Mock<IPostRepository>();
            var mockCommentRepository = new Mock<ICommentRepository>();
            var postId = Guid.NewGuid();

            mockCommentRepository.Setup(repo => repo.GetByPostId(postId)).Throws(new Exception("Database error"));

            var controller = new PostController(mockPostRepository.Object, mockCommentRepository.Object);

            // Act
            var result = controller.GetComments(postId);

            // Assert
            var response = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, response.StatusCode);
            Assert.Equal("Database error", response.Value);
        }

    }
}
