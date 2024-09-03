using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            IEnumerable<Post> posts;

            try
            {
                posts = _postRepository.GetAll();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, posts);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Post> Get([FromRoute] Guid id)
        {
            Post post;

            try
            {
                post = _postRepository.Get(id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, post);
        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] Post post)
        {
            Post savedPost;

            try
            {
                savedPost = _postRepository.Create(post);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return StatusCode(StatusCodes.Status200OK, savedPost);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Post> Put([FromRoute] Guid id, [FromBody] Post post)
        {
            try
            {
                _postRepository.Get(id);

                Post updatedPost;

                try
                {
                    updatedPost = _postRepository.Update(post);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }

                return StatusCode(StatusCodes.Status200OK, updatedPost);
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
                _postRepository.Get(id);

                try
                {
                    _postRepository.Delete(id);

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

        [HttpGet("{id:guid}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments([FromRoute] Guid id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _commentRepository.GetByPostId(id));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}