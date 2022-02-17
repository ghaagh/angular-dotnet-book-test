using System.ComponentModel.DataAnnotations;

namespace Book.Application.Controllers.Dto
{
    public class CreateBookRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public List<int> AuthorIds { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public DateTime PublishedAt { get; set; }
    } 
}
