namespace Scales.Journal.Domain.Entities
{
    public class Axles : BaseEntity
    {
        public int AxleNumber { get; set; }
        public float LoadPerAxle { get; set; }
        public int TransportId { get; set; }

        public Transport Transport { get; set; } = new();
    }
}
