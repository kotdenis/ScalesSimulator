namespace SharedLibrary.DTO.ReferenceBook
{
    public class RefTransportDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string CarPlate { get; set; } = string.Empty;
        public int NumberOfAxles { get; set; }
    }
}
