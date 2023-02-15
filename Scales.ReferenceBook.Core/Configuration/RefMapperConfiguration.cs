using AutoMapper;
using Scales.ReferenceBook.Domain.Entities;
using SharedLibrary.DTO.ReferenceBook;

namespace Scales.ReferenceBook.Core.Configuration
{
    public class RefMapperConfiguration : Profile
    {
        public RefMapperConfiguration()
        {
            CreateMap<RefTransportDto, ReferenceTransport>();
            CreateMap<ReferenceTransport, RefTransportDto>();
        }
    }
}
