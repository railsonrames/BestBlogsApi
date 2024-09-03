using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            IEnumerable<Comment> comments;

            try
            {
                comments = _commentRepository.GetAll();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, comments);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Comment> Get([FromRoute] Guid id)
        {
            Comment comment;

            try
            {
                comment = _commentRepository.Get(id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, comment);
        }

        [HttpPost]
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            try
            {
                _postRepository.Get(comment.PostId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status412PreconditionFailed, e.Message + " Must have a valid post to add a comment.");
            }

            Comment savedComment;

            try
            {
                savedComment = _commentRepository.Create(comment);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, savedComment);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Comment comment)
        {
            try
            {
                _commentRepository.Get(id);

                Comment updatedComment;

                try
                {
                    updatedComment = _commentRepository.Update(comment);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }

                return StatusCode(StatusCodes.Status200OK, updatedComment);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _commentRepository.Get(id);

                try
                {
                    _commentRepository.Delete(id);

                    return StatusCode(StatusCodes.Status204NoContent);
                }
                catch (Exception e)
                { 
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}