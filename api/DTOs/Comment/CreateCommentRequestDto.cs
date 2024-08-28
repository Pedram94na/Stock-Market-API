using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must contain at least 5 characters")]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(280, ErrorMessage = "Content can not be over 280 characters")]
        public string Content { get; set; } = string.Empty;
    }
}