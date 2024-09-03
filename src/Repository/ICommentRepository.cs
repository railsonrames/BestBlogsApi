using Model;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAll();
        Comment Get(Guid id);
        Comment Create(Comment comment);
        Comment Update(Comment comment);
        bool Delete(Guid id);
        IEnumerable<Comment> GetByPostId(Guid postId);
    }
}
