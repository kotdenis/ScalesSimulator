using AutoMapper;
using Scales.Journal.Domain.Entities;
using SharedLibrary.DTO.Journal;

namespace Scales.Journal.Core.Configuration
{
    public class JournalMapperConfiguration : Profile
    {
        public JournalMapperConfiguration()
        {
            CreateMap<Axles, AxlesDto>();
            CreateMap<AxlesDto, Axles>();

            CreateMap<Transport, TransportDto>()
                .ForMember(x => x.AxlesDtos, _ => _.MapFrom(x => x.Axles))
                .ForMember(x => x.Brand, _ => _.MapFrom(x => x.Name));
            CreateMap<TransportDto, Transport>()
                .ForMember(x => x.Axles, _ => _.MapFrom(x => x.AxlesDtos))
                .ForMember(x => x.Name, _ => _.MapFrom(x => x.Brand));
        }
    }
}
