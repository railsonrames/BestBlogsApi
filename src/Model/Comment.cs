using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Model
{
    public record Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        [StringLength(120, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Content { get; set; }
        [StringLength(30, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
    }
}