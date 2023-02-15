using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Scales.Journal.Core.Constants;
using Scales.Journal.Core.Options;
using Scales.Journal.Core.Services.Interfaces;
using Scales.Journal.Domain.Entities;
using Scales.Journal.Domain.Repositories.Implementations;
using Scales.Journal.Domain.Repositories.Interfaces;
using SharedLibrary.DTO.Journal;
using SharedLibrary.Paging;
using StackExchange.Redis;
using System.Text;

namespace Scales.Journal.Core.Services.Implementations
{
    public class JournalService : IJournalService
    {
        private readonly ITransportRepository _transportRepository;
        private readonly IAxlesRepository _axlesRepository;
        private readonly IMapper _mapper;
        private readonly IDatabase _database;
        private readonly IValidator<TransportDto> _transportValidator;
        private readonly UnitOfWork _unitOfWork;
        private readonly RabbitMqOptions _rabbitMqOptions;
        public JournalService(ITransportRepository transportRepository,
            IAxlesRepository axlesRepository,
            IMapper mapper,
            IConnectionMultiplexer multiplexer,
            IValidator<TransportDto> transportValidator,
            UnitOfWork unitOfWork,
            IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _mapper = mapper;
            _transportRepository = transportRepository;
            _database = multiplexer.GetDatabase();
            _transportValidator = transportValidator;
            _unitOfWork = unitOfWork;
            _axlesRepository = axlesRepository;
            _rabbitMqOptions = rabbitMqOptions.Value;
        }

        public async Task<PagedList<TransportDto>> GetJournalAsync(PageParameters parameters, CancellationToken ct)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var transportDtos = await GetJournalCahedAsync(ct);
            var pagedList = PagedList<TransportDto>.ToPagedList(transportDtos, parameters.PageNumber, parameters.PageSize);
            return pagedList;
        }

        public async Task<PagedList<TransportDto>> GetJournalByDatesAsync(PageParameters parameters, DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var start = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(endDate.Date.AddDays(1), DateTimeKind.Utc);
            var list = await GetJournalCahedAsync(ct);
            var transportDtos = list.Where(x => x.CreatedDate >= start && x.CreatedDate <= end).ToList();
            var pagedList = PagedList<TransportDto>.ToPagedList(transportDtos, parameters.PageNumber, parameters.PageSize);
            return pagedList;
        }

        public async Task SaveWeighingDataAsync(TransportDto transportDto, CancellationToken ct)
        {
            if (!_transportValidator.Validate(transportDto).IsValid)
                throw new ArgumentException();
            using var transaction = await _unitOfWork.EnsureTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            var axlesDatas = new List<Axles>();

            var transport = new Transport
            {
                Axles = axlesDatas,
                Cargo = transportDto.Cargo,
                NumberOfAxles = transportDto.AxlesDtos.Count,
                CarPlate = transportDto.CarPlate,
                Name = transportDto.Brand,
                Weight = transportDto.Weight
            };
            await _transportRepository.CreateAsync(transport, default);
            await _unitOfWork.SaveAsync(default);

            foreach (var item in transportDto.AxlesDtos)
            {
                var axleData = new Axles() { AxleNumber = item.AxleNumber, LoadPerAxle = item.LoadPerAxle, TransportId = transport.Id };
                await _axlesRepository.CreateAsync(axleData, default);
            }
            await _unitOfWork.SaveAsync(default);
            await _database.KeyDeleteAsync(CacheConstants.JOURNAL_ALL);
            transaction.Commit();
            SendByExchange(transportDto);
        }

        private async Task<List<TransportDto>> GetJournalCahedAsync(CancellationToken ct)
        {
            var redisValue = await _database.StringGetAsync(CacheConstants.JOURNAL_ALL);
            if (!redisValue.HasValue)
            {
                var transports = await _transportRepository.GetAsQueryable(ct)
                       .Include(x => x.Axles)
                       .OrderByDescending(x => x.CreatedDate)
                       .AsNoTracking()
                       .ToListAsync();
                var transportDtos = _mapper.Map<List<TransportDto>>(transports);
                var temp = System.Text.Json.JsonSerializer.Serialize(transportDtos);
                await _database.StringSetAsync(CacheConstants.JOURNAL_ALL, temp);
                return transportDtos;
            }
            else
            {
                var transportDtos = System.Text.Json.JsonSerializer.Deserialize<List<TransportDto>>(redisValue)!;
                return transportDtos;
            }
        }

        private void SendByExchange(TransportDto transportDto)
        {
            using var channel = RabbitMqFactory.CreateChannel(_rabbitMqOptions.HostName, _rabbitMqOptions.Port);
            channel.ExchangeDeclare(exchange: "refBook", ExchangeType.Fanout);
            var ob = new { NumberOfAxles = transportDto.NumberOfAxles, Brand = transportDto.Brand, CarPlate = transportDto.CarPlate };
            var message = System.Text.Json.JsonSerializer.Serialize(ob);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "refBook", routingKey: "", basicProperties: null, body: body);
        }
    }
}
