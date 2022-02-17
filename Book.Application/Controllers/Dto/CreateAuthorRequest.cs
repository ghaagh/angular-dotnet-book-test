using System.ComponentModel.DataAnnotations;

namespace Book.Application.Controllers.Dto
{
    public class CreateAuthorRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
