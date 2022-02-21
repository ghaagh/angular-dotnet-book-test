using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain;
using Book.Application.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Book.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookHistoryController : ControllerBase
    {
        private readonly IBookHistoryRepository _historyRepo;
        private readonly IMapper _mapper;

        public BookHistoryController(IBookHistoryRepository bookHistoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _historyRepo = bookHistoryRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GET(int id, [FromQuery] SearchRequest request)
        {
            var filter = _mapper.Map<Filter>(request);

            var bookPagedData = await _historyRepo.GetAsync(id, filter);

            return Ok(new Paged<HistoryResponse>(
                bookPagedData.Records.Select(c => _mapper.Map<HistoryResponse>(c)),
                bookPagedData.TotalSize,
                bookPagedData.CurrentPage,
                bookPagedData.PageSize));
        }
    }
}
