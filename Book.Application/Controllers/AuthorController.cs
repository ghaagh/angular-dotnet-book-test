using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ISaver _saver;
        private readonly IMapper _mapper;
        public AuthorController(IAuthorRepository authorRepository, ISaver saver, IMapper mapper)
        {
            _mapper = mapper;
            _saver = saver;
            _authorRepository = authorRepository;
        }
        [HttpPost]
        public async Task<IActionResult> POST(CreateAuthorRequest request)
        {
            var author = new Domain.Author(request.Name);

            await _authorRepository.AddAsync(author);
            await _saver.SaveAsync();

            return Ok(_mapper.Map<AuthorResponse>(author));
        }
    }
}
