using System.Collections.Generic;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Model;
using Xunit;

namespace Api.Tests
{
    public class CommentControllerTests
    {
        [Fact]
        public void GetAll_Returns_Existing_Comments()
        {
            // Arrange
            var expected = new List<Comment>();

            // Act
            var actual = new CommentController(null).GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }
    }
}