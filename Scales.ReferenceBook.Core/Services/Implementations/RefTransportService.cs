using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scales.ReferenceBook.Core.Constants;
using Scales.ReferenceBook.Core.Services.Interfaces;
using Scales.ReferenceBook.Domain.Entities;
using Scales.ReferenceBook.Domain.Repositories.Implementations;
using Scales.ReferenceBook.Domain.Repositories.Interfaces;
using SharedLibrary.DTO.ReferenceBook;
using SharedLibrary.Paging;
using StackExchange.Redis;

namespace Scales.ReferenceBook.Core.Services.Implementations
{
    public class RefTransportService : IRefTransportService
    {
        private readonly IReferenceTransportRepository _transportRepository;
        private readonly IMapper _mapper;
        private readonly IDatabase _database;
        private readonly IValidator<RefTransportDto> _refTransportValidator;
        private readonly UnitOfWork _unitOfWork;
        public RefTransportService(IReferenceTransportRepository transportRepository,
            IMapper mapper,
            IConnectionMultiplexer multiplexer,
            IValidator<RefTransportDto> refTransportValidator,
            UnitOfWork unitOfWork)
        {
            _transportRepository = transportRepository;
            _mapper = mapper;
            _refTransportValidator = refTransportValidator;
            _database = multiplexer.GetDatabase();
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<RefTransportDto>> GetReferanceBookAsync(PageParameters parameters, CancellationToken ct)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var refDtos = await GetCahedReferenceAsync(ct);
            var pagedList = PagedList<RefTransportDto>.ToPagedList(refDtos, parameters.PageNumber, parameters.PageSize);
            return pagedList;
        }

        public async Task UpdateRefTransportAsync(RefTransportDto refTransportDto, CancellationToken ct)
        {
            if (!_refTransportValidator.Validate(refTransportDto).IsValid)
                throw new ArgumentException("Validation failed");
            var transport = _mapper.Map<ReferenceTransport>(refTransportDto);
            await _transportRepository.UpdateAsync(transport, ct);
            await _unitOfWork.SaveAsync(ct);
            await _database.KeyDeleteAsync(CacheConstants.REFERENCE_ALL);
        }

        public async Task CreateRefTransportAsync(RefTransportDto refTransportDto, CancellationToken ct)
        {
            if (!_refTransportValidator.Validate(refTransportDto).IsValid)
                throw new ArgumentException("Validation failed");
            var transport = new ReferenceTransport
            {
                NumberOfAxles = refTransportDto.NumberOfAxles,
                Brand = refTransportDto.Brand,
                CarPlate = refTransportDto.CarPlate,
            };
            if (await CheckIfExistsAsync(refTransportDto))
                throw new InvalidOperationException("This transport already exists.");
            await _transportRepository.CreateAsync(transport, ct);
            await _unitOfWork.SaveAsync(ct);
            await _database.KeyDeleteAsync(CacheConstants.REFERENCE_ALL);
        }

        private async Task<List<RefTransportDto>> GetCahedReferenceAsync(CancellationToken ct)
        {
            var redisValue = await _database.StringGetAsync(CacheConstants.REFERENCE_ALL);
            if (!redisValue.HasValue)
            {
                var refTransports = await _transportRepository
                    .GetAsQueryable(ct)
                    .AsNoTracking()
                    .ToListAsync();
                var refDtos = _mapper.Map<List<RefTransportDto>>(refTransports);
                var temp = System.Text.Json.JsonSerializer.Serialize(refDtos);
                await _database.StringSetAsync(CacheConstants.REFERENCE_ALL, temp);
                return refDtos;
            }
            else
            {
                var refDtos = System.Text.Json.JsonSerializer.Deserialize<List<RefTransportDto>>(redisValue)!;
                return refDtos;
            }
        }

        private async Task<bool> CheckIfExistsAsync(RefTransportDto refTransportDto)
        {
            if (refTransportDto == null) 
                return false;
            var redisValue = await _database.StringGetAsync(CacheConstants.REFERENCE_ALL);
            var refDtos = System.Text.Json.JsonSerializer.Deserialize<List<RefTransportDto>>(redisValue)!;
            var refDto = refDtos.Where(x => x.CarPlate == refTransportDto.CarPlate && x.Brand == refTransportDto.Brand).FirstOrDefault();
            if(refDto == null) 
                return false;
            return true;
        }
    }
}
