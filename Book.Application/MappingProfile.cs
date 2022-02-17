using AutoMapper;
using Book.Application.Controllers.Dto;
using Book.Application.Domain;

namespace Book.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Book, BookResponse>()
                .ForMember(c => c.Title, action => action.MapFrom(d => d.BookTitle))
                .ForMember(c => c.Isbn, action => action.MapFrom(d=>d.ISBN));
            CreateMap<SearchBookRequest, Filter>();
        }
    }
}
