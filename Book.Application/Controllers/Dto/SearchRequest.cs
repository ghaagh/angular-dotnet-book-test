using System.ComponentModel.DataAnnotations;

namespace Book.Application.Controllers.Dto
{
    public class SearchRequest
    {
        public string? OrderBy { get; set; }
        public string? SearchValue { get; set; }
        public string? SearchFields { get; set; }
        [Required]
        public int CurrentPage { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}
