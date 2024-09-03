using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogContext _blogContext;

        public CommentRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IEnumerable<Comment> GetAll()
        {
            if (_blogContext.Comments.Any())
                return _blogContext.Comments.OrderBy(x => x.CreationDate);

            throw new Exception("No comments were found.");
        }

        public Comment Get(Guid id)
        {
            var comment = _blogContext.Comments.FirstOrDefault(x => x.Id == id);

            if (comment != null)
                return comment;

            throw new Exception("Comment not found.");
        }

        public Comment Create(Comment comment)
        {
            _blogContext.Comments.Add(comment);
            var successful = _blogContext.SaveChanges() != 0;

            if (successful)
                return comment;
            else
                throw new Exception("Failed to save comment.");
        }

        public Comment Update(Comment comment)
        {
            var isUpdated = false;
            var commentRetrieved = _blogContext.Comments.FirstOrDefault(x => x.Id == comment.Id);

            if (commentRetrieved != null)
            {
                if (!string.IsNullOrEmpty(comment.Content))
                {
                    commentRetrieved.Content = comment.Content;
                    isUpdated = true;
                }

                commentRetrieved.ModifiedDate = isUpdated ? DateTime.Now : null;

                _blogContext.Comments.Update(commentRetrieved);
                _blogContext.SaveChanges() ;

                return commentRetrieved;
            }

            throw new Exception("Failed to update comment.");
        }

        public bool Delete(Guid id)
        {
            var comment = _blogContext.Comments.FirstOrDefault(x => x.Id == id);

            if (comment != null)
            {
                _blogContext.Comments.Remove(comment);
                return _blogContext.SaveChanges() != 0;                
            }

            return false;
        }

        public IEnumerable<Comment> GetByPostId(Guid postId)
        {
            try
            {
                var postComments = _blogContext.Comments.Where(x => x.PostId == postId);

                if (postComments != null)
                    return postComments.OrderByDescending(x => x.CreationDate);

                return null;
            }
            catch (Exception)
            {
                throw new Exception("Failed attempt to query post comments.");
            }
        }
    }
}