using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _blogContext;

        public PostRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IEnumerable<Post> GetAll()
        {
            if (_blogContext.Posts.Any())
                return _blogContext.Posts.Include(x => x.Comments).OrderBy(x => x.CreationDate).ToList();

            throw new Exception("No posts were found.");
        }

        public Post Get(Guid id)
        {
            var post = _blogContext.Posts.Include(x => x.Comments).FirstOrDefault(x => x.Id == id);

            if (post != null)
                return post;

            throw new Exception("Post not found.");
        }

        public Post Create(Post post)
        {
            _blogContext.Posts.Add(post);
            var successful = _blogContext.SaveChanges() != 0;

            if (successful)
                return post;
            else
                throw new Exception("Failed to save post.");
        }

        public Post Update(Post post)
        {
            var isUpdated = false;
            var postRetrieved = _blogContext.Posts.FirstOrDefault(x => x.Id == post.Id);

            if (postRetrieved != null)
            {
                if (!string.IsNullOrEmpty(post.Title))
                {
                    postRetrieved.Title = post.Title;
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(post.Content))
                {
                    postRetrieved.Content = post.Content;
                    isUpdated = true;
                }

                postRetrieved.ModifiedDate = isUpdated ? DateTime.Now : null;

                _blogContext.Posts.Update(postRetrieved);
                _blogContext.SaveChanges();

                return postRetrieved;
            }

            throw new Exception("Failed to update post.");
        }

        public bool Delete(Guid id)
        {
            var post = _blogContext.Posts.FirstOrDefault(x => x.Id == id);

            if (post != null)
            {
                _blogContext.Posts.Remove(post);
                return _blogContext.SaveChanges() != 0;
            }

            throw new Exception("Failed to delete post.");
        }
    }
}