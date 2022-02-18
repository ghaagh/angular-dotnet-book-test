using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain;
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
            var author = new Author(request.Name);

            await _authorRepository.AddAsync(author);
            await _saver.SaveAsync();

            return Ok(_mapper.Map<AuthorResponse>(author));
        }

        [HttpGet]
        public async Task<IActionResult> GET([FromQuery] SearchBookRequest request)
        {
            var filter = _mapper.Map<Filter>(request);

            var bookPagedData = await _authorRepository.GetAsync(filter);

            return Ok(new Paged<AuthorResponse>(
                bookPagedData.Records.Select(c => _mapper.Map<AuthorResponse>(c)),
                bookPagedData.TotalSize,
                bookPagedData.CurrentPage,
                bookPagedData.PageSize));
        }
    }
}
