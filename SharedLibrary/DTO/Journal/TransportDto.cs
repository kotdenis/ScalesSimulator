namespace SharedLibrary.DTO.Journal
{
    public class TransportDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string CarPlate { get; set; } = string.Empty;
        public int NumberOfAxles { get; set; }
        public float Weight { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<AxlesDto> AxlesDtos { get; set; } = new List<AxlesDto>();
    }
}
