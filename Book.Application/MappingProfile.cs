using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain;

namespace Book.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBookRequest, Domain.Book>()
                .ForMember(c => c.BookTitle, action => action.MapFrom(d => d.Title))
                .ForMember(c => c.ISBN, action => action.MapFrom(d => d.ISBN))
                .ForMember(c=>c.AuthorBooks,action=>action.MapFrom(d=> d.AuthorIds.Select(e=>new AuthorBook(0,e))));

            CreateMap<History, HistoryResponse>();
            CreateMap<Domain.Book, BookResponse>()
                .ForMember(c => c.Title, action => action.MapFrom(d => d.BookTitle))
                .ForMember(c => c.Isbn, action => action.MapFrom(d => d.ISBN))
                .ForMember(c => c.Authors, action =>
                   action.MapFrom(d => d.AuthorBooks.Select(c => c.Author)
                   .Select(e => new AuthorResponse() { Id = e.Id, Name = e.Name })));

            CreateMap<Author, AuthorResponse>();
            CreateMap<SearchRequest, Filter>();
        }
    }
}
