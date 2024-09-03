using Model;
using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetAll();
        Post Get(Guid id);
        Post Create(Post post);
        Post Update(Post post);
        bool Delete(Guid id);
    }
}
