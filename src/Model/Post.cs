using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public record Post
    {
        public Guid Id { get; set; }
        [StringLength(30, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Title { get; set; }
        [StringLength(1200, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}