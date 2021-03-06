using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain;
using Book.Application.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Book.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ISaver _saver;
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository, ISaver saver, IMapper mapper)
        {
            _saver = saver;
            _mapper = mapper;
            _bookRepo = bookRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GET(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            return Ok(_mapper.Map<BookResponse>(book));
        }

        [HttpGet]
        public async Task<IActionResult> GET([FromQuery] SearchRequest request)
        {
            var filter = _mapper.Map<Filter>(request);

            var bookPagedData = await _bookRepo.GetAsync(filter);

            return Ok(new Paged<BookResponse>(
                bookPagedData.Records.Select(c => _mapper.Map<BookResponse>(c)),
                bookPagedData.TotalSize,
                bookPagedData.CurrentPage,
                bookPagedData.PageSize));
        }

        [HttpPost]
        public async Task<IActionResult> POST(CreateBookRequest request)
        {
            var book = new Domain.Book(request.Title, request.ISBN, request.PublishedAt, request.AuthorIds, request.Description);

            await _bookRepo.AddAsync(book);
            await _saver.SaveAsync();

            return Ok(_mapper.Map<BookResponse>(await _bookRepo.GetByIdAsync(book.Id)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PUT(int id, CreateBookRequest request)
        {
            var book = _mapper.Map<Domain.Book>(request);

            await _bookRepo.UpdateAsync(id, book);
            await _saver.SaveAsync();

            return Ok(_mapper.Map<BookResponse>(await _bookRepo.GetByIdAsync(id)));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DELETE(int id)
        {

            await _bookRepo.DeleteAsync(id);
            await _saver.SaveAsync();

            return Ok();
        }
    }
}
